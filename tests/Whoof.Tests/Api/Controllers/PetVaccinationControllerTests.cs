using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Whoof.Api.Common.Models;
using Whoof.Api.Controllers;
using Whoof.Application.Common.Models;
using Whoof.Application.PetVaccination.Dto;
using Whoof.Domain.Entities;
using Whoof.Domain.Enums;
using Whoof.Infrastructure.PetVaccination;
using Whoof.Tests.Api.Support;
using Whoof.Tests.Extensions;

namespace Whoof.Tests.Api;

public class PetVaccinationControllerTests : BaseControllerTests
{
    private static readonly Guid IdPetVaccinationAppliedInTheFuture = Guid.NewGuid();

    public PetVaccinationControllerTests()
    {
        HttpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", JwtGenerator.GenerateBasicJwt());
    }

    public static IEnumerable<object[]> MemberData_PetVaccination_And_ValidationErrors()
    {
        yield return new object[]
        {
            new PetVaccinationDto { Id = IdPetVaccinationAppliedInTheFuture, AppliedAt = DateTimeOffset.UtcNow.AddDays(1) },
            new[] { "'Applied At' must be less than or equal to current timestamp." }
        };
    }

    [Fact]
    public async Task GetPaginatedListByPetAsync_FiltersByPetAndReturnsAsExpected()
    {
        // Arrange
        var pet = new Pet
        {
            Id = Guid.NewGuid(),
            Name = "Tested",
            PetType = PetType.Dog
        };
        await DbContext.Pets.AddAsync(pet);

        var vaccines = await DbContext.Vaccines.Where(m => m.PetType == PetType.Dog).Take(3).ToListAsync();
        var petVaccinationDtos = new List<PetVaccinationDto>();
        vaccines.ForEach(v =>
        {
            var petVaccination = new PetVaccination
            {
                Id = Guid.NewGuid(),
                PetId = pet.Id,
                VaccineId = v.Id,
                AppliedAt = DateTimeOffset.UtcNow
            };
            
            petVaccinationDtos.Add(Mapper.Map<PetVaccinationDto>(petVaccination));
            DbContext.PetVaccinations.Add(petVaccination);
        });
        
        await DbContext.SaveChangesAsync();
        
        // Act
        var result = await HttpClient.GetFromJsonAsync<PaginatedList<PetVaccinationDto>>(
            $"/v1/petVaccination/pet/{pet.Id}?pageIndex={1}&pageSize={20}", JsonOptions
        );

        // Assert
        result.Should().NotBeNull();
        result!.PageIndex.Should().Be(1);
        result.TotalPages.Should().Be(1);
        result.Items.Should()
            .HaveCount(3)
            .And.BeEquivalentTo(petVaccinationDtos, c => c.Excluding(m => m.AppliedAt));
    }

    [Fact]
    public async Task GetPaginatedListAsync_WithHttpClient_ReturnsMethodNotAllowed()
    {
        // Act
        var response = await HttpClient.GetAsync("/v1/petVaccination");

        // Assert
        response.Should().Be405MethodNotAllowed();
    }
    
    [Fact]
    public async Task GetPaginatedListAsync_RegularMethodCall_Throws()
    {
        // Arrange
        var controller = new PetVaccinationController(null, null);
        
        // Act
        var act = async () => await controller.GetPaginatedListAsync(new PetVaccinationSearch());
        
        // Arrange
        await act.Should().ThrowAsync<NotSupportedException>();
    }

    [Fact]
    public async Task GetByIdAsync_WhenPetVaccinationExists_ReturnsAsExpected()
    {
        // Arrange
        var expectedPetVaccination = Mapper.Map<PetVaccinationDto>(DbContext.PetVaccinations.First());

        // Act
        var actualPetVaccination = await HttpClient.GetFromJsonAsync<PetVaccinationDto>(
            $"/v1/petVaccination/{expectedPetVaccination.Id}", JsonOptions
        );

        // Assert
        actualPetVaccination.Should()
            .NotBeNull().And
            .BeEquivalentTo(expectedPetVaccination, c => c
                .Excluding(m => m.AppliedAt)
                .ExcludingBaseFields());
    }

    [Fact]
    public async Task GetByIdAsync_WhenPetVaccinationDoesntExist_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        while (DbContext.PetVaccinations.Any(m => m.Id == id))
            id = Guid.NewGuid();

        // Act
        var response = await HttpClient.GetAsync($"/v1/petVaccination/{id}");

        // Assert
        response.Should().Be404NotFound();
    }

    [Fact]
    public async Task PostAsync_WithValidFields_AddsAsExpected()
    {
        // Arrange
        var petId = DbContext.Pets.First().Id;
        var vaccineId = DbContext.Vaccines.First().Id;
        
        var petVaccination = new PetVaccinationDto
        {
            PetId = petId,
            VaccineId = vaccineId,
            AppliedAt = DateTimeOffset.UtcNow.AddHours(-1)
        };

        var beforeCount = await DbContext.PetVaccinations.CountAsync();

        // Act
        var response = await HttpClient.PostAsJsonAsync("v1/petVaccination/", petVaccination, JsonOptions);

        // Assert
        response.Should()
            .NotBeNull().And
            .Be201Created().And
            .BeAs(petVaccination, c => c
                .ExcludingBaseFields());

        petVaccination = await response.Content.ReadFromJsonAsync<PetVaccinationDto>(JsonOptions);

        var afterCount = await DbContext.PetVaccinations.CountAsync();
        afterCount.Should().Be(beforeCount + 1);

        DbContext.PetVaccinations.Should().Contain(m => m.Id == petVaccination!.Id);
    }

    [Theory]
    [MemberData(nameof(MemberData_PetVaccination_And_ValidationErrors))]
    public async Task PostAsync_WithInvalidData_DoesntAdd(PetVaccinationDto petVaccination, string[] expectedErrors)
    {
        // Arrange
        var beforeCount = await DbContext.PetVaccinations.CountAsync();

        // Act
        var response = await HttpClient.PostAsJsonAsync("v1/petVaccination/", petVaccination, JsonOptions);

        // Assert
        response.Should()
            .NotBeNull().And
            .Be400BadRequest();

        var result = await response.Content.ReadFromJsonAsync<ValidationErrorsResult>(JsonOptions);
        result.Should().NotBeNull();
        result!.Code.Should().Be(ServiceError.Validation.Code);
        result.Message.Should().Be(ServiceError.Validation.Message);
        result.Errors!.SelectMany(m => m.Value).Should()
            .BeEquivalentTo(expectedErrors);

        var afterCount = await DbContext.PetVaccinations.CountAsync();
        afterCount.Should().Be(beforeCount);

        DbContext.PetVaccinations.Should().NotContain(m => m.Id == petVaccination.Id);
    }

    [Fact]
    public async Task PutAsync_WithValidData_UpdatesAsExpected()
    {
        // Arrange
        var petVaccination = Mapper.Map<PetVaccinationDto>(DbContext.PetVaccinations.AsNoTracking().First());
        var appliedAt = petVaccination.AppliedAt.AddHours(-6);
        petVaccination.AppliedAt = appliedAt;

        // Act
        var response = await HttpClient.PutAsJsonAsync($"/v1/petVaccination/{petVaccination.Id}", petVaccination, JsonOptions);

        // Assert
        response.Should()
            .NotBeNull().And
            .Be200Ok().And
            .BeAs(petVaccination, c => c
                .ExcludingBaseFields());

        petVaccination = Mapper.Map<PetVaccinationDto>(DbContext.PetVaccinations.AsNoTracking().First(m => m.Id == petVaccination.Id));
        petVaccination.AppliedAt.Should().Be(appliedAt);
    }

    [Fact]
    public async Task PutAsync_WithNonexistentPetVaccination_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        while (DbContext.PetVaccinations.Any(m => m.Id == id))
            id = Guid.NewGuid();

        var petVaccination = Mapper.Map<PetVaccinationDto>(DbContext.PetVaccinations.First());
        petVaccination.Id = id;

        // Act
        var response = await HttpClient.PutAsJsonAsync($"/v1/petVaccination/{petVaccination.Id}", petVaccination, JsonOptions);

        // Assert
        response.Should().Be404NotFound();
    }

    [Theory]
    [MemberData(nameof(MemberData_PetVaccination_And_ValidationErrors))]
    public async Task PutAsync_WithInvalidData_DoesntUpdate(PetVaccinationDto petVaccination, string[] expectedErrors)
    {
        // Assert
        var petId = DbContext.Pets.First().Id;
        var vaccineId = DbContext.Vaccines.First().Id;
        
        await DbContext.PetVaccinations.AddAsync(new PetVaccination
        {
            Id = petVaccination.Id,
            PetId = petId,
            VaccineId = vaccineId,
            AppliedAt = DateTimeOffset.UtcNow
        });

        // Act
        var response = await HttpClient.PutAsJsonAsync($"v1/petVaccination/{petVaccination.Id}", petVaccination, JsonOptions);

        // Assert
        response.Should()
            .NotBeNull().And
            .Be400BadRequest();

        var result = await response.Content.ReadFromJsonAsync<ValidationErrorsResult>(JsonOptions);
        result.Should().NotBeNull();
        result!.Code.Should().Be(ServiceError.Validation.Code);
        result.Message.Should().Be(ServiceError.Validation.Message);
        result.Errors!.SelectMany(m => m.Value).Should()
            .BeEquivalentTo(expectedErrors);
    }

    [Fact]
    public async Task DeleteAsync_WithExistingPetVaccination_DeletesAsExpected()
    {
        // Arrange
        var petVaccination = DbContext.PetVaccinations.First();

        // Act
        var response = await HttpClient.DeleteAsync($"/v1/petVaccination/{petVaccination.Id}");

        // Assert
        response.Should().Be200Ok();

        DbContext.PetVaccinations.Should().NotContain(m => m.Id == petVaccination.Id);
    }

    [Fact]
    public async Task DeleteAsync_WithNonexistentPetVaccination_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        while (DbContext.PetVaccinations.Any(m => m.Id == id))
            id = Guid.NewGuid();

        // Act
        var response = await HttpClient.DeleteAsync($"/v1/petVaccination/{id}");

        // Assert
        response.Should().Be404NotFound();
    }
}