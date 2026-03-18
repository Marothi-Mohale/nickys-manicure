using NickysManicurePedicure.Models.Options;

namespace NickysManicurePedicure.ViewModels;

public class GalleryPageViewModel
{
    public required BusinessProfileOptions Business { get; init; }
    public required IReadOnlyCollection<GalleryItemViewModel> GalleryItems { get; init; }
}

public class GalleryItemViewModel
{
    public required string Title { get; init; }
    public required string Description { get; init; }
}
