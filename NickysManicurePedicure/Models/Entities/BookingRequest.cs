namespace NickysManicurePedicure.Models.Entities;

public class BookingRequest : AuditableEntity
{
    public int? ServiceId { get; set; }
    public string RequestedServiceName { get; set; } = string.Empty;
    public BookingRequestStatus Status { get; set; } = BookingRequestStatus.Pending;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateOnly PreferredDate { get; set; }
    public TimeOnly PreferredTime { get; set; }
    public string Message { get; set; } = string.Empty;
    public string SourcePage { get; set; } = string.Empty;
    public string? AdminNotes { get; set; }

    public Service? Service { get; set; }
}
