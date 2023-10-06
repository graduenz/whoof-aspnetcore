using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Whoof.Application.Common.Models;
using Whoof.Domain.Entities;
using Whoof.Domain.Enums;

namespace Whoof.Infrastructure.Pets;

public class PetSearch : BaseSearch<Pet>
{
    /// <summary>
    /// The name of the pet
    /// </summary>
    /// <example>Scooby-Doo</example>
    [BindProperty(Name = "name")]
    public string? Name { get; set; }
    
    /// <summary>
    /// The type of the pet
    /// </summary>
    /// <example>Dog</example>
    [BindProperty(Name = "petType")]
    public PetType? PetType { get; set; }
    
    protected override IEnumerable<Expression<Func<Pet, bool>>> GetEntitySpecificFilters()
    {
        if (!string.IsNullOrEmpty(Search))
            yield return a => EF.Functions.ILike(a.Name!, Search);
        
        if (!string.IsNullOrEmpty(Name))
            yield return a => EF.Functions.ILike(a.Name!, Name);
        
        if (PetType.HasValue)
            yield return a => a.PetType == PetType.Value;
    }
}