using System;
using Artikelsystem.Api.Features.Artikel.Models.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artikelsystem.Api.Features.Artikel.Configurations;

public class ArtikelConfiguration : IEntityTypeConfiguration<Models.Entitys.Artikel>
{
    public void Configure(EntityTypeBuilder<Models.Entitys.Artikel> builder)
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