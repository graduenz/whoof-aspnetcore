using FluentValidation;
using Whoof.Domain.Entities;
using Whoof.Domain.Enums;

namespace Whoof.Application.Validators;

public class PetValidator : AbstractValidator<Pet>
{
    public PetValidator()
    {
        RuleFor(m => m.Name).NotEmpty();
        RuleFor(m => m.PetType).NotEqual(PetType.Unspecified);
    }
}