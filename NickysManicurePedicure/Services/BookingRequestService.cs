using NickysManicurePedicure.Application.Abstractions;
using NickysManicurePedicure.Data;
using NickysManicurePedicure.Dtos.Requests;
using NickysManicurePedicure.ViewModels;

namespace NickysManicurePedicure.Services;

public class BookingRequestService(
    IBookingPublicCommandService bookingPublicCommandService,
    ILogger<BookingRequestService> logger) : IBookingRequestService
{
    public async Task<SubmissionResult> CreateAsync(
        BookingRequestViewModel model,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(model);

        try
        {
            var response = await bookingPublicCommandService.CreateAsync(
                new CreateBookingRequestDto
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    PreferredServiceName = model.PreferredService,
                    PreferredDate = model.PreferredDate,
                    PreferredTime = model.PreferredTime,
                    Message = model.Message,
                    SourcePage = model.SourcePage
                },
                cancellationToken);

            logger.LogInformation(
                "Saved booking request {BookingRequestId} from MVC booking form.",
                response.BookingId);

            return new SubmissionResult(
                true,
                response.Message,
                response.BookingId);
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
