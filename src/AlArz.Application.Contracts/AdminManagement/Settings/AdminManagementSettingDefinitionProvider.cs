using AdminManagement.Settings;
using Volo.Abp.Settings;

namespace Baladiyat.Settings
{
    public class AdminManagementSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(
                    AdminSettings.MaxPageSize,
                    "100",
                    isVisibleToClients: true
                )
            );
        }
    }
}
