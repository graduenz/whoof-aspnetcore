using Whoof.Domain.Enums;

namespace Whoof.Application.Pets.Dto;

public class PetDto
{
    public string? Name { get; set; }
    public PetType PetType { get; set; }
}