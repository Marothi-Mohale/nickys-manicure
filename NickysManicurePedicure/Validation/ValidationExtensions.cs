using FluentValidation;

namespace NickysManicurePedicure.Validation;

public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, string> TrimmedRequiredText<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        int maxLength,
        string fieldName)
    {
        return ruleBuilder
            .NotEmpty().WithMessage($"{fieldName} is required.")
            .Must(value => !string.IsNullOrWhiteSpace(value)).WithMessage($"{fieldName} is required.")
            .MaximumLength(maxLength).WithMessage($"{fieldName} must be {maxLength} characters or fewer.");
    }

    public static IRuleBuilderOptions<T, string> OptionalMaxLength<T>(
        this IRuleBuilder<T, string> ruleBuilder,
        int maxLength,
        string fieldName)
    {
        return ruleBuilder
            .MaximumLength(maxLength)
            .WithMessage($"{fieldName} must be {maxLength} characters or fewer.");
    }

    public static IRuleBuilderOptions<T, string> SalonPhone<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(ValidationConstants.PhoneSanityPattern)
            .WithMessage("Phone number must contain a sensible mix of digits and separators.")
            .Must(HasSensibleDigitCount)
            .WithMessage("Phone number must contain between 7 and 15 digits.");
    }

    private static bool HasSensibleDigitCount(string? phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            return false;
        }

        var digits = phoneNumber.Count(char.IsDigit);
        return digits is >= 7 and <= 15;
    }
}
