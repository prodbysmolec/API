using System;
using Artikelsystem.Api.Features.Warenausgang.Models.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artikelsystem.Api.Features.Warenausgang.Configurations;

public class WarenausgaengeConfiguration : IEntityTypeConfiguration<Warenausgaenge>
{
    public void Configure(EntityTypeBuilder<Warenausgaenge> builder)
    {
        builder.ToTable("Warenausgaenge");

        builder.HasKey(w => w.Id);
        builder.Property(w => w.Id).UseIdentityColumn();

        builder.Property(w => w.AllgemeineBemerkungen)
            .HasMaxLength(500);

        builder.Property(w => w.Mitarbeiter)
            .HasMaxLength(100);

        // Beziehung zu den Positionen (1-zu-n)
        builder.HasMany(w => w.ArtikelPositionen)
               .WithOne(wp => wp.Warenausgang)
               .HasForeignKey(p => p.WarenausgangId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
