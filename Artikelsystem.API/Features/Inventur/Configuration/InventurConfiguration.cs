using System;
using Artikelsystem.Domain.Entities.Inventur;
using Microsoft.EntityFrameworkCore;

namespace Artikelsystem.Api.Features.Inventur.Configuration;

public class InventurConfiguration : IEntityTypeConfiguration<Domain.Entities.Inventur.Inventur>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Domain.Entities.Inventur.Inventur> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Bezeichnung)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.Bemerkung)
            .HasMaxLength(500);

        builder.Property(i => i.Status)
            .HasConversion<int>();

        // Beziehung zu Berichten
        builder.HasMany(i => i.Berichte)
            .WithOne(b => b.Inventur)
            .HasForeignKey(b => b.InventurId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
