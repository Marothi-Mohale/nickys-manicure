using NickysManicurePedicure.Models.Entities;
using NickysManicurePedicure.Models.Options;

namespace NickysManicurePedicure.ViewModels;

public class HomePageViewModel
{
    public required BusinessProfileOptions Business { get; init; }
    public required IReadOnlyCollection<Service> FeaturedServices { get; init; }
    public required IReadOnlyCollection<Testimonial> Testimonials { get; init; }
    public required IReadOnlyCollection<FaqItem> FaqItems { get; init; }
    public required SalonImageViewModel HeroImage { get; init; }
    public required IReadOnlyList<SalonImageViewModel> PreviewImages { get; init; }
    public required BookingRequestViewModel BookingForm { get; init; }
}
