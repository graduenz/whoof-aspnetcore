﻿using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Whoof.Api.Persistence;
using Whoof.Domain.Entities;

namespace Whoof.Api.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class VaccinesController : BaseCrudController<Vaccine>
{
    public VaccinesController(AppDbContext dbContext, IValidator<Vaccine> validator) : base(dbContext, validator)
    {
    }
}