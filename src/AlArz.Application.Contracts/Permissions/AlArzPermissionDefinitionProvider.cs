using AlArz.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Permissions;

public class PermissionsDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        //var myGroup = context.AddGroup(global::Permissions.Permissions.AlArz.AppGroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(Permissions.Permission.MyPermission1, L("Permission:MyPermission1"));

        //NewContent
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AlArzResource>(name);
    }
}
