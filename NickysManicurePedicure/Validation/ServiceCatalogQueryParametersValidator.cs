using FluentValidation;
using NickysManicurePedicure.Dtos.Requests;

namespace NickysManicurePedicure.Validation;

public sealed class ServiceCatalogQueryParametersValidator : PaginationRequestValidator<ServiceCatalogQueryParameters>
{
    public ServiceCatalogQueryParametersValidator()
    {
        RuleFor(x => x.Search)
            .MaximumLength(100).WithMessage("Search must be 100 characters or fewer.");

        RuleFor(x => x.Category)
            .MaximumLength(80).WithMessage("Category must be 80 characters or fewer.");

        RuleFor(x => x.SortBy)
            .Must(sortBy => sortBy is "displayOrder" or "name" or "price")
            .WithMessage("Sort by must be displayOrder, name, or price.");

        RuleFor(x => x.SortDirection)
            .Must(direction => direction is "asc" or "desc")
            .WithMessage("Sort direction must be asc or desc.");
    }
}
