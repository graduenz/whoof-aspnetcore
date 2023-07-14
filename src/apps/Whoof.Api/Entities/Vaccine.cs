using Whoof.Api.Enums;

namespace Whoof.Api.Entities;

public class Vaccine : BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public PetType PetType { get; set; }
    public int Duration { get; set; }
    
    public List<PetVaccination>? Vaccinations { get; set; }
}