using FluentValidation;
using Whoof.Application.Vaccines.Dto;
using Whoof.Domain.Enums;

namespace Whoof.Application.Vaccines.Validators;

public class VaccineDtoValidator : AbstractValidator<VaccineDto>
{
    public VaccineDtoValidator()
    {
        RuleFor(m => m.Name).NotEmpty();
        RuleFor(m => m.PetType).IsEnumName(typeof(PetType));
        RuleFor(m => m.Duration).GreaterThanOrEqualTo(TimeSpan.FromDays(1).Days);
    }
}
