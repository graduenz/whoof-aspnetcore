using System.Linq.Expressions;

namespace Whoof.Application.Common.Models;

public abstract class BaseSearch<TEntity> where TEntity : class
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? Sort { get; set; }
    public string? Order { get; set; }
    public string? Search { get; set; }

    public abstract IEnumerable<Expression<Func<TEntity, bool>>>? GetFilters();
}