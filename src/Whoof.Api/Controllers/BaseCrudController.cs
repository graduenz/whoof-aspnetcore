using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Whoof.Api.Entities;
using Whoof.Api.Models;
using Whoof.Api.Persistence;

namespace Whoof.Api.Controllers;

public abstract class BaseCrudController<TEntity> : ControllerBase
    where TEntity : BaseEntity
{
    protected BaseCrudController(AppDbContext dbContext, IValidator<TEntity> validator)
    {
        DbContext = dbContext;
        Validator = validator;
    }
    
    protected AppDbContext DbContext { get; }
    protected IValidator<TEntity> Validator { get; }

    [HttpGet]
    public async Task<IActionResult> GetManyAsync([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 20)
    {
        if (pageSize > 50)
            pageSize = 50;
        
        var query = DbContext.Set<TEntity>()
            .OrderByDescending(m => m.CreatedAt);
        
        return Ok(new PaginatedList<TEntity>(
            await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(),
            await query.CountAsync(),
            pageIndex,
            pageSize
        ));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOneAsync([FromRoute] Guid id)
    {
        var entity = await DbContext.Set<TEntity>()
            .FirstOrDefaultAsync(m => m.Id == id);

        return entity == null
            ? NotFound()
            : Ok(entity);
    }
    
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] TEntity entity)
    {
        var validationResult = await Validator.ValidateAsync(entity);

        if (!validationResult.IsValid)
            return BadRequest(new ValidationErrorsResult {
                Type = "VALIDATION_ERRORS",
                Errors = validationResult.Errors
            });

        var result = await DbContext.Set<TEntity>().AddAsync(entity);
        
        await DbContext.SaveChangesAsync();
        
        return CreatedAtAction("GetOne", new { id = result.Entity.Id }, result.Entity);
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> PutAsync([FromRoute] Guid id, [FromBody] TEntity entity)
    {
        var validationResult = await Validator.ValidateAsync(entity);

        if (!validationResult.IsValid)
            return BadRequest(new ValidationErrorsResult {
                Type = "VALIDATION_ERRORS",
                Errors = validationResult.Errors
            });
        
        var entityExists = await DbContext.Set<TEntity>()
            .AnyAsync(m => m.Id == id);

        if (entityExists is false)
            return NotFound();
        
        var result = DbContext.Set<TEntity>().Update(entity);

        await DbContext.SaveChangesAsync();
        
        return Ok(result.Entity);
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        var entity = await DbContext.Set<TEntity>()
            .FirstOrDefaultAsync(m => m.Id == id);

        if (entity == null)
            return NotFound();
        
        DbContext.Set<TEntity>().Remove(entity);
        
        await DbContext.SaveChangesAsync();

        return Ok();
    }
}