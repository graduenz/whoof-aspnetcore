using System.Net.Http.Json;
using FluentAssertions;
using Whoof.Api.Entities;
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
}