using System.ComponentModel.DataAnnotations;
using NickysManicurePedicure.Dtos.Common;
using NickysManicurePedicure.Validation;

namespace NickysManicurePedicure.Dtos.Requests;

public sealed class FaqQueryParameters : PaginationRequest
{
    [StringLength(100)]
    public string? Search { get; init; }

    [StringLength(20)]
    [AllowedStringValues("displayOrder", "question")]
    public string SortBy { get; init; } = "displayOrder";

    [StringLength(4)]
    [AllowedStringValues("asc", "desc")]
    public string SortDirection { get; init; } = "asc";
}
