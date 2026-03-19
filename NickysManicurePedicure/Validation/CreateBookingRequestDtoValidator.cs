using FluentValidation;
using NickysManicurePedicure.Dtos.Requests;

namespace NickysManicurePedicure.Validation;

public sealed class CreateBookingRequestDtoValidator : AbstractValidator<CreateBookingRequestDto>
{
    public CreateBookingRequestDtoValidator()
    {
        RuleFor(x => x.FullName)
            .TrimmedRequiredText(120, "Full name");

        RuleFor(x => x.PhoneNumber)
            .SalonPhone()
            .MaximumLength(30).WithMessage("Phone number must be 30 characters or fewer.");

        RuleFor(x => x.Email)
            .TrimmedRequiredText(200, "Email")
            .EmailAddress().WithMessage("Email must be a valid email address.");

        RuleFor(x => x.PreferredService)
            .TrimmedRequiredText(120, "Preferred service");

        RuleFor(x => x.PreferredDate)
            .NotNull().WithMessage("Preferred date is required.")
            .Must(date => date >= DateOnly.FromDateTime(DateTime.UtcNow.Date))
            .WithMessage("Preferred date must be today or later.")
            .Must(date => date <= DateOnly.FromDateTime(DateTime.UtcNow.Date.AddMonths(6)))
            .WithMessage("Preferred date must be within the next 6 months.");

        RuleFor(x => x.PreferredTime)
            .NotNull().WithMessage("Preferred time is required.");

        RuleFor(x => x.Message)
            .TrimmedRequiredText(2000, "Message")
            .MinimumLength(10).WithMessage("Message must be at least 10 characters long.");

        RuleFor(x => x.SourcePage)
            .OptionalMaxLength(30, "Source page");

        RuleFor(x => x)
            .Must(HaveReasonableBookingCombination)
            .WithMessage("Booking request contains an invalid preferred date and time combination.");
    }

    private static bool HaveReasonableBookingCombination(CreateBookingRequestDto request)
    {
        if (request.PreferredDate is null || request.PreferredTime is null)
        {
            return true;
        }

        if (request.PreferredDate > DateOnly.FromDateTime(DateTime.UtcNow.Date))
        {
            return true;
        }

        var nowTime = TimeOnly.FromDateTime(DateTime.UtcNow.AddMinutes(30));
        return request.PreferredTime >= nowTime;
    }
}
