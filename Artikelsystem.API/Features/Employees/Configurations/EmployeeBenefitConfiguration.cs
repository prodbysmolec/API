using System;
using Artikelsystem.Api.Features.Employees.Models.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artikelsystem.Api.Features.Employees.Configurations;

public class EmployeeBenefitConfiguration : IEntityTypeConfiguration<EmployeeBenefit>
{
    public void Configure(EntityTypeBuilder<EmployeeBenefit> builder)
    {
            builder
                .HasIndex(eb => new { eb.EmployeeId, eb.BenefitId })
                .IsUnique();
    }
}
