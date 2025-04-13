using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using Domain.Entities.Artikel;
using Domain.Entities.Inventur;
using Domain.Entities.Lieferant;
using Domain.Entities.Wareneingang;
using Domain.Entities.Warenausgang;
using Domain.Entities.Authentication;
using Infrastructure.Configurations.Artikel;
using Infrastructure.Configurations.Authentication;
using Infrastructure.Configurations.Inventur;
using Infrastructure.Configurations.Lieferant;
using Infrastructure.Configurations.Wareneingang;
using Infrastructure.Configurations.Warenausgang;
using Domain.Common;
using Domain.Entities.Employees;
using Infrastructure.Configurations.Employees;

namespace Infrastructure.Context;

public class AppDbContext : DbContext
{
    private readonly ISystemClock _systemClock;

    public AppDbContext(DbContextOptions<AppDbContext> options, ISystemClock systemClock) : base(options)
    {
        this._systemClock = systemClock;
    }

     public DbSet<Employee> Employees { get; set; }
     public DbSet<Benefit> Benefits { get; set; }
     public DbSet<EmployeeBenefit> EmployeeBenefits { get; set; }

    // Neue DbSets für Artikel, Lieferant und Wareneingang
    public DbSet<Artikel> Artikel { get; set; }
    public DbSet<ArtikelStatistik> ArtikelStatistiken { get; set; }
    public DbSet<Lieferant> Lieferanten { get; set; }
    public DbSet<Wareneingaenge> Wareneingaenge { get; set; }
    public DbSet<WareneingangArtikelPositionen> WareneingangArtikel { get; set; }
    public DbSet<Warenausgaenge> Warenausgaenge { get; set; }
    public DbSet<WarenausgangArtikelPositionen> WarenausgangArtikelPosition { get; set; }
    public DbSet<Artikelgruppe> Artikelgruppe { get; set; }
    public DbSet<ArtikelgruppeZusatzfelder> ArtikelgruppeZusatzfelder { get; set; }
    public DbSet<ArtikelZusatzWert> ArtikelZusatzWert { get; set; }
    public DbSet<Zusatzfeld> Zusatzfeld { get; set; }
    public DbSet<Produktkategorie> Produktkategorie { get; set; }
    public DbSet<Zusatzwert> Zusatzwert { get; set; }
    public DbSet<Inventur> Inventuren { get; set; }
    public DbSet<InventurPosition> InventurPositionen { get; set; }
    public DbSet<InventurBerichte> InventurBerichte { get; set; }
    public DbSet<ArtikelInventurHistorie> ArtikelInventurHistorie { get; set; }
    public DbSet<ArtikelLieferant> ArtikelLieferanten { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserGruppen> UserGruppen { get; set; }
    public DbSet<UserGruppenUser> UserGruppenUsers { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EmployeeBenefitConfiguration());

        // Wareneingang - Lieferant (N:1)
        modelBuilder.ApplyConfiguration(new WareneingaengeConfiguration());
        modelBuilder.ApplyConfiguration(new WareneingangArtikelPositionenConfiguration());

        modelBuilder.ApplyConfiguration(new WarenausgaengeConfiguration());
        modelBuilder.ApplyConfiguration(new WarenausgangArtikelConfiguration());


        // definiere berechnete Felder auf DB in ArtikelStatistiken
        modelBuilder.ApplyConfiguration(new ArtikelStatistikConfiguration());

        modelBuilder.ApplyConfiguration(new ArtikelgruppeConfiguration());
        modelBuilder.ApplyConfiguration(new ArtikelgruppeZusatzfelderConfiguration());
        modelBuilder.ApplyConfiguration(new ArtikelZusatzWertConfiguration());
        modelBuilder.ApplyConfiguration(new ProduktkategerieConfiguration());
        modelBuilder.ApplyConfiguration(new ZusatzfeldConfiguration());
        modelBuilder.ApplyConfiguration(new ZusatzwertConfiguration());
        modelBuilder.ApplyConfiguration(new InventurConfiguration());
        modelBuilder.ApplyConfiguration(new InventurPositionConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new UserGruppenConfiguration());
        modelBuilder.ApplyConfiguration(new UserGruppenUserConfiguration());
    }

    public override int SaveChanges()
    {
        UpdateAuditFields();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries<AuditableEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.ErstelltVon = "TheCreateUser";
                entry.Entity.ErstelltAm = _systemClock.UtcNow.UtcDateTime;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.BearbeitetVon = "TheUpdateUser";
                entry.Entity.BearbeitetAm = _systemClock.UtcNow.UtcDateTime;
            }
        }
    }
}