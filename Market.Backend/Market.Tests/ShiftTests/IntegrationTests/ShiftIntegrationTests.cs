using System.Net;

namespace Market.Tests.ShiftTests.IntegrationTests;

[Collection(IntegrationTestCollection.Name)]
public class ShiftIntegrationTests
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public ShiftIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_CurrentShift_Employee_WithoutOpenShift_ShouldReturnNoContent()
    {
        var client = await _factory.GetAuthenticatedClientAsync(
            CustomWebApplicationFactory<Program>.EmployeeEmail,
            CustomWebApplicationFactory<Program>.EmployeePassword);

        var response = await client.GetAsync("/Shifts/current");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Get_Shifts_Employee_ShouldReturnForbidden()
    {
        var client = await _factory.GetAuthenticatedClientAsync(
            CustomWebApplicationFactory<Program>.EmployeeEmail,
            CustomWebApplicationFactory<Program>.EmployeePassword);

        var response = await client.GetAsync("/Shifts");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Get_Shifts_Anonymous_ShouldReturnUnauthorized()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/Shifts");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
