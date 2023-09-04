using System.ComponentModel.DataAnnotations;
using Whoof.Application.Common.Dto;

namespace Whoof.Application.Pets.Dto;

public class PetDto : BaseOwnedDto
{
    /// <summary>
    /// The name of the pet
    /// </summary>
    /// <example>Scooby-Doo</example>
    [Required]
    public string? Name { get; set; }
    
    /// <summary>
    /// The type of the pet
    /// </summary>
    /// <example>Dog</example>
    [Required]
    public string? PetType { get; set; }
}