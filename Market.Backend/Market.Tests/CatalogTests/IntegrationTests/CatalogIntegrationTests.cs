using Market.Application.Modules.Catalog.CatalogHome.Queries.GetCatalogHome;
using System.Net.Http.Json;

namespace Market.Tests.CatalogTests.IntegrationTests;

[Collection(IntegrationTestCollection.Name)]
public class CatalogIntegrationTests
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public CatalogIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_CatalogHome_Anonymous_ReturnsSeededData()
    {
        // Arrange
        var client = _factory.CreateClient(); // no Bearer token

        // Act
        var home = await client.GetFromJsonAsync<GetCatalogHomeQueryDto>("/Catalog/home?categoryLimit=3&productLimit=5");

        // Assert
        Assert.NotNull(home);
        Assert.Equal(3, home.Categories.Count);
        Assert.Equal(5, home.NewestProducts.Count);
        Assert.True(home.Stats.TotalProducts >= home.NewestProducts.Count);
    }
}
