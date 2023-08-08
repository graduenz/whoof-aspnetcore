using System.Linq.Expressions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Whoof.Api.Common;
using Whoof.Api.Common.Controllers;
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

    [NonAction]
    public override Task<IActionResult> GetPaginatedListAsync(PetVaccinationSearch request)
    {
        throw new NotSupportedException();
    }

    [HttpGet("pet/{petId:guid}")]
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