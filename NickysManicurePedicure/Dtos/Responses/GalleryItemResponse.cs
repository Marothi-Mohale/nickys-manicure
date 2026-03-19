namespace NickysManicurePedicure.Dtos.Responses;

public sealed class GalleryItemResponse
{
    public required int Id { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
    public string? Category { get; init; }
    public string? ImageUrl { get; init; }
    public string? ThumbnailUrl { get; init; }
    public string? AltText { get; init; }
    public required bool IsFeatured { get; init; }
    public required int DisplayOrder { get; init; }
    public required DateTime CreatedUtc { get; init; }
}
