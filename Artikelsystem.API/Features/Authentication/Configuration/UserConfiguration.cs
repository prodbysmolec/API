using System;
using Artikelsystem.API.Features.Authentication.Models.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artikelsystem.API.Features.Authentication.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);  

        builder.HasMany(u => u.UserGruppenUsers)
            .WithOne(ugu => ugu.User)
            .HasForeignKey(u => u.UserID);
    }
}
