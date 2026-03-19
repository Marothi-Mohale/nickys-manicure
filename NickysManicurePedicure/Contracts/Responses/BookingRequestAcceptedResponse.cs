namespace NickysManicurePedicure.Contracts.Responses;

public sealed class BookingRequestAcceptedResponse
{
    public required int InquiryId { get; init; }
    public required string Message { get; init; }
}
