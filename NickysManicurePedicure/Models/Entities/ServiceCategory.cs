namespace NickysManicurePedicure.Models.Entities;

public class ServiceCategory : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public ContentStatus Status { get; set; } = ContentStatus.Published;
    public int DisplayOrder { get; set; }

    public ICollection<Service> Services { get; set; } = [];
}
