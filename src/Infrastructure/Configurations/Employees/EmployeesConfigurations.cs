using System;
using API.Features.Employees.Models.Entitys;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Employees.Configurations;

public class EmployeesConfigurations : IEntityTypeConfiguration<Employee>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Employee> builder)
    {

    }
}
