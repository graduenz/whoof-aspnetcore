using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Whoof.Application.Common.Interfaces;
using Whoof.Application.Common.Models;
using Whoof.Domain.Common;

namespace Whoof.Application.Common.Commands;

public abstract class BaseUpdateCommandHandler<TCommand, TDto, TEntity> : IRequestHandler<TCommand, ServiceResult<TDto>>
    where TCommand : BaseUpdateCommand<TDto>
    where TDto : class
    where TEntity : BaseEntity
{
    public IAppDbContext DbContext { get; }
    public IMapper Mapper { get; }
    public ICurrentUserService CurrentUserService { get; }

    protected BaseUpdateCommandHandler(IAppDbContext dbContext, IMapper mapper, ICurrentUserService currentUserService)
    {
        DbContext = dbContext;
        Mapper = mapper;
        CurrentUserService = currentUserService;
    }

    public async Task<ServiceResult<TDto>> Handle(TCommand request, CancellationToken cancellationToken)
    {
        var userId = CurrentUserService.GetCurrentUserUniqueId();

        // ReSharper disable once HeapView.BoxingAllocation
        var entity = await GetEntityAsync(request, userId, cancellationToken);

        if (entity == null)
            return ServiceResult.Failed<TDto>(ServiceError.NotFound);

        entity = Mapper.Map<TEntity>(request.Model);

        entity.Id = request.Id;

        if (entity is OwnedEntity owned2)
            owned2.OwnerId = userId;

        await BeforeUpdateAsync(entity, userId, cancellationToken);

        DbContext.Set<TEntity>().Update(entity);
        await DbContext.SaveChangesAsync(cancellationToken);

        var dto = Mapper.Map<TDto>(entity);
        return ServiceResult.Success(dto);
    }

    protected virtual Task BeforeUpdateAsync(TEntity entity, string userId, CancellationToken cancellationToken)
    {
        entity.ModifiedAt = DateTimeOffset.UtcNow;
        entity.ModifiedBy = userId;

        return Task.CompletedTask;
    }

    protected virtual async Task<TEntity?> GetEntityAsync(TCommand request, string userId,
        CancellationToken cancellationToken)
    {
        return await DbContext.Set<TEntity>().FirstOrDefaultAsync(
            m => m.Id == request.Id,
            cancellationToken);
    }
}