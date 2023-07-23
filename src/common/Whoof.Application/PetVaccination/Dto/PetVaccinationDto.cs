using Whoof.Application.Common.Dto;

namespace Whoof.Application.PetVaccination.Dto;

public class PetVaccinationDto : BaseDto
{
    public Guid PetId { get; set; }
    public Guid VaccineId { get; set; }
    public DateTimeOffset AppliedAt { get; set; }
}