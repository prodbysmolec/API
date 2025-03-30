using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Npgsql;
using Artikelsystem.Api;

namespace Artikelsystem.API.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private static readonly object _lock = new object();
    private static bool _databaseInitialized;
    public static TestSystemClock SystemClock { get; } = new TestSystemClock();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Lade die appsettings.json des Hauptprogramms
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        });

        builder.ConfigureServices((context, services) =>
        {
            // Hole die Configuration aus dem Kontext
            var configuration = context.Configuration;

            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

            if (dbContextDescriptor != null)
            {
                services.Remove(dbContextDescriptor);
            }

            var dbConnectionDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(NpgsqlConnection));

            if (dbConnectionDescriptor != null)
            {
                services.Remove(dbConnectionDescriptor);
            }

            // Verbindung aus der Konfigurationsdatei holen
            var connectionString = configuration.GetConnectionString("TestDBConnection");

            services.AddSingleton<NpgsqlConnection>(container =>
            {
                var connection = new NpgsqlConnection(connectionString);
                connection.Open();
                return connection;
            });

            services.AddDbContext<AppDbContext>((container, options) =>
            {
                var connection = container.GetRequiredService<NpgsqlConnection>();
                options.UseNpgsql(connection);
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            var systemClockDescriptor = services.Single(d => d.ServiceType == typeof(ISystemClock));
            services.Remove(systemClockDescriptor);
            services.AddSingleton<ISystemClock>(SystemClock);

            // Datenbank initialisieren - einmalig beim Start der Tests
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    // Baue Verbindung zur Master-Datenbank auf, um DB zu löschen/erstellen
                    using var masterConnection = new NpgsqlConnection(GetMasterConnectionString(connectionString!));
                    masterConnection.Open();

                    // Datenbanknamen aus Connection String extrahieren
                    var databaseName = GetDatabaseName(connectionString!);

                    // Drop Database wenn vorhanden
                    DropDatabase(masterConnection, databaseName);

                    // Datenbank neu erstellen
                    CreateDatabase(masterConnection, databaseName);
                    
                    masterConnection.Close();

                    // Migrationen anwenden und Seed-Daten einfügen
                    using var scope = services.BuildServiceProvider().CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    dbContext.Database.Migrate();
                    
                    // Optional: Hier könntest du TestSeedData.Initialize(dbContext) aufrufen 
                    // für spezifische Test-Seed-Daten
                    
                    _databaseInitialized = true;
                }
            }
        });
    }

    // Hilfsmethode für Connection String zur Master-DB
    private string GetMasterConnectionString(string connectionString)
    {
        var builder = new NpgsqlConnectionStringBuilder(connectionString)
        {
            Database = "postgres" // Verbinde zur Standard-Datenbank in PostgreSQL
        };
        return builder.ConnectionString;
    }

    // Hilfsmethode zum Extrahieren des Datenbanknamens
    private string GetDatabaseName(string connectionString)
    {
        var builder = new NpgsqlConnectionStringBuilder(connectionString);
        return builder.Database!;
    }

    // Hilfsmethode zum Löschen der Datenbank
    private void DropDatabase(NpgsqlConnection connection, string databaseName)
    {
        try
        {
            var dropCommand = $@"
                SELECT pg_terminate_backend(pg_stat_activity.pid)
                FROM pg_stat_activity
                WHERE pg_stat_activity.datname = '{databaseName}'
                  AND pid <> pg_backend_pid();
                
                DROP DATABASE IF EXISTS ""{databaseName}"";";
                
            using var command = new NpgsqlCommand(dropCommand, connection);
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler beim Löschen der Datenbank: {ex.Message}");
        }
    }

    // Hilfsmethode zum Erstellen der Datenbank
    private void CreateDatabase(NpgsqlConnection connection, string databaseName)
    {
        try
        {
            var createCommand = $@"CREATE DATABASE ""{databaseName}""";
            using var command = new NpgsqlCommand(createCommand, connection);
            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler beim Erstellen der Datenbank: {ex.Message}");
        }
    }

    public class TestSystemClock : ISystemClock
    {
        public DateTimeOffset UtcNow { get; } = DateTimeOffset.Parse("2022-01-01T00:00:00Z");
    }
}