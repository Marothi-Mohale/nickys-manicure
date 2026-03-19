using FluentValidation;
using NickysManicurePedicure.Dtos.Requests;

namespace NickysManicurePedicure.Validation;

public sealed class FaqQueryParametersValidator : PaginationRequestValidator<FaqQueryParameters>
{
    public FaqQueryParametersValidator()
    {
        RuleFor(x => x.Search)
            .MaximumLength(100).WithMessage("Search must be 100 characters or fewer.");

        RuleFor(x => x.SortBy)
            .Must(sortBy => sortBy is "displayOrder" or "question")
            .WithMessage("Sort by must be displayOrder or question.");

        RuleFor(x => x.SortDirection)
            .Must(direction => direction is "asc" or "desc")
            .WithMessage("Sort direction must be asc or desc.");
    }
}
