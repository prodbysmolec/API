using System;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1;

public class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
}
