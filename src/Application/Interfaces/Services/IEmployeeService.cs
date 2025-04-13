using System;
using Artikelsystem.Shared.DTOs;
using Domain.Common.ResultPattern;
using Domain.Entities.Employees;

namespace Application.Interfaces.Services;

public interface IEmployeeService
{
    Task<Employee> GetEmployeeByIdAsync(int id);
    Task<PagedResultDTO<Employee>> GetAllEmployeesAsync(int page,
        int recordsPerPage,
        string? firstNameContains = null,
        string? lastNameContains = null);
}
