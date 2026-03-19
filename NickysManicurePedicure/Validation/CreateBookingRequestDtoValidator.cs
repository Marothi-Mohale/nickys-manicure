using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NickysManicurePedicure.Data;
using NickysManicurePedicure.Dtos.Requests;
using NickysManicurePedicure.Models.Entities;

namespace NickysManicurePedicure.Validation;

public sealed class CreateBookingRequestDtoValidator : AbstractValidator<CreateBookingRequestDto>
{
    public CreateBookingRequestDtoValidator(ApplicationDbContext dbContext)
    {
        RuleFor(x => x.FullName)
            .TrimmedRequiredText(120, "Full name");

        RuleFor(x => x.PhoneNumber)
            .SalonPhone()
            .MaximumLength(30).WithMessage("Phone number must be 30 characters or fewer.");

        RuleFor(x => x.Email)
            .TrimmedRequiredText(200, "Email")
            .EmailAddress().WithMessage("Email must be a valid email address.");

        RuleFor(x => x.PreferredServiceName)
            .MaximumLength(120).WithMessage("Preferred service name must be 120 characters or fewer.");

        RuleFor(x => x)
            .Must(x => x.PreferredServiceId.HasValue || !string.IsNullOrWhiteSpace(x.PreferredServiceName))
            .WithMessage("Either preferred service id or preferred service name is required.");

        RuleFor(x => x.PreferredServiceId)
            .GreaterThan(0).When(x => x.PreferredServiceId.HasValue)
            .WithMessage("Preferred service id must be greater than 0.");

        RuleFor(x => x)
            .MustAsync(async (request, cancellationToken) =>
            {
                if (!request.PreferredServiceId.HasValue)
                {
                    return true;
                }

                var service = await dbContext.Services
                    .AsNoTracking()
                    .Where(x => x.Status == ContentStatus.Published)
                    .FirstOrDefaultAsync(x => x.Id == request.PreferredServiceId.Value, cancellationToken);

                if (service is null)
                {
                    return false;
                }

                if (string.IsNullOrWhiteSpace(request.PreferredServiceName))
                {
                    return true;
                }

                return string.Equals(service.Name, request.PreferredServiceName.Trim(), StringComparison.Ordinal);
            })
            .WithMessage("Preferred service does not reference a valid published service.");

        RuleFor(x => x.PreferredDate)
            .NotNull().WithMessage("Preferred date is required.")
            .Must(date => date >= DateOnly.FromDateTime(DateTime.UtcNow.Date))
            .WithMessage("Preferred date must be today or later.")
            .Must(date => date <= DateOnly.FromDateTime(DateTime.UtcNow.Date.AddMonths(6)))
            .WithMessage("Preferred date must be within the next 6 months.");

        RuleFor(x => x.PreferredTime)
            .NotNull().WithMessage("Preferred time is required.")
            .Must(time => time is not null && time.Value >= new TimeOnly(7, 0) && time.Value <= new TimeOnly(20, 0))
            .When(x => x.PreferredTime.HasValue)
            .WithMessage("Preferred time must be between 07:00 and 20:00.");

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
