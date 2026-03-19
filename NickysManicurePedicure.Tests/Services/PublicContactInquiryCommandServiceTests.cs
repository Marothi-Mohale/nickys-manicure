using Microsoft.Extensions.Logging.Abstractions;
using NickysManicurePedicure.Application.Services;
using NickysManicurePedicure.Tests.Infrastructure;
using Xunit;

namespace NickysManicurePedicure.Tests.Services;

public sealed class PublicContactInquiryCommandServiceTests
{
    [Fact]
    public async Task CreateAsync_PersistsTrimmedInquiryAndNotifies()
    {
        await using var database = new TestSqliteDatabase();
        await using var dbContext = database.CreateDbContext();

        var notifications = new RecordingContactInquiryNotificationService();
        var sut = new PublicContactInquiryCommandService(dbContext, notifications, NullLogger<PublicContactInquiryCommandService>.Instance);

        var response = await sut.CreateAsync(
            TestDataFactory.CreateContactInquiryRequest(),
            CancellationToken.None);

        var persistedInquiry = await dbContext.ContactInquiries.FindAsync(response.InquiryId);

        Assert.NotNull(persistedInquiry);
        Assert.Equal("Client Name", persistedInquiry.FullName);
        Assert.Equal("client@example.com", persistedInquiry.Email);
        Assert.Equal("+27682518739", persistedInquiry.PhoneNumber);
        Assert.Equal("Booking question", persistedInquiry.Subject);
        Assert.Equal("I would like to know which premium manicure lasts the longest.", persistedInquiry.Message);
        Assert.Equal("ContactPage", persistedInquiry.SourcePage);
        Assert.Equal("New", response.Status);
        Assert.Equal(1, notifications.CreatedCallCount);
        Assert.NotNull(notifications.LastCreatedInquiry);
        Assert.Equal(response.InquiryId, notifications.LastCreatedInquiry!.Id);
    }
}
