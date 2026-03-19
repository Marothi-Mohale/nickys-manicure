namespace NickysManicurePedicure.Contracts.Responses;

public sealed class SalonServiceResponse
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string Duration { get; init; }
    public required string PriceFrom { get; init; }
    public required bool IsFeatured { get; init; }
    public required int DisplayOrder { get; init; }
}
