using System.ComponentModel.DataAnnotations;
using NickysManicurePedicure.Dtos.Common;
using NickysManicurePedicure.Validation;

namespace NickysManicurePedicure.Dtos.Requests;

public sealed class ServiceCatalogQueryParameters : PaginationRequest
{
    [StringLength(100)]
    public string? Search { get; init; }

    [StringLength(80)]
    public string? Category { get; init; }

    public bool? FeaturedOnly { get; init; }

    [StringLength(20)]
    [AllowedStringValues("displayOrder", "name", "price")]
    public string SortBy { get; init; } = "displayOrder";

    [StringLength(4)]
    [AllowedStringValues("asc", "desc")]
    public string SortDirection { get; init; } = "asc";
}
