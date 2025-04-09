using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artikelsystem.Domain.Entities.Inventur;

public class InventurPositionConfiguration : IEntityTypeConfiguration<InventurPosition>
{
    public void Configure(EntityTypeBuilder<InventurPosition> builder)
    {
        builder.HasKey(ip => ip.Id);

        builder.Property(ip => ip.Bemerkung)
            .HasMaxLength(100);

        builder.HasOne(ip => ip.Inventur)
            .WithMany(i => i.Positionen)
            .HasForeignKey(ip => ip.InventurId)
            .OnDelete(DeleteBehavior.Cascade);


        builder.HasOne(ip => ip.Artikel)
            .WithMany()
            .HasForeignKey(ip => ip.ArtikelId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(ip => ip.DifferenzWert)
            .HasPrecision(18, 2);

        // Differenz als berechnetes Feld
        builder.Ignore(ip => ip.Differenz);
    }
}
