using Whoof.Application.Common.Models;

namespace Whoof.Application.Common.Interfaces;

public interface ISortAdapter
{
    IQueryable<TEntity> ApplySortExpressions<TEntity>(IQueryable<TEntity> queryable, ICollection<SortExpression>? sortExpressions);
}