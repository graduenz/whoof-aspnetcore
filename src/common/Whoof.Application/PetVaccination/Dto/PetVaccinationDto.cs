using System.ComponentModel.DataAnnotations;
using Whoof.Application.Common.Dto;
using Whoof.Application.Pets.Dto;
using Whoof.Application.Vaccines.Dto;

namespace Whoof.Application.PetVaccination.Dto;

public class PetVaccinationDto : BaseDto
{
    /// <summary>
    /// The ID of the pet
    /// </summary>
    [Required]
    public Guid PetId { get; set; }
    
    /// <summary>
    /// The ID of the vaccine
    /// </summary>
    [Required]
    public Guid VaccineId { get; set; }
    
    /// <summary>
    /// When the vaccine was applied at
    /// </summary>
    [Required]
    public DateTimeOffset AppliedAt { get; set; }
    
    /// <summary>
    /// The pet model
    /// </summary>
    public PetDto? Pet { get; set; }
    
    /// <summary>
    /// The vaccine model
    /// </summary>
    public VaccineDto? Vaccine { get; set; }
}