using System;
using Domain.Entities.Lieferant;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Lieferant;

public class ArtikelLieferantConfiguration : IEntityTypeConfiguration<ArtikelLieferant>
{
    public void Configure(EntityTypeBuilder<ArtikelLieferant> builder)
    {
        builder.HasKey(al => al.Id);

        // Beziehung zu Artikel
        builder.HasOne(al => al.Artikel)
            .WithMany(a => a.ArtikelLieferanten)
            .HasForeignKey(al => al.ArtikelId)
            .OnDelete(DeleteBehavior.Restrict);

        // Beziehung zu Lieferant
        builder.HasOne(al => al.Lieferant)
            .WithMany(l => l.ArtikelLieferanten)
            .HasForeignKey(al => al.LieferantId)
            .OnDelete(DeleteBehavior.Restrict);

        // Preisfestlegung
        builder.Property(al => al.Einkaufspreis)
            .HasPrecision(18, 2)
            .IsRequired();

        // Gültigkeitszeitraum
        builder.Property(al => al.GueltigVon);
        builder.Property(al => al.GueltigBis);
    }
}
