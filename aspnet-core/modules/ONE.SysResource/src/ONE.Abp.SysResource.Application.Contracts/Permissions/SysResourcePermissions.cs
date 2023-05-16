using Volo.Abp.Reflection;

namespace ONE.Abp.SysResource.Permissions;

public class SysResourcePermissions
{
    public const string GroupName = "SysResource";

    public static class SysApp
    {
        //Add your own permission names. Example:
        public const string Default = GroupName + ".App";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string Enable = Default + ".Enable";
    }
    public static class SysMenu
    {
        //Add your own permission names. Example:
        public const string Default = GroupName + ".Menu";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string Enable = Default + ".Enable";
    }


    public static class SaleVersion
    {
        //Add your own permission names. Example:
        public const string Default = GroupName + ".SaleVersion";
        public const string Create = Default + ".Create";
        public const string Update = Default + ".Update";
        public const string Delete = Default + ".Delete";
        public const string Enable = Default + ".Enable";
        public const string Menu = Default + ".Menu";
    }

    public static class RoleMenu
    {
        public const string Default = GroupName + ".RoleMenu";
        public const string Authorization = Default + ".Authorization";
    }
    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(SysResourcePermissions));
    }
}
