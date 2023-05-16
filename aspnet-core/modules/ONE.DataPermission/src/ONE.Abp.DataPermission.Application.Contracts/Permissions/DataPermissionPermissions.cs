using Volo.Abp.Reflection;

namespace ONE.Abp.DataPermission.Permissions;

public class DataPermissionPermissions
{
    public const string GroupName = "DataPermission";


    public static class UserRule
    {
        //Add your own permission names. Example:
        public const string Default = GroupName + ".User";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string Enable = Default + ".Enable";
    }

    public static class DataRule
    {
        //Add your own permission names. Example:
        public const string Default = GroupName + ".Data";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string Enable = Default + ".Enable";
    }

    public static class Rule
    {
        //Add your own permission names. Example:
        public const string Default = GroupName + ".Rule";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string Enable = Default + ".Enable";
    }


    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(DataPermissionPermissions));
    }
}
