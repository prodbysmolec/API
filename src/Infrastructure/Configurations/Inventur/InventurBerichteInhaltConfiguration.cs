using Domain.Entities.Inventur;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Inventur;


public class InventurBerichteInhaltConfiguration
{
    public void Configure(EntityTypeBuilder<InventurBerichte> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Titel)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.Inhalt)
            .IsRequired();

        builder.Property(b => b.GesamtDifferenzWert)
            .HasPrecision(18, 2);

        builder.Property(b => b.Erstellungsdatum)
            .IsRequired();

        builder.HasOne(b => b.Inventur)
            .WithMany(i => i.Berichte)
            .HasForeignKey(b => b.InventurId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
