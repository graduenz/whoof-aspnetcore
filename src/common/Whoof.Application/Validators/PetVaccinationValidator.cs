using FluentValidation;
using Whoof.Domain.Entities;

namespace Whoof.Application.Validators;

public class PetVaccinationValidator : AbstractValidator<PetVaccination>
{
    public PetVaccinationValidator()
    {
        RuleFor(m => m.AppliedAt).LessThanOrEqualTo(DateTime.Now);
    }
}