using System.Net;

namespace Market.Tests.FloorPlanTests.IntegrationTests;

[Collection(IntegrationTestCollection.Name)]
public class FloorPlanIntegrationTests
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public FloorPlanIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_FloorPlan_Employee_ShouldReturnOk()
    {
        var client = await _factory.GetAuthenticatedClientAsync(
            CustomWebApplicationFactory<Program>.EmployeeEmail,
            CustomWebApplicationFactory<Program>.EmployeePassword);

        var response = await client.GetAsync("/FloorPlan");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_FloorPlan_Customer_ShouldReturnForbidden()
    {
        var client = await _factory.GetAuthenticatedClientAsync("nina.bijedic@email.com", "Customer123!");

        var response = await client.GetAsync("/FloorPlan");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}
