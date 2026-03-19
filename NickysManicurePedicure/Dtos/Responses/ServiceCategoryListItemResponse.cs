namespace NickysManicurePedicure.Dtos.Responses;

public sealed class ServiceCategoryListItemResponse
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string Slug { get; init; }
    public string? Description { get; init; }
    public required int DisplayOrder { get; init; }
    public required int ServiceCount { get; init; }
}
