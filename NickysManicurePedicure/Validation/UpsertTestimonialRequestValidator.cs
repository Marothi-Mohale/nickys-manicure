using FluentValidation;
using NickysManicurePedicure.Dtos.Requests.Admin;

namespace NickysManicurePedicure.Validation;

public sealed class UpsertTestimonialRequestValidator : AbstractValidator<UpsertTestimonialRequest>
{
    public UpsertTestimonialRequestValidator()
    {
        RuleFor(x => x.ClientName)
            .TrimmedRequiredText(80, "Client name");

        RuleFor(x => x.Highlight)
            .TrimmedRequiredText(120, "Highlight");

        RuleFor(x => x.Review)
            .TrimmedRequiredText(500, "Review")
            .MinimumLength(20).WithMessage("Review must be at least 20 characters long.");

        RuleFor(x => x.Status)
            .Must(status => status is "Draft" or "Published" or "Archived")
            .WithMessage("Status must be Draft, Published, or Archived.");

        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo(0).WithMessage("Display order must be 0 or greater.");
    }
}
