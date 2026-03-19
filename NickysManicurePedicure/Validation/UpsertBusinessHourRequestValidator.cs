using FluentValidation;
using NickysManicurePedicure.Dtos.Requests.Admin;

namespace NickysManicurePedicure.Validation;

public sealed class UpsertBusinessHourRequestValidator : AbstractValidator<UpsertBusinessHourRequest>
{
    public UpsertBusinessHourRequestValidator()
    {
        RuleFor(x => x.DayOfWeek)
            .Must(day => day is "Monday" or "Tuesday" or "Wednesday" or "Thursday" or "Friday" or "Saturday" or "Sunday")
            .WithMessage("Day of week must be a valid weekday name.");

        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo(0).WithMessage("Display order must be 0 or greater.");

        RuleFor(x => x.Notes)
            .MaximumLength(120).WithMessage("Notes must be 120 characters or fewer.");

        RuleFor(x => x)
            .Must(x => x.IsClosed || (x.OpenTime is not null && x.CloseTime is not null))
            .WithMessage("Open time and close time are required when the business is not closed.");

        RuleFor(x => x)
            .Must(x => x.IsClosed || x.OpenTime < x.CloseTime)
            .When(x => x.OpenTime is not null && x.CloseTime is not null)
            .WithMessage("Open time must be earlier than close time.");
    }
}
