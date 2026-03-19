using System.ComponentModel.DataAnnotations;
using NickysManicurePedicure.Dtos.Common;
using NickysManicurePedicure.Validation;

namespace NickysManicurePedicure.Dtos.Requests;

public sealed class BookingQueryParameters : PaginationRequest
{
    [StringLength(100)]
    public string? Search { get; init; }

    [StringLength(20)]
    [AllowedStringValues("Pending", "Contacted", "Confirmed", "Completed", "Cancelled")]
    public string? Status { get; init; }

    public DateOnly? FromDate { get; init; }

    public DateOnly? ToDate { get; init; }

    [StringLength(20)]
    [AllowedStringValues("createdAtUtc", "preferredDate", "status")]
    public string SortBy { get; init; } = "createdAtUtc";

    [StringLength(4)]
    [AllowedStringValues("asc", "desc")]
    public string SortDirection { get; init; } = "desc";
}
