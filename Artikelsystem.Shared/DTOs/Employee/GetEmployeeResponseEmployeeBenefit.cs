namespace API.Features.Employees.Models.DTOs;

public class GetEmployeeResponseEmployeeBenefit
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public decimal Cost { get; set; }
}