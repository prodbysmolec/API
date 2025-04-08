using System;
using Artikelsystem.API.Features.Authentication.Models.Entitys;
using Microsoft.EntityFrameworkCore;

namespace Artikelsystem.API.Features.Authentication.Configuration;

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
