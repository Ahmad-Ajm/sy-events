using Microsoft.Extensions.Localization;
using EventManagement.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace EventManagement;

[Dependency(ReplaceServices = true)]
public class EventManagementBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<EventManagementResource> _localizer;

    public EventManagementBrandingProvider(IStringLocalizer<EventManagementResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
