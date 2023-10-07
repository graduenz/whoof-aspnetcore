using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Whoof.Application.Common.Interfaces;
using Whoof.Application.Common.Models;
using Whoof.Domain.Common;

namespace Whoof.Application.Common.Queries;

[SuppressMessage("SonarLint", "S2436", Justification = "Abstraction tradeoffs")]
public abstract class
    BaseGetListQueryHandler<TQuery, TDto, TEntity> : IRequestHandler<TQuery, ServiceResult<PaginatedList<TDto>>>
    where TQuery : BaseGetListQuery<TDto, TEntity>
    where TDto : class
    where TEntity : BaseEntity
{
    public IAppDbContext DbContext { get; }
    public IMapper Mapper { get; }
    public IFilterAdapter FilterAdapter { get; }
    public ISortAdapter SortAdapter { get; }
    public ICurrentUserService CurrentUserService { get; }

    protected BaseGetListQueryHandler(IAppDbContext dbContext, IMapper mapper, IFilterAdapter filterAdapter,
        ISortAdapter sortAdapter, ICurrentUserService currentUserService)
    {
        DbContext = dbContext;
        Mapper = mapper;
        FilterAdapter = filterAdapter;
        SortAdapter = sortAdapter;
        CurrentUserService = currentUserService;
    }

    public async Task<ServiceResult<PaginatedList<TDto>>> Handle(TQuery request, CancellationToken cancellationToken)
    {
        var userId = CurrentUserService.GetCurrentUserUniqueId();

        var queryable = DbContext.Set<TEntity>()
            .AsNoTracking();

        queryable = await CustomizeQueryAsync(queryable);

        queryable = await FilterQueryAsync(request, queryable, userId, cancellationToken);
        
        var count = await GetTotalRecordsAsync(request, queryable, userId, cancellationToken);

        queryable = await SortQueryAsync(request, queryable, userId, cancellationToken);
        queryable = await PaginateQueryAsync(request, queryable, userId, cancellationToken);

        var items = queryable.Select(entity => Mapper.Map<TDto>(entity)).ToList();

        var list = new PaginatedList<TDto>(items, count, request.PageIndex, request.PageSize);
        return ServiceResult.Success(list);
    }

    protected virtual Task<IQueryable<TEntity>> CustomizeQueryAsync(IQueryable<TEntity> queryable)
    {
        return Task.FromResult(queryable);
    }

    protected virtual Task<int> GetTotalRecordsAsync(TQuery request, IQueryable<TEntity> queryable, string userId,
        CancellationToken cancellationToken)
    {
        return queryable.CountAsync(cancellationToken);
    }

    protected virtual Task<IQueryable<TEntity>> FilterQueryAsync(TQuery request, IQueryable<TEntity> queryable,
        string userId, CancellationToken cancellationToken)
    {
        return Task.FromResult(FilterAdapter.ApplyFilterExpressions(queryable, request.Filters));
    }

    protected virtual Task<IQueryable<TEntity>> SortQueryAsync(TQuery request, IQueryable<TEntity> queryable, string userId, CancellationToken cancellationToken)
    {
        return Task.FromResult(SortAdapter.ApplySortExpressions(queryable, request.SortExpressions));
    }

    protected virtual Task<IQueryable<TEntity>> PaginateQueryAsync(TQuery request, IQueryable<TEntity> queryable, string userId, CancellationToken cancellationToken)
    {
        return Task.FromResult(queryable.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize));
    }
}