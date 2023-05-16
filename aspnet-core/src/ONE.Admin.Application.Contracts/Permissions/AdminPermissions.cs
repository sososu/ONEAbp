namespace ONE.Admin.Permissions;

public static class AdminPermissions
{
    public const string GroupName = "Admin";

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";
}

public static class SettingManagementPermissions
{
    public const string GroupName = "SettingManagement";


    public static class IdentitySettings
    {

        public const string Default = GroupName + ".IdentitySettings";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string Enable = Default + ".Enable";
    }

    public static class AccountSettings
    {
        public const string Default = GroupName + ".AccountSettings";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string Enable = Default + ".Enable";
    }

    public static class FileSettings
    {
        public const string Default = GroupName + ".FileSettings";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string Enable = Default + ".Enable";
    }
}
