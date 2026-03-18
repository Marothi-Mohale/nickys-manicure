using NickysManicurePedicure.Models.Entities;
using NickysManicurePedicure.Models.Options;

namespace NickysManicurePedicure.ViewModels;

public class HomePageViewModel
{
    public required BusinessProfileOptions Business { get; init; }
    public required IReadOnlyCollection<SalonService> FeaturedServices { get; init; }
    public required IReadOnlyCollection<Testimonial> Testimonials { get; init; }
    public required IReadOnlyCollection<FaqItem> FaqItems { get; init; }
    public required InquiryFormViewModel InquiryForm { get; init; }
}
