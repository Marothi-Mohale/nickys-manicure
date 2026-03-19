using FluentValidation;
using NickysManicurePedicure.Dtos.Requests;

namespace NickysManicurePedicure.Validation;

public sealed class UpdateContactInquiryStatusDtoValidator : AbstractValidator<UpdateContactInquiryStatusDto>
{
    public UpdateContactInquiryStatusDtoValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required.")
            .Must(status => status is "New" or "InProgress" or "Responded" or "Closed" or "Spam")
            .WithMessage("Status must be New, InProgress, Responded, Closed, or Spam.");

        RuleFor(x => x.AdminNotes)
            .MaximumLength(1000).WithMessage("Admin notes must be 1000 characters or fewer.");
    }
}
