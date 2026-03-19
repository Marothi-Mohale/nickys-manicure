namespace NickysManicurePedicure.Dtos.Responses;

public sealed class ServiceListItemResponse
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Slug { get; init; }
    public required string Description { get; init; }
    public required string DurationLabel { get; init; }
    public required string PriceFromLabel { get; init; }
    public required bool IsFeatured { get; init; }
    public required int DisplayOrder { get; init; }
    public required ServiceCategorySummaryResponse Category { get; init; }
}
