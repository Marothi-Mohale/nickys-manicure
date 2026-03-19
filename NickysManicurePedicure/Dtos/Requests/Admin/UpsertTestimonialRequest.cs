namespace NickysManicurePedicure.Dtos.Requests.Admin;

public sealed class UpsertTestimonialRequest
{
    public string ClientName { get; init; } = string.Empty;
    public string Quote { get; init; } = string.Empty;
    public int Rating { get; init; } = 5;
    public bool IsFeatured { get; init; }
    public bool IsApproved { get; init; } = true;
    public string Status { get; init; } = "Published";
    public int DisplayOrder { get; init; }
}
