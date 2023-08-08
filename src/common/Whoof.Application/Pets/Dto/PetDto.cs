using Whoof.Application.Common.Dto;
using Whoof.Domain.Enums;

namespace Whoof.Application.Pets.Dto;

public class PetDto : BaseOwnedDto
{
    public string? Name { get; set; }
    public PetType PetType { get; set; }
}