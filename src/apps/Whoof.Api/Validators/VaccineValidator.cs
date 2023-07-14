using FluentValidation;
using Whoof.Domain.Entities;
using Whoof.Domain.Enums;

namespace Whoof.Api.Validators;

public class VaccineValidator : AbstractValidator<Vaccine>
{
    public VaccineValidator()
    {
        RuleFor(m => m.Name).NotEmpty();
        RuleFor(m => m.PetType).NotEqual(PetType.Unspecified);
        RuleFor(m => m.Duration).GreaterThanOrEqualTo(TimeSpan.FromDays(1).Days);
    }
}
