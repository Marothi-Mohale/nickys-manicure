namespace NickysManicurePedicure.Dtos.Requests;

public sealed class UpdateBookingStatusDto
{
    public string Status { get; init; } = string.Empty;
    public string? AdminNotes { get; init; }
}
