using System;
using Artikelsystem.Api.Features.Employees.Models.Entitys;
using Microsoft.EntityFrameworkCore;

namespace Artikelsystem.Api.Features.Employees.Configurations;

public class EmployeesConfigurations : IEntityTypeConfiguration<Employee>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Employee> builder)
    {
        
    }
}
