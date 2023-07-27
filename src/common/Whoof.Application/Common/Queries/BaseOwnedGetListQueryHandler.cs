using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Whoof.Application.Common.Interfaces;
using Whoof.Domain.Common;

namespace Whoof.Application.Common.Queries;

[SuppressMessage("SonarLint", "S2436", Justification = "Abstraction tradeoffs")]
public abstract class
    BaseOwnedGetListQueryHandler<TQuery, TDto, TEntity> : BaseGetListQueryHandler<TQuery, TDto, TEntity>
    where TQuery : BaseGetListQuery<TDto, TEntity>
    where TDto : class
    where TEntity : BaseOwnedEntity
{
    protected BaseOwnedGetListQueryHandler(IAppDbContext dbContext, IMapper mapper, IFilterAdapter filterAdapter,
        ISortAdapter sortAdapter, ICurrentUserService currentUserService) : base(dbContext, mapper, filterAdapter,
        sortAdapter, currentUserService)
    {
    }

    protected override async Task<IQueryable<TEntity>> FilterQueryAsync(TQuery request, IQueryable<TEntity> queryable,
        string userId, CancellationToken cancellationToken)
    {
        queryable = await base.FilterQueryAsync(request, queryable, userId, cancellationToken);
        return queryable.Where(m => m.OwnerId == userId);
    }
}