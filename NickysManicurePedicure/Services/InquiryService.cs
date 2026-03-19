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
            var inquiry = new Inquiry
            {
                InquiryType = model.InquiryType,
                FullName = model.FullName.Trim(),
                Email = model.Email.Trim(),
                PhoneNumber = model.PhoneNumber.Trim(),
                Message = model.Message.Trim(),
                SourcePage = model.SourcePage.Trim()
            };

            dbContext.Inquiries.Add(inquiry);
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Saved {InquiryType} inquiry {InquiryId} from {Email} via {SourcePage} at {CreatedUtc}.",
                inquiry.InquiryType,
                inquiry.Id,
                inquiry.Email,
                inquiry.SourcePage,
                inquiry.CreatedUtc);

            var message = inquiry.InquiryType == InquiryType.Booking
                ? "Your appointment request has been received. We will contact you shortly to confirm the details."
                : "Your message has been received. We will get back to you as soon as possible.";

            return new SubmissionResult(true, message, inquiry.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to save inquiry from {Email}.", model.Email);
            return new SubmissionResult(false, "We could not send your message just now. Please call or WhatsApp us directly and we will assist you.");
        }
    }
}
