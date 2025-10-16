using EventManagement.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace EventManagement.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(EventManagementEntityFrameworkCoreModule),
    typeof(EventManagementApplicationContractsModule)
    )]
public class EventManagementDbMigratorModule : AbpModule
{
}
