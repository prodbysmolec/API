using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities.Artikel;
using Domain.Entities.Lieferant;
using Domain.Entities.Warenausgang;
using Domain.Entities.Wareneingang;
using Domain.Entities.Inventur;
using API.Features.Inventur.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Domain.Entities.Authentication;
using Artikelsystem.Shared.DTOs.Artikel.Enums;
using Artikelsystem.Shared.DTOs.Warenausgang.Enums;
using Infrastructure.Context;
using Domain.Entities.Employees;
using System.Threading.Tasks;
using Infrastructure.Services.Authentication;

namespace API.Infrastructure.Persistence.Seeding;

public static class SeedData
{
    public async static Task MigrateAndSeed(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<AppDbContext>();
        //context.Database.Migrate();

        // Aktuelle Zeit und Benutzer
        var currentDateTime = DateTime.SpecifyKind(
            DateTime.Parse("2025-04-02 10:57:53"),
            DateTimeKind.Utc
        );

        var currentUser = "prodbysmolec";

        // SeedEmployees(context);
        // SeedBenefits(context);
        // SeedProduktkategorien(context);
        // SeedArtikelGruppen(context);
        // SeedZusatzFelder(context);
        // SeedZusatzWerte(context);
        // SeedArtikelgruppeZusatzfelder(context);
        // SeedArtikelData(context, currentDateTime, currentUser);
        // SeedArtikelStatistiken(context, currentDateTime, currentUser);
        // SeedArtikelZusatzwerte(context, currentDateTime, currentUser);
        // SeedWareneingaenge(context, currentDateTime, currentUser);
        // SeedWarenausganege(context, currentDateTime, currentUser);   
        // SeedInventuren(context, currentDateTime, currentUser); 
        // SeedLieferanten(context);
        // SeedArtikelLieferant(context, currentDateTime, currentUser);
        await SeedUsers(context);
    }

    private static async Task SeedUsers(AppDbContext context)
    {
        if(!await context.Permissions.AnyAsync())
        {
            var permissions = new List<Permission>
            {
                new Permission { Name = "Artikel anzeigen", Beschreibung = "Kann Artikel sehen.", Code = "ARTIKEL_VIEW" },
                new Permission { Name = "Artikel erstellen", Beschreibung = "Kann Artikel erstellen.", Code = "ARTIKEL_CREATE" },
                new Permission { Name = "Admin-Zugriff", Beschreibung = "Hat vollen Admin-Zugriff", Code = "ADMIN_ACCESS" }
            };

            await context.Permissions.AddRangeAsync(permissions);
            await context.SaveChangesAsync();
        }

        // Wenn noch keine UserGruppen existieren, erstelle diese
        if (!await context.UserGruppen.AnyAsync())
        {
            var userGruppen = new List<UserGruppen>
            {
                new UserGruppen { Name = "Admin" },
                new UserGruppen { Name = "User" },
                new UserGruppen { Name = "Manager" }
            };

            await context.UserGruppen.AddRangeAsync(userGruppen);
            await context.SaveChangesAsync();
        }

        // Wenn noch keine User existieren, erstelle diese
        if (!await context.Users.AnyAsync())
        {
            var users = new List<User>
            {
                new User
                {
                    UserName = "admin",
                    PasswordHash = "", // Wird später gesetzt
                    Name = "Admin",
                    Nachname = "User",
                    Email = "admin@example.com"
                },
                new User
                {
                    UserName = "user",
                    PasswordHash = "", // Wird später gesetzt
                    Name = "Normal",
                    Nachname = "Benutzer",
                    Email = "benutzer@example.com"
                },
                new User
                {
                    UserName = "manager",
                    PasswordHash = "", // Wird später gesetzt
                    Name = "Manager",
                    Nachname = "Nutzer",
                    Email = "manager@example.com"
                }
            };
            var defaultPasswort = "123456";

            // Passwort für jeden Benutzer hashen
            var passwordService = new PasswordService();
            foreach (var user in users)
            {
                user.PasswordHash = passwordService.HashPassword(defaultPasswort);
            }

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();
        }

        // Wenn noch keine UserGruppenUser existieren, erstelle diese
        if (!await context.UserGruppenUsers.AnyAsync())
        {
            // Hole IDs für die vorhandenen UserGruppen
            var adminGruppe = await context.UserGruppen.FirstOrDefaultAsync(g => g.Name == "Admin");
            var userGruppe = await context.UserGruppen.FirstOrDefaultAsync(g => g.Name == "User");
            var managerGruppe = await context.UserGruppen.FirstOrDefaultAsync(g => g.Name == "Manager");

            // Hole IDs für die vorhandenen User
            var adminUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == "admin");
            var normalUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == "user"); // Korrigiert von "benutzer" zu "user"
            var managerUser = await context.Users.FirstOrDefaultAsync(u => u.UserName == "manager");

            if (adminGruppe != null && userGruppe != null && managerGruppe != null &&
                adminUser != null && normalUser != null && managerUser != null)
            {
                var userGruppenUsers = new List<UserGruppenUser>
                {
                    // Admin gehört zur Admin-Gruppe
                    new UserGruppenUser
                    {
                        UserID = adminUser.Id,
                        UserGruppenID = adminGruppe.Id
                    },
                    
                    // Normaler Benutzer gehört zur User-Gruppe
                    new UserGruppenUser
                    {
                        UserID = normalUser.Id,
                        UserGruppenID = userGruppe.Id
                    },
                    
                    // Manager gehört zur Manager-Gruppe
                    new UserGruppenUser
                    {
                        UserID = managerUser.Id,
                        UserGruppenID = managerGruppe.Id
                    },
                    
                    // Admin gehört auch zur User-Gruppe
                    new UserGruppenUser
                    {
                        UserID = adminUser.Id,
                        UserGruppenID = userGruppe.Id
                    }
                };

                await context.Set<UserGruppenUser>().AddRangeAsync(userGruppenUsers);
                await context.SaveChangesAsync();
            }
        }

        // Wenn noch keine GroupPermission existieren, erstelle diese
        if(!await context.GroupPermissions.AnyAsync())
        {
            var adminGruppe = await context.UserGruppen.FirstOrDefaultAsync(g => g.Name == "Admin");
            var userGruppe = await context.UserGruppen.FirstOrDefaultAsync(g => g.Name == "User");
            var managerGruppe = await context.UserGruppen.FirstOrDefaultAsync(g => g.Name == "Manager");

            // Hole alle Permissions
            var permissions = await context.Permissions.ToListAsync();
            var permissionMap = permissions.ToDictionary(p => p.Code, p => p);

            var groupPermissions = new List<GroupPermission>();

            if (adminGruppe != null && userGruppe != null && managerGruppe != null && permissions.Any())
            {
                // Admin bekommt ALLE Berechtigungen
                foreach (var permission in permissions)
                {
                    groupPermissions.Add(new GroupPermission
                    {
                        UserGruppenID = adminGruppe.Id,
                        PermissionID = permission.Id
                    });
                }

                // User bekommt nur Artikel-Ansicht
                if (permissionMap.TryGetValue("ARTIKEL_VIEW", out var artikelViewPerm))
                    groupPermissions.Add(new GroupPermission { UserGruppenID = userGruppe.Id, PermissionID = artikelViewPerm.Id });
                
                // Manager bekommt Artikel ansehen und erstellen, aber keinen Admin-Zugriff
                if (permissionMap.TryGetValue("ARTIKEL_VIEW", out var artikelViewPerm2))
                    groupPermissions.Add(new GroupPermission { UserGruppenID = managerGruppe.Id, PermissionID = artikelViewPerm2.Id });
                
                if (permissionMap.TryGetValue("ARTIKEL_CREATE", out var artikelCreatePerm))
                    groupPermissions.Add(new GroupPermission { UserGruppenID = managerGruppe.Id, PermissionID = artikelCreatePerm.Id });
                
                await context.GroupPermissions.AddRangeAsync(groupPermissions);
                await context.SaveChangesAsync();
            }
        }
    }

    private static void SeedEmployees(AppDbContext context)
    {
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
    }

    private static void SeedBenefits(AppDbContext context)
    {
        
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
            var healthBenefit = context.Benefits.FirstOrDefault(b => b.Name == "Health");
            var dentalBenefit = context.Benefits.FirstOrDefault(b => b.Name == "Dental");
            var visionBenefit = context.Benefits.FirstOrDefault(b => b.Name == "Vision");

            if (healthBenefit != null && dentalBenefit != null && visionBenefit != null)
            {
                var john = context.Employees.FirstOrDefault(e => e.FirstName == "John");
                var jane = context.Employees.FirstOrDefault(e => e.FirstName == "Jane");

                if (john != null && jane != null)
                {
                    var employeeBenefits = new List<EmployeeBenefit>
                    {
                        // John's Benefits
                        new EmployeeBenefit {
                            EmployeeId = john.Id,
                            BenefitId = healthBenefit.Id,
                            CostToEmployee = 100m
                        },
                        new EmployeeBenefit {
                            EmployeeId = john.Id,
                            BenefitId = dentalBenefit.Id
                        },
                        
                        // Jane's Benefits
                        new EmployeeBenefit {
                            EmployeeId = jane.Id,
                            BenefitId = healthBenefit.Id,
                            CostToEmployee = 120m
                        },
                        new EmployeeBenefit {
                            EmployeeId = jane.Id,
                            BenefitId = visionBenefit.Id
                        }
                    };

                    context.EmployeeBenefits.AddRange(employeeBenefits);
                    context.SaveChanges();
                }
            }
        }
    }

    private static void SeedProduktkategorien(AppDbContext context)
    {
        if (!context.Produktkategorie.Any())
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
    }

    private static void SeedArtikelGruppen(AppDbContext context)
    {
        // Seed Artikelgruppen
        if (!context.Artikelgruppe.Any())
        {
            var elektronik = context.Set<Produktkategorie>().FirstOrDefault(p => p.Name == "Elektronik");
            var bekleidung = context.Set<Produktkategorie>().FirstOrDefault(p => p.Name == "Bekleidung");
            var buero = context.Set<Produktkategorie>().FirstOrDefault(p => p.Name == "Bürobedarf");

            if (elektronik != null && bekleidung != null && buero != null)
            {
                var artikelgruppen = new List<Artikelgruppe>
                {
                    new Artikelgruppe
                    {
                        Name = "Computer",
                        ProduktkategorieId = elektronik.Id
                    },
                    new Artikelgruppe
                    {
                        Name = "Peripheriegeräte",
                        ProduktkategorieId = elektronik.Id
                    },
                    new Artikelgruppe
                    {
                        Name = "T-Shirts",
                        ProduktkategorieId = bekleidung.Id
                    },
                    new Artikelgruppe
                    {
                        Name = "Schreibwaren",
                        ProduktkategorieId = buero.Id
                    }
                };

                context.Set<Artikelgruppe>().AddRange(artikelgruppen);
                context.SaveChanges();
            }
        }
    }

    private static void SeedZusatzFelder(AppDbContext context)
    {
        if (!context.Zusatzfeld.Any())
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
    }

    private static void SeedZusatzWerte(AppDbContext context)
    {
        if (!context.Zusatzwert.Any())
        {
            // Farben
            var farbe = context.Set<Zusatzfeld>().FirstOrDefault(z => z.Name == "Farbe");
            // Größen
            var größe = context.Set<Zusatzfeld>().FirstOrDefault(z => z.Name == "Größe");
            // Material
            var material = context.Set<Zusatzfeld>().FirstOrDefault(z => z.Name == "Material");
            // Prozessortyp
            var prozessor = context.Set<Zusatzfeld>().FirstOrDefault(z => z.Name == "Prozessortyp");
            // Arbeitsspeicher
            var ram = context.Set<Zusatzfeld>().FirstOrDefault(z => z.Name == "Arbeitsspeicher");
            // Festplattentyp
            var festplatte = context.Set<Zusatzfeld>().FirstOrDefault(z => z.Name == "Festplattentyp");
            // Anschlusstyp
            var anschluss = context.Set<Zusatzfeld>().FirstOrDefault(z => z.Name == "Anschlusstyp");
            // Schnittstellen
            var schnittstelle = context.Set<Zusatzfeld>().FirstOrDefault(z => z.Name == "Schnittstelle");

            if (farbe != null && größe != null && material != null && prozessor != null &&
                ram != null && festplatte != null && anschluss != null && schnittstelle != null)
            {
                var zusatzwerte = new List<Zusatzwert>();

                // Farben
                zusatzwerte.AddRange(new List<Zusatzwert>
                {
                    new Zusatzwert { ZusatzFeldID = farbe.ZusatzfeldID, Wert = "Schwarz" },
                    new Zusatzwert { ZusatzFeldID = farbe.ZusatzfeldID, Wert = "Weiß" },
                    new Zusatzwert { ZusatzFeldID = farbe.ZusatzfeldID, Wert = "Blau" },
                    new Zusatzwert { ZusatzFeldID = farbe.ZusatzfeldID, Wert = "Rot" },
                    new Zusatzwert { ZusatzFeldID = farbe.ZusatzfeldID, Wert = "Grün" }
                });

                // Größen
                zusatzwerte.AddRange(new List<Zusatzwert>
                {
                    new Zusatzwert { ZusatzFeldID = größe.ZusatzfeldID, Wert = "S" },
                    new Zusatzwert { ZusatzFeldID = größe.ZusatzfeldID, Wert = "M" },
                    new Zusatzwert { ZusatzFeldID = größe.ZusatzfeldID, Wert = "L" },
                    new Zusatzwert { ZusatzFeldID = größe.ZusatzfeldID, Wert = "XL" },
                    new Zusatzwert { ZusatzFeldID = größe.ZusatzfeldID, Wert = "XXL" }
                });

                // Material
                zusatzwerte.AddRange(new List<Zusatzwert>
                {
                    new Zusatzwert { ZusatzFeldID = material.ZusatzfeldID, Wert = "Baumwolle" },
                    new Zusatzwert { ZusatzFeldID = material.ZusatzfeldID, Wert = "Kunststoff" },
                    new Zusatzwert { ZusatzFeldID = material.ZusatzfeldID, Wert = "Aluminium" },
                    new Zusatzwert { ZusatzFeldID = material.ZusatzfeldID, Wert = "Edelstahl" }
                });

                // Prozessortyp
                zusatzwerte.AddRange(new List<Zusatzwert>
                {
                    new Zusatzwert { ZusatzFeldID = prozessor.ZusatzfeldID, Wert = "Intel i5" },
                    new Zusatzwert { ZusatzFeldID = prozessor.ZusatzfeldID, Wert = "Intel i7" },
                    new Zusatzwert { ZusatzFeldID = prozessor.ZusatzfeldID, Wert = "Intel i9" },
                    new Zusatzwert { ZusatzFeldID = prozessor.ZusatzfeldID, Wert = "AMD Ryzen 5" },
                    new Zusatzwert { ZusatzFeldID = prozessor.ZusatzfeldID, Wert = "AMD Ryzen 7" }
                });

                // Arbeitsspeicher
                zusatzwerte.AddRange(new List<Zusatzwert>
                {
                    new Zusatzwert { ZusatzFeldID = ram.ZusatzfeldID, Wert = "8 GB" },
                    new Zusatzwert { ZusatzFeldID = ram.ZusatzfeldID, Wert = "16 GB" },
                    new Zusatzwert { ZusatzFeldID = ram.ZusatzfeldID, Wert = "32 GB" },
                    new Zusatzwert { ZusatzFeldID = ram.ZusatzfeldID, Wert = "64 GB" }
                });

                // Festplattentyp
                zusatzwerte.AddRange(new List<Zusatzwert>
                {
                    new Zusatzwert { ZusatzFeldID = festplatte.ZusatzfeldID, Wert = "SSD 256 GB" },
                    new Zusatzwert { ZusatzFeldID = festplatte.ZusatzfeldID, Wert = "SSD 512 GB" },
                    new Zusatzwert { ZusatzFeldID = festplatte.ZusatzfeldID, Wert = "SSD 1 TB" },
                    new Zusatzwert { ZusatzFeldID = festplatte.ZusatzfeldID, Wert = "HDD 1 TB" }
                });

                // Anschlusstyp
                zusatzwerte.AddRange(new List<Zusatzwert>
                {
                    new Zusatzwert { ZusatzFeldID = anschluss.ZusatzfeldID, Wert = "USB" },
                    new Zusatzwert { ZusatzFeldID = anschluss.ZusatzfeldID, Wert = "USB-C" },
                    new Zusatzwert { ZusatzFeldID = anschluss.ZusatzfeldID, Wert = "Bluetooth" },
                    new Zusatzwert { ZusatzFeldID = anschluss.ZusatzfeldID, Wert = "Kabelgebunden" }
                });

                // Schnittstellen
                zusatzwerte.AddRange(new List<Zusatzwert>
                {
                    new Zusatzwert { ZusatzFeldID = schnittstelle.ZusatzfeldID, Wert = "HDMI" },
                    new Zusatzwert { ZusatzFeldID = schnittstelle.ZusatzfeldID, Wert = "DisplayPort" },
                    new Zusatzwert { ZusatzFeldID = schnittstelle.ZusatzfeldID, Wert = "VGA" },
                    new Zusatzwert { ZusatzFeldID = schnittstelle.ZusatzfeldID, Wert = "USB 3.0" }
                });

                context.Set<Zusatzwert>().AddRange(zusatzwerte);
                context.SaveChanges();
            }
        }
    }

    private static void SeedArtikelgruppeZusatzfelder(AppDbContext context)
    {
        // Verknüpfe Artikelgruppen mit Zusatzfeldern
        if (!context.ArtikelgruppeZusatzfelder.Any())
        {
            var computerGruppe = context.Set<Artikelgruppe>().FirstOrDefault(a => a.Name == "Computer");
            var peripherieGruppe = context.Set<Artikelgruppe>().FirstOrDefault(a => a.Name == "Peripheriegeräte");
            var tshirtGruppe = context.Set<Artikelgruppe>().FirstOrDefault(a => a.Name == "T-Shirts");

            var farbe = context.Set<Zusatzfeld>().FirstOrDefault(z => z.Name == "Farbe");
            var größe = context.Set<Zusatzfeld>().FirstOrDefault(z => z.Name == "Größe");
            var material = context.Set<Zusatzfeld>().FirstOrDefault(z => z.Name == "Material");
            var prozessor = context.Set<Zusatzfeld>().FirstOrDefault(z => z.Name == "Prozessortyp");
            var ram = context.Set<Zusatzfeld>().FirstOrDefault(z => z.Name == "Arbeitsspeicher");
            var festplatte = context.Set<Zusatzfeld>().FirstOrDefault(z => z.Name == "Festplattentyp");
            var anschluss = context.Set<Zusatzfeld>().FirstOrDefault(z => z.Name == "Anschlusstyp");
            var schnittstelle = context.Set<Zusatzfeld>().FirstOrDefault(z => z.Name == "Schnittstelle");

            if (computerGruppe != null && peripherieGruppe != null && tshirtGruppe != null &&
                farbe != null && größe != null && material != null && prozessor != null &&
                ram != null && festplatte != null && anschluss != null && schnittstelle != null)
            {
                var verknüpfungen = new List<ArtikelgruppeZusatzfelder>
                {
                    // Computer-Gruppe
                    new ArtikelgruppeZusatzfelder { ArtikelgruppeID = computerGruppe.Id, ZusatzfelderID = farbe.ZusatzfeldID },
                    new ArtikelgruppeZusatzfelder { ArtikelgruppeID = computerGruppe.Id, ZusatzfelderID = prozessor.ZusatzfeldID },
                    new ArtikelgruppeZusatzfelder { ArtikelgruppeID = computerGruppe.Id, ZusatzfelderID = ram.ZusatzfeldID },
                    new ArtikelgruppeZusatzfelder { ArtikelgruppeID = computerGruppe.Id, ZusatzfelderID = festplatte.ZusatzfeldID },
                    
                    // Peripherie-Gruppe
                    new ArtikelgruppeZusatzfelder { ArtikelgruppeID = peripherieGruppe.Id, ZusatzfelderID = farbe.ZusatzfeldID },
                    new ArtikelgruppeZusatzfelder { ArtikelgruppeID = peripherieGruppe.Id, ZusatzfelderID = anschluss.ZusatzfeldID },
                    new ArtikelgruppeZusatzfelder { ArtikelgruppeID = peripherieGruppe.Id, ZusatzfelderID = schnittstelle.ZusatzfeldID },
                    new ArtikelgruppeZusatzfelder { ArtikelgruppeID = peripherieGruppe.Id, ZusatzfelderID = material.ZusatzfeldID },
                    
                    // T-Shirt-Gruppe
                    new ArtikelgruppeZusatzfelder { ArtikelgruppeID = tshirtGruppe.Id, ZusatzfelderID = farbe.ZusatzfeldID },
                    new ArtikelgruppeZusatzfelder { ArtikelgruppeID = tshirtGruppe.Id, ZusatzfelderID = größe.ZusatzfeldID },
                    new ArtikelgruppeZusatzfelder { ArtikelgruppeID = tshirtGruppe.Id, ZusatzfelderID = material.ZusatzfeldID }
                };

                context.Set<ArtikelgruppeZusatzfelder>().AddRange(verknüpfungen);
                context.SaveChanges();
            }
        }
    }

    private static void SeedArtikelData(AppDbContext context, DateTime currentDateTime, string currentUser)
    {
        // Seed Artikel data if none exists
        List<Artikel> artikelList = new List<Artikel>();
        if (!context.Artikel.Any())
        {
            var computerGruppe = context.Set<Artikelgruppe>().FirstOrDefault(a => a.Name == "Computer");
            var peripherieGruppe = context.Set<Artikelgruppe>().FirstOrDefault(a => a.Name == "Peripheriegeräte");
            var tshirtGruppe = context.Set<Artikelgruppe>().FirstOrDefault(a => a.Name == "T-Shirts");

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
                    ErstelltAm = currentDateTime,
                    ErstelltVon = currentUser,
                    BearbeitetAm = currentDateTime,
                    BearbeitetVon = currentUser
                },
                new Artikel
                {
                    Name = "Gaming Notebook Ultimate",
                    Preis = 1499.99m,
                    Mindestbestand = 3,
                    Maximalbestand = 30,
                    Menge = 15,
                    Status = ArtikelStatus.Verfügbar,
                    ErstelltAm = currentDateTime,
                    ErstelltVon = currentUser,
                    BearbeitetAm = currentDateTime,
                    BearbeitetVon = currentUser
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
                    ErstelltAm = currentDateTime,
                    ErstelltVon = currentUser,
                    BearbeitetAm = currentDateTime,
                    BearbeitetVon = currentUser
                },
                new Artikel
                {
                    Name = "Mechanische Tastatur",
                    Preis = 59.99m,
                    Mindestbestand = 10,
                    Maximalbestand = 80,
                    Menge = 30,
                    Status = ArtikelStatus.Verfügbar,
                    ErstelltAm = currentDateTime,
                    ErstelltVon = currentUser,
                    BearbeitetAm = currentDateTime,
                    BearbeitetVon = currentUser
                },
                new Artikel
                {
                    Name = "4K Monitor 27 Zoll",
                    Preis = 249.99m,
                    Mindestbestand = 3,
                    Maximalbestand = 30,
                    Menge = 12,
                    Status = ArtikelStatus.Verfügbar,
                    ErstelltAm = currentDateTime,
                    ErstelltVon = currentUser,
                    BearbeitetAm = currentDateTime,
                    BearbeitetVon = currentUser
                },
                new Artikel
                {
                    Name = "USB-Stick 128GB",
                    Preis = 14.99m,
                    Mindestbestand = 20,
                    Maximalbestand = 200,
                    Menge = 75,
                    Status = ArtikelStatus.Verfügbar,
                    ErstelltAm = currentDateTime,
                    ErstelltVon = currentUser,
                    BearbeitetAm = currentDateTime,
                    BearbeitetVon = currentUser
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
                    ErstelltAm = currentDateTime,
                    ErstelltVon = currentUser,
                    BearbeitetAm = currentDateTime,
                    BearbeitetVon = currentUser
                },
                new Artikel
                {
                    Name = "Event T-Shirt 2025",
                    Preis = 24.99m,
                    Mindestbestand = 10,
                    Maximalbestand = 100,
                    Menge = 50,
                    Status = ArtikelStatus.Verfügbar,
                    ErstelltAm = currentDateTime,
                    ErstelltVon = currentUser,
                    BearbeitetAm = currentDateTime,
                    BearbeitetVon = currentUser
                }
            };
            context.Artikel.AddRange(artikelList);
            context.SaveChanges();
        }
    }

    private static void SeedArtikelStatistiken(AppDbContext context, DateTime currentDateTime, string currentUser)
    {
        List<Artikel> artikelList = context.Artikel.ToList();
        if(!context.ArtikelStatistiken.Any())
            {
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
                        ErstelltAm = currentDateTime,
                        ErstelltVon = currentUser,
                        BearbeitetAm = currentDateTime,
                        BearbeitetVon = currentUser
                    };
                    artikelStatistikList.Add(statistik);
                }

                context.ArtikelStatistiken.AddRange(artikelStatistikList);
                context.SaveChanges();
            }
    }

    private static void SeedArtikelZusatzwerte(AppDbContext context, DateTime currentDateTime, string currentUser)
    {
        List<Artikel> artikelList = context.Artikel.ToList();
        if(!context.ArtikelZusatzWert.Any())
            {
            // Zusatzwerte zu Artikeln hinzufügen
            var schwarz = context.Set<Zusatzwert>().FirstOrDefault(z => z.Wert == "Schwarz");
            var weiß = context.Set<Zusatzwert>().FirstOrDefault(z => z.Wert == "Weiß");
            var blau = context.Set<Zusatzwert>().FirstOrDefault(z => z.Wert == "Blau");
            var rot = context.Set<Zusatzwert>().FirstOrDefault(z => z.Wert == "Rot");

            var sizeS = context.Set<Zusatzwert>().FirstOrDefault(z => z.Wert == "S");
            var sizeM = context.Set<Zusatzwert>().FirstOrDefault(z => z.Wert == "M");
            var sizeL = context.Set<Zusatzwert>().FirstOrDefault(z => z.Wert == "L");
            var sizeXL = context.Set<Zusatzwert>().FirstOrDefault(z => z.Wert == "XL");

            var baumwolle = context.Set<Zusatzwert>().FirstOrDefault(z => z.Wert == "Baumwolle");
            var kunststoff = context.Set<Zusatzwert>().FirstOrDefault(z => z.Wert == "Kunststoff");
            var aluminium = context.Set<Zusatzwert>().FirstOrDefault(z => z.Wert == "Aluminium");

            var i5 = context.Set<Zusatzwert>().FirstOrDefault(z => z.Wert == "Intel i5");
            var i7 = context.Set<Zusatzwert>().FirstOrDefault(z => z.Wert == "Intel i7");
            var ryzen5 = context.Set<Zusatzwert>().FirstOrDefault(z => z.Wert == "AMD Ryzen 5");

            var ram8 = context.Set<Zusatzwert>().FirstOrDefault(z => z.Wert == "8 GB");
            var ram16 = context.Set<Zusatzwert>().FirstOrDefault(z => z.Wert == "16 GB");
            var ram32 = context.Set<Zusatzwert>().FirstOrDefault(z => z.Wert == "32 GB");

            var ssd256 = context.Set<Zusatzwert>().FirstOrDefault(z => z.Wert == "SSD 256 GB");
            var ssd512 = context.Set<Zusatzwert>().FirstOrDefault(z => z.Wert == "SSD 512 GB");
            var ssd1tb = context.Set<Zusatzwert>().FirstOrDefault(z => z.Wert == "SSD 1 TB");

            var usb = context.Set<Zusatzwert>().FirstOrDefault(z => z.Wert == "USB");
            var usbc = context.Set<Zusatzwert>().FirstOrDefault(z => z.Wert == "USB-C");
            var bluetooth = context.Set<Zusatzwert>().FirstOrDefault(z => z.Wert == "Bluetooth");

            var hdmi = context.Set<Zusatzwert>().FirstOrDefault(z => z.Wert == "HDMI");
            var displayport = context.Set<Zusatzwert>().FirstOrDefault(z => z.Wert == "DisplayPort");
            var usb30 = context.Set<Zusatzwert>().FirstOrDefault(z => z.Wert == "USB 3.0");

            if (schwarz != null && weiß != null && blau != null && rot != null &&
                sizeM != null && sizeL != null &&
                baumwolle != null && kunststoff != null && aluminium != null &&
                i5 != null && i7 != null && ram16 != null && ram32 != null &&
                ssd512 != null && ssd1tb != null && usb != null && bluetooth != null &&
                hdmi != null && displayport != null && usb30 != null)
            {
                var artikelZusatzwerte = new List<ArtikelZusatzWert>
                {
                    // Business Laptop Pro
                    new ArtikelZusatzWert { ArtikelId = artikelList[0].Id, ZusatzwertId = schwarz.Id },
                    new ArtikelZusatzWert { ArtikelId = artikelList[0].Id, ZusatzwertId = i5.Id },
                    new ArtikelZusatzWert { ArtikelId = artikelList[0].Id, ZusatzwertId = ram16.Id },
                    new ArtikelZusatzWert { ArtikelId = artikelList[0].Id, ZusatzwertId = ssd512.Id },
                    
                    // Gaming Notebook Ultimate
                    new ArtikelZusatzWert { ArtikelId = artikelList[1].Id, ZusatzwertId = rot.Id },
                    new ArtikelZusatzWert { ArtikelId = artikelList[1].Id, ZusatzwertId = i7.Id },
                    new ArtikelZusatzWert { ArtikelId = artikelList[1].Id, ZusatzwertId = ram32.Id },
                    new ArtikelZusatzWert { ArtikelId = artikelList[1].Id, ZusatzwertId = ssd1tb.Id },
                    
                    // Ergonomische Maus
                    new ArtikelZusatzWert { ArtikelId = artikelList[2].Id, ZusatzwertId = schwarz.Id },
                    new ArtikelZusatzWert { ArtikelId = artikelList[2].Id, ZusatzwertId = kunststoff.Id },
                    new ArtikelZusatzWert { ArtikelId = artikelList[2].Id, ZusatzwertId = usb.Id },
                    new ArtikelZusatzWert { ArtikelId = artikelList[2].Id, ZusatzwertId = bluetooth.Id },
                    
                    // Mechanische Tastatur
                    new ArtikelZusatzWert { ArtikelId = artikelList[3].Id, ZusatzwertId = schwarz.Id },
                    new ArtikelZusatzWert { ArtikelId = artikelList[3].Id, ZusatzwertId = aluminium.Id },
                    new ArtikelZusatzWert { ArtikelId = artikelList[3].Id, ZusatzwertId = usb.Id },
                    
                    // 4K Monitor
                    new ArtikelZusatzWert { ArtikelId = artikelList[4].Id, ZusatzwertId = schwarz.Id },
                    new ArtikelZusatzWert { ArtikelId = artikelList[4].Id, ZusatzwertId = kunststoff.Id },
                    new ArtikelZusatzWert { ArtikelId = artikelList[4].Id, ZusatzwertId = hdmi.Id },
                    new ArtikelZusatzWert { ArtikelId = artikelList[4].Id, ZusatzwertId = displayport.Id },
                    
                    // USB-Stick
                    new ArtikelZusatzWert { ArtikelId = artikelList[5].Id, ZusatzwertId = blau.Id },
                    new ArtikelZusatzWert { ArtikelId = artikelList[5].Id, ZusatzwertId = kunststoff.Id },
                    new ArtikelZusatzWert { ArtikelId = artikelList[5].Id, ZusatzwertId = usb30.Id },
                    
                    // Firmen T-Shirt Logo
                    new ArtikelZusatzWert { ArtikelId = artikelList[6].Id, ZusatzwertId = weiß.Id },
                    new ArtikelZusatzWert { ArtikelId = artikelList[6].Id, ZusatzwertId = sizeM.Id },
                    new ArtikelZusatzWert { ArtikelId = artikelList[6].Id, ZusatzwertId = baumwolle.Id },
                    
                    // Event T-Shirt 2025
                    new ArtikelZusatzWert { ArtikelId = artikelList[7].Id, ZusatzwertId = blau.Id },
                    new ArtikelZusatzWert { ArtikelId = artikelList[7].Id, ZusatzwertId = sizeL.Id },
                    new ArtikelZusatzWert { ArtikelId = artikelList[7].Id, ZusatzwertId = baumwolle.Id }
                };

                context.Set<ArtikelZusatzWert>().AddRange(artikelZusatzwerte);
                context.SaveChanges();
            }
        }
    }  

    private static void SeedWareneingaenge(AppDbContext context, DateTime currentDateTime, string currentUser)
    {
        List<Artikel> artikelList = context.Artikel.ToList();
        if(artikelList == null)
            return;
        if (!context.Wareneingaenge.Any())
        {
            // Erstelle zwei Wareneingänge mit jeweils unterschiedlichen Artikelpositionen
            var wareneingang1 = new Wareneingaenge
            {
                Gesamtpreis = 850.0m,
                AllgemeineBemerkungen = "Standardlieferung vom Hauptlieferanten",
                ErstelltAm = currentDateTime.AddDays(-14),
                ErstelltVon = currentUser,
                BearbeitetAm = currentDateTime.AddDays(-14),
                BearbeitetVon = currentUser
            };

            var wareneingang2 = new Wareneingaenge
            {
                Gesamtpreis = 1200.0m,
                AllgemeineBemerkungen = "Dringende Nachbestellung",
                ErstelltAm = currentDateTime.AddDays(-5),
                ErstelltVon = currentUser,
                BearbeitetAm = currentDateTime.AddDays(-5),
                BearbeitetVon = currentUser
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
    }

    private static void SeedWarenausganege(AppDbContext context, DateTime currentDateTime, string currentUser)
    {
        List<Artikel> artikelList = context.Artikel.ToList();
        if(artikelList == null)
            return;
        if (!context.Warenausgaenge.Any())
        {
            // Erstelle zwei Warenausgänge
            var warenausgang1 = new Warenausgaenge
            {
                AllgemeineBemerkungen = "Bestellung für IT-Abteilung",
                ErstelltAm = currentDateTime.AddDays(-10),
                ErstelltVon = currentUser,
                BearbeitetAm = currentDateTime.AddDays(-10),
                BearbeitetVon = currentUser,
                Zweck = WarenausgangZweckEnum.Bestellung
            };

            var warenausgang2 = new Warenausgaenge
            {
                AllgemeineBemerkungen = "Verkauf an externen Kunden",
                ErstelltAm = currentDateTime.AddDays(-3),
                ErstelltVon = currentUser,
                BearbeitetAm = currentDateTime.AddDays(-3),
                BearbeitetVon = currentUser,
                Zweck = WarenausgangZweckEnum.KooperationspartnerOesterreich
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
                    Menge = 2,
                    Bemerkung = "Neue Laptops für Entwickler"
                },
                new WarenausgangArtikelPositionen
                {
                    WarenausgangId = warenausgang1.Id,
                    ArtikelId = artikelList[2].Id, // Maus
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
                    Menge = 1,
                    Verkaufspreis = 249.99m,
                    Gesamtpreis = 249.99m,
                    Rechnungsnummer = "RE-2025-0042"
                },
                new WarenausgangArtikelPositionen
                {
                    WarenausgangId = warenausgang2.Id,
                    ArtikelId = artikelList[5].Id, // USB-Stick
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

    private static void SeedInventuren(AppDbContext context, DateTime currentDateTime, string currentUser)
    {
        List<Artikel> artikelList = context.Artikel.ToList();
        if(artikelList == null)
            return;
        // Seed für Inventuren
        if (!context.Inventuren.Any())
        {
            // Aktuelle Zeit und Benutzer für die Seed-Daten
            var inventur1 = new Inventur
            {
                Bezeichnung = "Jahresinventur 2024",
                StartDatum = currentDateTime.AddDays(-30),
                AbschlussDatum = currentDateTime.AddDays(-28),
                Status = InventurStatus.Abgeschlossen,
                Bemerkung = "Reguläre Jahresinventur",
                ErstelltVon = currentUser,
                ErstelltAm = currentDateTime.AddDays(-30),
                BearbeitetVon = currentUser,
                BearbeitetAm = currentDateTime.AddDays(-28)
            };

            var inventur2 = new Inventur
            {
                Bezeichnung = "Quartalsinventur Q2/2025",
                StartDatum = currentDateTime.AddDays(-1),
                Status = InventurStatus.InBearbeitung,
                Bemerkung = "Überprüfung der Lagerbestände",
                ErstelltVon = currentUser,
                ErstelltAm = currentDateTime.AddDays(-2),
                BearbeitetVon = currentUser,
                BearbeitetAm = currentDateTime.AddDays(-1)
            };

            context.Inventuren.Add(inventur1);
            context.Inventuren.Add(inventur2);
            context.SaveChanges();

            // Inventurpositionen für die abgeschlossene Inventur
            var artikel = context.Artikel.ToList();

            var positionen1 = artikelList.Select(item => new InventurPosition
            {
                InventurId = inventur1.Id,
                ArtikelId = item.Id,
                Menge = item.Menge,
                GezaehlteMenge = item.Menge + (item.Id % 3 == 0 ? 2 : (item.Id % 4 == 0 ? -1 : 0)),
                IstGeprueft = true,
                DifferenzWert = (item.Id % 3 == 0 || item.Id % 4 == 0) ? (item.Menge + (item.Id % 3 == 0 ? 2 : -1)) * item.Preis : 0,
                Bemerkung = (item.Id % 3 == 0 || item.Id % 4 == 0) ? "Abweichung festgestellt" : "Bestand korrekt",
                ErstelltVon = currentUser,
                ErstelltAm = currentDateTime.AddDays(-30),
                BearbeitetVon = currentUser,
                BearbeitetAm = currentDateTime.AddDays(-29)
            }).ToList();

            var positionen2 = artikelList.Select(item => new InventurPosition
            {
                InventurId = inventur2.Id,
                ArtikelId = item.Id,
                Menge = item.Menge,
                GezaehlteMenge = item.Id % 2 == 0 ? (int?)(item.Menge + (item.Id % 5 == 0 ? 1 : 0)) : null,
                IstGeprueft = item.Id % 2 == 0,
                DifferenzWert = (item.Id % 2 == 0 && item.Menge + (item.Id % 5 == 0 ? 1 : 0) != item.Menge) ? (item.Menge + (item.Id % 5 == 0 ? 1 : 0) - item.Menge) * item.Preis : null,
                Bemerkung = item.Id % 2 == 0 ? "Bereits gezählt" : null,
                ErstelltVon = currentUser,
                ErstelltAm = currentDateTime.AddDays(-1),
                BearbeitetVon = currentUser,
                BearbeitetAm = item.Id % 2 == 0 ? currentDateTime.AddHours(-2) : currentDateTime.AddDays(-1)
            }).ToList();

            context.InventurPositionen.AddRange(positionen1);
            context.InventurPositionen.AddRange(positionen2);
            context.SaveChanges();
        }
    }

    private static void SeedLieferanten(AppDbContext context)
    {
        if (!context.Lieferanten.Any())
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
    }

    private static void SeedArtikelLieferant(AppDbContext context, DateTime currentDateTime, string currentUser)
    {
        if (!context.ArtikelLieferanten.Any())
        {
            var artikelList = context.Artikel.ToList();
            var lieferanten = context.Lieferanten.ToList();

            var techSupply = lieferanten.FirstOrDefault(l => l.Firma == "TechSupply GmbH");
            var officeSolutions = lieferanten.FirstOrDefault(l => l.Firma == "Office Solutions AG");
            var electronicWholesale = lieferanten.FirstOrDefault(l => l.Firma == "ElectronicWholesale KG");

            var artikelLieferanten = new List<ArtikelLieferant>();

            foreach (var artikel in artikelList)
            {
                Lieferant? zugeordneterLieferant = null;

                if (artikel.Name.Contains("Laptop", StringComparison.OrdinalIgnoreCase)
                    || artikel.Name.Contains("Notebook", StringComparison.OrdinalIgnoreCase)
                    || artikel.Name.Contains("Computer", StringComparison.OrdinalIgnoreCase))
                {
                    zugeordneterLieferant = techSupply;
                }
                else if (artikel.Name.Contains("Tastatur", StringComparison.OrdinalIgnoreCase)
                    || artikel.Name.Contains("Maus", StringComparison.OrdinalIgnoreCase)
                    || artikel.Name.Contains("Monitor", StringComparison.OrdinalIgnoreCase)
                    || artikel.Name.Contains("USB", StringComparison.OrdinalIgnoreCase))
                {
                    zugeordneterLieferant = officeSolutions;
                }
                else
                {
                    zugeordneterLieferant = electronicWholesale;
                }

                if (zugeordneterLieferant != null)
                {
                    var einkaufspreis = Math.Round(artikel.Preis * 0.7m, 2); // 70% vom Verkaufspreis

                    artikelLieferanten.Add(new ArtikelLieferant
                    {
                        ArtikelId = artikel.Id,
                        LieferantId = zugeordneterLieferant.Id,
                        Einkaufspreis = einkaufspreis,
                        Mindestbestellmenge = 5,
                        Lieferzeit = 7,
                        ArtikelNrBeimLieferanten = $"L-{zugeordneterLieferant.Id}-A-{artikel.Id}",
                        IstAktiv = true,
                        IstPrimaerLieferant = true,
                        GueltigVon = currentDateTime.AddMonths(-1),
                        GueltigBis = null,
                        ErstelltAm = currentDateTime,
                        ErstelltVon = currentUser,
                        BearbeitetAm = currentDateTime,
                        BearbeitetVon = currentUser
                    });
                }
            }

            context.ArtikelLieferanten.AddRange(artikelLieferanten);
            context.SaveChanges();
        }
    }
}