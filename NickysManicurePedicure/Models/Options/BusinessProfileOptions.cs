using System.ComponentModel.DataAnnotations;

namespace NickysManicurePedicure.Models.Options;

public class BusinessProfileOptions
{
    public const string SectionName = "BusinessProfile";

    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Tagline { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    [Required]
    public string Phone { get; set; } = string.Empty;
    [Required]
    public string PhoneHref { get; set; } = string.Empty;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string AddressLine1 { get; set; } = string.Empty;
    [Required]
    public string Suburb { get; set; } = string.Empty;
    [Required]
    public string City { get; set; } = string.Empty;
    [Required]
    public string Region { get; set; } = string.Empty;
    [Required]
    public string PostalCode { get; set; } = string.Empty;
    public string InstagramHandle { get; set; } = string.Empty;
    [Range(0, 100)]
    public int YearsOfExperience { get; set; }
    public string HeroHeadline { get; set; } = string.Empty;
    public string HeroSubheadline { get; set; } = string.Empty;
    [Required]
    public string WhatsAppHref { get; set; } = string.Empty;
    [MinLength(1)]
    public List<string> BusinessHours { get; set; } = [];

    public string FullAddress => $"{AddressLine1}, {Suburb}, {City}, {Region} {PostalCode}";
}
