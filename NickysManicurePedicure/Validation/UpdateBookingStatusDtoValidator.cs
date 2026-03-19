using FluentValidation;
using NickysManicurePedicure.Dtos.Requests;

namespace NickysManicurePedicure.Validation;

public sealed class UpdateBookingStatusDtoValidator : AbstractValidator<UpdateBookingStatusDto>
{
    public UpdateBookingStatusDtoValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required.")
            .Must(status => status is "Pending" or "Contacted" or "Confirmed" or "Completed" or "Cancelled")
            .WithMessage("Status must be Pending, Contacted, Confirmed, Completed, or Cancelled.");

        RuleFor(x => x.AdminNotes)
            .MaximumLength(1000).WithMessage("Admin notes must be 1000 characters or fewer.");
    }
}
