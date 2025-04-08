using System;
using System.Net;

namespace Artikelsystem.API.Tests.Tests.Features.Warenausgang;

public class WarenausgangControllerTests : IClassFixture<CustomWebApplicationFactory>
{ 
    private readonly CustomWebApplicationFactory _factory;

    public WarenausgangControllerTests()
    {
        _factory = new CustomWebApplicationFactory();
    }

    [Fact]    
    public async Task GetAllWarenausgaenge_ReturnsSuccessResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = "/Warenausgang";

        // Act
        var response = await client.GetAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetWarenausgangById_ReturnsSuccessResult()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = "/Warenausgang/1";

        // Act
        var response = await client.GetAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
}

