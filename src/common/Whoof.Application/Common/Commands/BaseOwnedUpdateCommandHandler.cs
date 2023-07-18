using AutoMapper;
using Whoof.Application.Common.Interfaces;
using Whoof.Domain.Common;

namespace Whoof.Application.Common.Commands;

public class BaseOwnedUpdateCommandHandler<TCommand, TDto, TEntity> : BaseUpdateCommandHandler<TCommand, TDto, TEntity>
    where TCommand : BaseUpdateCommand<TDto>
    where TDto : class
    where TEntity : BaseOwnedEntity
{
    protected BaseOwnedUpdateCommandHandler(IAppDbContext dbContext, IMapper mapper,
        ICurrentUserService currentUserService)
        : base(dbContext, mapper, currentUserService)
    {
    }

    protected override async Task<TEntity?> GetEntityAsync(TCommand request, string userId,
        CancellationToken cancellationToken)
    {
        var entity = await base.GetEntityAsync(request, userId, cancellationToken);
        return entity?.OwnerId == userId ? entity : null;
    }

    protected override Task BeforeUpdateAsync(TEntity entity, string userId, CancellationToken cancellationToken)
    {
        entity.OwnerId = userId;
        return base.BeforeUpdateAsync(entity, userId, cancellationToken);
    }
}