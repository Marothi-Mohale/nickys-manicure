namespace NickysManicurePedicure.Models.Entities;

public class Testimonial : AuditableEntity
{
    public string ClientName { get; set; } = string.Empty;
    public string Highlight { get; set; } = string.Empty;
    public string Review { get; set; } = string.Empty;
    public ContentStatus Status { get; set; } = ContentStatus.Published;
    public bool IsFeatured { get; set; }
    public int DisplayOrder { get; set; }
}
