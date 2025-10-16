using EventManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace EventManagement.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class EventManagementController : AbpControllerBase
{
    protected EventManagementController()
    {
        LocalizationResource = typeof(EventManagementResource);
    }
}
