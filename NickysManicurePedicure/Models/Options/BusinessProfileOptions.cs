namespace NickysManicurePedicure.Models.Options;

public class BusinessProfileOptions
{
    public const string SectionName = "BusinessProfile";

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
    public string InstagramHandle { get; set; } = string.Empty;
    public string WhatsAppHref { get; set; } = string.Empty;
    public List<string> BusinessHours { get; set; } = [];

    public string FullAddress => $"{AddressLine1}, {Suburb}, {City}, {Region} {PostalCode}";
}
