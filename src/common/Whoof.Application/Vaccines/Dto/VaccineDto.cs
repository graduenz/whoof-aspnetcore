using System.ComponentModel.DataAnnotations;
using Whoof.Application.Common.Dto;

namespace Whoof.Application.Vaccines.Dto;

public abstract class VaccineDto : BaseDto
{
    /// <summary>
    /// The name of the vaccine
    /// </summary>
    /// <example>Acme</example>
    [Required]
    public string? Name { get; set; }
    
    /// <summary>
    /// The description of the vaccine
    /// </summary>
    /// <example>Acme is a vaccine that prevents dogs from walking over water</example>
    public string? Description { get; set; }
    
    /// <summary>
    /// The pet type which the vaccine applies to
    /// </summary>
    /// <example>Dog</example>
    [Required]
    public string? PetType { get; set; }
    
    /// <summary>
    /// The duration of the vaccine in days
    /// </summary>
    /// <example>365</example>
    [Required]
    public int Duration { get; set; }
}
