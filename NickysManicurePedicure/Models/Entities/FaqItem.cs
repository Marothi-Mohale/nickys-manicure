namespace NickysManicurePedicure.Models.Entities;

public class FaqItem : AuditableEntity
{
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public ContentStatus Status { get; set; } = ContentStatus.Published;
    public int DisplayOrder { get; set; }
}
