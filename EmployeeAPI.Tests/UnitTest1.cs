
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using WebApplication1.Employees;
namespace EmployeeAPI.Tests;

public class BasicTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public BasicTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetEmployeeById_ReturnsOkResult()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/employees/1");
        
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task CreateEmployee_ReturnsCreatedResult()
    {
        var client = _factory.CreateClient();
        var response = await client.PostAsJsonAsync("/employees", new Employee { 
            FirstName = "Lukas", 
            LastName = "Schmolz",
            SocialSecurityNumber = "12345"
            });

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task CreateEmploye_ReturnsBadRequestResult()
    {
        // Arrange 
        var client = _factory.CreateClient();
        var invalidEmployee = new CreateEmployeeRequest(); // Leeres Objekt

        // Act
        var response = await client.PostAsJsonAsync("/employees", invalidEmployee);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.NotNull(problemDetails);
        Assert.Contains("FirstName", problemDetails.Errors.Keys);
        Assert.Contains("LastName", problemDetails.Errors.Keys);
        Assert.Contains("First name is required.", problemDetails.Errors["FirstName"]);
        Assert.Contains("Last name is required.", problemDetails.Errors["LastName"]);
    }

    [Fact]
    public async Task UpdateEmployee_ReturnsOkResult()
    {
        var client = _factory.CreateClient();
        var response = await client.PutAsJsonAsync("/employees/1", new Employee {FirstName = "Linda", LastName = "Schmolz", SocialSecurityNumber = "5131-123"});
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task UpdateEmployee_ReturnsNotFoundForNonExistentEmployee()
    {
        var client = _factory.CreateClient();
        var response = await client.PutAsJsonAsync("/employees/1123123123", new Employee {FirstName = "Linda", LastName = "Schmolz", SocialSecurityNumber = "5131-123"});
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}