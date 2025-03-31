using System;
using Artikelsystem.Api.Features.Wareneingang.Models.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artikelsystem.Api.Features.Wareneingang.Configurations;

public class WareneingangArtikelConfiguration : IEntityTypeConfiguration<WareneingangArtikel>
{
    public void Configure(EntityTypeBuilder<WareneingangArtikel> builder)
    {
            builder.HasKey(wa => wa.Id);

            // Eindeutigkeitsbeschränkung erstellen
            builder.HasIndex(wa => new { wa.WareneingangId, wa.ArtikelId })
                   .IsUnique();

            // Beziehungen definieren
            builder.HasOne(wa => wa.Artikel)
                   .WithMany(a => a.Wareneingaenge)
                   .HasForeignKey(wa => wa.ArtikelId);

            builder.HasOne(wa => wa.Wareneingang)
                   .WithMany(w => w.ArtikelPositionen)
                   .HasForeignKey(wa => wa.WareneingangId);

            builder.Property(wa => wa.Gesamtpreis)
                   .HasComputedColumnSql("\"Menge\" * \"Einzelpreis\"", stored: true);  
    }
}
