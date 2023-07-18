using AutoMapper;
using Whoof.Application.Common.Interfaces;
using Whoof.Domain.Common;

namespace Whoof.Application.Common.Commands;

public abstract class
    BaseOwnedCreateCommandHandler<TCommand, TDto, TEntity> : BaseCreateCommandHandler<TCommand, TDto, TEntity>
    where TCommand : BaseCreateCommand<TDto>
    where TDto : class
    where TEntity : BaseOwnedEntity
{
    protected BaseOwnedCreateCommandHandler(IAppDbContext dbContext, IMapper mapper,
        ICurrentUserService currentUserService)
        : base(dbContext, mapper, currentUserService)
    {
    }

    protected override Task BeforeAddAsync(TEntity entity, string userId, CancellationToken cancellationToken)
    {
        entity.OwnerId = userId;
        return base.BeforeAddAsync(entity, userId, cancellationToken);
    }
}