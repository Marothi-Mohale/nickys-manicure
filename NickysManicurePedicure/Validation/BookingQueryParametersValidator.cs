using FluentValidation;
using NickysManicurePedicure.Dtos.Requests;

namespace NickysManicurePedicure.Validation;

public sealed class BookingQueryParametersValidator : AbstractValidator<BookingQueryParameters>
{
    public BookingQueryParametersValidator()
    {
        RuleFor(x => x.Search)
            .MaximumLength(100).WithMessage("Search must be 100 characters or fewer.");

        RuleFor(x => x.Status)
            .Must(status => string.IsNullOrWhiteSpace(status) || status is "Pending" or "Contacted" or "Confirmed" or "Completed" or "Cancelled")
            .WithMessage("Status must be Pending, Contacted, Confirmed, Completed, or Cancelled.");

        RuleFor(x => x)
            .Must(x => x.FromDate is null || x.ToDate is null || x.FromDate <= x.ToDate)
            .WithMessage("From date must be earlier than or equal to to date.");
    }
}
