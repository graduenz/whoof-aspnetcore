using FluentValidation;
using Whoof.Application.Pets.Dto;
using Whoof.Domain.Enums;

namespace Whoof.Application.Pets.Validators;

public class PetDtoValidator : AbstractValidator<PetDto>
{
    public PetDtoValidator()
    {
        RuleFor(m => m.Name).NotEmpty();
        RuleFor(m => m.PetType).IsEnumName(typeof(PetType));
    }
}