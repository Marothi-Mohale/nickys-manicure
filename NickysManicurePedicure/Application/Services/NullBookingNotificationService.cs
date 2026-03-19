using NickysManicurePedicure.Application.Abstractions;
using NickysManicurePedicure.Dtos.Responses;

namespace NickysManicurePedicure.Application.Services;

public sealed class NullBookingNotificationService : IBookingNotificationService
{
    public Task OnBookingCreatedAsync(BookingReadResponse booking, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task OnBookingStatusUpdatedAsync(BookingReadResponse booking, CancellationToken cancellationToken) => Task.CompletedTask;
}
