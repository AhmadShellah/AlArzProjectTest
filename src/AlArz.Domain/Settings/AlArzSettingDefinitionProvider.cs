using Volo.Abp.Settings;

namespace AlArz.Settings;

public class AlArzSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(AlArzSettings.MySetting1));
    }
}
