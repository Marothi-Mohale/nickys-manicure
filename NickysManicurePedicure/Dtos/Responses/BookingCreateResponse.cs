namespace NickysManicurePedicure.Dtos.Responses;

public sealed class BookingCreateResponse
{
    public required int BookingId { get; init; }
    public required string Status { get; init; }
    public required string Message { get; init; }
}
