using System;
using Domain.Entities.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations.Authentication;

public class UserGruppenConfiguration : IEntityTypeConfiguration<UserGruppen>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<UserGruppen> builder)
    {
        builder.HasKey(ug => ug.Id);
        
        // UserGruppenUser 
        builder.HasMany(ug => ug.UserGruppenUsers)
            .WithOne(ugu => ugu.UserGruppen)
            .HasForeignKey(ugu => ugu.UserGruppenID);

        // GroupPermission 
        builder.HasMany(ug => ug.GroupPermissions)
            .WithOne(gp => gp.UserGruppen)
            .HasForeignKey(gp => gp.UserGruppenID);
    }
}
