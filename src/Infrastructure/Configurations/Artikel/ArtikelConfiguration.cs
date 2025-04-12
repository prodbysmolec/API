using System;
using Domain.Entities.Artikel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Artikel;

public class ArtikelConfiguration : IEntityTypeConfiguration<Domain.Entities.Artikel.Artikel>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Artikel.Artikel> builder)
    {
        // Artikel - ArtikelStatistik (1:1)
        builder
            .HasOne(a => a.ArtikelStatistik)
            .WithOne(s => s.Artikel)
            .HasForeignKey<ArtikelStatistik>(s => s.ArtikelId);

        builder
            .HasMany(a => a.ArtikelZusatzWerte)
            .WithOne(azw => azw.Artikel)
            .HasForeignKey(az => az.ArtikelId);
    }
}