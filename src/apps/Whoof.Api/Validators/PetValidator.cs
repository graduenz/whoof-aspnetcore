using FluentValidation;
using Whoof.Domain.Entities;
using Whoof.Domain.Enums;

namespace Whoof.Api.Validators;

public class PetValidator : AbstractValidator<Pet>
{
    public PetValidator()
    {
        RuleFor(m => m.Name).NotEmpty();
        RuleFor(m => m.PetType).NotEqual(PetType.Unspecified);
    }
}