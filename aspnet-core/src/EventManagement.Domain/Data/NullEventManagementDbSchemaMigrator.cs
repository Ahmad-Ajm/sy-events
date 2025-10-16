using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace EventManagement.Data;

/* This is used if database provider does't define
 * IEventManagementDbSchemaMigrator implementation.
 */
public class NullEventManagementDbSchemaMigrator : IEventManagementDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
