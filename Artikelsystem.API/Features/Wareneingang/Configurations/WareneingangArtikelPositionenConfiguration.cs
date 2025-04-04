using System;
using Artikelsystem.Api.Features.Wareneingang.Models.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artikelsystem.Api.Features.Wareneingang.Configurations;

public class WareneingangArtikelPositionenConfiguration : IEntityTypeConfiguration<WareneingangArtikelPositionen>
{
    public void Configure(EntityTypeBuilder<WareneingangArtikelPositionen> builder)
    {
       builder.ToTable("WareneingangArtikelPositionen");
       
       builder.HasKey(wareneingangPosition => wareneingangPosition.Id);
       builder.Property(wareneingangPosition => wareneingangPosition.Id).UseIdentityColumn();

       builder.Property(wareneingangPosition => wareneingangPosition.Menge)
              .IsRequired();

       builder.Property(wareneingangPosition => wareneingangPosition.Einzelpreis)
            .HasComputedColumnSql("\"Menge\" * \"Gesamtpreis\"", stored: true);

       builder.Property(wareneingangPosition => wareneingangPosition.Gesamtpreis)
            .HasPrecision(18, 2);

       // Eindeutigkeitsbeschränkung erstellen
       builder.HasIndex(wareneingangPosition => new { wareneingangPosition.WareneingangId, wareneingangPosition.ArtikelId })
              .IsUnique();

       // Beziehungen definieren

       // Wenn eine WareneingangPosition gelöscht wird soll der Artikel nicht gelöscht werden!
       builder.HasOne(wareneingangPosition => wareneingangPosition.Artikel)
              .WithMany(a => a.Wareneingaenge)
              .HasForeignKey(wa => wa.ArtikelId)
              .OnDelete(DeleteBehavior.Restrict);

       builder.HasOne(wa => wa.Wareneingang)
              .WithMany(w => w.WareneingangsPositionen)
              .HasForeignKey(wa => wa.WareneingangId);
    }
}
