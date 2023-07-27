using AutoMapper;
using Whoof.Application.Common.Commands;
using Whoof.Application.Common.Interfaces;
using Whoof.Application.Common.Queries;
using Whoof.Application.Vaccines.Dto;
using Whoof.Domain.Entities;

namespace Whoof.Application.Vaccines;

public class CreateVaccineCommand : BaseCreateCommand<VaccineDto>
{
}

public class CreateVaccineCommandHandler : BaseCreateCommandHandler<CreateVaccineCommand, VaccineDto, Vaccine>
{
    public CreateVaccineCommandHandler(IAppDbContext dbContext, IMapper mapper, ICurrentUserService currentUserService)
        : base(dbContext, mapper, currentUserService)
    {
    }
}

public class UpdateVaccineCommand : BaseUpdateCommand<VaccineDto>
{
}

public class UpdateVaccineCommandHandler : BaseUpdateCommandHandler<UpdateVaccineCommand, VaccineDto, Vaccine>
{
    public UpdateVaccineCommandHandler(IAppDbContext dbContext, IMapper mapper, ICurrentUserService currentUserService)
        : base(dbContext, mapper, currentUserService)
    {
    }
}

public class DeleteVaccineCommand : BaseDeleteCommand<VaccineDto>
{
}

public class DeleteVaccineCommandHandler : BaseDeleteCommandHandler<DeleteVaccineCommand, VaccineDto, Vaccine>
{
    public DeleteVaccineCommandHandler(IAppDbContext dbContext, IMapper mapper, ICurrentUserService currentUserService)
        : base(dbContext, mapper, currentUserService)
    {
    }
}

public class GetVaccineByIdQuery : BaseGetByIdQuery<VaccineDto>
{
}

public class GetVaccineByIdQueryHandler : BaseGetByIdQueryHandler<GetVaccineByIdQuery, VaccineDto, Vaccine>
{
    public GetVaccineByIdQueryHandler(IAppDbContext dbContext, IMapper mapper, ICurrentUserService currentUserService) :
        base(dbContext, mapper, currentUserService)
    {
    }
}

public class GetVaccineListQuery : BaseGetListQuery<VaccineDto, Vaccine>
{
}

public class GetVaccineListQueryHandler : BaseGetListQueryHandler<GetVaccineListQuery, VaccineDto, Vaccine>
{
    public GetVaccineListQueryHandler(IAppDbContext dbContext, IMapper mapper, IFilterAdapter filterAdapter,
        ISortAdapter sortAdapter, ICurrentUserService currentUserService) : base(dbContext, mapper, filterAdapter,
        sortAdapter, currentUserService)
    {
    }
}