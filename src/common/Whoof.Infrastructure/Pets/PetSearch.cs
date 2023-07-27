using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Whoof.Application.Common.Models;
using Whoof.Domain.Entities;
using Whoof.Domain.Enums;

namespace Whoof.Infrastructure.Pets;

public class PetSearch : BaseSearch<Pet>
{
    public string? Name { get; set; }
    public PetType? PetType { get; set; }
    
    public override IEnumerable<Expression<Func<Pet, bool>>>? GetFilters()
    {
        if (!string.IsNullOrEmpty(Search))
            yield return a => EF.Functions.ILike(a.Name!, Search);
        
        if (!string.IsNullOrEmpty(Name))
            yield return a => EF.Functions.ILike(a.Name!, Name);
        
        if (PetType.HasValue)
            yield return a => a.PetType == PetType.Value;
    }
}