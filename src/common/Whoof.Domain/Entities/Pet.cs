using Whoof.Domain.Common;
using Whoof.Domain.Enums;

namespace Whoof.Domain.Entities;

public class Pet : BaseEntity
{
    public string? Name { get; set; }
    public PetType PetType { get; set; }
    
    public List<PetVaccination>? Vaccinations { get; set; }
}