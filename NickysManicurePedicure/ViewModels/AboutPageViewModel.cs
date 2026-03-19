using NickysManicurePedicure.Models.Entities;
using NickysManicurePedicure.Models.Options;

namespace NickysManicurePedicure.ViewModels;

public class AboutPageViewModel
{
    public required BusinessProfileOptions Business { get; init; }
    public required IReadOnlyCollection<Testimonial> Testimonials { get; init; }
    public required SalonImageViewModel SignatureImage { get; init; }
}
