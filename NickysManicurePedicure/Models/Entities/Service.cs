namespace NickysManicurePedicure.Models.Entities;

public class Service : AuditableEntity
{
    public int ServiceCategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DurationLabel { get; set; } = string.Empty;
    public string PriceFromLabel { get; set; } = string.Empty;
    public bool IsFeatured { get; set; }
    public ContentStatus Status { get; set; } = ContentStatus.Published;
    public int DisplayOrder { get; set; }

    public ServiceCategory? ServiceCategory { get; set; }
    public ICollection<BookingRequest> BookingRequests { get; set; } = [];
}
