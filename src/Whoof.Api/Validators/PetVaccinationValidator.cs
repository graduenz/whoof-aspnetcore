﻿using FluentValidation;
using Whoof.Api.Entities;

namespace Whoof.Api.Validators;

public class PetVaccinationValidator : AbstractValidator<PetVaccination>
{
    public PetVaccinationValidator()
    {
        RuleFor(m => m.AppliedAt).LessThanOrEqualTo(DateTime.Now);
    }
}