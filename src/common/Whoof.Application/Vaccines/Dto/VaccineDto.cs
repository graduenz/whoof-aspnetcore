using Whoof.Application.Common.Dto;
using Whoof.Domain.Enums;

namespace Whoof.Application.Vaccines.Dto;

public class VaccineDto : BaseDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public PetType PetType { get; set; }
    public string? PetType { get; set; }
    public int Duration { get; set; }
}