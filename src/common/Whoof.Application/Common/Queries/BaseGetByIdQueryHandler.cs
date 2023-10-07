using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Whoof.Application.Common.Interfaces;
using Whoof.Application.Common.Models;
using Whoof.Domain.Common;

namespace Whoof.Application.Common.Queries;

[SuppressMessage("SonarLint", "S2436", Justification = "Abstraction tradeoffs")]
public abstract class BaseGetByIdQueryHandler<TQuery, TDto, TEntity> : IRequestHandler<TQuery, ServiceResult<TDto>>
    where TQuery : BaseGetByIdQuery<TDto>
    where TDto : class
    where TEntity : BaseEntity
{
    public IAppDbContext DbContext { get; }
    public IMapper Mapper { get; }
    public ICurrentUserService CurrentUserService { get; }

    protected BaseGetByIdQueryHandler(IAppDbContext dbContext, IMapper mapper, ICurrentUserService currentUserService)
    {
        DbContext = dbContext;
        Mapper = mapper;
        CurrentUserService = currentUserService;
    }
    
    public async Task<ServiceResult<TDto>> Handle(TQuery request, CancellationToken cancellationToken)
    {
        var userId = CurrentUserService.GetCurrentUserUniqueId();
        
        // ReSharper disable once HeapView.BoxingAllocation
        var entity = await GetEntityAsync(request, userId, cancellationToken);

        if (entity == null)
            return ServiceResult.Failed<TDto>(ServiceError.NotFound);
        
        var dto = Mapper.Map<TDto>(entity);
        return ServiceResult.Success(dto);
    }
    
    protected virtual Task<IQueryable<TEntity>> CustomizeQueryAsync(IQueryable<TEntity> queryable)
    {
        return Task.FromResult(queryable);
    }
    
    protected virtual async Task<TEntity?> GetEntityAsync(TQuery request, string userId,
        CancellationToken cancellationToken)
    {
        var query = await CustomizeQueryAsync(DbContext.Set<TEntity>());
        return await query.FirstOrDefaultAsync(
            m => m.Id == request.Id,
            cancellationToken);
    }
}