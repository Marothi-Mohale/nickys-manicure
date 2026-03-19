using Microsoft.EntityFrameworkCore;
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
        ArgumentNullException.ThrowIfNull(model);

        try
        {
            var requestedServiceName = model.PreferredService.Trim();
            var matchedService = await dbContext.Services
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Name == requestedServiceName,
                    cancellationToken);

            var bookingRequest = new BookingRequest
            {
                FullName = model.FullName.Trim(),
                Email = model.Email.Trim(),
                PhoneNumber = model.PhoneNumber.Trim(),
                RequestedServiceName = requestedServiceName,
                ServiceId = matchedService?.Id,
                PreferredDate = model.PreferredDate!.Value,
                PreferredTime = model.PreferredTime!.Value,
                Message = model.Message.Trim(),
                SourcePage = model.SourcePage.Trim()
            };

            dbContext.BookingRequests.Add(bookingRequest);
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Saved booking request {BookingRequestId} for {Email} from {SourcePage} on {PreferredDate} at {PreferredTime}.",
                bookingRequest.Id,
                bookingRequest.Email,
                bookingRequest.SourcePage,
                bookingRequest.PreferredDate,
                bookingRequest.PreferredTime);

            return new SubmissionResult(
                true,
                "Your appointment request is in. We will confirm availability and follow up with you shortly.",
                bookingRequest.Id);
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
