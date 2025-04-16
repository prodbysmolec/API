using API.Features.Employees.Models.DTOs;
using Application.Interfaces.Repositories;
using Artikelsystem.Shared.DTOs;
using Domain.Common.ResultPattern;
using Domain.Entities.Employees;
using Domain.Errors;
using Infrastructure.Common;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EmployeeRepository(AppDbContext context) : IEmployeeRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Employee> AddEmployeeAsync(Employee employee)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return await Task.FromResult(employee);
    }

    public async Task<PagedResultDTO<Employee>> GetAllEmployeesAsync(int page, int recordsPerPage, string? firstNameContains = null, string? lastNameContains = null)
    {
        IQueryable<Employee> query = _context.Employees.Include(e => e.Benefits);

        // Filterung anwenden
        if (!string.IsNullOrWhiteSpace(firstNameContains))
        {
            query = query.Where(e => e.FirstName.Contains(firstNameContains));
        }

        if (!string.IsNullOrWhiteSpace(lastNameContains))
        {
            query = query.Where(e => e.LastName.Contains(lastNameContains));
        }

        // Paging anwenden
        return await PagingService.ApplyPagingAsync(query, page, recordsPerPage);
    }



    public Task<Employee?> UpdateEmployeeAsync(int id, UpdateEmployeeRequest employeeRequest)
    {
        throw new NotImplementedException();
    }

/// <summary>
    /// Prüft, ob ein Employee mit der angegebenen E-Mail-Adresse existiert.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public async Task<Result<bool>> EmailExistsAsync(string email)
    {
        var exists = await _context.Employees.AnyAsync(e => e.Email == email);

        if (exists)
        {
            return Result<bool>.Success(true);
        }
        return Result<bool>.Failure(EmployeeErrors.EMailAlreadyExists(email));
    }

    public async Task<Employee> GetEmployeeByIdAsync(int id)
    {
        var employee = await _context.Employees
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync();
        var employees = await _context.Employees.ToListAsync();
        if(employee == null)
        {
            throw new Exception($"Employee with ID {id} not found.");
        }
        
        return employee;
    }
}
