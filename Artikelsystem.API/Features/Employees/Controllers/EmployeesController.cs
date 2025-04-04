using System;
using System.Runtime.CompilerServices;
using Artikelsystem.Api.Features.Employees.Models.DTOs;
using Artikelsystem.Api.Features.Employees.Models.Entitys;
using Artikelsystem.Api.Infrastructure.Persistence.Context;
using Artikelsystem.API.Shared.Controllers;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Artikelsystem.Api.Employees.Controllers;

public class EmployeesController : BaseController
{

    private readonly ILogger<EmployeesController> _logger;
    private readonly AppDbContext _dbContext;

    public EmployeesController(
        ILogger<EmployeesController> logger,
        AppDbContext dbContext
    )
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    /// <summary>
    /// Gets all the employees in the system.
    /// </summary>
    /// <returns>Returns the employees in a JSON array.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetEmployeeResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<GetEmployeeResponse>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllEmployees([FromQuery] GetAllEmployeesRequest? request)
    {
        int page = request?.Page ?? 1;
        int recordsPerPage = request?.RecordsPerPage ?? 100;

        IQueryable<Employee> query = _dbContext.Employees
            .Include(e => e.Benefits)
            .Skip((page - 1) * recordsPerPage)
            .Take(recordsPerPage);

        if (request != null)
        {
            if (!string.IsNullOrWhiteSpace(request.FirstNameContains))
            {
                query = query.Where(e => e.FirstName.Contains(request.FirstNameContains));
            }

            if (!string.IsNullOrWhiteSpace(request.LastNameContains))
            {
                query = query.Where(e => e.LastName.Contains(request.LastNameContains));
            }
        }

        var employees = await query.ToArrayAsync();

        return Ok(employees.Select(EmployeeToGetEmployeeResponse));
    }

    /// <summary>
    /// Gets an employee by ID.
    /// </summary>
    /// <param name="id">The ID of the employee.</param>
    /// <returns>The single Employee record.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(GetEmployeeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetEmployeeById(int id)
    {
        var employee = await _dbContext.Employees.SingleOrDefaultAsync(e => e.Id == id);
        if (employee == null)
        {
            return NotFound();
        }

        var employeeResponse = EmployeeToGetEmployeeResponse(employee);

        return Ok(employeeResponse);
    }

    /// <summary>
    /// Creates an employee.
    /// </summary>
    /// <param name="employeeRequest">The employee data to create.</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(GetEmployeeResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeRequest employeeRequest)
    {
        var newEmployee = new Employee
        {
            FirstName = employeeRequest.FirstName!,
            LastName = employeeRequest.LastName!,
            SocialSecurityNumber = employeeRequest.SocialSecurityNumber,
            Address1 = employeeRequest.Address1,
            Address2 = employeeRequest.Address2,
            City = employeeRequest.City,
            State = employeeRequest.State,
            ZipCode = employeeRequest.ZipCode,
            PhoneNumber = employeeRequest.PhoneNumber,
            Email = employeeRequest.Email
        };
        _dbContext.Employees.Add(newEmployee);
        await _dbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetEmployeeById), new { id = newEmployee.Id }, newEmployee);
    }

    /// <summary>
    /// Updates an employee.
    /// </summary>
    /// <param name="id">The ID of the employee to update.</param>
    /// <param name="employeeRequest">The employee Data to update.</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(GetEmployeeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdateEmployeeRequest employeeRequest)
    {
        _logger.LogInformation("Updating employee with ID: {EmployeeId}", id);

        var existingEmployee = await _dbContext.Employees
            .AsTracking()
            .SingleOrDefaultAsync(e => e.Id == id);
        if (existingEmployee == null)
        {
            _logger.LogWarning("Employee with ID: {EmployeeId} not found", id);
            return NotFound();
        }

        _logger.LogDebug("Updating employee details for ID: {EmployeeId}", id);
        existingEmployee.Address1 = employeeRequest.Address1;
        existingEmployee.Address2 = employeeRequest.Address2;
        existingEmployee.City = employeeRequest.City;
        existingEmployee.State = employeeRequest.State;
        existingEmployee.ZipCode = employeeRequest.ZipCode;
        existingEmployee.PhoneNumber = employeeRequest.PhoneNumber;
        existingEmployee.Email = employeeRequest.Email;

        var hasChanged = _dbContext.ChangeTracker.HasChanges();
        try
        {
            if (!hasChanged)
            {
                _logger.LogInformation("No changes detected for employee with ID: {EmployeeId}", id);
                return BadRequest("No changes detected for employee update.");
            }
            _dbContext.Entry(existingEmployee).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Employee with ID: {EmployeeId} successfully updated", id);
            return Ok(existingEmployee);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating employee with ID: {EmployeeId}", id);
            return StatusCode(500, "An error occurred while updating the employee");
        }
    }

    /// <summary>
    /// Deletes an employee.
    /// </summary>
    /// <param name="id">The ID of the employee to delete.</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var employee = await _dbContext.Employees.FindAsync(id);

        if (employee == null)
        {
            return NotFound();
        }

        _dbContext.Employees.Remove(employee);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Gets the benefits for an employee.
    /// </summary>
    /// <param name="employeeId">The ID to get the benefits for.</param>
    /// <returns>The benefits for that employee.</returns>
    [HttpGet("{employeeId}/benefits")]
    [ProducesResponseType(typeof(IEnumerable<GetEmployeeResponseEmployeeBenefit>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBenefitsForEmployee(int employeeId)
    {
        var employee = await _dbContext.Employees
            .Include(e => e.Benefits)
            .ThenInclude(e => e.Benefit)
            .SingleOrDefaultAsync(e => e.Id == employeeId);

        if (employee == null)
        {
            return NotFound();
        }

        var benefits = employee.Benefits.Select(b => new GetEmployeeResponseEmployeeBenefit
        {
            Id = b.Id,
            Name = b.Benefit.Name,
            Description = b.Benefit.Description,
            Cost = b.CostToEmployee ?? b.Benefit.BaseCost
        });

        return Ok(benefits);
    }

    private static GetEmployeeResponse EmployeeToGetEmployeeResponse(Employee employee)
    {
        return new GetEmployeeResponse
        {
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Address1 = employee.Address1,
            Address2 = employee.Address2,
            City = employee.City,
            State = employee.State,
            ZipCode = employee.ZipCode,
            PhoneNumber = employee.PhoneNumber,
            Email = employee.Email
        };
    }
}