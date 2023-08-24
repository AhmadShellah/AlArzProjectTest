
namespace Permissions
{
    public partial class AdminPermissions
    {
        public const string Management = "Management";

        public static class Admins
        {
            public const string AdminGroupName = "Admin";

            public static class Config
            {
                public const string BasicGroupName = AdminGroupName + ".Config";

                public const string GroupName = BasicGroupName + Management;
                public const string Default = BasicGroupName;
                public const string Delete = Default + ".Delete";
                public const string Update = Default + ".Update";
                public const string Create = Default + ".Create";
            }

            public static class LookupType
            {
                public const string BasicGroupName = AdminGroupName + ".LookupType";

                public const string GroupName = BasicGroupName + Management;
                public const string Default = BasicGroupName;
                public const string Update = Default + ".Update";
                public const string Create = Default + ".Create";
                public const string Delete = Default + ".Delete";
            }

            public static class Lookup
            {
                public const string BasicGroupName = AdminGroupName + ".Lookup";

                public const string GroupName = BasicGroupName + Management;
                public const string Default = BasicGroupName;
                public const string Delete = Default + ".Delete";
                public const string Update = Default + ".Update";
                public const string Create = Default + ".Create";
            }
            public static class User
            {
                public const string BasicGroupName = AdminGroupName + ".User";

                public const string GroupName = BasicGroupName + Management;
                public const string Default = BasicGroupName;
                public const string Delete = Default + ".Delete";
                public const string Update = Default + ".Update";
                public const string Create = Default + ".Create";
                public const string ResetPassword = Default + ".ResetPassword";
                public const string ChangePassword = Default + ".ChangePassword";
            }

            public static class ErpSystem
            {
                public const string BasicGroupName = AdminGroupName + ".ErpSystem";

                public const string GroupName = BasicGroupName + Management;
                public const string Default = BasicGroupName;
                public const string Delete = Default + ".Delete";
                public const string Update = Default + ".Update";
                public const string Create = Default + ".Create";
            }

            //NewContent

        }


    }
}
