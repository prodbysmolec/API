using System;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1;

public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Benefit> Benefits { get; set; }
    public DbSet<EmployeeBenefit> EmployeeBenefits { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    modelBuilder.Entity<EmployeeBenefit>()
        .HasIndex(eb => new { eb.EmployeeId, eb.BenefitId })
        .IsUnique();

    modelBuilder.Entity<Employee>()
        .HasMany(e => e.Benefits)
        .WithOne(eb => eb.Employee)
        .HasForeignKey(eb => eb.EmployeeId);

    modelBuilder.Entity<EmployeeBenefit>()
        .HasKey(eb => new { eb.EmployeeId, eb.BenefitId })
        .HasName("PK_EmployeeBenefit");
    }
}
