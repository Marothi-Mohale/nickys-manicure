namespace NickysManicurePedicure.Dtos.Responses;

public sealed class BookingRequestAcceptedResponse
{
    public required int BookingRequestId { get; init; }
    public required string Message { get; init; }
}
