using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Whoof.Application.Common.Models;
using Whoof.Domain.Entities;
using Whoof.Domain.Enums;

namespace Whoof.Infrastructure.Vaccines;

public class VaccineSearch : BaseSearch<Vaccine>
{
    /// <summary>
    /// The name of the vaccine
    /// </summary>
    /// <example>Acme</example>
    [BindProperty(Name = "name")]
    public string? Name { get; set; }
    
    /// <summary>
    /// The description of the vaccine
    /// </summary>
    /// <example>%prevents from madness%</example>
    [BindProperty(Name = "description")]
    public string? Description { get; set; }
    
    /// <summary>
    /// The pet type which the vaccine applies to
    /// </summary>
    /// <example>Dog</example>
    [BindProperty(Name = "petType")]
    public PetType? PetType { get; set; }
    
    /// <summary>
    /// The minimum duration of the vaccine in days
    /// </summary>
    /// <example>30</example>
    [BindProperty(Name = "durationMin")]
    public int? DurationMin { get; set; }
    
    /// <summary>
    /// The maximum duration of the vaccine in days
    /// </summary>
    /// <example>90</example>
    [BindProperty(Name = "durationMax")]
    public int? DurationMax { get; set; }
    
    protected override IEnumerable<Expression<Func<Vaccine, bool>>> GetEntitySpecificFilters()
    {
        if (!string.IsNullOrEmpty(Search))
            yield return a => EF.Functions.ILike(a.Name!, Search)
                || EF.Functions.ILike(a.Description!, Search);
        
        if (!string.IsNullOrEmpty(Name))
            yield return a => EF.Functions.ILike(a.Name!, Name);
        
        if (!string.IsNullOrEmpty(Description))
            yield return a => EF.Functions.ILike(a.Description!, Description);
        
        if (PetType.HasValue)
            yield return a => a.PetType == PetType.Value;
        
        if (DurationMin.HasValue)
            yield return a => a.Duration >= DurationMin.Value;
        
        if (DurationMax.HasValue)
            yield return a => a.Duration <= DurationMax.Value;
    }
}