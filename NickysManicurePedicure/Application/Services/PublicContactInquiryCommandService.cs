using NickysManicurePedicure.Application.Abstractions;
using NickysManicurePedicure.Data;
using NickysManicurePedicure.Dtos.Requests;
using NickysManicurePedicure.Dtos.Responses;
using NickysManicurePedicure.Models.Entities;

namespace NickysManicurePedicure.Application.Services;

public sealed class PublicContactInquiryCommandService(
    ApplicationDbContext dbContext,
    IContactInquiryNotificationService notificationService,
    ILogger<PublicContactInquiryCommandService> logger) : IContactInquiryPublicCommandService
{
    public async Task<ContactInquiryCreateResponse> CreateAsync(
        CreateContactInquiryDto request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var inquiry = new ContactInquiry
        {
            FullName = request.FullName.Trim(),
            Email = request.Email.Trim(),
            PhoneNumber = request.PhoneNumber.Trim(),
            Subject = string.IsNullOrWhiteSpace(request.Subject) ? null : request.Subject.Trim(),
            Message = request.Message.Trim(),
            SourcePage = string.IsNullOrWhiteSpace(request.SourcePage) ? "Api" : request.SourcePage.Trim()
        };

        dbContext.ContactInquiries.Add(inquiry);
        await dbContext.SaveChangesAsync(cancellationToken);

        var response = MapReadResponse(inquiry);

        logger.LogInformation(
            "Created contact inquiry {InquiryId} for {Email} from {SourcePage}.",
            inquiry.Id,
            inquiry.Email,
            inquiry.SourcePage);

        await notificationService.OnInquiryCreatedAsync(response, cancellationToken);

        return new ContactInquiryCreateResponse
        {
            InquiryId = inquiry.Id,
            Status = inquiry.Status.ToString(),
            Message = "Your inquiry has been received. We will get back to you as soon as possible.",
            DetailUrl = $"/api/contact-inquiries/{inquiry.Id}"
        };
    }

    private static ContactInquiryReadResponse MapReadResponse(ContactInquiry inquiry) => new()
    {
        Id = inquiry.Id,
        Status = inquiry.Status.ToString(),
        FullName = inquiry.FullName,
        Email = inquiry.Email,
        PhoneNumber = inquiry.PhoneNumber,
        Subject = inquiry.Subject,
        Message = inquiry.Message,
        SourcePage = inquiry.SourcePage,
        CreatedAtUtc = inquiry.CreatedAtUtc,
        UpdatedAtUtc = inquiry.UpdatedAtUtc
    };
}
