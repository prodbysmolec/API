using System;
using Microsoft.EntityFrameworkCore;

namespace Artikelsystem.Api;

public static class SeedData
{

public static void MigrateAndSeed(IServiceProvider serviceProvider)
{
    var context = serviceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();

    if (!context.Employees.Any())
    {
        var employees = new List<Employee>
        {
            new Employee
            {
                FirstName = "John",
                LastName = "Doe",
                SocialSecurityNumber = "123-45-6789",
                Address1 = "123 Main St",
                City = "Anytown",
                State = "NY",
                ZipCode = "12345",
                PhoneNumber = "555-123-4567",
                Email = "john.doe@example.com"
            },
            new Employee
            {
                FirstName = "Jane",
                LastName = "Smith",
                SocialSecurityNumber = "987-65-4321",
                Address1 = "456 Elm St",
                Address2 = "Apt 2B",
                City = "Othertown",
                State = "CA",
                ZipCode = "98765",
                PhoneNumber = "555-987-6543",
                Email = "jane.smith@example.com"
            }
        };

        context.Employees.AddRange(employees);
        context.SaveChanges();
    }

    if (!context.Benefits.Any())
    {
        var benefits = new List<Benefit>
        {
            new Benefit { Name = "Health", Description = "Medical, dental, and vision coverage", BaseCost = 100.00m },
            new Benefit { Name = "Dental", Description = "Dental coverage", BaseCost = 50.00m },
            new Benefit { Name = "Vision", Description = "Vision coverage", BaseCost = 30.00m }
        };

        context.Benefits.AddRange(benefits);
        context.SaveChanges();

        // Besorge IDs
        var healthBenefitId = context.Benefits.Single(b => b.Name == "Health").Id;
        var dentalBenefitId = context.Benefits.Single(b => b.Name == "Dental").Id;
        var visionBenefitId = context.Benefits.Single(b => b.Name == "Vision").Id;

        var john = context.Employees.Single(e => e.FirstName == "John");
        var jane = context.Employees.Single(e => e.FirstName == "Jane");

        var employeeBenefits = new List<EmployeeBenefit>
        {
            // John's Benefits
            new EmployeeBenefit { 
                EmployeeId = john.Id, 
                BenefitId = healthBenefitId, 
                CostToEmployee = 100m 
            },
            new EmployeeBenefit { 
                EmployeeId = john.Id, 
                BenefitId = dentalBenefitId 
            },
            
            // Jane's Benefits
            new EmployeeBenefit { 
                EmployeeId = jane.Id, 
                BenefitId = healthBenefitId, 
                CostToEmployee = 120m 
            },
            new EmployeeBenefit { 
                EmployeeId = jane.Id, 
                BenefitId = visionBenefitId 
            }
        };

        context.EmployeeBenefits.AddRange(employeeBenefits);
        context.SaveChanges();
    }
}
}