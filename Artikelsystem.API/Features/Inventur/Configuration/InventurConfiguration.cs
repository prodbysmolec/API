using System;
using Artikelsystem.Api.Features.Inventur.Models.Entitys;
using Microsoft.EntityFrameworkCore;

namespace Artikelsystem.Api.Features.Inventur.Configuration;

public class InventurConfiguration : IEntityTypeConfiguration<Models.Entitys.Inventur>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Models.Entitys.Inventur> builder)
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
