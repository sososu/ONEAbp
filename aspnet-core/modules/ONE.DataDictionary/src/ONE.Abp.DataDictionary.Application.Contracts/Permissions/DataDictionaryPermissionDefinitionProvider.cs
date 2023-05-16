using ONE.Abp.DataDictionary.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace ONE.Abp.DataDictionary.Permissions;

public class DataDictionaryPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(DataDictionaryPermissions.GroupName, L("Permission:DataDictionary"));

        //Define your own permissions here. Example:
        var lpermission = myGroup.AddPermission(DataDictionaryPermissions.Default, L("Permission:Default"));
        lpermission.AddChild(DataDictionaryPermissions.Create, L("Permission:Create"));
        lpermission.AddChild(DataDictionaryPermissions.Update, L("Permission:Update"));
        lpermission.AddChild(DataDictionaryPermissions.Enable, L("Permission:Enable"));
        lpermission.AddChild(DataDictionaryPermissions.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<DataDictionaryResource>(name);
    }
}
