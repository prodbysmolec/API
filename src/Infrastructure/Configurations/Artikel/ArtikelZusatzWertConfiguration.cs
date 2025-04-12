using System;
using Domain.Entities.Artikel;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Artikel;

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
