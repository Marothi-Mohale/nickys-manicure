using System.ComponentModel.DataAnnotations;
using NickysManicurePedicure.Validation;

namespace NickysManicurePedicure.Dtos.Requests;

public sealed class CreateBookingRequestDto
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
    [StringLength(120)]
    public string PreferredService { get; init; } = string.Empty;

    [Required]
    [NotInPastDate]
    public DateOnly? PreferredDate { get; init; }

    [Required]
    public TimeOnly? PreferredTime { get; init; }

    [Required]
    [StringLength(2000, MinimumLength = 10)]
    public string Message { get; init; } = string.Empty;

    [StringLength(30)]
    public string SourcePage { get; init; } = "Api";
}
