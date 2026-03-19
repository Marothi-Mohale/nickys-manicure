namespace NickysManicurePedicure.Dtos.Responses;

public sealed class ContactInquiryReadResponse
{
    public required int Id { get; init; }
    public required string Status { get; init; }
    public required string FullName { get; init; }
    public required string Email { get; init; }
    public required string PhoneNumber { get; init; }
    public string? Subject { get; init; }
    public required string Message { get; init; }
    public required string SourcePage { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; init; }
}
