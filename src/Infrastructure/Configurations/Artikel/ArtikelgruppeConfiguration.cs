using System;
using Domain.Entities.Artikel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Artikel;

public class ArtikelgruppeConfiguration : IEntityTypeConfiguration<Artikelgruppe>
{
    public void Configure(EntityTypeBuilder<Artikelgruppe> builder)
    {
        builder.HasKey(ag => ag.Id);

        builder.Property(ag => ag.Name)
            .IsRequired()
            .HasMaxLength(100);

        // Artikelgruppe -> Produktkategorie n:1
        builder.HasOne(ag => ag.Produktkategorie)
            .WithMany(ag => ag.ArtikelGruppen)
            .HasForeignKey(ag => ag.ProduktkategorieId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}