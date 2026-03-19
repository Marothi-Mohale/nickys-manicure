using System.ComponentModel.DataAnnotations;
using NickysManicurePedicure.Contracts.Common;

namespace NickysManicurePedicure.Contracts.Requests;

public sealed class GalleryItemQueryParameters : PaginationRequest, IValidatableObject
{
    [StringLength(60)]
    public string? Category { get; init; }

    public bool? FeaturedOnly { get; init; }

    [StringLength(20)]
    public string SortBy { get; init; } = "displayOrder";

    [StringLength(4)]
    public string SortDirection { get; init; } = "asc";

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (SortBy is not ("displayOrder" or "title" or "createdUtc"))
        {
            yield return new ValidationResult("SortBy must be either 'displayOrder', 'title', or 'createdUtc'.", [nameof(SortBy)]);
        }

        if (SortDirection is not ("asc" or "desc"))
        {
            yield return new ValidationResult("SortDirection must be either 'asc' or 'desc'.", [nameof(SortDirection)]);
        }
    }
}
