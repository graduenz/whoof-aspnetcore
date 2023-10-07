using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Whoof.Domain.Common;

namespace Whoof.Application.Common.Models;

public abstract class BaseSearch<TEntity> where TEntity : BaseEntity
{
    /// <summary>
    /// Page index, starts from 1
    /// </summary>
    /// <example>1</example>
    [BindProperty(Name = "pageIndex")]
    public int PageIndex { get; set; } = 1;
    
    /// <summary>
    /// Amount of records per page
    /// </summary>
    /// <example>20</example>
    [BindProperty(Name = "pageSize")]
    public int PageSize { get; set; } = 100;
    
    /// <summary>
    /// Field used for sorting
    /// </summary>
    /// <example>id</example>
    [BindProperty(Name = "sort")]
    public string? Sort { get; set; }
    
    /// <summary>
    /// Order used for sorting (asc or desc)
    /// </summary>
    /// <example>asc</example>
    [BindProperty(Name = "order")]
    public string? Order { get; set; }
    
    /// <summary>
    /// Full-text search clause that can match multiple fields
    /// </summary>
    /// <example>%foo%</example>
    [BindProperty(Name = "search")]
    public string? Search { get; set; }
    
    /// <summary>
    /// Optional list of IDs to filter the records
    /// </summary>
    [BindProperty(Name = "ids")]
    public ICollection<Guid>? Ids { get; set; }

    public IEnumerable<Expression<Func<TEntity, bool>>> GetFilters()
    {
        if (Ids?.Count > 0)
            yield return e => Ids.Contains(e.Id);
        
        foreach (var filter in GetEntitySpecificFilters())
            yield return filter;
    }

    protected abstract IEnumerable<Expression<Func<TEntity, bool>>> GetEntitySpecificFilters();
}