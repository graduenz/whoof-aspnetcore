using AutoMapper;
using Whoof.Application.Pets.Dto;
using Whoof.Application.PetVaccination.Dto;
using Whoof.Application.Vaccines.Dto;
using Whoof.Domain.Entities;

namespace Whoof.Application.AutoMapper;

public class Dto2DomainProfile : Profile
{
    public Dto2DomainProfile()
    {
        CreateMap<PetDto, Pet>();
        CreateMap<VaccineDto, Vaccine>();
        CreateMap<PetVaccinationDto, Domain.Entities.PetVaccination>();
    }
}