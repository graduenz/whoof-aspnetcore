using Whoof.Domain.Enums;

namespace Whoof.Application.Vaccines.Dto;

public class VaccineDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public PetType PetType { get; set; }
    public int Duration { get; set; }
}