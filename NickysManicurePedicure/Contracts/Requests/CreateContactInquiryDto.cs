using System.ComponentModel.DataAnnotations;

namespace NickysManicurePedicure.Contracts.Requests;

public sealed class CreateContactInquiryDto
{
    [Required]
    [StringLength(120)]
    public string FullName { get; init; } = string.Empty;

    [Required]
    [StringLength(30)]
    [Phone]
    public string PhoneNumber { get; init; } = string.Empty;

    [Required]
    [StringLength(200)]
    [EmailAddress]
    public string Email { get; init; } = string.Empty;

    [Required]
    [StringLength(2000, MinimumLength = 10)]
    public string Message { get; init; } = string.Empty;

    [StringLength(30)]
    public string SourcePage { get; init; } = "Api";
}
