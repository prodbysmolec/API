using System;
using System.Collections.Generic;
using Artikelsystem.Api.Features.Artikel.Models.Entitys;
using Artikelsystem.Api.Features.Employees.Enums;
using Artikelsystem.Api.Features.Employees.Models.Entitys;
using Artikelsystem.Api.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Artikelsystem.Api.Infrastructure.Persistence.Seeding;

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

        // Seed Artikel data if none exists
        if (!context.Artikel.Any())
        {
            // Parse the date and convert to UTC
            // Use DateTime.SpecifyKind to ensure the DateTime is marked as UTC
            var currentDateTime = DateTime.SpecifyKind(
                DateTime.Parse("2025-03-30 16:12:01"), 
                DateTimeKind.Utc
            );
            
            var currentUser = "prodbysmolec";
            
            var artikelList = new List<Artikel>
            {
                new Artikel
                {
                    Name = "Laptop",
                    Preis = 999.99m,
                    Mindestbestand = 5,
                    Maximalbestand = 50,
                    Menge = 25,
                    Status = ArtikelStatus.Verfügbar,
                    CreatedOn = currentDateTime,
                    CreatedBy = currentUser,
                    LastModifiedOn = currentDateTime,
                    LastModifiedBy = currentUser
                },
                new Artikel
                {
                    Name = "Maus",
                    Preis = 29.99m,
                    Mindestbestand = 10,
                    Maximalbestand = 100,
                    Menge = 45,
                    Status = ArtikelStatus.Verfügbar,
                    CreatedOn = currentDateTime,
                    CreatedBy = currentUser,
                    LastModifiedOn = currentDateTime,
                    LastModifiedBy = currentUser
                },
                new Artikel
                {
                    Name = "Tastatur",
                    Preis = 59.99m,
                    Mindestbestand = 10,
                    Maximalbestand = 80,
                    Menge = 30,
                    Status = ArtikelStatus.Verfügbar,
                    CreatedOn = currentDateTime,
                    CreatedBy = currentUser,
                    LastModifiedOn = currentDateTime,
                    LastModifiedBy = currentUser
                },
                new Artikel
                {
                    Name = "Monitor",
                    Preis = 249.99m,
                    Mindestbestand = 3,
                    Maximalbestand = 30,
                    Menge = 12,
                    Status = ArtikelStatus.Verfügbar,
                    CreatedOn = currentDateTime,
                    CreatedBy = currentUser,
                    LastModifiedOn = currentDateTime,
                    LastModifiedBy = currentUser
                },
                new Artikel
                {
                    Name = "USB-Stick",
                    Preis = 14.99m,
                    Mindestbestand = 20,
                    Maximalbestand = 200,
                    Menge = 75,
                    Status = ArtikelStatus.Verfügbar,
                    CreatedOn = currentDateTime,
                    CreatedBy = currentUser,
                    LastModifiedOn = currentDateTime,
                    LastModifiedBy = currentUser
                }
            };

            context.Artikel.AddRange(artikelList);
            context.SaveChanges();

            // Create statistics for each article
            var artikelStatistikList = new List<ArtikelStatistik>();
            foreach (var artikel in artikelList)
            {
                var statistik = new ArtikelStatistik
                {
                    ArtikelId = artikel.Id,
                    Gesamtmenge = artikel.Menge,
                    DurchschnittlicherEinzelpreis = artikel.Preis * 0.7m, // Annahme: Einkaufspreis ist 70% des Verkaufspreises
                    DurchschnittlicherVerkaufspreis = artikel.Preis,
                    VerkaufsMenge = 0,
                    Lagerwert = artikel.Menge * (artikel.Preis * 0.7m),
                    GesamtVerkaufswert = 0,
                    CreatedOn = currentDateTime,
                    CreatedBy = currentUser,
                    LastModifiedOn = currentDateTime,
                    LastModifiedBy = currentUser
                };
                artikelStatistikList.Add(statistik);
            }

            context.ArtikelStatistiken.AddRange(artikelStatistikList);
            context.SaveChanges();
        }
    }
}