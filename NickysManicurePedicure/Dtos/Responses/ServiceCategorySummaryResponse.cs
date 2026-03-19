namespace NickysManicurePedicure.Dtos.Responses;

public sealed class ServiceCategorySummaryResponse
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Slug { get; init; }
}
