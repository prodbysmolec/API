using System;

namespace Application.DTOs.Employee;

public class EmployeeListContainerDto
{
    public List<EmployeeListDto> Employees { get; set; } = new List<EmployeeListDto>();
}
