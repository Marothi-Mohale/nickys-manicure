using NickysManicurePedicure.Application.Abstractions;
using NickysManicurePedicure.Dtos.Requests;
using NickysManicurePedicure.Dtos.Responses;
using NickysManicurePedicure.Data;
using NickysManicurePedicure.Models.Entities;

namespace NickysManicurePedicure.Application.Services;

public sealed class InquiryApiCommandService(
    ApplicationDbContext dbContext,
    ILogger<InquiryApiCommandService> logger) : IInquiryApiCommandService
{
    public async Task<ContactInquiryCreateResponse> CreateContactInquiryAsync(
        CreateContactInquiryDto request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var contactInquiry = new ContactInquiry
        {
            FullName = request.FullName.Trim(),
            Email = request.Email.Trim(),
            PhoneNumber = request.PhoneNumber.Trim(),
            Subject = "General website inquiry",
            Message = request.Message.Trim(),
            SourcePage = string.IsNullOrWhiteSpace(request.SourcePage) ? "Api" : request.SourcePage.Trim()
        };

        dbContext.ContactInquiries.Add(contactInquiry);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Created contact inquiry {ContactInquiryId} for {Email} from API source {SourcePage}.",
            contactInquiry.Id,
            contactInquiry.Email,
            contactInquiry.SourcePage);

        return new ContactInquiryCreateResponse
        {
            InquiryId = contactInquiry.Id,
            Status = contactInquiry.Status.ToString(),
            Message = "Your inquiry has been received. We will get back to you as soon as possible."
        };
    }
}
