using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using Whoof.Api.Common.Controllers;
using Whoof.Application.Common.Models;
using Whoof.Application.Common.Models.Examples;
using Whoof.Application.Pets;
using Whoof.Application.Pets.Dto;
using Whoof.Domain.Entities;
using Whoof.Infrastructure.Pets;

namespace Whoof.Api.Controllers;

[ApiController]
[Route("v1/pets")]
[Authorize]
public class PetsController : BaseCrudController<PetDto, Pet, CreatePetCommand, UpdatePetCommand, DeletePetCommand,
    GetPetByIdQuery, GetPetListQuery, PetSearch>
{
    public PetsController(IMediator mediator, IValidator<PetDto> validator) : base(mediator, validator)
    {
    }

    [HttpGet("{id:guid}")]
    [SwaggerResponse(200, Type = typeof(PetDto))]
    [SwaggerResponse(404, Type = typeof(ServiceError))]
    [SwaggerResponseExample(404, typeof(ServiceErrorNotFoundExampleProvider))]
    [SwaggerResponse(500, Type = typeof(ServiceError))]
    [SwaggerResponseExample(500, typeof(ServiceErrorInternalServerErrorExampleProvider))]
    public Task<IActionResult> GetByIdAsync([FromRoute, SwaggerParameter("Pet ID")] Guid id) =>
        GetByIdInternalAsync(id);

    [HttpGet]
    [SwaggerResponse(200, Type = typeof(PaginatedList<PetDto>))]
    [SwaggerResponse(500, Type = typeof(ServiceError))]
    [SwaggerResponseExample(500, typeof(ServiceErrorInternalServerErrorExampleProvider))]
    public virtual Task<IActionResult> GetPaginatedListAsync([FromQuery] PetSearch request) =>
        GetPaginatedListInternalAsync(request);

    [HttpPost]
    [SwaggerResponse(201, Type = typeof(PetDto))]
    [SwaggerResponse(400, Type = typeof(ServiceError))]
    [SwaggerResponseExample(400, typeof(ServiceErrorValidationExampleProvider))]
    [SwaggerResponse(500, Type = typeof(ServiceError))]
    [SwaggerResponseExample(500, typeof(ServiceErrorInternalServerErrorExampleProvider))]
    public virtual Task<IActionResult> PostAsync([FromBody] PetDto model) =>
        PostInternalAsync(model);

    [HttpPut("{id:guid}")]
    [SwaggerResponse(200, Type = typeof(PetDto))]
    [SwaggerResponse(400, Type = typeof(ServiceError))]
    [SwaggerResponseExample(400, typeof(ServiceErrorValidationExampleProvider))]
    [SwaggerResponse(404, Type = typeof(ServiceError))]
    [SwaggerResponseExample(404, typeof(ServiceErrorNotFoundExampleProvider))]
    [SwaggerResponse(500, Type = typeof(ServiceError))]
    [SwaggerResponseExample(500, typeof(ServiceErrorInternalServerErrorExampleProvider))]
    public virtual Task<IActionResult> PutAsync([FromRoute, SwaggerParameter("Pet ID")] Guid id,
        [FromBody] PetDto model) =>
        PutInternalAsync(id, model);

    [HttpDelete("{id:guid}")]
    [SwaggerResponse(200, Type = typeof(PetDto))]
    [SwaggerResponse(404, Type = typeof(ServiceError))]
    [SwaggerResponseExample(404, typeof(ServiceErrorNotFoundExampleProvider))]
    [SwaggerResponse(500, Type = typeof(ServiceError))]
    [SwaggerResponseExample(500, typeof(ServiceErrorInternalServerErrorExampleProvider))]
    public virtual Task<IActionResult> DeleteAsync([FromRoute, SwaggerParameter("Pet ID")] Guid id) =>
        DeleteInternalAsync(id);
}