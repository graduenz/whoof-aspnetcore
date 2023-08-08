using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Whoof.Api.Common.Models;
using Whoof.Application.Common.Models;
using Whoof.Application.Pets.Dto;
using Whoof.Domain.Entities;
using Whoof.Domain.Enums;
using Whoof.Tests.Api.Support;
using Whoof.Tests.Extensions;

namespace Whoof.Tests.Api;

public class PetsControllerTests : BaseControllerTests
{
    private static readonly Guid IdPetWithEmptyName = Guid.NewGuid();
    private static readonly Guid IdPetWithUnspecifiedType = Guid.NewGuid();

    public PetsControllerTests()
    {
        HttpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", JwtGenerator.GenerateBasicJwt());
    }

    public static IEnumerable<object[]> MemberData_Pets_And_ValidationErrors()
    {
        yield return new object[]
        {
            new PetDto { Id = IdPetWithEmptyName, Name = "", PetType = PetType.Capybara },
            new[] { "'Name' must not be empty." }
        };

        yield return new object[]
        {
            new PetDto { Id = IdPetWithUnspecifiedType, Name = "Qwerty", PetType = PetType.Unspecified },
            new[] { "'Pet Type' must not be equal to 'Unspecified'." }
        };
    }

    [Theory]
    [InlineData(1, 20)]
    [InlineData(2, 20)]
    [InlineData(3, 10)]
    public async Task GetPaginatedListAsync_WithDefaultData_ReturnsAsExpected(int pageIndex, int expectedPageSize)
    {
        // Act
        var result = await HttpClient.GetFromJsonAsync<PaginatedList<PetDto>>(
            $"/v1/pets?pageIndex={pageIndex}&pageSize={20}", JsonOptions
        );

        // Assert
        result.Should().NotBeNull();
        result.PageIndex.Should().Be(pageIndex);
        result.TotalPages.Should().Be(3);
        result.Items.Should().HaveCount(expectedPageSize);
    }

    [Fact]
    public async Task GetByIdAsync_WhenPetExists_ReturnsAsExpected()
    {
        // Arrange
        var expectedPet = Mapper.Map<PetDto>(DbContext.Pets.First());

        // Act
        var actualPet = await HttpClient.GetFromJsonAsync<PetDto>(
            $"/v1/pets/{expectedPet.Id}", JsonOptions
        );

        // Assert
        actualPet.Should()
            .NotBeNull().And
            .BeEquivalentTo(expectedPet, c => c
                .ExcludingBaseFields()
                .ExcludingOwnershipFields());
    }

    [Fact]
    public async Task GetByIdAsync_WhenPetDoesntExist_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        while (DbContext.Pets.Any(m => m.Id == id))
            id = Guid.NewGuid();

        // Act
        var response = await HttpClient.GetAsync($"/v1/pets/{id}");

        // Assert
        response.Should().Be404NotFound();
    }

    [Fact]
    public async Task PostAsync_WithValidFields_AddsAsExpected()
    {
        // Arrange
        var pet = new PetDto
        {
            Name = "Ravena",
            PetType = PetType.Dog
        };

        var beforeCount = await DbContext.Pets.CountAsync();

        // Act
        var response = await HttpClient.PostAsJsonAsync("v1/pets/", pet, JsonOptions);

        // Assert
        response.Should()
            .NotBeNull().And
            .Be201Created().And
            .BeAs(pet, c => c
                .ExcludingBaseFields()
                .ExcludingOwnershipFields());

        pet = await response.Content.ReadFromJsonAsync<PetDto>(JsonOptions);

        var afterCount = await DbContext.Pets.CountAsync();
        afterCount.Should().Be(beforeCount + 1);

        DbContext.Pets.Should().Contain(m => m.Id == pet.Id);
    }

    [Theory]
    [MemberData(nameof(MemberData_Pets_And_ValidationErrors))]
    public async Task PostAsync_WithInvalidData_DoesntAdd(PetDto pet, string[] expectedErrors)
    {
        // Arrange
        var beforeCount = await DbContext.Pets.CountAsync();

        // Act
        var response = await HttpClient.PostAsJsonAsync("v1/pets/", pet, JsonOptions);

        // Assert
        response.Should()
            .NotBeNull().And
            .Be400BadRequest();

        var result = await response.Content.ReadFromJsonAsync<ValidationErrorsResult>(JsonOptions);
        result.Should().NotBeNull();
        result.Code.Should().Be(ServiceError.Validation.Code);
        result.Message.Should().Be(ServiceError.Validation.Message);
        result.Errors.SelectMany(m => m.Value).Should()
            .BeEquivalentTo(expectedErrors);

        var afterCount = await DbContext.Pets.CountAsync();
        afterCount.Should().Be(beforeCount);

        DbContext.Pets.Should().NotContain(m => m.Id == pet.Id);
    }

    [Fact]
    public async Task PutAsync_WithValidData_UpdatesAsExpected()
    {
        // Arrange
        var pet = Mapper.Map<PetDto>(DbContext.Pets.AsNoTracking().First());
        pet.Name = "Updated";

        // Act
        var response = await HttpClient.PutAsJsonAsync($"/v1/pets/{pet.Id}", pet, JsonOptions);

        // Assert
        response.Should()
            .NotBeNull().And
            .Be200Ok().And
            .BeAs(pet, c => c
                .ExcludingBaseFields()
                .ExcludingOwnershipFields());

        pet = Mapper.Map<PetDto>(DbContext.Pets.AsNoTracking().First(m => m.Id == pet.Id));
        pet.Name.Should().Be("Updated");
    }

    [Fact]
    public async Task PutAsync_WithNonexistentPet_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        while (DbContext.Pets.Any(m => m.Id == id))
            id = Guid.NewGuid();

        var pet = Mapper.Map<PetDto>(DbContext.Pets.First());
        pet.Id = id;

        // Act
        var response = await HttpClient.PutAsJsonAsync($"/v1/pets/{pet.Id}", pet, JsonOptions);

        // Assert
        response.Should().Be404NotFound();
    }

    [Theory]
    [MemberData(nameof(MemberData_Pets_And_ValidationErrors))]
    public async Task PutAsync_WithInvalidData_DoesntUpdate(PetDto pet, string[] expectedErrors)
    {
        // Assert
        await DbContext.Pets.AddAsync(new Pet
        {
            Id = pet.Id,
            Name = "Dollar",
            PetType = PetType.Dog
        });

        // Act
        var response = await HttpClient.PutAsJsonAsync($"v1/pets/{pet.Id}", pet, JsonOptions);

        // Assert
        response.Should()
            .NotBeNull().And
            .Be400BadRequest();

        var result = await response.Content.ReadFromJsonAsync<ValidationErrorsResult>(JsonOptions);
        result.Should().NotBeNull();
        result.Code.Should().Be(ServiceError.Validation.Code);
        result.Message.Should().Be(ServiceError.Validation.Message);
        result.Errors.SelectMany(m => m.Value).Should()
            .BeEquivalentTo(expectedErrors);
    }

    [Fact]
    public async Task DeleteAsync_WithExistingPet_DeletesAsExpected()
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
    public async Task DeleteAsync_WithNonexistentPet_ReturnsNotFound()
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