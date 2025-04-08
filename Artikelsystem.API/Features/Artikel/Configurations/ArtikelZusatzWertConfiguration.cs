using System;
using Artikelsystem.Api.Features.Artikel.Models.Entitys;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artikelsystem.Api.Features.Artikel.Configurations;

public class ArtikelZusatzWertConfiguration : IEntityTypeConfiguration<ArtikelZusatzWert>
{
    public void Configure(EntityTypeBuilder<ArtikelZusatzWert> builder)
    {
        builder.HasKey(azw => new { azw.ArtikelId, azw.ZusatzwertId });

        // Many zu Many zwischen Artikel und Zusatzwert
        builder.HasOne(azw => azw.Artikel)
            .WithMany(a => a.ArtikelZusatzWerte)
            .HasForeignKey(azw => azw.ArtikelId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(azw => azw.Zusatzwert)
            .WithMany(a => a.ArtikelZusatzwerte)
            .HasForeignKey(az => az.ZusatzwertId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
