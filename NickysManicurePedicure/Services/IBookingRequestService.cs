using NickysManicurePedicure.ViewModels;

namespace NickysManicurePedicure.Services;

public interface IBookingRequestService
{
    Task<SubmissionResult> CreateAsync(BookingRequestViewModel model, CancellationToken cancellationToken = default);
}
