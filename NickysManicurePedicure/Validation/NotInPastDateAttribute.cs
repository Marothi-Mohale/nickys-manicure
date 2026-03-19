using System.ComponentModel.DataAnnotations;

namespace NickysManicurePedicure.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public sealed class NotInPastDateAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
        {
            return ValidationResult.Success;
        }

        if (value is DateOnly date && date < DateOnly.FromDateTime(DateTime.UtcNow.Date))
        {
            var memberName = validationContext.MemberName ?? validationContext.DisplayName;
            return new ValidationResult(
                ErrorMessage ?? $"{memberName} must be today or later.",
                memberNames: memberName is null ? null : [memberName]);
        }

        return ValidationResult.Success;
    }
}
