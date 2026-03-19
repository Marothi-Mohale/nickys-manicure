namespace NickysManicurePedicure.Dtos.Common;

public sealed class PagedResponse<T>
{
    public required IReadOnlyCollection<T> Items { get; init; }
    public required PaginationMetadata Pagination { get; init; }
}
