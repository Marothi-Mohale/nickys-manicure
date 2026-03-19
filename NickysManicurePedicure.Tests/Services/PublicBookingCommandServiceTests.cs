using Microsoft.Extensions.Logging.Abstractions;
using NickysManicurePedicure.Application.Services;
using NickysManicurePedicure.Common.Exceptions;
using NickysManicurePedicure.Tests.Infrastructure;
using Xunit;

namespace NickysManicurePedicure.Tests.Services;

public sealed class PublicBookingCommandServiceTests
{
    [Fact]
    public async Task CreateAsync_WithPublishedService_PersistsTrimmedBookingAndNotifies()
    {
        await using var database = new TestSqliteDatabase();
        await using var dbContext = database.CreateDbContext();

        var category = TestDataFactory.CreateCategory();
        dbContext.ServiceCategories.Add(category);
        await dbContext.SaveChangesAsync();

        var service = TestDataFactory.CreateService(category.Id);
        dbContext.Services.Add(service);
        await dbContext.SaveChangesAsync();

        var notifications = new RecordingBookingNotificationService();
        var sut = new PublicBookingCommandService(dbContext, notifications, NullLogger<PublicBookingCommandService>.Instance);

        var response = await sut.CreateAsync(
            TestDataFactory.CreateBookingRequest(preferredServiceId: service.Id),
            CancellationToken.None);

        var persistedBooking = await dbContext.BookingRequests.FindAsync(response.BookingId);

        Assert.NotNull(persistedBooking);
        Assert.Equal(service.Id, persistedBooking.ServiceId);
        Assert.Equal(service.Name, persistedBooking.RequestedServiceName);
        Assert.Equal("Nicky Client", persistedBooking.FullName);
        Assert.Equal("nicky@example.com", persistedBooking.Email);
        Assert.Equal("+27682518739", persistedBooking.PhoneNumber);
        Assert.Equal("I would love a weekday appointment if one is available.", persistedBooking.Message);
        Assert.Equal("Homepage", persistedBooking.SourcePage);
        Assert.Equal("Pending", response.Status);
        Assert.Equal(1, notifications.CreatedCallCount);
        Assert.NotNull(notifications.LastCreatedBooking);
        Assert.Equal(response.BookingId, notifications.LastCreatedBooking!.Id);
    }

    [Fact]
    public async Task CreateAsync_WithUnknownServiceId_ThrowsBadRequestException()
    {
        await using var database = new TestSqliteDatabase();
        await using var dbContext = database.CreateDbContext();

        var notifications = new RecordingBookingNotificationService();
        var sut = new PublicBookingCommandService(dbContext, notifications, NullLogger<PublicBookingCommandService>.Instance);

        var exception = await Assert.ThrowsAsync<BadRequestException>(() => sut.CreateAsync(
            TestDataFactory.CreateBookingRequest(preferredServiceId: 99999),
            CancellationToken.None));

        Assert.Equal("invalid_service_reference", exception.ErrorCode);
        Assert.Equal(0, notifications.CreatedCallCount);
    }
}
