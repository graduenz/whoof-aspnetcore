using AutoMapper;
using MediatR;
using Whoof.Application.Common.Interfaces;
using Whoof.Application.Common.Models;
using Whoof.Domain.Common;

namespace Whoof.Application.Common.Commands;

public abstract class BaseCreateCommandHandler<TCommand, TDto, TEntity> : IRequestHandler<TCommand, ServiceResult<TDto>>
    where TCommand : BaseCreateCommand<TDto>
    where TDto : class
    where TEntity : BaseEntity
{
    public IAppDbContext DbContext { get; }
    public IMapper Mapper { get; }
    public ICurrentUserService CurrentUserService { get; }

    protected BaseCreateCommandHandler(IAppDbContext dbContext, IMapper mapper, ICurrentUserService currentUserService)
    {
        DbContext = dbContext;
        Mapper = mapper;
        CurrentUserService = currentUserService;
    }

    public virtual async Task<ServiceResult<TDto>> Handle(TCommand request, CancellationToken cancellationToken)
    {
        var userId = CurrentUserService.GetCurrentUserUniqueId();

        // ReSharper disable once HeapView.PossibleBoxingAllocation
        var entity = Mapper.Map<TEntity>(request.Model);

        await BeforeAddAsync(entity, userId, cancellationToken);

        await DbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);

        var dto = Mapper.Map<TDto>(entity);
        return ServiceResult.Success(dto);
    }

    protected virtual Task BeforeAddAsync(TEntity entity, string userId, CancellationToken cancellationToken)
    {
        entity.CreatedAt = entity.ModifiedAt = DateTimeOffset.UtcNow;
        entity.CreatedBy = entity.ModifiedBy = userId;

        return Task.CompletedTask;
    }
}