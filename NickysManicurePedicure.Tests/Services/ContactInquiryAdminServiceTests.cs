using Microsoft.Extensions.Logging.Abstractions;
using NickysManicurePedicure.Application.Services;
using NickysManicurePedicure.Dtos.Requests;
using NickysManicurePedicure.Models.Entities;
using NickysManicurePedicure.Tests.Infrastructure;
using Xunit;

namespace NickysManicurePedicure.Tests.Services;

public sealed class ContactInquiryAdminServiceTests
{
    [Fact]
    public async Task GetInquiriesAsync_FiltersSortsAndPaginatesResults()
    {
        await using var database = new TestSqliteDatabase();
        await using var dbContext = database.CreateDbContext();

        dbContext.ContactInquiries.AddRange(
            TestDataFactory.CreateContactInquiryEntity(
                fullName: "Ariana Client",
                email: "ariana@example.com",
                subject: "Luxury manicure",
                status: ContactInquiryStatus.Responded,
                createdAtUtc: DateTime.UtcNow.AddHours(-3)),
            TestDataFactory.CreateContactInquiryEntity(
                fullName: "Bianca Client",
                email: "bianca@example.com",
                subject: "Pedicure question",
                status: ContactInquiryStatus.New,
                createdAtUtc: DateTime.UtcNow.AddHours(-2)),
            TestDataFactory.CreateContactInquiryEntity(
                fullName: "Ariana Bridal",
                email: "ariana-bridal@example.com",
                subject: "Bridal booking",
                status: ContactInquiryStatus.Responded,
                createdAtUtc: DateTime.UtcNow.AddHours(-1)));
        await dbContext.SaveChangesAsync();

        var sut = new ContactInquiryAdminService(dbContext, new RecordingContactInquiryNotificationService(), NullLogger<ContactInquiryAdminService>.Instance);

        var result = await sut.GetInquiriesAsync(
            new ContactInquiryQueryParameters
            {
                Search = "Ariana",
                Status = "Responded",
                SortBy = "fullName",
                SortDirection = "asc",
                Page = 1,
                PageSize = 1
            },
            CancellationToken.None);

        Assert.Single(result.Items);
        Assert.Equal("Ariana Bridal", result.Items.First().FullName);
        Assert.Equal(2, result.Pagination.TotalCount);
        Assert.Equal(2, result.Pagination.TotalPages);
        Assert.True(result.Pagination.HasNextPage);
    }

    [Fact]
    public async Task UpdateStatusAsync_UpdatesInquiryAndNotifies()
    {
        await using var database = new TestSqliteDatabase();
        await using var dbContext = database.CreateDbContext();

        var inquiry = TestDataFactory.CreateContactInquiryEntity(
            fullName: "Status Inquiry",
            email: "status-inquiry@example.com",
            subject: "Booking question",
            status: ContactInquiryStatus.New,
            createdAtUtc: DateTime.UtcNow.AddHours(-1));

        dbContext.ContactInquiries.Add(inquiry);
        await dbContext.SaveChangesAsync();

        var notifications = new RecordingContactInquiryNotificationService();
        var sut = new ContactInquiryAdminService(dbContext, notifications, NullLogger<ContactInquiryAdminService>.Instance);

        var response = await sut.UpdateStatusAsync(
            inquiry.Id,
            new UpdateContactInquiryStatusDto
            {
                Status = "Responded",
                AdminNotes = "  Replied via email.  "
            },
            CancellationToken.None);

        Assert.NotNull(response);
        Assert.Equal("Responded", response!.Status);
        Assert.Equal(1, notifications.StatusUpdatedCallCount);
        Assert.Equal("Responded", notifications.LastStatusUpdatedInquiry!.Status);
    }
}
