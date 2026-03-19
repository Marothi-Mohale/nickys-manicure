namespace NickysManicurePedicure.Models.Entities;

public class GalleryItem : AuditableEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? ThumbnailUrl { get; set; }
    public string AltText { get; set; } = string.Empty;
    public bool IsFeatured { get; set; }
    public ContentStatus Status { get; set; } = ContentStatus.Published;
    public int DisplayOrder { get; set; }
}
