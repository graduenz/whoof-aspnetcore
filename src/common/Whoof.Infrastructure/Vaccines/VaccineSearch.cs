using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Whoof.Application.Common.Models;
using Whoof.Domain.Entities;
using Whoof.Domain.Enums;

namespace Whoof.Infrastructure.Vaccines;

public class VaccineSearch : BaseSearch<Vaccine>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public PetType? PetType { get; set; }
    public int? DurationMin { get; set; }
    public int? DurationMax { get; set; }
    
    public override IEnumerable<Expression<Func<Vaccine, bool>>>? GetFilters()
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