using FluentValidation;
using NickysManicurePedicure.Dtos.Requests.Admin;

namespace NickysManicurePedicure.Validation;

public sealed class UpsertBusinessProfileRequestValidator : AbstractValidator<UpsertBusinessProfileRequest>
{
    public UpsertBusinessProfileRequestValidator()
    {
        RuleFor(x => x.BusinessName)
            .TrimmedRequiredText(160, "Business name");

        RuleFor(x => x.Tagline)
            .TrimmedRequiredText(260, "Tagline");

        RuleFor(x => x.Description)
            .MaximumLength(1500).WithMessage("Description must be 1500 characters or fewer.");

        RuleFor(x => x.PhoneNumber)
            .SalonPhone()
            .MaximumLength(30).WithMessage("Phone number must be 30 characters or fewer.");

        RuleFor(x => x.Email)
            .TrimmedRequiredText(200, "Email")
            .EmailAddress().WithMessage("Email must be a valid email address.");

        RuleFor(x => x.AddressLine1)
            .TrimmedRequiredText(160, "Address line 1");

        RuleFor(x => x.City)
            .TrimmedRequiredText(120, "City");

        RuleFor(x => x.Region)
            .TrimmedRequiredText(120, "Region");

        RuleFor(x => x.PostalCode)
            .TrimmedRequiredText(20, "Postal code");

        RuleFor(x => x.InstagramHandle)
            .MaximumLength(120).WithMessage("Instagram handle must be 120 characters or fewer.");

        RuleFor(x => x.YearsOfExperience)
            .InclusiveBetween(0, 100).WithMessage("Years of experience must be between 0 and 100.");

        RuleFor(x => x.HeroHeadline)
            .MaximumLength(220).WithMessage("Hero headline must be 220 characters or fewer.");

        RuleFor(x => x.HeroSubheadline)
            .MaximumLength(600).WithMessage("Hero subheadline must be 600 characters or fewer.");

        RuleFor(x => x.BusinessHours)
            .NotNull().WithMessage("Business hours are required.");

        RuleFor(x => x.BusinessHours)
            .Must(hours => hours.Count <= 7)
            .When(x => x.BusinessHours is not null)
            .WithMessage("Business hours cannot contain more than 7 entries.");

        RuleFor(x => x.BusinessHours)
            .Must(HaveUniqueDaysOfWeek)
            .When(x => x.BusinessHours is not null)
            .WithMessage("Business hours cannot contain duplicate days of the week.");

        RuleForEach(x => x.BusinessHours)
            .SetValidator(new UpsertBusinessHourRequestValidator());
    }

    private static bool HaveUniqueDaysOfWeek(IReadOnlyCollection<UpsertBusinessHourRequest> businessHours) =>
        businessHours
            .Select(hour => hour.DayOfWeek.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Count() == businessHours.Count;
}
