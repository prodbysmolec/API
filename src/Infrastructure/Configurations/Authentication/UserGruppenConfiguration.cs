using System;
using Domain.Entities.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations.Authentication;

public class UserGruppenConfiguration : IEntityTypeConfiguration<UserGruppen>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<UserGruppen> builder)
    {
        builder.HasAlternateKey(ug => ug.Id);

        builder.HasMany(ug => ug.UserGruppenUsers)
            .WithOne(ugu => ugu.UserGruppen)
            .HasForeignKey(ugu => ugu.UserGruppenID);
    }
}
