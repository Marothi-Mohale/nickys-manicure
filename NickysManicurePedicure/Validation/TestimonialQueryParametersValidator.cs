using FluentValidation;
using NickysManicurePedicure.Dtos.Requests;

namespace NickysManicurePedicure.Validation;

public sealed class TestimonialQueryParametersValidator : PaginationRequestValidator<TestimonialQueryParameters>
{
    public TestimonialQueryParametersValidator()
    {
        RuleFor(x => x.SortBy)
            .Must(sortBy => sortBy is "displayOrder" or "clientName")
            .WithMessage("Sort by must be displayOrder or clientName.");

        RuleFor(x => x.SortDirection)
            .Must(direction => direction is "asc" or "desc")
            .WithMessage("Sort direction must be asc or desc.");
    }
}
