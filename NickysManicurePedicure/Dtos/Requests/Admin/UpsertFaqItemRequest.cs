namespace NickysManicurePedicure.Dtos.Requests.Admin;

public sealed class UpsertFaqItemRequest
{
    public string Question { get; init; } = string.Empty;
    public string Answer { get; init; } = string.Empty;
    public bool IsActive { get; init; } = true;
    public string Status { get; init; } = "Published";
    public int DisplayOrder { get; init; }
}
