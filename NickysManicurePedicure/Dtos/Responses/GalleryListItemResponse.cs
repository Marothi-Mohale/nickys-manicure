namespace NickysManicurePedicure.Dtos.Responses;

public sealed class GalleryListItemResponse
{
    public required int Id { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
    public string? Category { get; init; }
    public required string ImageUrl { get; init; }
    public required string AltText { get; init; }
    public required bool IsFeatured { get; init; }
    public required int DisplayOrder { get; init; }
}
