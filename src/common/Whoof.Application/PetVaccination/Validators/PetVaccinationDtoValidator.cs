﻿using FluentValidation;
using Whoof.Application.PetVaccination.Dto;

namespace Whoof.Application.PetVaccination.Validators;

public class PetVaccinationDtoValidator : AbstractValidator<PetVaccinationDto>
{
    public PetVaccinationDtoValidator()
    {
        RuleFor(m => m.AppliedAt)
            .LessThanOrEqualTo(DateTimeOffset.UtcNow)
            .WithMessage("'Applied At' must be less than or equal to current timestamp.");
    }
}