using Whoof.Domain.Common;

namespace Whoof.Domain.Entities;

public class PetVaccination : BaseEntity
{
    public Guid PetId { get; set; }
    public Guid VaccineId { get; set; }
    public DateTimeOffset AppliedAt { get; set; }
    
    public Pet? Pet { get; set; }
    public Vaccine? Vaccine { get; set; }
}