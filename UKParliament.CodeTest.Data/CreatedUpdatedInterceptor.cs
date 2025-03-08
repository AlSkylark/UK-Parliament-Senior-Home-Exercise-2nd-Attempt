using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using UKParliament.CodeTest.Data.Models;

namespace CommissionMe.API.Infrastructure.Database;

public class CreatedUpdatedInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result
    )
    {
        BeforeSaveTriggers(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
    )
    {
        BeforeSaveTriggers(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void BeforeSaveTriggers(DbContext? context)
    {
        var entries = context
            ?.ChangeTracker.Entries()
            .Where(e =>
                (e.Entity is BaseEntity)
                && (e.State == EntityState.Added || e.State == EntityState.Modified)
            );

        if (entries is null)
            return;

        foreach (var entityEntry in entries)
        {
            if (entityEntry.Entity is BaseEntity baseEntity)
            {
                baseEntity.UpdatedAt = DateTime.UtcNow;
                if (entityEntry.State == EntityState.Added)
                {
                    baseEntity.CreatedAt = DateTime.UtcNow;
                }
            }
        }
    }
}
