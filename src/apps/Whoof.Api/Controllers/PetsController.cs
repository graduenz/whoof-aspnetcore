// using FluentValidation;
// using Microsoft.AspNetCore.Mvc;
// using Whoof.Api.Persistence;
// using Whoof.Domain.Entities;
//
// namespace Whoof.Api.Controllers;
//
// [ApiController]
// [Route("v1/[controller]")]
// public class PetsController : BaseCrudController<Pet>
// {
//     public PetsController(AppDbContext dbContext, IValidator<Pet> validator) : base(dbContext, validator)
//     {
//     }
// }