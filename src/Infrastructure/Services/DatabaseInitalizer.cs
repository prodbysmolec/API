using System;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class DatabaseInitalizer(IServiceProvider serviceProvider, ILogger<DatabaseInitalizer> logger)
    : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Datenbank Initialisierung gestartet");
        using(var scope = serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            try
            {
                logger.LogInformation("Prüfe, ob verbindung zur Datenbank aufgebaut werden kann");
                if(await dbContext.Database.CanConnectAsync(cancellationToken))
                    logger.LogInformation("Verbindung zur Datenbank erfolgreich");
                else
                    logger.LogError("Verbindung zur Datenbank fehlgeschlagen");
                    logger.LogInformation("Prüfe, ob Migrationen vorhanden sind");
                    dbContext.Database.Migrate();
                    return;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Fehler bei der Datenbank Initialisierung");
            }
            logger.LogInformation("Datenbank Initialisierung abgeschlossen");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
