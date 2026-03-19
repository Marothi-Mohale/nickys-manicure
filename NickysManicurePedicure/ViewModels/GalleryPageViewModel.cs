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
    public required string Category { get; init; }
    public required string ImageUrl { get; init; }
    public required string AltText { get; init; }
    public bool IsFeatured { get; init; }
}
