using Domain.Entities.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations.Authentication;


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
