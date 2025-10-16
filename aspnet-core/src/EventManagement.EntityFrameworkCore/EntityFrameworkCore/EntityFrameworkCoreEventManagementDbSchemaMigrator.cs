using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using EventManagement.Data;
using Volo.Abp.DependencyInjection;

namespace EventManagement.EntityFrameworkCore;

public class EntityFrameworkCoreEventManagementDbSchemaMigrator
    : IEventManagementDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreEventManagementDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the EventManagementDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        // استخدم سياق الترحيلات الموحد لفحص/تطبيق المايغريشن لتجنب التعارض مع جداول ABP
        var migrationsDb = _serviceProvider.GetRequiredService<EventManagementMigrationsDbContext>();

        var pendingMigrations = await migrationsDb.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            await migrationsDb.Database.MigrateAsync();
        }
    }
}
