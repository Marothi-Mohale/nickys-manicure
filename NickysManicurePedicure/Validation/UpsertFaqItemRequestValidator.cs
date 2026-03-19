using FluentValidation;
using NickysManicurePedicure.Dtos.Requests.Admin;

namespace NickysManicurePedicure.Validation;

public sealed class UpsertFaqItemRequestValidator : AbstractValidator<UpsertFaqItemRequest>
{
    public UpsertFaqItemRequestValidator()
    {
        RuleFor(x => x.Question)
            .TrimmedRequiredText(200, "Question");

        RuleFor(x => x.Answer)
            .TrimmedRequiredText(1000, "Answer")
            .MinimumLength(20).WithMessage("Answer must be at least 20 characters long.");

        RuleFor(x => x.Status)
            .Must(status => status is "Draft" or "Published" or "Archived")
            .WithMessage("Status must be Draft, Published, or Archived.");

        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo(0).WithMessage("Display order must be 0 or greater.");
    }
}
