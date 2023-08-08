using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Whoof.Api.Common.Models;
using Whoof.Application.Common.Models;
using Whoof.Application.Vaccines.Dto;
using Whoof.Domain.Entities;
using Whoof.Domain.Enums;
using Whoof.Tests.Api.Support;
using Whoof.Tests.Extensions;

namespace Whoof.Tests.Api;

public class VaccinesControllerTests : BaseControllerTests
{
    private static readonly Guid IdVaccineWithEmptyName = Guid.NewGuid();
    private static readonly Guid IdVaccineWithUnspecifiedType = Guid.NewGuid();
    private static readonly Guid IdVaccineWithNoDuration = Guid.NewGuid();

    public VaccinesControllerTests()
    {
        HttpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", JwtGenerator.GenerateBasicJwt());
    }

    public static IEnumerable<object[]> MemberData_Vaccines_And_ValidationErrors()
    {
        yield return new object[]
        {
            new VaccineDto { Id = IdVaccineWithEmptyName, Name = "", PetType = PetType.Capybara, Duration = 1 },
            new[] { "'Name' must not be empty." }
        };

        yield return new object[]
        {
            new VaccineDto { Id = IdVaccineWithUnspecifiedType, Name = "Qwerty", PetType = PetType.Unspecified, Duration = 365 },
            new[] { "'Pet Type' must not be equal to 'Unspecified'." }
        };

        yield return new object[]
        {
            new VaccineDto { Id = IdVaccineWithNoDuration, Name = "Doggo", PetType = PetType.Dog, Duration = 0 },
            new[] { "'Duration' must be greater than or equal to '1'." }
        };
    }

    [Theory]
    [InlineData(1, 10)]
    [InlineData(2, 1)]
    public async Task GetPaginatedListAsync_WithDefaultData_ReturnsAsExpected(int pageIndex, int expectedPageSize)
    {
        // Act
        var result = await HttpClient.GetFromJsonAsync<PaginatedList<VaccineDto>>(
            $"/v1/vaccines?pageIndex={pageIndex}&pageSize={10}", JsonOptions
        );

        // Assert
        result.Should().NotBeNull();
        result.PageIndex.Should().Be(pageIndex);
        result.TotalPages.Should().Be(2);
        result.Items.Should().HaveCount(expectedPageSize);
    }

    [Fact]
    public async Task GetByIdAsync_WhenVaccineExists_ReturnsAsExpected()
    {
        // Arrange
        var expectedVaccine = Mapper.Map<VaccineDto>(DbContext.Vaccines.First());

        // Act
        var actualVaccine = await HttpClient.GetFromJsonAsync<VaccineDto>(
            $"/v1/vaccines/{expectedVaccine.Id}", JsonOptions
        );

        // Assert
        actualVaccine.Should()
            .NotBeNull().And
            .BeEquivalentTo(expectedVaccine, c => c
                .ExcludingBaseFields());
    }

    [Fact]
    public async Task GetByIdAsync_WhenVaccineDoesntExist_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        while (DbContext.Vaccines.Any(m => m.Id == id))
            id = Guid.NewGuid();

        // Act
        var response = await HttpClient.GetAsync($"/v1/vaccines/{id}");

        // Assert
        response.Should().Be404NotFound();
    }

    [Fact]
    public async Task PostAsync_WithValidFields_AddsAsExpected()
    {
        // Arrange
        var vaccine = new VaccineDto
        {
            Name = "Vaccine",
            PetType = PetType.Dog,
            Duration = 1
        };

        var beforeCount = await DbContext.Vaccines.CountAsync();

        // Act
        var response = await HttpClient.PostAsJsonAsync("v1/vaccines/", vaccine, JsonOptions);

        // Assert
        response.Should()
            .NotBeNull().And
            .Be201Created().And
            .BeAs(vaccine, c => c
                .ExcludingBaseFields());

        vaccine = await response.Content.ReadFromJsonAsync<VaccineDto>(JsonOptions);

        var afterCount = await DbContext.Vaccines.CountAsync();
        afterCount.Should().Be(beforeCount + 1);

        DbContext.Vaccines.Should().Contain(m => m.Id == vaccine.Id);
    }

    [Theory]
    [MemberData(nameof(MemberData_Vaccines_And_ValidationErrors))]
    public async Task PostAsync_WithInvalidData_DoesntAdd(VaccineDto vaccine, string[] expectedErrors)
    {
        // Arrange
        var beforeCount = await DbContext.Vaccines.CountAsync();

        // Act
        var response = await HttpClient.PostAsJsonAsync("v1/vaccines/", vaccine, JsonOptions);

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

        var afterCount = await DbContext.Vaccines.CountAsync();
        afterCount.Should().Be(beforeCount);

        DbContext.Vaccines.Should().NotContain(m => m.Id == vaccine.Id);
    }

    [Fact]
    public async Task PutAsync_WithValidData_UpdatesAsExpected()
    {
        // Arrange
        var vaccine = Mapper.Map<VaccineDto>(DbContext.Vaccines.AsNoTracking().First());
        vaccine.Name = "Updated";

        // Act
        var response = await HttpClient.PutAsJsonAsync($"/v1/vaccines/{vaccine.Id}", vaccine, JsonOptions);

        // Assert
        response.Should()
            .NotBeNull().And
            .Be200Ok().And
            .BeAs(vaccine, c => c
                .ExcludingBaseFields());

        vaccine = Mapper.Map<VaccineDto>(DbContext.Vaccines.AsNoTracking().First(m => m.Id == vaccine.Id));
        vaccine.Name.Should().Be("Updated");
    }

    [Fact]
    public async Task PutAsync_WithNonexistentVaccine_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        while (DbContext.Vaccines.Any(m => m.Id == id))
            id = Guid.NewGuid();

        var vaccine = Mapper.Map<VaccineDto>(DbContext.Vaccines.First());
        vaccine.Id = id;

        // Act
        var response = await HttpClient.PutAsJsonAsync($"/v1/vaccines/{vaccine.Id}", vaccine, JsonOptions);

        // Assert
        response.Should().Be404NotFound();
    }

    [Theory]
    [MemberData(nameof(MemberData_Vaccines_And_ValidationErrors))]
    public async Task PutAsync_WithInvalidData_DoesntUpdate(VaccineDto vaccine, string[] expectedErrors)
    {
        // Assert
        await DbContext.Vaccines.AddAsync(new Vaccine
        {
            Id = vaccine.Id,
            Name = "Dollar",
            PetType = PetType.Dog
        });

        // Act
        var response = await HttpClient.PutAsJsonAsync($"v1/vaccines/{vaccine.Id}", vaccine, JsonOptions);

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
    public async Task DeleteAsync_WithExistingVaccine_DeletesAsExpected()
    {
        // Arrange
        var vaccine = DbContext.Vaccines.First();

        // Act
        var response = await HttpClient.DeleteAsync($"/v1/vaccines/{vaccine.Id}");

        // Assert
        response.Should().Be200Ok();

        DbContext.Vaccines.Should().NotContain(m => m.Id == vaccine.Id);
    }

    [Fact]
    public async Task DeleteAsync_WithNonexistentVaccine_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        while (DbContext.Vaccines.Any(m => m.Id == id))
            id = Guid.NewGuid();

        // Act
        var response = await HttpClient.DeleteAsync($"/v1/vaccines/{id}");

        // Assert
        response.Should().Be404NotFound();
    }
}