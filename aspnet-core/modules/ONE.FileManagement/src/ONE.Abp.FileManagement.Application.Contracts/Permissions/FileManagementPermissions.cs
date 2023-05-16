using Volo.Abp.Reflection;

namespace ONE.Abp.FileManagement.Permissions;

public class FileManagementPermissions
{
    public const string GroupName = "FileManagement";
    public const string Default = GroupName + "File";
    public const string Create = Default + ".Create";
    public const string Update = Default + ".Update";
    public const string Delete = Default + ".Delete";
    public const string Download = Default + ".Download";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(FileManagementPermissions));
    }
}
