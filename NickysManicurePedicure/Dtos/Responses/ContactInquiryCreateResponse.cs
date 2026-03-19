namespace NickysManicurePedicure.Dtos.Responses;

public sealed class ContactInquiryCreateResponse
{
    public required int InquiryId { get; init; }
    public required string Status { get; init; }
    public required string Message { get; init; }
}
