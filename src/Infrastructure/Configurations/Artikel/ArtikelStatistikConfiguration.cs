using System;
using Domain.Entities.Artikel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Artikel;

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
