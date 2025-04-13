using System;
using Application.DTOs.Employee;
using Application.Interfaces.Repositories;
using Domain.Common.ResultPattern;
using Domain.Entities.Employees;
using Domain.Errors;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class EmployeeRepository(AppDbContext context) 
    : GenericRepository<Employee>(context), IEmployeeRepository
{
    private readonly AppDbContext _context = context;

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

    public async Task<Result<IEnumerable<Employee>>> GetEmployeeByIdAsync(int id)
    {
        return await _context.Employees
            .Where(e => e.Id == id)
            .AsNoTracking()
            .ToListAsync();
    }

}
