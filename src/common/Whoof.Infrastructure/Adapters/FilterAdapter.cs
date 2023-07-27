using System.Linq.Expressions;
using Whoof.Application.Common.Interfaces;

namespace Whoof.Infrastructure.Adapters;

public class FilterAdapter : IFilterAdapter
{
    public IQueryable<TEntity> ApplyFilterExpressions<TEntity>(IQueryable<TEntity> queryable,
        IList<Expression<Func<TEntity, bool>>>? filters)
    {
        if (filters == null || filters.Count == 0)
            return queryable;

        return filters.Aggregate(queryable, (current, filter) => current.Where(filter));
    }
}