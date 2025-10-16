using Volo.Abp.Settings;

namespace EventManagement.Settings;

public class EventManagementSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(EventManagementSettings.MySetting1));
    }
}
