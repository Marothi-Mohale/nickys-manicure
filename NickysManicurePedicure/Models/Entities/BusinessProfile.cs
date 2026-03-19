namespace NickysManicurePedicure.Models.Entities;

public class BusinessProfile : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public string Tagline { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string PhoneHref { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string AddressLine1 { get; set; } = string.Empty;
    public string Suburb { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string WhatsAppHref { get; set; } = string.Empty;
    public string? InstagramHandle { get; set; }
    public string? BookingPolicy { get; set; }
    public string? AboutSummary { get; set; }

    public ICollection<BusinessHour> BusinessHours { get; set; } = [];
}
