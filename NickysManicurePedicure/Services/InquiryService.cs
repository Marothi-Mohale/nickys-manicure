using NickysManicurePedicure.Application.Abstractions;
using NickysManicurePedicure.Data;
using NickysManicurePedicure.Dtos.Requests;
using NickysManicurePedicure.Models.Entities;
using NickysManicurePedicure.ViewModels;

namespace NickysManicurePedicure.Services;

public class InquiryService(
    IContactInquiryPublicCommandService contactInquiryPublicCommandService,
    ILogger<InquiryService> logger) : IInquiryService
{
    public async Task<SubmissionResult> CreateAsync(
        InquiryFormViewModel model,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);

        try
        {
            var response = await contactInquiryPublicCommandService.CreateAsync(
                new CreateContactInquiryDto
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Subject = model.InquiryType == InquiryType.Booking
                        ? "Booking-related website inquiry"
                        : "General website inquiry",
                    Message = model.Message,
                    SourcePage = model.SourcePage
                },
                cancellationToken);

            logger.LogInformation(
                "Saved contact inquiry {ContactInquiryId} from MVC contact form.",
                response.InquiryId);

            return new SubmissionResult(
                true,
                response.Message,
                response.InquiryId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to save inquiry from {Email}.", model.Email);
            return new SubmissionResult(false, "We could not send your message just now. Please call or WhatsApp us directly and we will assist you.");
        }
    }
}
