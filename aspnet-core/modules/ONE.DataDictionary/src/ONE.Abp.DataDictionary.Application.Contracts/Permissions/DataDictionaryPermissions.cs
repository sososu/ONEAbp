using Volo.Abp.Reflection;

namespace ONE.Abp.DataDictionary.Permissions;

public class DataDictionaryPermissions
{
    public const string GroupName = "DataDictionary";


    public const string Default = GroupName + ".Base";
    public const string Create = Default + ".Create";
    public const string Update = Default + ".Update";
    public const string Delete = Default + ".Delete";
    public const string Enable = Default + ".Enable";

    public static string[] GetAll()
    {
        return ReflectionHelper.GetPublicConstantsRecursively(typeof(DataDictionaryPermissions));
    }
}
