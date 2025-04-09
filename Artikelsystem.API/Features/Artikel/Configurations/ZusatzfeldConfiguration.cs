using System;
using Artikelsystem.Domain.Entities.Artikel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artikelsystem.Api.Features.Artikel.Configurations;

public class ZusatzfeldConfiguration : IEntityTypeConfiguration<Zusatzfeld>
{
    public void Configure(EntityTypeBuilder<Zusatzfeld> builder)
    {
        builder.HasKey(z => z.ZusatzfeldID);

        builder.Property(z => z.Name)
            .IsRequired()
            .HasMaxLength(100);

        // IsChecked ist eine UI-bezogene Property, wird nicht in der db gespeichert
        builder.Ignore(z => z.IsChecked);
    }
}
