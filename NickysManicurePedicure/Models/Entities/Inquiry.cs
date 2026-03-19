namespace NickysManicurePedicure.Models.Entities;

public class Inquiry
{
    public int Id { get; set; }
    public InquiryType InquiryType { get; set; }
    public InquiryStatus Status { get; set; } = InquiryStatus.New;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? PreferredService { get; set; }
    public DateOnly? PreferredDate { get; set; }
    public TimeOnly? PreferredTime { get; set; }
    public string Message { get; set; } = string.Empty;
    public string SourcePage { get; set; } = string.Empty;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
}
