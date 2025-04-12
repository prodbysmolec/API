using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities.Lieferant;
namespace Infrastructure.Configurations.Lieferant;

public class LieferantConfiguration : IEntityTypeConfiguration<Domain.Entities.Lieferant.Lieferant>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Lieferant.Lieferant> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Firma)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(l => l.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(l => l.Vorname)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(l => l.EmailAdresse)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(l => l.Strasse)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(l => l.Hausnummer)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(l => l.PLZ)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(l => l.Ort)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(l => l.Telefonnummer)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(l => l.Notizen)
            .HasMaxLength(500);

        builder.Property(l => l.IstAktiv)
            .HasDefaultValue(true);

        // Beziehung zu ArtikelLieferanten
        builder.HasMany(l => l.ArtikelLieferanten)
            .WithOne(al => al.Lieferant)
            .HasForeignKey(al => al.LieferantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
