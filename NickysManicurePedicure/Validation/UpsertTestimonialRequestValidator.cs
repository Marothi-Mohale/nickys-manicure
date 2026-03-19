using FluentValidation;
using NickysManicurePedicure.Dtos.Requests.Admin;

namespace NickysManicurePedicure.Validation;

public sealed class UpsertTestimonialRequestValidator : AbstractValidator<UpsertTestimonialRequest>
{
    public UpsertTestimonialRequestValidator()
    {
        RuleFor(x => x.ClientName)
            .TrimmedRequiredText(80, "Client name");

        RuleFor(x => x.Quote)
            .TrimmedRequiredText(600, "Quote")
            .MinimumLength(20).WithMessage("Quote must be at least 20 characters long.");

        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");

        RuleFor(x => x.Status)
            .Must(status => status is "Draft" or "Published" or "Archived")
            .WithMessage("Status must be Draft, Published, or Archived.");

        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo(0).WithMessage("Display order must be 0 or greater.");
    }
}
