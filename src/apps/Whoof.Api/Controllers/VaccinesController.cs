using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Whoof.Api.Common;
using Whoof.Api.Common.Controllers;
using Whoof.Application.Vaccines;
using Whoof.Application.Vaccines.Dto;
using Whoof.Domain.Entities;
using Whoof.Infrastructure.Vaccines;

namespace Whoof.Api.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class VaccinesController : BaseCrudController<VaccineDto, Vaccine, CreateVaccineCommand, UpdateVaccineCommand,
    DeleteVaccineCommand, GetVaccineByIdQuery, GetVaccineListQuery, VaccineSearch>
{
    public VaccinesController(IMediator mediator, IValidator<VaccineDto> validator) : base(mediator, validator)
    {
    }
}