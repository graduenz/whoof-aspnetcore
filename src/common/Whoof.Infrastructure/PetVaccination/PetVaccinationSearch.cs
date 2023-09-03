using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Whoof.Application.Common.Models;

namespace Whoof.Infrastructure.PetVaccination;

public class PetVaccinationSearch : BaseSearch<Domain.Entities.PetVaccination>
{
    /// <summary>
    /// The ID of the pet
    /// </summary>
    [BindProperty(Name = "petId")]
    public Guid? PetId { get; set; }
    
    /// <summary>
    /// The ID of the vaccine
    /// </summary>
    [BindProperty(Name = "vaccineId")]
    public Guid? VaccineId { get; set; }
    
    /// <summary>
    /// Exact match of when the vaccine was applied at
    /// </summary>
    [BindProperty(Name = "appliedAt")]
    public DateTimeOffset? AppliedAt { get; set; }
    
    /// <summary>
    /// Minimum date of when the vaccine was applied at
    /// </summary>
    [BindProperty(Name = "appliedAtMin")]
    public DateTimeOffset? AppliedAtMin { get; set; }
    
    /// <summary>
    /// Maximum date of when the vaccine was applied at
    /// </summary>
    [BindProperty(Name = "appliedAtMax")]
    public DateTimeOffset? AppliedAtMax { get; set; }
    
    public override IEnumerable<Expression<Func<Domain.Entities.PetVaccination, bool>>>? GetFilters()
    {
        if (PetId.HasValue)
            yield return a => a.PetId == PetId.Value;
        
        if (VaccineId.HasValue)
            yield return a => a.VaccineId == VaccineId.Value;
        
        if (AppliedAt.HasValue)
            yield return a => a.AppliedAt == AppliedAt.Value;
        
        if (AppliedAtMin.HasValue)
            yield return a => a.AppliedAt >= AppliedAtMin.Value;
        
        if (AppliedAtMax.HasValue)
            yield return a => a.AppliedAt <= AppliedAtMax.Value;
    }
}