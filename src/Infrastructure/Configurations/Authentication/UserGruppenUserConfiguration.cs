using System;
using Domain.Entities.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Authentication;

public class UserGruppenUserConfiguration : IEntityTypeConfiguration<UserGruppenUser>
{
    public void Configure(EntityTypeBuilder<UserGruppenUser> builder)
    {
        builder.HasKey(ugu => new { ugu.UserID, ugu.UserGruppenID});
    }
}
