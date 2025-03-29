using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using WebApplication1;

namespace EmployeeAPI.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services => 
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<AppDbContext>));

            services.Remove(dbContextDescriptor!);

            var dbConnectionDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(NpgsqlConnection));

            services.Remove(dbConnectionDescriptor!);

            // Create open SqliteConnection so EF won't automatically close it.
            services.AddSingleton<NpgsqlConnection>(container =>
            {
                var connectionString = "Host=localhost;Port=5432;Username=Admin;Database=Test01_TestDB";
                var connection = new NpgsqlConnection(connectionString);
                connection.Open();

                return connection;
            });

            services.AddDbContext<AppDbContext>((container, options) =>
            {
                var connection = container.GetRequiredService<NpgsqlConnection>();
                options.UseNpgsql(connection);
            });

        });
    }
}