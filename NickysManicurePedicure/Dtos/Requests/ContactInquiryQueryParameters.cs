using System.ComponentModel.DataAnnotations;
using NickysManicurePedicure.Dtos.Common;
using NickysManicurePedicure.Validation;

namespace NickysManicurePedicure.Dtos.Requests;

public sealed class ContactInquiryQueryParameters : PaginationRequest
{
    [StringLength(100)]
    public string? Search { get; init; }

    [StringLength(20)]
    [AllowedStringValues("New", "InProgress", "Responded", "Closed", "Spam")]
    public string? Status { get; init; }

    [StringLength(20)]
    [AllowedStringValues("createdAtUtc", "status", "fullName")]
    public string SortBy { get; init; } = "createdAtUtc";

    [StringLength(4)]
    [AllowedStringValues("asc", "desc")]
    public string SortDirection { get; init; } = "desc";
}
