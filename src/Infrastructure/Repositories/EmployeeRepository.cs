using System;
using Application.DTOs.Employee;
using Application.Interfaces.Repositories;
using Domain.Common.ResultPattern;
using Domain.Entities.Employees;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class EmployeeRepository(AppDbContext context) 
    : GenericRepository<Employee>(context), IEmployeeRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Result<IEnumerable<Employee>>> GetEmployeeByIdAsync(int id)
    {
        return await _context.Employees
            .Where(e => e.Id == id)
            .AsNoTracking()
            .ToListAsync();
    }

}
