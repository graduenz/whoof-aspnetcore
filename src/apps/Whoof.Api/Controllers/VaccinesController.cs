using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using Whoof.Api.Common.Controllers;
using Whoof.Application.Common.Models;
using Whoof.Application.Common.Models.Examples;
using Whoof.Application.Vaccines;
using Whoof.Application.Vaccines.Dto;
using Whoof.Domain.Entities;
using Whoof.Infrastructure.Vaccines;

namespace Whoof.Api.Controllers;

[ApiController]
[Route("v1/vaccines")]
public class VaccinesController : BaseCrudController<VaccineDto, Vaccine, CreateVaccineCommand, UpdateVaccineCommand,
    DeleteVaccineCommand, GetVaccineByIdQuery, GetVaccineListQuery, VaccineSearch>
{
    public VaccinesController(IMediator mediator, IValidator<VaccineDto> validator) : base(mediator, validator)
    {
    }
    
    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Gets a single vaccine by ID")]
    [SwaggerResponse(200, Type = typeof(VaccineDto))]
    [SwaggerResponse(404, Type = typeof(ServiceError))]
    [SwaggerResponseExample(404, typeof(ServiceErrorNotFoundExampleProvider))]
    [SwaggerResponse(500, Type = typeof(ServiceError))]
    [SwaggerResponseExample(500, typeof(ServiceErrorInternalServerErrorExampleProvider))]
    public Task<IActionResult> GetByIdAsync([FromRoute, SwaggerParameter("Vaccine ID")] Guid id) =>
        GetByIdInternalAsync(id);

    [HttpGet]
    [SwaggerOperation(Summary = "Gets a paginated list of vaccines")]
    [SwaggerResponse(200, Type = typeof(PaginatedList<VaccineDto>))]
    [SwaggerResponse(500, Type = typeof(ServiceError))]
    [SwaggerResponseExample(500, typeof(ServiceErrorInternalServerErrorExampleProvider))]
    public virtual Task<IActionResult> GetPaginatedListAsync([FromQuery] VaccineSearch request) =>
        GetPaginatedListInternalAsync(request);

    [HttpPost]
    [SwaggerOperation(Summary = "Creates a new vaccine")]
    [SwaggerResponse(201, Type = typeof(VaccineDto))]
    [SwaggerResponse(400, Type = typeof(ServiceError))]
    [SwaggerResponseExample(400, typeof(ServiceErrorValidationExampleProvider))]
    [SwaggerResponse(500, Type = typeof(ServiceError))]
    [SwaggerResponseExample(500, typeof(ServiceErrorInternalServerErrorExampleProvider))]
    public virtual Task<IActionResult> PostAsync([FromBody] VaccineDto model) =>
        PostInternalAsync(model);

    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = "Updates an existing vaccine")]
    [SwaggerResponse(200, Type = typeof(VaccineDto))]
    [SwaggerResponse(400, Type = typeof(ServiceError))]
    [SwaggerResponseExample(400, typeof(ServiceErrorValidationExampleProvider))]
    [SwaggerResponse(404, Type = typeof(ServiceError))]
    [SwaggerResponseExample(404, typeof(ServiceErrorNotFoundExampleProvider))]
    [SwaggerResponse(500, Type = typeof(ServiceError))]
    [SwaggerResponseExample(500, typeof(ServiceErrorInternalServerErrorExampleProvider))]
    public virtual Task<IActionResult> PutAsync([FromRoute, SwaggerParameter("Vaccine ID")] Guid id,
        [FromBody] VaccineDto model) =>
        PutInternalAsync(id, model);

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Deletes an existing vaccine")]
    [SwaggerResponse(200, Type = typeof(VaccineDto))]
    [SwaggerResponse(404, Type = typeof(ServiceError))]
    [SwaggerResponseExample(404, typeof(ServiceErrorNotFoundExampleProvider))]
    [SwaggerResponse(500, Type = typeof(ServiceError))]
    [SwaggerResponseExample(500, typeof(ServiceErrorInternalServerErrorExampleProvider))]
    public virtual Task<IActionResult> DeleteAsync([FromRoute, SwaggerParameter("Vaccine ID")] Guid id) =>
        DeleteInternalAsync(id);
}