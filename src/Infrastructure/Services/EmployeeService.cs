using System;
using API.Features.Employees.Models.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Artikelsystem.Shared.DTOs;
using Domain.Entities.Employees;
using Infrastructure.Common;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class EmployeeService(AppDbContext context) : IEmployeeService
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
