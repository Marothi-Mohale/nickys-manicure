using System.ComponentModel.DataAnnotations;

namespace NickysManicurePedicure.Dtos.Common;

public abstract class PaginationRequest
{
    private const int MaxPageSize = 100;

    [Range(1, int.MaxValue)]
    public int Page { get; init; } = 1;

    [Range(1, MaxPageSize)]
    public int PageSize { get; init; } = 20;
}
