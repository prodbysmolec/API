using System;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Internal;

namespace Infrastructure.Persistence.Interceptors;

public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    // ToDo: Hier das UpdateAuditFields und override savechanges implementieren
    private readonly ISystemClock _systemClock;

    public AuditableEntitySaveChangesInterceptor(ISystemClock systemClock)
    {
        _systemClock = systemClock;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateAuditFields(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateAuditFields(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateAuditFields(DbContext? context)
    {
        if (context == null) return;

        var entries = context.ChangeTracker.Entries<AuditableEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.ErstelltVon = "TheCreateUser";
                entry.Entity.ErstelltAm = _systemClock.UtcNow.UtcDateTime;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.BearbeitetVon = "TheUpdateUser";
                entry.Entity.BearbeitetAm = _systemClock.UtcNow.UtcDateTime;
            }
        }
    }
}
