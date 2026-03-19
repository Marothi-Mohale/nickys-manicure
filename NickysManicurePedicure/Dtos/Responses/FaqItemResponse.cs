namespace NickysManicurePedicure.Dtos.Responses;

public sealed class FaqItemResponse
{
    public required int Id { get; init; }
    public required string Question { get; init; }
    public required string Answer { get; init; }
    public required int DisplayOrder { get; init; }
}
