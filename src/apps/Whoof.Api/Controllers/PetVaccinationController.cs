using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Whoof.Api.Entities;
using Whoof.Api.Persistence;

namespace Whoof.Api.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class PetVaccinationController : BaseCrudController<PetVaccination>
{
    public PetVaccinationController(AppDbContext dbContext, IValidator<PetVaccination> validator) : base(dbContext, validator)
    {
    }
    
    [HttpGet("pet/{petId:guid}")]
    public async Task<IActionResult> GetManyAsync([FromRoute] Guid petId)
    {
        return Ok(await DbContext.PetVaccinations
            .Where(m => m.PetId == petId)
            .OrderByDescending(m => m.AppliedAt)
            .ToListAsync()
        );
    }
}