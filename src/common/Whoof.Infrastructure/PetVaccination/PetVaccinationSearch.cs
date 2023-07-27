using System.Linq.Expressions;
using Whoof.Application.Common.Models;

namespace Whoof.Infrastructure.PetVaccination;

public class PetVaccinationSearch : BaseSearch<Domain.Entities.PetVaccination>
{
    public Guid? PetId { get; set; }
    public Guid? VaccineId { get; set; }
    public DateTimeOffset? AppliedAt { get; set; }
    public DateTimeOffset? AppliedAtMin { get; set; }
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