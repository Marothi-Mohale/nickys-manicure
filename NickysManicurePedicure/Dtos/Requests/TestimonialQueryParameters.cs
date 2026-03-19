using System.ComponentModel.DataAnnotations;
using NickysManicurePedicure.Dtos.Common;
using NickysManicurePedicure.Validation;

namespace NickysManicurePedicure.Dtos.Requests;

public sealed class TestimonialQueryParameters : PaginationRequest
{
    [StringLength(20)]
    [AllowedStringValues("displayOrder", "clientName")]
    public string SortBy { get; init; } = "displayOrder";

    [StringLength(4)]
    [AllowedStringValues("asc", "desc")]
    public string SortDirection { get; init; } = "asc";
}
