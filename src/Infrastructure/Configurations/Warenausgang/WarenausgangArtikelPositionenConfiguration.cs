using System;
using Domain.Entities.Warenausgang;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Warenausgang;

public class WarenausgangArtikelConfiguration : IEntityTypeConfiguration<WarenausgangArtikelPositionen>
{
    public void Configure(EntityTypeBuilder<WarenausgangArtikelPositionen> builder)
    {
        builder.ToTable("WarenausgangArtikelPositionen");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).UseIdentityColumn();

        builder.Property(p => p.Menge)
            .IsRequired();

        builder.Property(p => p.Bemerkung)
            .HasMaxLength(500);

        builder.Property(p => p.Verkaufspreis)
            .HasPrecision(18, 2);

        builder.Property(p => p.Rechnungsnummer)
            .HasMaxLength(50);

        builder.Property(p => p.Gesamtpreis)
            .HasPrecision(18, 2);

        builder.HasOne(p => p.Artikel)
            .WithMany(a => a.Warenausgaenge)
            .HasForeignKey(p => p.ArtikelId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
