using System.Linq.Expressions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using Whoof.Api.Common.Controllers;
using Whoof.Application.Common.Models;
using Whoof.Application.Common.Models.Examples;
using Whoof.Application.PetVaccination;
using Whoof.Application.PetVaccination.Dto;
using Whoof.Domain.Entities;
using Whoof.Infrastructure.PetVaccination;

namespace Whoof.Api.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class PetVaccinationController : BaseCrudController<PetVaccinationDto, PetVaccination,
    CreatePetVaccinationCommand, UpdatePetVaccinationCommand, DeletePetVaccinationCommand, GetPetVaccinationByIdQuery,
    GetPetVaccinationListQuery, PetVaccinationSearch>
{
    public PetVaccinationController(IMediator mediator, IValidator<PetVaccinationDto> validator)
        : base(mediator, validator)
    {
    }
    
    [HttpGet("{id:guid}")]
    [SwaggerResponse(200, Type = typeof(PetVaccinationDto))]
    [SwaggerResponse(404, Type = typeof(ServiceError))]
    [SwaggerResponseExample(404, typeof(ServiceErrorNotFoundExampleProvider))]
    [SwaggerResponse(500, Type = typeof(ServiceError))]
    [SwaggerResponseExample(500, typeof(ServiceErrorInternalServerErrorExampleProvider))]
    public Task<IActionResult> GetByIdAsync([FromRoute, SwaggerParameter("Pet vaccination ID")] Guid id) =>
        GetByIdInternalAsync(id);

    [HttpPost]
    [SwaggerResponse(201, Type = typeof(PetVaccinationDto))]
    [SwaggerResponse(400, Type = typeof(ServiceError))]
    [SwaggerResponseExample(400, typeof(ServiceErrorValidationExampleProvider))]
    [SwaggerResponse(500, Type = typeof(ServiceError))]
    [SwaggerResponseExample(500, typeof(ServiceErrorInternalServerErrorExampleProvider))]
    public virtual Task<IActionResult> PostAsync([FromBody] PetVaccinationDto model) =>
        PostInternalAsync(model);

    [HttpPut("{id:guid}")]
    [SwaggerResponse(200, Type = typeof(PetVaccinationDto))]
    [SwaggerResponse(400, Type = typeof(ServiceError))]
    [SwaggerResponseExample(400, typeof(ServiceErrorValidationExampleProvider))]
    [SwaggerResponse(404, Type = typeof(ServiceError))]
    [SwaggerResponseExample(404, typeof(ServiceErrorNotFoundExampleProvider))]
    [SwaggerResponse(500, Type = typeof(ServiceError))]
    [SwaggerResponseExample(500, typeof(ServiceErrorInternalServerErrorExampleProvider))]
    public virtual Task<IActionResult> PutAsync([FromRoute, SwaggerParameter("Pet vaccination ID")] Guid id,
        [FromBody] PetVaccinationDto model) =>
        PutInternalAsync(id, model);

    [HttpDelete("{id:guid}")]
    [SwaggerResponse(200, Type = typeof(PetVaccinationDto))]
    [SwaggerResponse(404, Type = typeof(ServiceError))]
    [SwaggerResponseExample(404, typeof(ServiceErrorNotFoundExampleProvider))]
    [SwaggerResponse(500, Type = typeof(ServiceError))]
    [SwaggerResponseExample(500, typeof(ServiceErrorInternalServerErrorExampleProvider))]
    public virtual Task<IActionResult> DeleteAsync([FromRoute, SwaggerParameter("Pet vaccination ID")] Guid id) =>
        DeleteInternalAsync(id);

    [HttpGet("pet/{petId:guid}")]
    [SwaggerResponse(200, Type = typeof(PaginatedList<PetVaccinationDto>))]
    [SwaggerResponse(500, Type = typeof(ServiceError))]
    [SwaggerResponseExample(500, typeof(ServiceErrorInternalServerErrorExampleProvider))]
    public async Task<IActionResult> GetPaginatedListByPetAsync([FromRoute] Guid petId,
        [FromQuery] PetVaccinationSearch request)
    {
        var filters = (request.GetFilters() ?? Array.Empty<Expression<Func<PetVaccination, bool>>>()).ToList();
        filters.Add(m => m.PetId == petId);

        if (request.PageSize > 50)
            request.PageSize = 50;

        var query = new GetPetVaccinationListQuery
        {
            Filters = filters,
            SortExpressions = BuildSortExpressions(request),
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };

        var result = await Mediator.Send(query);
        return result.Succeeded
            ? Ok(result.Data)
            : HandleServiceError(result.Error);
    }
}