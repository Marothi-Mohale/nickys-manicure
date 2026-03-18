using NickysManicurePedicure.Models.Options;

namespace NickysManicurePedicure.ViewModels;

public class ContactPageViewModel
{
    public required BusinessProfileOptions Business { get; init; }
    public required InquiryFormViewModel InquiryForm { get; init; }
}
