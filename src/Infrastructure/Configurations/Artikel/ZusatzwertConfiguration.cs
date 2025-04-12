using System;
using Domain.Entities.Artikel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Artikel;

public class ZusatzwertConfiguration : IEntityTypeConfiguration<Zusatzwert>
{
    public void Configure(EntityTypeBuilder<Zusatzwert> builder)
    {

        builder.HasKey(zw => zw.Id);

        builder.Property(zw => zw.Wert)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(zw => zw.ZusatzFeld)
            .WithMany(zf => zf.ZusatzWerte)
            .HasForeignKey(z => z.ZusatzFeldID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
