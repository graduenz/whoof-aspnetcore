using System.Linq.Expressions;
using MediatR;
using Whoof.Application.Common.Models;

namespace Whoof.Application.Common.Queries;

public abstract class BaseGetListQuery<TDto, TEntity> : IRequest<ServiceResult<PaginatedList<TDto>>>
    where TDto : class
    where TEntity : class
{
    public IList<Expression<Func<TEntity, bool>>>? Filters { get; set; }
    public ICollection<SortExpression>? SortExpressions { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}