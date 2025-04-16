using System;
using System.Threading.Tasks;
using API.Infrastructure.Persistence.Seeding;
using Infrastructure.Context;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extension;

public static class DatabaseExtension
{
    public static async Task<IServiceCollection> AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure(3);
                npgsqlOptions.CommandTimeout(30);
            });
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        var serviceProvider = services.BuildServiceProvider();
        await SeedData.MigrateAndSeed(serviceProvider);

        services.AddHostedService<DatabaseInitalizer>();
        return services;
    }
}
