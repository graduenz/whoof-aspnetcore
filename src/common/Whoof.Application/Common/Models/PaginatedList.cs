namespace Whoof.Application.Common.Models;

public class PaginatedList<T>
{
    /// <summary>
    /// List of records of the current page
    /// </summary>
    public List<T>? Items { get; set; }
    
    /// <summary>
    /// Requested page index
    /// </summary>
    /// <example>1</example>
    public int PageIndex { get; set; }
    
    /// <summary>
    /// Requested page size
    /// </summary>
    /// <example>20</example>
    public int PageSize { get; set; }
    
    /// <summary>
    /// Total amount of pages based on request configuration
    /// </summary>
    /// <example>7</example>
    public int TotalPages { get; set; }
    
    /// <summary>
    /// Total amount of records based on request filters
    /// </summary>
    /// <example>123</example>
    public int TotalCount { get; set; }

    public PaginatedList()
    {
    }

    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
        Items = items;
    }
    
    /// <summary>
    /// Indicates whether it has a previous page or not
    /// </summary>
    /// <example>false</example>
    public bool HasPreviousPage => PageIndex > 1;

    /// <summary>
    /// Indicates whether it has a previous page or not
    /// </summary>
    /// <example>true</example>
    public bool HasNextPage => PageIndex < TotalPages;
}