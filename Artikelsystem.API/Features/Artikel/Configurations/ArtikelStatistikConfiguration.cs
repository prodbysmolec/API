using System;
using Artikelsystem.Domain.Entities.Artikel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artikelsystem.Api.Features.Artikel.Configurations;

public class ArtikelStatistikConfiguration : IEntityTypeConfiguration<ArtikelStatistik>
{
    public void Configure(EntityTypeBuilder<ArtikelStatistik> builder)
    {
        builder
            .Property(s => s.Lagerwert)
            .HasComputedColumnSql("\"Gesamtmenge\" * \"DurchschnittlicherEinzelpreis\"", stored: true);

        builder
            .Property(s => s.GesamtVerkaufswert)
            .HasComputedColumnSql("\"VerkaufsMenge\" * \"DurchschnittlicherVerkaufspreis\"", stored: true);

    }
}
