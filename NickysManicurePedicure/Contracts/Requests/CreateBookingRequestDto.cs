using System.ComponentModel.DataAnnotations;

namespace NickysManicurePedicure.Contracts.Requests;

public sealed class CreateBookingRequestDto : IValidatableObject
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
    public DateOnly? PreferredDate { get; init; }

    [Required]
    public TimeOnly? PreferredTime { get; init; }

    [Required]
    [StringLength(2000, MinimumLength = 10)]
    public string Message { get; init; } = string.Empty;

    [StringLength(30)]
    public string SourcePage { get; init; } = "Api";

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (PreferredDate is not null && PreferredDate < DateOnly.FromDateTime(DateTime.UtcNow.Date))
        {
            yield return new ValidationResult(
                "PreferredDate must be today or later.",
                [nameof(PreferredDate)]);
        }
    }
}
