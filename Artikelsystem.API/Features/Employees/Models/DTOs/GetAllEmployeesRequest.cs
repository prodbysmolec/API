namespace Artikelsystem.Api.Features.Employees.Models.DTOs;

public class GetAllEmployeesRequest
{
    public int? Page { get; set; }
    public int? RecordsPerPage { get; set; }
    
    public string? FirstNameContains { get; set; }
    public string? LastNameContains { get; set; }
}
