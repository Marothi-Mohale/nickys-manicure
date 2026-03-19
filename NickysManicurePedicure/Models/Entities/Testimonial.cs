namespace NickysManicurePedicure.Models.Entities;

public class Testimonial : AuditableEntity
{
    public string ClientName { get; set; } = string.Empty;
    public string Quote { get; set; } = string.Empty;
    public int Rating { get; set; } = 5;
    public ContentStatus Status { get; set; } = ContentStatus.Published;
    public bool IsFeatured { get; set; }
    public bool IsApproved { get; set; } = true;
    public int DisplayOrder { get; set; }
}
