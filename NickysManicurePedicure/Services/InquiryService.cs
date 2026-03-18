using NickysManicurePedicure.Data;
using NickysManicurePedicure.Models.Entities;
using NickysManicurePedicure.ViewModels;

namespace NickysManicurePedicure.Services;

public class InquiryService(
    ApplicationDbContext dbContext,
    ILogger<InquiryService> logger) : IInquiryService
{
    public async Task<(bool Success, string Message)> CreateAsync(
        InquiryFormViewModel model,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var inquiry = new Inquiry
            {
                InquiryType = model.InquiryType,
                FullName = model.FullName.Trim(),
                Email = model.Email.Trim(),
                PhoneNumber = model.PhoneNumber.Trim(),
                PreferredDate = model.PreferredDate,
                ServiceInterest = string.IsNullOrWhiteSpace(model.ServiceInterest) ? null : model.ServiceInterest.Trim(),
                Message = model.Message.Trim()
            };

            dbContext.Inquiries.Add(inquiry);
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Saved {InquiryType} inquiry from {Email} at {CreatedUtc}.",
                inquiry.InquiryType,
                inquiry.Email,
                inquiry.CreatedUtc);

            var message = inquiry.InquiryType == InquiryType.Booking
                ? "Your appointment request has been received. We will contact you shortly to confirm the details."
                : "Your message has been received. We will get back to you as soon as possible.";

            return (true, message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to save inquiry from {Email}.", model.Email);
            return (false, "We could not send your request just now. Please call or WhatsApp us directly and we will assist you.");
        }
    }
}
