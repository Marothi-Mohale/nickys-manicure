namespace NickysManicurePedicure.Dtos.Requests;

public sealed class CreateBookingRequestDto
{
    public string FullName { get; init; } = string.Empty;

    public string PhoneNumber { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public int? PreferredServiceId { get; init; }

    public string? PreferredServiceName { get; init; }

    public DateOnly? PreferredDate { get; init; }

    public TimeOnly? PreferredTime { get; init; }

    public string Message { get; init; } = string.Empty;

    public string SourcePage { get; init; } = "Api";
}
