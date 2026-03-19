namespace NickysManicurePedicure.Dtos.Requests.Admin;

public sealed class UpsertGalleryItemRequest
{
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? Category { get; init; }
    public string ImageUrl { get; init; } = string.Empty;
    public string? ThumbnailUrl { get; init; }
    public string AltText { get; init; } = string.Empty;
    public bool IsFeatured { get; init; }
    public string Status { get; init; } = "Published";
    public int DisplayOrder { get; init; }
}
