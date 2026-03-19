using NickysManicurePedicure.Dtos.Responses;

namespace NickysManicurePedicure.Application.Abstractions;

public interface IBookingNotificationService
{
    Task OnBookingCreatedAsync(BookingReadResponse booking, CancellationToken cancellationToken);

    Task OnBookingStatusUpdatedAsync(BookingReadResponse booking, CancellationToken cancellationToken);
}
