using System;
using Domain.Entities.Wareneingang;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Configurations.Wareneingang;

public class WareneingaengeConfiguration : IEntityTypeConfiguration<Wareneingaenge>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Wareneingaenge> builder)
    {
        builder.HasKey(e => e.Id);

        // Id als Autoinkrement
        builder.Property(w => w.Id).UseIdentityColumn();

        builder.Property(w => w.Gesamtpreis)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(w => w.AllgemeineBemerkungen)
            .HasMaxLength(1000);

        // Setze 1:n 
        // Wenn ein Wareneingang gelöscht wird werden die Positionen auch gelöscht
        builder.HasMany(w => w.WareneingangsPositionen)
            .WithOne(p => p.Wareneingang)
            .HasForeignKey(p => p.WareneingangId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}