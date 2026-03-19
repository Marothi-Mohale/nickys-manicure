namespace NickysManicurePedicure.Dtos.Requests;

public sealed class CreateContactInquiryDto
{
    public string FullName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string PhoneNumber { get; init; } = string.Empty;

    public string? Subject { get; init; }

    public string Message { get; init; } = string.Empty;

    public string SourcePage { get; init; } = "Api";
}
