using System;
using Artikelsystem.API.Features.Authentication.Models.Entitys;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artikelsystem.API.Features.Authentication.Configuration;

public class UserGruppenUserConfiguration : IEntityTypeConfiguration<UserGruppenUser>
{
    public void Configure(EntityTypeBuilder<UserGruppenUser> builder)
    {
        builder.HasKey(ugu => new { ugu.UserID, ugu.UserGruppenID});
    }
}
