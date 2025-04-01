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
            DateTime.Parse("2025-04-01 22:35:48"), 
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

        // Seed Produktkategorien
        if (!context.Set<Produktkategorie>().Any())
        {
            var kategorien = new List<Produktkategorie>
            {
                new Produktkategorie 
                { 
                    Name = "Elektronik", 
                    Beschreibung = "Elektronische Geräte und Zubehör"
                },
                new Produktkategorie 
                { 
                    Name = "Bekleidung", 
                    Beschreibung = "Kleidung und Accessoires"
                },
                new Produktkategorie 
                { 
                    Name = "Bürobedarf", 
                    Beschreibung = "Material für den Büroalltag"
                }
            };
            
            context.Set<Produktkategorie>().AddRange(kategorien);
            context.SaveChanges();
        }

        // Seed Artikelgruppen
        if (!context.Set<Artikelgruppe>().Any())
        {
            var elektronikId = context.Set<Produktkategorie>().Single(p => p.Name == "Elektronik").Id;
            var bekleidungId = context.Set<Produktkategorie>().Single(p => p.Name == "Bekleidung").Id;
            var bueroId = context.Set<Produktkategorie>().Single(p => p.Name == "Bürobedarf").Id;
            
            var artikelgruppen = new List<Artikelgruppe>
            {
                new Artikelgruppe 
                { 
                    Name = "Computer", 
                    ProduktkategorieId = elektronikId
                },
                new Artikelgruppe 
                { 
                    Name = "Peripheriegeräte", 
                    ProduktkategorieId = elektronikId 
                },
                new Artikelgruppe 
                { 
                    Name = "T-Shirts", 
                    ProduktkategorieId = bekleidungId 
                },
                new Artikelgruppe 
                { 
                    Name = "Schreibwaren", 
                    ProduktkategorieId = bueroId 
                }
            };
            
            context.Set<Artikelgruppe>().AddRange(artikelgruppen);
            context.SaveChanges();
        }

        // Seed Zusatzfelder
        if (!context.Set<Zusatzfeld>().Any())
        {
            var zusatzfelder = new List<Zusatzfeld>
            {
                new Zusatzfeld { Name = "Farbe" },
                new Zusatzfeld { Name = "Größe" },
                new Zusatzfeld { Name = "Material" },
                new Zusatzfeld { Name = "Prozessortyp" },
                new Zusatzfeld { Name = "Arbeitsspeicher" },
                new Zusatzfeld { Name = "Festplattentyp" },
                new Zusatzfeld { Name = "Anschlusstyp" },
                new Zusatzfeld { Name = "Schnittstelle" }
            };
            
            context.Set<Zusatzfeld>().AddRange(zusatzfelder);
            context.SaveChanges();
        }

        // Seed Zusatzwerte
        if (!context.Set<Zusatzwert>().Any())
        {
            // Farben
            var farbeId = context.Set<Zusatzfeld>().Single(z => z.Name == "Farbe").ZusatzfeldID;
            var farben = new List<Zusatzwert>
            {
                new Zusatzwert { ZusatzFeldID = farbeId, Wert = "Schwarz" },
                new Zusatzwert { ZusatzFeldID = farbeId, Wert = "Weiß" },
                new Zusatzwert { ZusatzFeldID = farbeId, Wert = "Blau" },
                new Zusatzwert { ZusatzFeldID = farbeId, Wert = "Rot" },
                new Zusatzwert { ZusatzFeldID = farbeId, Wert = "Grün" }
            };
            
            // Größen
            var größeId = context.Set<Zusatzfeld>().Single(z => z.Name == "Größe").ZusatzfeldID;
            var größen = new List<Zusatzwert>
            {
                new Zusatzwert { ZusatzFeldID = größeId, Wert = "S" },
                new Zusatzwert { ZusatzFeldID = größeId, Wert = "M" },
                new Zusatzwert { ZusatzFeldID = größeId, Wert = "L" },
                new Zusatzwert { ZusatzFeldID = größeId, Wert = "XL" },
                new Zusatzwert { ZusatzFeldID = größeId, Wert = "XXL" }
            };
            
            // Material
            var materialId = context.Set<Zusatzfeld>().Single(z => z.Name == "Material").ZusatzfeldID;
            var materialien = new List<Zusatzwert>
            {
                new Zusatzwert { ZusatzFeldID = materialId, Wert = "Baumwolle" },
                new Zusatzwert { ZusatzFeldID = materialId, Wert = "Kunststoff" },
                new Zusatzwert { ZusatzFeldID = materialId, Wert = "Aluminium" },
                new Zusatzwert { ZusatzFeldID = materialId, Wert = "Edelstahl" }
            };
            
            // Prozessortyp
            var prozessorId = context.Set<Zusatzfeld>().Single(z => z.Name == "Prozessortyp").ZusatzfeldID;
            var prozessoren = new List<Zusatzwert>
            {
                new Zusatzwert { ZusatzFeldID = prozessorId, Wert = "Intel i5" },
                new Zusatzwert { ZusatzFeldID = prozessorId, Wert = "Intel i7" },
                new Zusatzwert { ZusatzFeldID = prozessorId, Wert = "Intel i9" },
                new Zusatzwert { ZusatzFeldID = prozessorId, Wert = "AMD Ryzen 5" },
                new Zusatzwert { ZusatzFeldID = prozessorId, Wert = "AMD Ryzen 7" }
            };
            
            // Arbeitsspeicher
            var ramId = context.Set<Zusatzfeld>().Single(z => z.Name == "Arbeitsspeicher").ZusatzfeldID;
            var ram = new List<Zusatzwert>
            {
                new Zusatzwert { ZusatzFeldID = ramId, Wert = "8 GB" },
                new Zusatzwert { ZusatzFeldID = ramId, Wert = "16 GB" },
                new Zusatzwert { ZusatzFeldID = ramId, Wert = "32 GB" },
                new Zusatzwert { ZusatzFeldID = ramId, Wert = "64 GB" }
            };
            
            // Festplattentyp
            var festplatteId = context.Set<Zusatzfeld>().Single(z => z.Name == "Festplattentyp").ZusatzfeldID;
            var festplatten = new List<Zusatzwert>
            {
                new Zusatzwert { ZusatzFeldID = festplatteId, Wert = "SSD 256 GB" },
                new Zusatzwert { ZusatzFeldID = festplatteId, Wert = "SSD 512 GB" },
                new Zusatzwert { ZusatzFeldID = festplatteId, Wert = "SSD 1 TB" },
                new Zusatzwert { ZusatzFeldID = festplatteId, Wert = "HDD 1 TB" }
            };
            
            // Anschlusstyp
            var anschlussId = context.Set<Zusatzfeld>().Single(z => z.Name == "Anschlusstyp").ZusatzfeldID;
            var anschlüsse = new List<Zusatzwert>
            {
                new Zusatzwert { ZusatzFeldID = anschlussId, Wert = "USB" },
                new Zusatzwert { ZusatzFeldID = anschlussId, Wert = "USB-C" },
                new Zusatzwert { ZusatzFeldID = anschlussId, Wert = "Bluetooth" },
                new Zusatzwert { ZusatzFeldID = anschlussId, Wert = "Kabelgebunden" }
            };
            
            // Schnittstellen
            var schnittstelleId = context.Set<Zusatzfeld>().Single(z => z.Name == "Schnittstelle").ZusatzfeldID;
            var schnittstellen = new List<Zusatzwert>
            {
                new Zusatzwert { ZusatzFeldID = schnittstelleId, Wert = "HDMI" },
                new Zusatzwert { ZusatzFeldID = schnittstelleId, Wert = "DisplayPort" },
                new Zusatzwert { ZusatzFeldID = schnittstelleId, Wert = "VGA" },
                new Zusatzwert { ZusatzFeldID = schnittstelleId, Wert = "USB 3.0" }
            };
            
            context.Set<Zusatzwert>().AddRange(farben);
            context.Set<Zusatzwert>().AddRange(größen);
            context.Set<Zusatzwert>().AddRange(materialien);
            context.Set<Zusatzwert>().AddRange(prozessoren);
            context.Set<Zusatzwert>().AddRange(ram);
            context.Set<Zusatzwert>().AddRange(festplatten);
            context.Set<Zusatzwert>().AddRange(anschlüsse);
            context.Set<Zusatzwert>().AddRange(schnittstellen);
            context.SaveChanges();
        }

        // Verknüpfe Artikelgruppen mit Zusatzfeldern
        if (!context.Set<ArtikelgruppeZusatzfelder>().Any())
        {
            var computerGruppe = context.Set<Artikelgruppe>().Single(a => a.Name == "Computer");
            var peripherieGruppe = context.Set<Artikelgruppe>().Single(a => a.Name == "Peripheriegeräte");
            var tshirtGruppe = context.Set<Artikelgruppe>().Single(a => a.Name == "T-Shirts");
            
            var farbeId = context.Set<Zusatzfeld>().Single(z => z.Name == "Farbe").ZusatzfeldID;
            var größeId = context.Set<Zusatzfeld>().Single(z => z.Name == "Größe").ZusatzfeldID;
            var materialId = context.Set<Zusatzfeld>().Single(z => z.Name == "Material").ZusatzfeldID;
            var prozessorId = context.Set<Zusatzfeld>().Single(z => z.Name == "Prozessortyp").ZusatzfeldID;
            var ramId = context.Set<Zusatzfeld>().Single(z => z.Name == "Arbeitsspeicher").ZusatzfeldID;
            var festplatteId = context.Set<Zusatzfeld>().Single(z => z.Name == "Festplattentyp").ZusatzfeldID;
            var anschlussId = context.Set<Zusatzfeld>().Single(z => z.Name == "Anschlusstyp").ZusatzfeldID;
            var schnittstelleId = context.Set<Zusatzfeld>().Single(z => z.Name == "Schnittstelle").ZusatzfeldID;
            
            var verknüpfungen = new List<ArtikelgruppeZusatzfelder>
            {
                // Computer-Gruppe
                new ArtikelgruppeZusatzfelder { ArtikelgruppeID = computerGruppe.Id, ZusatzfelderID = farbeId },
                new ArtikelgruppeZusatzfelder { ArtikelgruppeID = computerGruppe.Id, ZusatzfelderID = prozessorId },
                new ArtikelgruppeZusatzfelder { ArtikelgruppeID = computerGruppe.Id, ZusatzfelderID = ramId },
                new ArtikelgruppeZusatzfelder { ArtikelgruppeID = computerGruppe.Id, ZusatzfelderID = festplatteId },
                
                // Peripherie-Gruppe
                new ArtikelgruppeZusatzfelder { ArtikelgruppeID = peripherieGruppe.Id, ZusatzfelderID = farbeId },
                new ArtikelgruppeZusatzfelder { ArtikelgruppeID = peripherieGruppe.Id, ZusatzfelderID = anschlussId },
                new ArtikelgruppeZusatzfelder { ArtikelgruppeID = peripherieGruppe.Id, ZusatzfelderID = schnittstelleId },
                new ArtikelgruppeZusatzfelder { ArtikelgruppeID = peripherieGruppe.Id, ZusatzfelderID = materialId },
                
                // T-Shirt-Gruppe
                new ArtikelgruppeZusatzfelder { ArtikelgruppeID = tshirtGruppe.Id, ZusatzfelderID = farbeId },
                new ArtikelgruppeZusatzfelder { ArtikelgruppeID = tshirtGruppe.Id, ZusatzfelderID = größeId },
                new ArtikelgruppeZusatzfelder { ArtikelgruppeID = tshirtGruppe.Id, ZusatzfelderID = materialId }
            };
            
            context.Set<ArtikelgruppeZusatzfelder>().AddRange(verknüpfungen);
            context.SaveChanges();
        }

        // Seed Artikel data if none exists
        List<Artikel> artikelList = new List<Artikel>();
        if (!context.Artikel.Any())
        {
            var computerGruppe = context.Set<Artikelgruppe>().Single(a => a.Name == "Computer");
            var peripherieGruppe = context.Set<Artikelgruppe>().Single(a => a.Name == "Peripheriegeräte");
            var tshirtGruppe = context.Set<Artikelgruppe>().Single(a => a.Name == "T-Shirts");
            
            artikelList = new List<Artikel>
            {
                // Computer
                new Artikel
                {
                    Name = "Business Laptop Pro",
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
                    Name = "Gaming Notebook Ultimate",
                    Preis = 1499.99m,
                    Mindestbestand = 3,
                    Maximalbestand = 30,
                    Menge = 15,
                    Status = ArtikelStatus.Verfügbar,
                    CreatedOn = currentDateTime,
                    CreatedBy = currentUser,
                    LastModifiedOn = currentDateTime,
                    LastModifiedBy = currentUser
                },
                
                // Peripherie
                new Artikel
                {
                    Name = "Ergonomische Maus",
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
                    Name = "Mechanische Tastatur",
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
                    Name = "4K Monitor 27 Zoll",
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
                    Name = "USB-Stick 128GB",
                    Preis = 14.99m,
                    Mindestbestand = 20,
                    Maximalbestand = 200,
                    Menge = 75,
                    Status = ArtikelStatus.Verfügbar,
                    CreatedOn = currentDateTime,
                    CreatedBy = currentUser,
                    LastModifiedOn = currentDateTime,
                    LastModifiedBy = currentUser
                },
                
                // T-Shirts
                new Artikel
                {
                    Name = "Firmen T-Shirt Logo",
                    Preis = 19.99m,
                    Mindestbestand = 15,
                    Maximalbestand = 150,
                    Menge = 100,
                    Status = ArtikelStatus.Verfügbar,
                    CreatedOn = currentDateTime,
                    CreatedBy = currentUser,
                    LastModifiedOn = currentDateTime,
                    LastModifiedBy = currentUser
                },
                new Artikel
                {
                    Name = "Event T-Shirt 2025",
                    Preis = 24.99m,
                    Mindestbestand = 10,
                    Maximalbestand = 100,
                    Menge = 50,
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

            // Zusatzwerte zu Artikeln hinzufügen
            var schwarzId = context.Set<Zusatzwert>().Single(z => z.Wert == "Schwarz").Id;
            var weißId = context.Set<Zusatzwert>().Single(z => z.Wert == "Weiß").Id;
            var blauId = context.Set<Zusatzwert>().Single(z => z.Wert == "Blau").Id;
            var rotId = context.Set<Zusatzwert>().Single(z => z.Wert == "Rot").Id;
            
            var sizeS = context.Set<Zusatzwert>().Single(z => z.Wert == "S").Id;
            var sizeM = context.Set<Zusatzwert>().Single(z => z.Wert == "M").Id;
            var sizeL = context.Set<Zusatzwert>().Single(z => z.Wert == "L").Id;
            var sizeXL = context.Set<Zusatzwert>().Single(z => z.Wert == "XL").Id;
            
            var baumwolle = context.Set<Zusatzwert>().Single(z => z.Wert == "Baumwolle").Id;
            var kunststoff = context.Set<Zusatzwert>().Single(z => z.Wert == "Kunststoff").Id;
            var aluminium = context.Set<Zusatzwert>().Single(z => z.Wert == "Aluminium").Id;
            
            var i5 = context.Set<Zusatzwert>().Single(z => z.Wert == "Intel i5").Id;
            var i7 = context.Set<Zusatzwert>().Single(z => z.Wert == "Intel i7").Id;
            var ryzen5 = context.Set<Zusatzwert>().Single(z => z.Wert == "AMD Ryzen 5").Id;
            
            var ram8 = context.Set<Zusatzwert>().Single(z => z.Wert == "8 GB").Id;
            var ram16 = context.Set<Zusatzwert>().Single(z => z.Wert == "16 GB").Id;
            var ram32 = context.Set<Zusatzwert>().Single(z => z.Wert == "32 GB").Id;
            
            var ssd256 = context.Set<Zusatzwert>().Single(z => z.Wert == "SSD 256 GB").Id;
            var ssd512 = context.Set<Zusatzwert>().Single(z => z.Wert == "SSD 512 GB").Id;
            var ssd1tb = context.Set<Zusatzwert>().Single(z => z.Wert == "SSD 1 TB").Id;
            
            var usb = context.Set<Zusatzwert>().Single(z => z.Wert == "USB").Id;
            var usbc = context.Set<Zusatzwert>().Single(z => z.Wert == "USB-C").Id;
            var bluetooth = context.Set<Zusatzwert>().Single(z => z.Wert == "Bluetooth").Id;
            
            var hdmi = context.Set<Zusatzwert>().Single(z => z.Wert == "HDMI").Id;
            var displayport = context.Set<Zusatzwert>().Single(z => z.Wert == "DisplayPort").Id;
            var usb30 = context.Set<Zusatzwert>().Single(z => z.Wert == "USB 3.0").Id;

            var artikelZusatzwerte = new List<ArtikelZusatzWert>
            {
                // Business Laptop Pro
                new ArtikelZusatzWert { ArtikelId = artikelList[0].Id, ZusatzwertId = schwarzId },
                new ArtikelZusatzWert { ArtikelId = artikelList[0].Id, ZusatzwertId = i5 },
                new ArtikelZusatzWert { ArtikelId = artikelList[0].Id, ZusatzwertId = ram16 },
                new ArtikelZusatzWert { ArtikelId = artikelList[0].Id, ZusatzwertId = ssd512 },
                
                // Gaming Notebook Ultimate
                new ArtikelZusatzWert { ArtikelId = artikelList[1].Id, ZusatzwertId = rotId },
                new ArtikelZusatzWert { ArtikelId = artikelList[1].Id, ZusatzwertId = i7 },
                new ArtikelZusatzWert { ArtikelId = artikelList[1].Id, ZusatzwertId = ram32 },
                new ArtikelZusatzWert { ArtikelId = artikelList[1].Id, ZusatzwertId = ssd1tb },
                
                // Ergonomische Maus
                new ArtikelZusatzWert { ArtikelId = artikelList[2].Id, ZusatzwertId = schwarzId },
                new ArtikelZusatzWert { ArtikelId = artikelList[2].Id, ZusatzwertId = kunststoff },
                new ArtikelZusatzWert { ArtikelId = artikelList[2].Id, ZusatzwertId = usb },
                new ArtikelZusatzWert { ArtikelId = artikelList[2].Id, ZusatzwertId = bluetooth },
                
                // Mechanische Tastatur
                new ArtikelZusatzWert { ArtikelId = artikelList[3].Id, ZusatzwertId = schwarzId },
                new ArtikelZusatzWert { ArtikelId = artikelList[3].Id, ZusatzwertId = aluminium },
                new ArtikelZusatzWert { ArtikelId = artikelList[3].Id, ZusatzwertId = usb },
                
                // 4K Monitor
                new ArtikelZusatzWert { ArtikelId = artikelList[4].Id, ZusatzwertId = schwarzId },
                new ArtikelZusatzWert { ArtikelId = artikelList[4].Id, ZusatzwertId = kunststoff },
                new ArtikelZusatzWert { ArtikelId = artikelList[4].Id, ZusatzwertId = hdmi },
                new ArtikelZusatzWert { ArtikelId = artikelList[4].Id, ZusatzwertId = displayport },
                
                // USB-Stick
                new ArtikelZusatzWert { ArtikelId = artikelList[5].Id, ZusatzwertId = blauId },
                new ArtikelZusatzWert { ArtikelId = artikelList[5].Id, ZusatzwertId = kunststoff },
                new ArtikelZusatzWert { ArtikelId = artikelList[5].Id, ZusatzwertId = usb30 },
                
                // Firmen T-Shirt Logo
                new ArtikelZusatzWert { ArtikelId = artikelList[6].Id, ZusatzwertId = weißId },
                new ArtikelZusatzWert { ArtikelId = artikelList[6].Id, ZusatzwertId = sizeM },
                new ArtikelZusatzWert { ArtikelId = artikelList[6].Id, ZusatzwertId = baumwolle },
                
                // Event T-Shirt 2025
                new ArtikelZusatzWert { ArtikelId = artikelList[7].Id, ZusatzwertId = blauId },
                new ArtikelZusatzWert { ArtikelId = artikelList[7].Id, ZusatzwertId = sizeL },
                new ArtikelZusatzWert { ArtikelId = artikelList[7].Id, ZusatzwertId = baumwolle }
            };
            
            context.Set<ArtikelZusatzWert>().AddRange(artikelZusatzwerte);
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
                    ArtikelId = artikelList[0].Id, // Business Laptop
                    Menge = 5,
                    Einzelpreis = 700.0m,
                    Gesamtpreis = 3500.0m
                },
                new WareneingangArtikelPositionen
                {
                    WareneingangId = wareneingang1.Id,
                    ArtikelId = artikelList[2].Id, // Maus
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
                    ArtikelId = artikelList[4].Id, // Monitor
                    Menge = 3,
                    Einzelpreis = 180.0m,
                    Gesamtpreis = 540.0m
                },
                new WareneingangArtikelPositionen
                {
                    WareneingangId = wareneingang2.Id,
                    ArtikelId = artikelList[3].Id, // Tastatur
                    Menge = 8,
                    Einzelpreis = 42.0m,
                    Gesamtpreis = 336.0m
                },
                new WareneingangArtikelPositionen
                {
                    WareneingangId = wareneingang2.Id,
                    ArtikelId = artikelList[5].Id, // USB-Stick
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
                    ArtikelId = artikelList[0].Id, // Business Laptop
                    Artikel = artikelList[0], // Artikel korrekt zuweisen
                    Zweck = WarenausgangZweckEnum.Ausbildungskurs,
                    Menge = 2,
                    Bemerkung = "Neue Laptops für Entwickler"
                },
                new WarenausgangArtikelPositionen
                {
                    WarenausgangId = warenausgang1.Id,
                    ArtikelId = artikelList[2].Id, // Maus
                    Artikel = artikelList[2], // Artikel korrekt zuweisen
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
                    ArtikelId = artikelList[4].Id, // Monitor
                    Artikel = artikelList[4], // Artikel korrekt zuweisen
                    Zweck = WarenausgangZweckEnum.Bestellung,
                    Menge = 1,
                    Verkaufspreis = 249.99m,
                    Gesamtpreis = 249.99m,
                    Rechnungsnummer = "RE-2025-0042"
                },
                new WarenausgangArtikelPositionen
                {
                    WarenausgangId = warenausgang2.Id,
                    ArtikelId = artikelList[5].Id, // USB-Stick
                    Artikel = artikelList[5], // Artikel korrekt zuweisen
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