using System;
using Domain.Entities.Artikel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Artikel;

public class ProduktkategerieConfiguration : IEntityTypeConfiguration<Produktkategorie>
{
    public void Configure(EntityTypeBuilder<Produktkategorie> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Beschreibung)
            .HasMaxLength(500);
    }
}
