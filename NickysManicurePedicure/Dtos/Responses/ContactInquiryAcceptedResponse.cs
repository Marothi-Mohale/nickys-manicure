namespace NickysManicurePedicure.Dtos.Responses;

public sealed class ContactInquiryAcceptedResponse
{
    public required int ContactInquiryId { get; init; }
    public required string Message { get; init; }
}
