using System;
using System.Runtime.CompilerServices;

namespace Artikelsystem.Shared.DTOs.Employee.Response;

public record EmployeeDto
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? SocialSecurityNumber { get; set; }

    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public List<EmployeeBenefitDto> Benefits { get; set; } = new List<EmployeeBenefitDto>();
}

public record EmployeeBenefitDto
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }
    public EmployeeDto Employee { get; set; } = null!;

    public int BenefitId { get; set; }
    public BenefitDto Benefit { get; set; } = null!;

    public decimal? CostToEmployee { get; set; }
}

public record BenefitDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public decimal CostToEmployee { get; set; }
    public decimal CostToEmployer { get; set; }
    public bool IsActive { get; set; } = true;
}
