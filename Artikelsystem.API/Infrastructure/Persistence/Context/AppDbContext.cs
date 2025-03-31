using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Internal;
using Artikelsystem.Api.Models;
using Artikelsystem.Api.Features.Employees.Models.Entitys;
using Artikelsystem.Api.Features.Artikel.Models.Entitys;
using Artikelsystem.Api.Features.Lieferant.Models.Entitys;
using Artikelsystem.Api.Features.Wareneingang.Models.Entitys;
using Artikelsystem.Api.Features.Wareneingang.Configurations;
using Artikelsystem.Api.Features.Employees.Configurations;
using Artikelsystem.Api.Features.Artikel.Configurations;


namespace Artikelsystem.Api.Infrastructure.Persistence.Context;
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
    public DbSet<Wareneingang> Wareneingaenge { get; set; }
    public DbSet<WareneingangArtikel> WareneingangArtikel { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EmployeeBenefitConfiguration());

        // Wareneingang - Lieferant (N:1)
        modelBuilder.ApplyConfiguration(new WareneingangConfiguration());            
        modelBuilder.ApplyConfiguration(new WareneingangArtikelConfiguration());

        // definiere berechnete Felder auf DB in ArtikelStatistiken
        modelBuilder.ApplyConfiguration(new ArtikelStatistikConfiguration());
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
                entry.Entity.CreatedBy = "TheCreateUser";
                entry.Entity.CreatedOn = _systemClock.UtcNow.UtcDateTime;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.LastModifiedBy = "TheUpdateUser";
                entry.Entity.LastModifiedOn = _systemClock.UtcNow.UtcDateTime;
            }
        }
    }
}