using NickysManicurePedicure.Data;
using NickysManicurePedicure.Models.Entities;
using NickysManicurePedicure.ViewModels;

namespace NickysManicurePedicure.Services;

public class BookingRequestService(
    ApplicationDbContext dbContext,
    ILogger<BookingRequestService> logger) : IBookingRequestService
{
    public async Task<SubmissionResult> CreateAsync(
        BookingRequestViewModel model,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var inquiry = new Inquiry
            {
                InquiryType = InquiryType.Booking,
                FullName = model.FullName.Trim(),
                Email = model.Email.Trim(),
                PhoneNumber = model.PhoneNumber.Trim(),
                PreferredService = model.PreferredService.Trim(),
                PreferredDate = model.PreferredDate,
                PreferredTime = model.PreferredTime,
                Message = model.Message.Trim(),
                SourcePage = model.SourcePage.Trim()
            };

            dbContext.Inquiries.Add(inquiry);
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Saved booking request {InquiryId} for {Email} from {SourcePage} on {PreferredDate} at {PreferredTime}.",
                inquiry.Id,
                inquiry.Email,
                inquiry.SourcePage,
                inquiry.PreferredDate,
                inquiry.PreferredTime);

            return new SubmissionResult(
                true,
                "Your appointment request is in. We will confirm availability and follow up with you shortly.",
                inquiry.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Failed to save booking request from {Email} for {PreferredDate} at {PreferredTime}.",
                model.Email,
                model.PreferredDate,
                model.PreferredTime);

            return new SubmissionResult(
                false,
                "We could not save your appointment request just now. Please try again, or call or WhatsApp us for immediate help.");
        }
    }
}
