using Whoof.Application.Common.Dto;
using Whoof.Domain.Enums;

namespace Whoof.Application.Pets.Dto;

public class PetDto : BaseDto
{
    public string? Name { get; set; }
    public PetType PetType { get; set; }
}