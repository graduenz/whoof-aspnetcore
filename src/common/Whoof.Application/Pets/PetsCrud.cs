using AutoMapper;
using Whoof.Application.Common.Commands;
using Whoof.Application.Common.Interfaces;
using Whoof.Application.Common.Queries;
using Whoof.Application.Pets.Dto;
using Whoof.Domain.Entities;

namespace Whoof.Application.Pets;

public class CreatePetCommand : BaseCreateCommand<PetDto>
{
}

public class CreatePetCommandHandler : BaseOwnedCreateCommandHandler<CreatePetCommand, PetDto, Pet>
{
    public CreatePetCommandHandler(IAppDbContext dbContext, IMapper mapper, ICurrentUserService currentUserService) :
        base(dbContext, mapper, currentUserService)
    {
    }
}

public class UpdatePetCommand : BaseUpdateCommand<PetDto>
{
}

public class UpdatePetCommandHandler : BaseOwnedUpdateCommandHandler<UpdatePetCommand, PetDto, Pet>
{
    public UpdatePetCommandHandler(IAppDbContext dbContext, IMapper mapper, ICurrentUserService currentUserService) :
        base(dbContext, mapper, currentUserService)
    {
    }
}

public class DeletePetCommand : BaseDeleteCommand<PetDto>
{
}

public class DeletePetCommandHandler : BaseOwnedDeleteCommandHandler<DeletePetCommand, PetDto, Pet>
{
    public DeletePetCommandHandler(IAppDbContext dbContext, IMapper mapper, ICurrentUserService currentUserService) :
        base(dbContext, mapper, currentUserService)
    {
    }
}

public class GetPetByIdQuery : BaseGetByIdQuery<PetDto>
{
}

public class GetPetByIdQueryHandler : BaseOwnedGetByIdQueryHandler<GetPetByIdQuery, PetDto, Pet>
{
    public GetPetByIdQueryHandler(IAppDbContext dbContext, IMapper mapper, ICurrentUserService currentUserService) :
        base(dbContext, mapper, currentUserService)
    {
    }
}

public class GetPetListQuery : BaseGetListQuery<PetDto, Pet>
{
}

public class GetPetListQueryHandler : BaseOwnedGetListQueryHandler<GetPetListQuery, PetDto, Pet>
{
    public GetPetListQueryHandler(IAppDbContext dbContext, IMapper mapper, IFilterAdapter filterAdapter,
        ISortAdapter sortAdapter, ICurrentUserService currentUserService) : base(dbContext, mapper, filterAdapter,
        sortAdapter, currentUserService)
    {
    }
}