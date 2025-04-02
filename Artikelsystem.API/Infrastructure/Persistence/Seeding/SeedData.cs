using System;
using System.Collections.Generic;
using Artikelsystem.Api.Features.Artikel.Models.Entitys;
using Artikelsystem.Api.Features.Employees.Enums;
using Artikelsystem.Api.Features.Employees.Models.Entitys;
using Artikelsystem.Api.Features.Lieferant.Models.Entitys;
using Artikelsystem.Api.Features.Warenausgang.Models.Entitys;
using Artikelsystem.Api.Features.Wareneingang.Models.Entitys;
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

        // Aktuelle Zeit und Benutzer
        var currentDateTime = DateTime.SpecifyKind(
            DateTime.Parse("2025-03-31 20:11:59"), 
            DateTimeKind.Utc
        );
        
        var currentUser = "prodbysmolec";

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
        List<Artikel> artikelList = new List<Artikel>();
        if (!context.Artikel.Any())
        {
            artikelList = new List<Artikel>
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
        else
        {
            artikelList = context.Artikel.ToList();
        }

        if(!context.Lieferanten.Any())
        {
            var lieferanten = new List<Lieferant>
            {
                new Lieferant
                {
                    Firma = "TechSupply GmbH",
                    Name = "Müller",
                    Vorname = "Thomas",
                    EmailAdresse = "t.mueller@techsupply.de",
                    Strasse = "Industrieweg",
                    Hausnummer = "42",
                    PLZ = "10115",
                    Ort = "Berlin",
                    Telefonnummer = "030-12345678",
                    Notizen = "Bevorzugter Lieferant für IT-Ausrüstung"
                },
                new Lieferant
                {
                    Firma = "Office Solutions AG",
                    Name = "Schmidt",
                    Vorname = "Anna",
                    EmailAdresse = "a.schmidt@officesolutions.de",
                    Strasse = "Büroallee",
                    Hausnummer = "15",
                    PLZ = "60313",
                    Ort = "Frankfurt",
                    Telefonnummer = "069-87654321",
                    Notizen = "Liefert zuverlässig Büroartikel"
                },
                new Lieferant
                {
                    Firma = "ElectronicWholesale KG",
                    Name = "Wagner",
                    Vorname = "Michael",
                    EmailAdresse = "m.wagner@electronic-wholesale.de",
                    Strasse = "Elektronikstraße",
                    Hausnummer = "7",
                    PLZ = "80331",
                    Ort = "München",
                    Telefonnummer = "089-11223344",
                    Notizen = "Spezialist für elektronische Bauteile",
                }
            };

            context.Lieferanten.AddRange(lieferanten);
            context.SaveChanges();
        }

        if (!context.Wareneingaenge.Any())
        {
            // Erstelle zwei Wareneingänge mit jeweils unterschiedlichen Artikelpositionen
            var wareneingang1 = new Wareneingaenge
            {
                Gesamtpreis = 850.0m,
                AllgemeineBemerkungen = "Standardlieferung vom Hauptlieferanten",
                CreatedOn = currentDateTime.AddDays(-14),
                CreatedBy = currentUser,
                LastModifiedOn = currentDateTime.AddDays(-14),
                LastModifiedBy = currentUser
            };
            
            var wareneingang2 = new Wareneingaenge
            {
                Gesamtpreis = 1200.0m,
                AllgemeineBemerkungen = "Dringende Nachbestellung",
                CreatedOn = currentDateTime.AddDays(-5),
                CreatedBy = currentUser,
                LastModifiedOn = currentDateTime.AddDays(-5),
                LastModifiedBy = currentUser
            };
            
            context.Wareneingaenge.Add(wareneingang1);
            context.Wareneingaenge.Add(wareneingang2);
            context.SaveChanges();
            
            // Positionen für Wareneingang 1
            var positionen1 = new List<WareneingangArtikelPositionen>
            {
                new WareneingangArtikelPositionen
                {
                    WareneingangId = wareneingang1.Id,
                    ArtikelId = artikelList[0].Id, // Laptop
                    Menge = 5,
                    Einzelpreis = 700.0m,
                    Gesamtpreis = 3500.0m
                },
                new WareneingangArtikelPositionen
                {
                    WareneingangId = wareneingang1.Id,
                    ArtikelId = artikelList[1].Id, // Maus
                    Menge = 10,
                    Einzelpreis = 20.0m,
                    Gesamtpreis = 200.0m
                }
            };
            
            // Positionen für Wareneingang 2
            var positionen2 = new List<WareneingangArtikelPositionen>
            {
                new WareneingangArtikelPositionen
                {
                    WareneingangId = wareneingang2.Id,
                    ArtikelId = artikelList[3].Id, // Monitor
                    Menge = 3,
                    Einzelpreis = 180.0m,
                    Gesamtpreis = 540.0m
                },
                new WareneingangArtikelPositionen
                {
                    WareneingangId = wareneingang2.Id,
                    ArtikelId = artikelList[2].Id, // Tastatur
                    Menge = 8,
                    Einzelpreis = 42.0m,
                    Gesamtpreis = 336.0m
                },
                new WareneingangArtikelPositionen
                {
                    WareneingangId = wareneingang2.Id,
                    ArtikelId = artikelList[4].Id, // USB-Stick
                    Menge = 25,
                    Einzelpreis = 10.0m,
                    Gesamtpreis = 250.0m
                }
            };
            
            context.Set<WareneingangArtikelPositionen>().AddRange(positionen1);
            context.Set<WareneingangArtikelPositionen>().AddRange(positionen2);
            context.SaveChanges();
        }

        if (!context.Warenausgaenge.Any())
        {
            // Erstelle zwei Warenausgänge
            var warenausgang1 = new Warenausgaenge
            {
                Mitarbeiter = "John Doe",
                AllgemeineBemerkungen = "Bestellung für IT-Abteilung",
                CreatedOn = currentDateTime.AddDays(-10),
                CreatedBy = currentUser,
                LastModifiedOn = currentDateTime.AddDays(-10),
                LastModifiedBy = currentUser
            };

            var warenausgang2 = new Warenausgaenge
            {
                Mitarbeiter = "Jane Smith",
                AllgemeineBemerkungen = "Verkauf an externen Kunden",
                CreatedOn = currentDateTime.AddDays(-3),
                CreatedBy = currentUser,
                LastModifiedOn = currentDateTime.AddDays(-3),
                LastModifiedBy = currentUser
            };
            
            context.Warenausgaenge.Add(warenausgang1);
            context.Warenausgaenge.Add(warenausgang2);
            context.SaveChanges();
            
            // Positionen für Warenausgang 1 (intern)
            var positionen1 = new List<WarenausgangArtikelPositionen>
            {
                new WarenausgangArtikelPositionen
                {
                    WarenausgangId = warenausgang1.Id,
                    ArtikelId = artikelList[0].Id, // Laptop
                    Artikel = artikelList[0], // Artikel korrekt zuweisen
                    Zweck = WarenausgangZweckEnum.Ausbildungskurs,
                    Menge = 2,
                    Bemerkung = "Neue Laptops für Entwickler"
                },
                new WarenausgangArtikelPositionen
                {
                    WarenausgangId = warenausgang1.Id,
                    ArtikelId = artikelList[1].Id, // Maus
                    Artikel = artikelList[1], // Artikel korrekt zuweisen
                    Zweck = WarenausgangZweckEnum.Bestellung,
                    Menge = 2,
                    Bemerkung = "Mäuse für neue Laptops"
                }
            };
            
            // Positionen für Warenausgang 2 (Verkauf)
            var positionen2 = new List<WarenausgangArtikelPositionen>
            {
                new WarenausgangArtikelPositionen
                {
                    WarenausgangId = warenausgang2.Id,
                    ArtikelId = artikelList[3].Id, // Monitor
                    Zweck = WarenausgangZweckEnum.Bestellung,
                    Menge = 1,
                    Verkaufspreis = 249.99m,
                    Gesamtpreis = 249.99m,
                    Rechnungsnummer = "RE-2025-0042"
                },
                new WarenausgangArtikelPositionen
                {
                    WarenausgangId = warenausgang2.Id,
                    ArtikelId = artikelList[4].Id, // USB-Stick
                    Zweck = WarenausgangZweckEnum.Ausbildungskurs,
                    Menge = 5,
                    Verkaufspreis = 14.99m,
                    Gesamtpreis = 74.95m,
                    Rechnungsnummer = "RE-2025-0042"
                }
            };
            
            context.Set<WarenausgangArtikelPositionen>().AddRange(positionen1);
            context.Set<WarenausgangArtikelPositionen>().AddRange(positionen2);
            context.SaveChanges();
        }
    }
}