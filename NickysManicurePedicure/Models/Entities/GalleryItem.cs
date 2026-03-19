namespace NickysManicurePedicure.Models.Entities;

public class GalleryItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? ImageUrl { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? AltText { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsPublished { get; set; } = true;
    public int DisplayOrder { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
}
