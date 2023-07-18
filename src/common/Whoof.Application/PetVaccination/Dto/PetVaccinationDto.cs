namespace Whoof.Application.PetVaccination.Dto;

public class PetVaccinationDto
{
    public Guid PetId { get; set; }
    public Guid VaccineId { get; set; }
    public DateTimeOffset AppliedAt { get; set; }
}