using FluentValidation;
using NickysManicurePedicure.Dtos.Requests.Admin;

namespace NickysManicurePedicure.Validation;

public sealed class UpsertServiceRequestValidator : AbstractValidator<UpsertServiceRequest>
{
    public UpsertServiceRequestValidator()
    {
        RuleFor(x => x.ServiceCategoryId)
            .GreaterThan(0).WithMessage("Service category id must be greater than 0.");

        RuleFor(x => x.Name)
            .TrimmedRequiredText(120, "Name");

        RuleFor(x => x.Slug)
            .TrimmedRequiredText(100, "Slug")
            .Matches(ValidationConstants.SlugPattern)
            .WithMessage("Slug must use lowercase letters, numbers, and hyphens only.");

        RuleFor(x => x.Description)
            .TrimmedRequiredText(1200, "Description")
            .MinimumLength(20).WithMessage("Description must be at least 20 characters long.");

        RuleFor(x => x.DurationLabel)
            .TrimmedRequiredText(60, "Duration label");

        RuleFor(x => x.PriceFromLabel)
            .TrimmedRequiredText(60, "Price from label");

        RuleFor(x => x.Status)
            .Must(BeKnownContentStatus)
            .WithMessage("Status must be Draft, Published, or Archived.");

        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo(0).WithMessage("Display order must be 0 or greater.");
    }

    private static bool BeKnownContentStatus(string? status) =>
        status is "Draft" or "Published" or "Archived";
}
