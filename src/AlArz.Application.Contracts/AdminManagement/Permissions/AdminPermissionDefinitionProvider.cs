using AlArz.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Permissions
{
    public class AdminPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            //User
            var UserGroup = context.AddGroup(AdminPermissions.Admins.User.GroupName, L("Permission:UserManagement"));

            var UserAdmin = UserGroup.AddPermission(AdminPermissions.Admins.User.Default, L("Permission:User"));
            UserAdmin.AddChild(AdminPermissions.Admins.User.Update, L("Permission:Edit"));
            UserAdmin.AddChild(AdminPermissions.Admins.User.Delete, L("Permission:Delete"));
            UserAdmin.AddChild(AdminPermissions.Admins.User.Create, L("Permission:Create"));
            UserAdmin.AddChild(AdminPermissions.Admins.User.ResetPassword, L("Permission:ResetPassword"));
            UserAdmin.AddChild(AdminPermissions.Admins.User.ChangePassword, L("Permission:ChangePassword"));

            //Config
            var configGroup = context.AddGroup(AdminPermissions.Admins.Config.GroupName, L("Permission:Config"));

            var configAdmin = configGroup.AddPermission(AdminPermissions.Admins.Config.Default, L("Permission:ConfigAdmin"));
            configAdmin.AddChild(AdminPermissions.Admins.Config.Update, L("Permission:Edit"));
            configAdmin.AddChild(AdminPermissions.Admins.Config.Delete, L("Permission:Delete"));
            configAdmin.AddChild(AdminPermissions.Admins.Config.Create, L("Permission:Create"));

            //Lookup
            var lookupGroup = context.AddGroup(AdminPermissions.Admins.Lookup.GroupName, L("Permission:Lookup"));

            var lookupAdmin = lookupGroup.AddPermission(AdminPermissions.Admins.Lookup.Default, L("Permission:LookupAdmin"));
            lookupAdmin.AddChild(AdminPermissions.Admins.Lookup.Update, L("Permission:Edit"));
            lookupAdmin.AddChild(AdminPermissions.Admins.Lookup.Delete, L("Permission:Delete"));
            lookupAdmin.AddChild(AdminPermissions.Admins.Lookup.Create, L("Permission:Create"));

            //LookupType
            var lookupTypeGroup = context.AddGroup(AdminPermissions.Admins.LookupType.GroupName, L("Permission:LookupType"));

            var lookupTypeAdmin = lookupTypeGroup.AddPermission(AdminPermissions.Admins.LookupType.Default, L("Permission:LookupType"));
            lookupTypeAdmin.AddChild(AdminPermissions.Admins.LookupType.Update, L("Permission:Edit"));
            lookupTypeAdmin.AddChild(AdminPermissions.Admins.LookupType.Create, L("Permission:Create"));
            lookupTypeAdmin.AddChild(AdminPermissions.Admins.LookupType.Delete, L("Permission:Delete"));

            //ErpSystem
            var ErpSystemGroup = context.AddGroup(AdminPermissions.Admins.ErpSystem.GroupName, L("Permission:ErpSystemManagement"));

            var ErpSystem = ErpSystemGroup.AddPermission(AdminPermissions.Admins.ErpSystem.Default, L("Permission:ErpSystem"));
            ErpSystem.AddChild(AdminPermissions.Admins.ErpSystem.Update, L("Permission:Edit"));
            ErpSystem.AddChild(AdminPermissions.Admins.ErpSystem.Create, L("Permission:Create"));
            ErpSystem.AddChild(AdminPermissions.Admins.ErpSystem.Delete, L("Permission:Delete"));



           // //CheckListQuestionAnswer 
           // //Ibrahim Amira 
           // var CheckListQuestionAnswerGroup = context.AddGroup(Permissions.CheckListQuestionAnswer.GroupName, L("Permission:CheckListQuestionAnswer"));
           //var CheckListQuestionAnswer = CheckListQuestionAnswerGroup.AddPermission(Permissions.CheckListQuestionAnswer.Default, L("Permission:CheckListQuestionAnswerAdmin")); 
           //CheckListQuestionAnswer.AddChild(Permissions.CheckListQuestionAnswer.Update, L("Permission:Edit")); 
           //CheckListQuestionAnswer.AddChild(Permissions.CheckListQuestionAnswer.Delete, L("Permission:Delete")); 
           //CheckListQuestionAnswer.AddChild(Permissions.CheckListQuestionAnswer.Create, L("Permission:Create")); 


            //NewContent 
  
  
  
  



        }
        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<AlArzResource>(name);
        }
    }
}
