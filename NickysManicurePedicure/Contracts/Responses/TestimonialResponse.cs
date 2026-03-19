namespace NickysManicurePedicure.Contracts.Responses;

public sealed class TestimonialResponse
{
    public required int Id { get; init; }
    public required string ClientName { get; init; }
    public required string Highlight { get; init; }
    public required string Review { get; init; }
    public required int DisplayOrder { get; init; }
}
