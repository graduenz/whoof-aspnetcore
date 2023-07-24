using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Whoof.Api.Common;
using Whoof.Api.Common.Controllers;
using Whoof.Application.Pets;
using Whoof.Application.Pets.Dto;
using Whoof.Domain.Entities;
using Whoof.Infrastructure.Pets;

namespace Whoof.Api.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class PetsController : BaseCrudController<PetDto, Pet, CreatePetCommand, UpdatePetCommand, DeletePetCommand,
    GetPetByIdQuery, GetPetListQuery, PetSearch>
{
    public PetsController(IMediator mediator, IValidator<PetDto> validator) : base(mediator, validator)
    {
    }
}