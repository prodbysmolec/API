
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Employees;
namespace EmployeeAPI.Tests;

public class BasicTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly int _employeeId = 1;

    private readonly CustomWebApplicationFactory _factory;
    private int _employeeIdForAdressTest;

    public BasicTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetAllEmployees_ReturnsOkResult()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/employees");

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetEmployeeById_ReturnsOkResult()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/employees/1");
        
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetAllEmployees_WithFilter_ReturnsOneResult()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/employees?FirstNameContains=John");

        response.EnsureSuccessStatusCode();

        var employees = await response.Content.ReadFromJsonAsync<IEnumerable<GetEmployeeResponse>>();
        Assert.Single(employees);
    }

    /*

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
        var response = await client.PutAsJsonAsync("/employees/1", new Employee {
            FirstName = "John", LastName = "Doe", Address1 = "TestAdress" 
            });

        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task UpdateEmployee_ReturnsBadRequestWhenAddress()
    {
        // Arrange
        var client = _factory.CreateClient();
        var invalidEmployee = new UpdateEmployeeRequest(); // Empty object to trigger validation errors

        // Act
        var response = await client.PutAsJsonAsync($"/employees/{_employeeIdForAdressTest}", invalidEmployee);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
        Assert.NotNull(problemDetails);
        Assert.Contains("Address1", problemDetails.Errors.Keys);
    }

    [Fact]
    public async Task GetBenefitsForEmployee_ReturnsOkResult()
    {
        // Act
        var client = _factory.CreateClient();
        var response = await client.GetAsync($"/employees/{_employeeId}/benefits");

        // Assert
        response.EnsureSuccessStatusCode();
        
        var benefits = await response.Content.ReadFromJsonAsync<IEnumerable<GetEmployeeResponseEmployeeBenefit>>();
        Assert.Equal(2, benefits.Count());
    }
    */
}