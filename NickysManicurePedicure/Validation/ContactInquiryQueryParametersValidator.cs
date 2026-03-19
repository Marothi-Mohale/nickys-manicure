using FluentValidation;
using NickysManicurePedicure.Dtos.Requests;

namespace NickysManicurePedicure.Validation;

public sealed class ContactInquiryQueryParametersValidator : PaginationRequestValidator<ContactInquiryQueryParameters>
{
    public ContactInquiryQueryParametersValidator()
    {
        RuleFor(x => x.Search)
            .MaximumLength(100).WithMessage("Search must be 100 characters or fewer.");

        RuleFor(x => x.Status)
            .Must(status => string.IsNullOrWhiteSpace(status) || status is "New" or "InProgress" or "Responded" or "Closed" or "Spam")
            .WithMessage("Status must be New, InProgress, Responded, Closed, or Spam.");

        RuleFor(x => x.SortBy)
            .Must(sortBy => sortBy is "createdAtUtc" or "status" or "fullName")
            .WithMessage("Sort by must be createdAtUtc, status, or fullName.");

        RuleFor(x => x.SortDirection)
            .Must(direction => direction is "asc" or "desc")
            .WithMessage("Sort direction must be asc or desc.");
    }
}
