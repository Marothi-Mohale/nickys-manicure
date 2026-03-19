using Microsoft.Extensions.Logging.Abstractions;
using NickysManicurePedicure.Application.Services;
using NickysManicurePedicure.Dtos.Requests;
using NickysManicurePedicure.Models.Entities;
using NickysManicurePedicure.Tests.Infrastructure;
using Xunit;

namespace NickysManicurePedicure.Tests.Services;

public sealed class BookingAdminServiceTests
{
    [Fact]
    public async Task GetBookingsAsync_FiltersSortsAndPaginatesResults()
    {
        await using var database = new TestSqliteDatabase();
        await using var dbContext = database.CreateDbContext();

        dbContext.BookingRequests.AddRange(
            TestDataFactory.CreateBookingEntity(
                fullName: "Alicia Premium",
                email: "alicia@example.com",
                requestedServiceName: "Luxury Gel Manicure",
                status: BookingRequestStatus.Confirmed,
                preferredDate: DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(4)),
                preferredTime: new TimeOnly(14, 0),
                createdAtUtc: DateTime.UtcNow.AddHours(-3)),
            TestDataFactory.CreateBookingEntity(
                fullName: "Bianca Guest",
                email: "bianca@example.com",
                requestedServiceName: "Restorative Pedicure",
                status: BookingRequestStatus.Pending,
                preferredDate: DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(2)),
                preferredTime: new TimeOnly(9, 30),
                createdAtUtc: DateTime.UtcNow.AddHours(-2)),
            TestDataFactory.CreateBookingEntity(
                fullName: "Alicia Weekend",
                email: "alicia-two@example.com",
                requestedServiceName: "Luxury Gel Manicure",
                status: BookingRequestStatus.Confirmed,
                preferredDate: DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(1)),
                preferredTime: new TimeOnly(11, 15),
                createdAtUtc: DateTime.UtcNow.AddHours(-1)));
        await dbContext.SaveChangesAsync();

        var sut = new BookingAdminService(dbContext, new RecordingBookingNotificationService(), NullLogger<BookingAdminService>.Instance);

        var result = await sut.GetBookingsAsync(
            new BookingQueryParameters
            {
                Search = "Alicia",
                Status = "Confirmed",
                SortBy = "preferredDate",
                SortDirection = "asc",
                Page = 1,
                PageSize = 1
            },
            CancellationToken.None);

        Assert.Single(result.Items);
        Assert.Equal("Alicia Weekend", result.Items.First().FullName);
        Assert.Equal(2, result.Pagination.TotalCount);
        Assert.Equal(2, result.Pagination.TotalPages);
        Assert.Equal(1, result.Pagination.CurrentItemCount);
        Assert.True(result.Pagination.HasNextPage);
        Assert.False(result.Pagination.HasPreviousPage);
    }

    [Fact]
    public async Task UpdateStatusAsync_UpdatesBookingAndNotifies()
    {
        await using var database = new TestSqliteDatabase();
        await using var dbContext = database.CreateDbContext();

        var booking = TestDataFactory.CreateBookingEntity(
            fullName: "Status Change Client",
            email: "status@example.com",
            requestedServiceName: "Bridal Manicure",
            status: BookingRequestStatus.Pending,
            preferredDate: DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(5)),
            preferredTime: new TimeOnly(13, 0),
            createdAtUtc: DateTime.UtcNow.AddHours(-1));

        dbContext.BookingRequests.Add(booking);
        await dbContext.SaveChangesAsync();

        var notifications = new RecordingBookingNotificationService();
        var sut = new BookingAdminService(dbContext, notifications, NullLogger<BookingAdminService>.Instance);

        var response = await sut.UpdateStatusAsync(
            booking.Id,
            new UpdateBookingStatusDto
            {
                Status = "Confirmed",
                AdminNotes = "  Reserved with front desk.  "
            },
            CancellationToken.None);

        Assert.NotNull(response);
        Assert.Equal("Confirmed", response!.Status);
        Assert.Equal("Reserved with front desk.", response.AdminNotes);
        Assert.Equal(1, notifications.StatusUpdatedCallCount);
        Assert.Equal("Confirmed", notifications.LastStatusUpdatedBooking!.Status);
    }
}
