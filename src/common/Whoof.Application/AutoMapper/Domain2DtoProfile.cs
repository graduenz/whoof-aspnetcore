using AutoMapper;
using Whoof.Application.Pets.Dto;
using Whoof.Application.PetVaccination.Dto;
using Whoof.Application.Vaccines.Dto;
using Whoof.Domain.Entities;

namespace Whoof.Application.AutoMapper;

public class Domain2DtoProfile : Profile
{
    public Domain2DtoProfile()
    {
        CreateMap<Pet, PetDto>();
        CreateMap<Vaccine, VaccineDto>();
        CreateMap<Domain.Entities.PetVaccination, PetVaccinationDto>();
    }
}