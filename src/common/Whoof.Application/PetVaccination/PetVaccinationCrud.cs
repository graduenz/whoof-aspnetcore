using AutoMapper;
using Whoof.Application.Common.Commands;
using Whoof.Application.Common.Interfaces;
using Whoof.Application.Common.Queries;
using Whoof.Application.PetVaccination.Dto;

namespace Whoof.Application.PetVaccination;

public class CreatePetVaccinationCommand : BaseCreateCommand<PetVaccinationDto>
{
}

public class CreatePetVaccinationCommandHandler : BaseCreateCommandHandler<CreatePetVaccinationCommand,
    PetVaccinationDto, Domain.Entities.PetVaccination>
{
    public CreatePetVaccinationCommandHandler(IAppDbContext dbContext, IMapper mapper,
        ICurrentUserService currentUserService) : base(dbContext, mapper, currentUserService)
    {
    }
}

public class UpdatePetVaccinationCommand : BaseUpdateCommand<PetVaccinationDto>
{
}

public class UpdatePetVaccinationCommandHandler : BaseUpdateCommandHandler<UpdatePetVaccinationCommand,
    PetVaccinationDto, Domain.Entities.PetVaccination>
{
    public UpdatePetVaccinationCommandHandler(IAppDbContext dbContext, IMapper mapper,
        ICurrentUserService currentUserService) : base(dbContext, mapper, currentUserService)
    {
    }
}

public class DeletePetVaccinationCommand : BaseDeleteCommand<PetVaccinationDto>
{
}

public class DeletePetVaccinationCommandHandler : BaseDeleteCommandHandler<DeletePetVaccinationCommand,
    PetVaccinationDto, Domain.Entities.PetVaccination>
{
    public DeletePetVaccinationCommandHandler(IAppDbContext dbContext, IMapper mapper,
        ICurrentUserService currentUserService) : base(dbContext, mapper, currentUserService)
    {
    }
}

public class GetPetVaccinationByIdQuery : BaseGetByIdQuery<PetVaccinationDto>
{
}

public class GetPetVaccinationByIdQueryHandler : BaseGetByIdQueryHandler<GetPetVaccinationByIdQuery, PetVaccinationDto,
    Domain.Entities.PetVaccination>
{
    public GetPetVaccinationByIdQueryHandler(IAppDbContext dbContext, IMapper mapper,
        ICurrentUserService currentUserService) : base(dbContext, mapper, currentUserService)
    {
    }
}

public class GetPetVaccinationListQuery : BaseGetListQuery<PetVaccinationDto, Domain.Entities.PetVaccination>
{
}

public class GetPetVaccinationListQueryHandler : BaseGetListQueryHandler<GetPetVaccinationListQuery, PetVaccinationDto,
    Domain.Entities.PetVaccination>
{
    public GetPetVaccinationListQueryHandler(IAppDbContext dbContext, IMapper mapper, IFilterAdapter filterAdapter,
        ISortAdapter sortAdapter, ICurrentUserService currentUserService) : base(dbContext, mapper, filterAdapter,
        sortAdapter, currentUserService)
    {
    }
}