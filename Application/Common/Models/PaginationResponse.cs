namespace Application.Common.Models;

/// <summary>
/// Generic paginated response wrapper returned by paginated API endpoints.
/// </summary>
public class PaginationResponse<T>
{
    /// <summary>The items on the current page.</summary>
    public List<T> Items { get; set; } = [];

    /// <summary>Total number of records matching the query (across all pages).</summary>
    public int TotalCount { get; set; }

    /// <summary>Current 1-based page number.</summary>
    public int PageNumber { get; set; }

    /// <summary>Number of items per page.</summary>
    public int PageSize { get; set; }

    /// <summary>Total number of pages.</summary>
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalCount / PageSize) : 0;

    /// <summary>Whether there is a previous page.</summary>
    public bool HasPreviousPage => PageNumber > 1;

    /// <summary>Whether there is a next page.</summary>
    public bool HasNextPage => PageNumber < TotalPages;

    public static PaginationResponse<T> Create(List<T> items, int totalCount, int pageNumber, int pageSize)
        => new()
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
        };
}