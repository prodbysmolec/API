using System;
using Artikelsystem.Api.Features.Artikel.Models.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artikelsystem.Api.Features.Inventur.Configuration;

public class ArtikelInventurHistorieConfiguration
{
    public void Configure(EntityTypeBuilder<ArtikelInventurHistorie> builder)
    {
        builder.HasKey(h => h.Id);
        
        builder.Property(h => h.DifferenzWert)
            .HasPrecision(18, 2);
            
        builder.HasOne(h => h.Artikel)
            .WithMany(a => a.InventurHistorie)
            .HasForeignKey(h => h.ArtikelId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(h => h.Inventur)
            .WithMany()
            .HasForeignKey(h => h.InventurId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
