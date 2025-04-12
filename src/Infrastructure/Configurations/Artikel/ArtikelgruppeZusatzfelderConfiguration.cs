using System;
using Domain.Entities.Artikel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Artikel;

public class ArtikelgruppeZusatzfelderConfiguration : IEntityTypeConfiguration<ArtikelgruppeZusatzfelder>
{
    public void Configure(EntityTypeBuilder<ArtikelgruppeZusatzfelder> builder)
    {
        builder.HasKey(agz => new { agz.ArtikelgruppeID, agz.ZusatzfelderID });

        builder.HasOne<Artikelgruppe>()
            .WithMany(agz => agz.ArtikelgruppeZusatzfelder)
            .HasForeignKey(az => az.ArtikelgruppeID)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Zusatzfeld>()
            .WithMany(z => z.ArtikelGruppeZusatzFelder)
            .HasForeignKey(az => az.ZusatzfelderID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
