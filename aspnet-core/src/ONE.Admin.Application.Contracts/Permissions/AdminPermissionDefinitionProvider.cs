using ONE.Admin.Localization;
using System.Text.RegularExpressions;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace ONE.Admin.Permissions;

public class AdminPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        //var myGroup = context.AddGroup(SettingManagementPermissions.GroupName, L("Permission:SettingManagementPermissions"));
        //Define your own permissions here. Example:
        //myGroup.AddPermission(AdminPermissions.MyPermission1, L("Permission:MyPermission1"));

       
    }


    public override void PostDefine(IPermissionDefinitionContext context)
    {
        var myGroup =context.GetGroupOrNull(SettingManagementPermissions.GroupName);
        if(myGroup == null)
            context.AddGroup(SettingManagementPermissions.GroupName, L("Permission:SettingManagementPermissions"));
        var settingPermission = myGroup.AddPermission(SettingManagementPermissions.AccountSettings.Default, L("Permission:AccountSetting"));
        settingPermission.AddChild(SettingManagementPermissions.AccountSettings.Create, L("Permission:Create"));
        settingPermission.AddChild(SettingManagementPermissions.AccountSettings.Update, L("Permission:Edit"));
        settingPermission.AddChild(SettingManagementPermissions.AccountSettings.Delete, L("Permission:Delete"));
        settingPermission.AddChild(SettingManagementPermissions.AccountSettings.Enable, L("Permission:Enable"));


        var identityPermission = myGroup.AddPermission(SettingManagementPermissions.IdentitySettings.Default, L("Permission:IdentitySetting"));
        identityPermission.AddChild(SettingManagementPermissions.IdentitySettings.Create, L("Permission:Create"));
        identityPermission.AddChild(SettingManagementPermissions.IdentitySettings.Update, L("Permission:Edit"));
        identityPermission.AddChild(SettingManagementPermissions.IdentitySettings.Delete, L("Permission:Delete"));
        identityPermission.AddChild(SettingManagementPermissions.IdentitySettings.Enable, L("Permission:Enable"));


        var fileSettingPermission = myGroup.AddPermission(SettingManagementPermissions.FileSettings.Default, L("Permission:FileSetting"));
        fileSettingPermission.AddChild(SettingManagementPermissions.FileSettings.Create, L("Permission:Create"));
        fileSettingPermission.AddChild(SettingManagementPermissions.FileSettings.Update, L("Permission:Edit"));
        fileSettingPermission.AddChild(SettingManagementPermissions.FileSettings.Delete, L("Permission:Delete"));
        fileSettingPermission.AddChild(SettingManagementPermissions.FileSettings.Enable, L("Permission:Enable"));

    }
    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AdminResource>(name);
    }
}
