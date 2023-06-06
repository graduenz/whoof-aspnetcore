using System.Net.Http.Json;
using FluentAssertions;
using Whoof.Api.Entities;
using Whoof.Api.Enums;
using Whoof.Api.Models;
using Whoof.Tests.Api.Support;

namespace Whoof.Tests.Api;

public class PetsControllerTests : BaseControllerTests
{
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
            PetType = PetType.Dog,
            OwnerId = Guid.NewGuid()
        };
        
        // Act
        var response = await HttpClient.PostAsJsonAsync("v1/pets/", pet);
        
        // Assert
        response.Should()
            .NotBeNull().And
            .Be201Created().And
            .BeAs(pet, c => c
                .Excluding(m => m.Id));
    }

    public static IEnumerable<object[]> _AddOneAsync_WithInvalidData_DoesntAdd()
    {
        yield return new object[] {
            new Pet { Name = "", PetType = PetType.Capybara },
            new[] { "'Name' must not be empty." }
        };
        
        yield return new object[] {
            new Pet { Name = "Qwerty", PetType = PetType.Unspecified },
            new[] { "'Pet Type' must not be equal to 'Unspecified'." }
        };
    }

    [Theory]
    [MemberData(nameof(_AddOneAsync_WithInvalidData_DoesntAdd))]
    public async Task AddOneAsync_WithInvalidData_DoesntAdd(Pet pet, string[] expectedErrors)
    {
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
    }
}