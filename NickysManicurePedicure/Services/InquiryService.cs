using NickysManicurePedicure.Data;
using NickysManicurePedicure.Models.Entities;
using NickysManicurePedicure.ViewModels;

namespace NickysManicurePedicure.Services;

public class InquiryService(
    ApplicationDbContext dbContext,
    ILogger<InquiryService> logger) : IInquiryService
{
    public async Task<SubmissionResult> CreateAsync(
        InquiryFormViewModel model,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);

        try
        {
            var contactInquiry = new ContactInquiry
            {
                FullName = model.FullName.Trim(),
                Email = model.Email.Trim(),
                PhoneNumber = model.PhoneNumber.Trim(),
                Subject = model.InquiryType == InquiryType.Booking
                    ? "Booking-related website inquiry"
                    : "General website inquiry",
                Message = model.Message.Trim(),
                SourcePage = model.SourcePage.Trim()
            };

            dbContext.ContactInquiries.Add(contactInquiry);
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Saved contact inquiry {ContactInquiryId} from {Email} via {SourcePage} at {CreatedAtUtc}.",
                contactInquiry.Id,
                contactInquiry.Email,
                contactInquiry.SourcePage,
                contactInquiry.CreatedAtUtc);

            return new SubmissionResult(
                true,
                "Your message has been received. We will get back to you as soon as possible.",
                contactInquiry.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to save inquiry from {Email}.", model.Email);
            return new SubmissionResult(false, "We could not send your message just now. Please call or WhatsApp us directly and we will assist you.");
        }
    }
}
