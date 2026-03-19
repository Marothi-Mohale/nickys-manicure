using FluentValidation;
using NickysManicurePedicure.Dtos.Common;

namespace NickysManicurePedicure.Validation;

public abstract class PaginationRequestValidator<T> : AbstractValidator<T>
    where T : PaginationRequest
{
    protected PaginationRequestValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page must be 1 or greater.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page size must be 1 or greater.")
            .LessThanOrEqualTo(PaginationRequest.MaxPageSize)
            .WithMessage($"Page size must be {PaginationRequest.MaxPageSize} or fewer.");
    }
}
