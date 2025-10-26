using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using Volo.Abp.BackgroundWorkers;
using Microsoft.Extensions.DependencyInjection;
using EventManagement.BackgroundJobs;

namespace EventManagement;

[DependsOn(
    typeof(EventManagementDomainModule),
    typeof(AbpAccountApplicationModule),
    typeof(EventManagementApplicationContractsModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule)
    )]
public class EventManagementApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<EventManagementApplicationModule>();
        });
        // Register background worker
        Configure<AbpBackgroundWorkerOptions>(options => { });
    }

    public override async Task OnApplicationInitializationAsync(ApplicationInitializationContext context)
    {
        // Start periodic reminder worker
        await base.OnApplicationInitializationAsync(context);
        var workerManager = context.ServiceProvider.GetRequiredService<IBackgroundWorkerManager>();
        await workerManager.AddAsync(context.ServiceProvider.GetRequiredService<UpcomingEventReminderWorker>());
    }
}
