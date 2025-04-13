using System;
using Application.DTOs.Employee;
using Domain.Common.ResultPattern;
using Domain.Entities.Employees;

namespace Application.Interfaces.Repositories;

public interface IEmployeeRepository : IGenericRepository<Employee>
{
    public Task<Result<IEnumerable<Employee>>> GetEmployeeByIdAsync(int id); 
    public Task<Result<bool>> EmailExistsAsync(string email);
}
