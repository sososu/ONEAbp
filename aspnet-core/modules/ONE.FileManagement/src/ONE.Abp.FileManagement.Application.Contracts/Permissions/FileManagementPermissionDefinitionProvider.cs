using ONE.Abp.FileManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
namespace ONE.Abp.FileManagement.Permissions;

public class FileManagementPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(FileManagementPermissions.GroupName, L("Permission:FileManagement"));
        var appPermission = myGroup.AddPermission(FileManagementPermissions.Default, L("Permission:Default"));
        appPermission.AddChild(FileManagementPermissions.Create, L("Permission:Create"));
        appPermission.AddChild(FileManagementPermissions.Update, L("Permission:Update"));
        appPermission.AddChild(FileManagementPermissions.Delete, L("Permission:Delete"));
        appPermission.AddChild(FileManagementPermissions.Download, L("Permission:Download"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<FileManagementResource>(name);
    }
}
