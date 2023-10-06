using System.Linq.Expressions;

namespace Whoof.Application.Common.Interfaces;

public interface IFilterAdapter
{
    IQueryable<TEntity> ApplyFilterExpressions<TEntity>(IQueryable<TEntity> queryable,
        ICollection<Expression<Func<TEntity, bool>>>? filters);
}