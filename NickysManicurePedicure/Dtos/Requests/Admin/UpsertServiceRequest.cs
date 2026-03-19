namespace NickysManicurePedicure.Dtos.Requests.Admin;

public sealed class UpsertServiceRequest
{
    public int ServiceCategoryId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string DurationLabel { get; init; } = string.Empty;
    public string PriceFromLabel { get; init; } = string.Empty;
    public bool IsFeatured { get; init; }
    public string Status { get; init; } = "Published";
    public int DisplayOrder { get; init; }
}
