namespace NickysManicurePedicure.Models.Entities;

public class ContactInquiry : AuditableEntity
{
    public ContactInquiryStatus Status { get; set; } = ContactInquiryStatus.New;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Subject { get; set; }
    public string Message { get; set; } = string.Empty;
    public string SourcePage { get; set; } = string.Empty;
    public string? AdminNotes { get; set; }
}
