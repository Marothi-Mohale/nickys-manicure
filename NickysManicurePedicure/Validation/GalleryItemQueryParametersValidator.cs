using FluentValidation;
using NickysManicurePedicure.Dtos.Requests;

namespace NickysManicurePedicure.Validation;

public sealed class GalleryItemQueryParametersValidator : PaginationRequestValidator<GalleryItemQueryParameters>
{
    public GalleryItemQueryParametersValidator()
    {
        RuleFor(x => x.Category)
            .MaximumLength(60).WithMessage("Category must be 60 characters or fewer.");

        RuleFor(x => x.SortBy)
            .Must(sortBy => sortBy is "displayOrder" or "title" or "createdUtc")
            .WithMessage("Sort by must be displayOrder, title, or createdUtc.");

        RuleFor(x => x.SortDirection)
            .Must(direction => direction is "asc" or "desc")
            .WithMessage("Sort direction must be asc or desc.");
    }
}
