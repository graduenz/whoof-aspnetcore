using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using Whoof.Application.Common.Interfaces;
using Whoof.Domain.Common;

namespace Whoof.Application.Common.Queries;

[SuppressMessage("SonarLint", "S2436", Justification = "Abstraction tradeoffs")]
public abstract class
    BaseOwnedGetByIdQueryHandler<TQuery, TDto, TEntity> : BaseGetByIdQueryHandler<TQuery, TDto, TEntity>
    where TQuery : BaseGetByIdQuery<TDto>
    where TDto : class
    where TEntity : BaseOwnedEntity
{
    protected BaseOwnedGetByIdQueryHandler(IAppDbContext dbContext, IMapper mapper,
        ICurrentUserService currentUserService)
        : base(dbContext, mapper, currentUserService)
    {
    }

    protected override async Task<TEntity?> GetEntityAsync(TQuery request, string userId,
        CancellationToken cancellationToken)
    {
        var entity = await base.GetEntityAsync(request, userId, cancellationToken);
        return entity?.OwnerId == userId ? entity : null;
    }
}