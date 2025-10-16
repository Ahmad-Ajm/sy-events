using System.Threading.Tasks;

namespace EventManagement.Data;

public interface IEventManagementDbSchemaMigrator
{
    Task MigrateAsync();
}
