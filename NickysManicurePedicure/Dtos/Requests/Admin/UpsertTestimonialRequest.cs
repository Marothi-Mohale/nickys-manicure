namespace NickysManicurePedicure.Dtos.Requests.Admin;

public sealed class UpsertTestimonialRequest
{
    public string ClientName { get; init; } = string.Empty;
    public string Highlight { get; init; } = string.Empty;
    public string Review { get; init; } = string.Empty;
    public bool IsFeatured { get; init; }
    public string Status { get; init; } = "Published";
    public int DisplayOrder { get; init; }
}
