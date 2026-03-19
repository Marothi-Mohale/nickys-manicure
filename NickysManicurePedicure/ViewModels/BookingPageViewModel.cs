using NickysManicurePedicure.Models.Entities;
using NickysManicurePedicure.Models.Options;

namespace NickysManicurePedicure.ViewModels;

public class BookingPageViewModel
{
    public required BusinessProfileOptions Business { get; init; }
    public required IReadOnlyCollection<SalonService> Services { get; init; }
    public required BookingRequestViewModel BookingForm { get; init; }
}
