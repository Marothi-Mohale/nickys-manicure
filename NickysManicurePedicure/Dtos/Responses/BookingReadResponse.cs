namespace NickysManicurePedicure.Dtos.Responses;

public sealed class BookingReadResponse
{
    public required int Id { get; init; }
    public int? PreferredServiceId { get; init; }
    public required string PreferredServiceName { get; init; }
    public required string Status { get; init; }
    public required string FullName { get; init; }
    public required string Email { get; init; }
    public required string PhoneNumber { get; init; }
    public required DateOnly PreferredDate { get; init; }
    public required TimeOnly PreferredTime { get; init; }
    public required string Message { get; init; }
    public required string SourcePage { get; init; }
    public string? AdminNotes { get; init; }
    public required DateTime CreatedAtUtc { get; init; }
    public required DateTime UpdatedAtUtc { get; init; }
}
