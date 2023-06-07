using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Whoof.Api.Entities;
using Whoof.Api.Enums;
using Whoof.Api.Models;
using Whoof.Tests.Api.Support;

namespace Whoof.Tests.Api;

public class PetsControllerTests : BaseControllerTests
{
    private static readonly Guid IdPetWithEmptyName = Guid.NewGuid();
    private static readonly Guid IdPetWithUnspecifiedType = Guid.NewGuid();
    
    public static IEnumerable<object[]> MemberData_Pets_And_ValidationErrors()
    {
        yield return new object[] {
            new Pet { Id = IdPetWithEmptyName, Name = "", PetType = PetType.Capybara },
            new[] { "'Name' must not be empty." }
        };
        
        yield return new object[] {
            new Pet { Id = IdPetWithUnspecifiedType, Name = "Qwerty", PetType = PetType.Unspecified },
            new[] { "'Pet Type' must not be equal to 'Unspecified'." }
        };
    }
    
    [Theory]
    [InlineData(1, 20)]
    [InlineData(2, 20)]
    [InlineData(3, 10)]
    public async Task GetManyAsync_WithDefaultData_ReturnsAsExpected(int pageIndex, int expectedPageSize)
    {
        // Act
        var result = await HttpClient.GetFromJsonAsync<PaginatedList<Pet>>(
            $"/v1/pets?pageIndex={pageIndex}&pageSize={20}"
        );
        
        // Assert
        result.Should().NotBeNull();
        result.PageIndex.Should().Be(pageIndex);
        result.TotalPages.Should().Be(3);
        result.Items.Should().HaveCount(expectedPageSize);
    }

    [Fact]
    public async Task GetOneAsync_ReturnsAsExpected()
    {
        // Arrange
        var expectedPet = DbContext.Pets.First();
        
        // Act
        var actualPet = await HttpClient.GetFromJsonAsync<Pet>(
            $"/v1/pets/{expectedPet.Id}"
        );
        
        // Assert
        actualPet.Should()
            .NotBeNull().And
            .BeEquivalentTo(expectedPet, c => c
                .Excluding(m => m.Vaccinations));
    }

    [Fact]
    public async Task AddOneAsync_WithValidFields_AddsAsExpected()
    {
        // Arrange
        var pet = new Pet
        {
            Name = "Ravena",
            PetType = PetType.Dog
        };

        var beforeCount = await DbContext.Pets.CountAsync();
        
        // Act
        var response = await HttpClient.PostAsJsonAsync("v1/pets/", pet);
        
        // Assert
        response.Should()
            .NotBeNull().And
            .Be201Created().And
            .BeAs(pet, c => c
                .Excluding(m => m.Id));

        pet = await response.Content.ReadFromJsonAsync<Pet>();

        var afterCount = await DbContext.Pets.CountAsync();
        afterCount.Should().Be(beforeCount + 1);

        DbContext.Pets.Should().Contain(m => m.Id == pet.Id);
    }

    [Theory]
    [MemberData(nameof(MemberData_Pets_And_ValidationErrors))]
    public async Task AddOneAsync_WithInvalidData_DoesntAdd(Pet pet, string[] expectedErrors)
    {
        // Arrange
        var beforeCount = await DbContext.Pets.CountAsync();
        
        // Act
        var response = await HttpClient.PostAsJsonAsync("v1/pets/", pet);
        
        // Assert
        response.Should()
            .NotBeNull().And
            .Be400BadRequest();

        var result = await response.Content.ReadFromJsonAsync<ValidationErrorsResult>();
        result.Should().NotBeNull();
        result.Type.Should().Be("VALIDATION_ERRORS");
        result.Errors.Select(m => m.ErrorMessage).Should()
            .BeEquivalentTo(expectedErrors);
        
        var afterCount = await DbContext.Pets.CountAsync();
        afterCount.Should().Be(beforeCount);
        
        DbContext.Pets.Should().NotContain(m => m.Id == pet.Id);
    }

    [Fact]
    public async Task UpdateOneAsync_WithValidData_UpdatesAsExpected()
    {
        // Arrange
        var pet = DbContext.Pets.First();
        pet.Vaccinations = null;
        pet.Name = "Updated";
        
        // Act
        var response = await HttpClient.PutAsJsonAsync($"/v1/pets/{pet.Id}", pet);
        
        // Assert
        response.Should()
            .NotBeNull().And
            .Be200Ok().And
            .BeAs(pet);

        pet = DbContext.Pets.First(m => m.Id == pet.Id);
        pet.Name.Should().Be("Updated");
    }

    [Fact]
    public async Task UpdateOneAsync_WithInexistentPet_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        while (DbContext.Pets.Any(m => m.Id == id))
            id = Guid.NewGuid();

        var pet = DbContext.Pets.First();
        pet.Vaccinations = null;
        pet.Id = id;
        
        // Act
        var response = await HttpClient.PutAsJsonAsync($"/v1/pets/{pet.Id}", pet);
        
        // Assert
        response.Should().Be404NotFound();
    }

    [Theory]
    [MemberData(nameof(MemberData_Pets_And_ValidationErrors))]
    public async Task UpdateOneAsync_WithInvalidData_DoesntUpdate(Pet pet, string[] expectedErrors)
    {
        // Assert
        await DbContext.Pets.AddAsync(new Pet
        {
            Id = pet.Id,
            Name = "Dollar",
            PetType = PetType.Dog
        });
        
        // Act
        var response = await HttpClient.PutAsJsonAsync($"v1/pets/{pet.Id}", pet);
        
        // Assert
        response.Should()
            .NotBeNull().And
            .Be400BadRequest();

        var result = await response.Content.ReadFromJsonAsync<ValidationErrorsResult>();
        result.Should().NotBeNull();
        result.Type.Should().Be("VALIDATION_ERRORS");
        result.Errors.Select(m => m.ErrorMessage).Should()
            .BeEquivalentTo(expectedErrors);
    }
    
    [Fact]
    public async Task DeleteOneAsync_WithExistingPet_DeletesAsExpected()
    {
        // Arrange
        var pet = DbContext.Pets.First();
        
        // Act
        var response = await HttpClient.DeleteAsync($"/v1/pets/{pet.Id}");
        
        // Assert
        response.Should().Be200Ok();

        DbContext.Pets.Should().NotContain(m => m.Id == pet.Id);
    }

    [Fact]
    public async Task DeleteOneAsync_WithInexistentPet_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        while (DbContext.Pets.Any(m => m.Id == id))
            id = Guid.NewGuid();
        
        // Act
        var response = await HttpClient.DeleteAsync($"/v1/pets/{id}");
        
        // Assert
        response.Should().Be404NotFound();
    }
}