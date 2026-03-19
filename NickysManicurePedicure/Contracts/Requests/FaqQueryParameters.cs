using System.ComponentModel.DataAnnotations;
using NickysManicurePedicure.Contracts.Common;

namespace NickysManicurePedicure.Contracts.Requests;

public sealed class FaqQueryParameters : PaginationRequest, IValidatableObject
{
    [StringLength(100)]
    public string? Search { get; init; }

    [StringLength(20)]
    public string SortBy { get; init; } = "displayOrder";

    [StringLength(4)]
    public string SortDirection { get; init; } = "asc";

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (SortBy is not ("displayOrder" or "question"))
        {
            yield return new ValidationResult("SortBy must be either 'displayOrder' or 'question'.", [nameof(SortBy)]);
        }

        if (SortDirection is not ("asc" or "desc"))
        {
            yield return new ValidationResult("SortDirection must be either 'asc' or 'desc'.", [nameof(SortDirection)]);
        }
    }
}
