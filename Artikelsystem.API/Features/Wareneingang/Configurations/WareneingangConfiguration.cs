using System;
using Artikelsystem.Api.Features.Wareneingang.Models.Entitys;
using Microsoft.EntityFrameworkCore;


namespace Artikelsystem.Api.Features.Wareneingang.Configurations;

public class WareneingangConfiguration : IEntityTypeConfiguration<Models.Entitys.Wareneingang>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Models.Entitys.Wareneingang> builder)
    {
        builder
            .HasOne(w => w.Lieferant)
            .WithMany(l => l.Wareneingaenge)
            .HasForeignKey(w => w.LieferantId);
    }
}
