using System.ComponentModel.DataAnnotations;

namespace NickysManicurePedicure.Validation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public sealed class AllowedStringValuesAttribute(params string[] allowedValues) : ValidationAttribute
{
    private readonly HashSet<string> _allowedValues = allowedValues
        .Where(value => !string.IsNullOrWhiteSpace(value))
        .ToHashSet(StringComparer.OrdinalIgnoreCase);

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
        {
            return ValidationResult.Success;
        }

        if (value is string stringValue && _allowedValues.Contains(stringValue))
        {
            return ValidationResult.Success;
        }

        var memberName = validationContext.MemberName ?? validationContext.DisplayName;
        var joinedValues = string.Join(", ", _allowedValues.OrderBy(value => value));

        return new ValidationResult(
            ErrorMessage ?? $"{memberName} must be one of: {joinedValues}.",
            memberNames: memberName is null ? null : [memberName]);
    }
}
