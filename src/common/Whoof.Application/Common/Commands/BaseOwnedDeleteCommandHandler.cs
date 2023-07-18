using AutoMapper;
using Whoof.Application.Common.Interfaces;
using Whoof.Application.Common.Models;
using Whoof.Domain.Common;

namespace Whoof.Application.Common.Commands;

public abstract class
    BaseOwnedDeleteCommandHandler<TCommand, TDto, TEntity> : BaseDeleteCommandHandler<TCommand, TDto, TEntity>
    where TCommand : BaseDeleteCommand<TDto>
    where TDto : class
    where TEntity : BaseOwnedEntity
{
    protected BaseOwnedDeleteCommandHandler(IAppDbContext dbContext, IMapper mapper,
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
}