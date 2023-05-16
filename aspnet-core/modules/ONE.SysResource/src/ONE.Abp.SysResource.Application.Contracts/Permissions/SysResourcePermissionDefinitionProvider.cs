using ONE.Abp.SysResource.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace ONE.Abp.SysResource.Permissions;

public class SysResourcePermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(SysResourcePermissions.GroupName, L("Permission:SysResource"));

        var appPermission = myGroup.AddPermission(SysResourcePermissions.SysApp.Default, L("Permission:Default"));
        appPermission.AddChild(SysResourcePermissions.SysApp.Create, L("Permission:Create"));
        appPermission.AddChild(SysResourcePermissions.SysApp.Update, L("Permission:Update"));
        appPermission.AddChild(SysResourcePermissions.SysApp.Enable, L("Permission:Enable"));
        appPermission.AddChild(SysResourcePermissions.SysApp.Delete, L("Permission:Delete"));

        var menuPermission = myGroup.AddPermission(SysResourcePermissions.SysMenu.Default, L("Permission:Default"));
        menuPermission.AddChild(SysResourcePermissions.SysMenu.Create, L("Permission:Create"));
        menuPermission.AddChild(SysResourcePermissions.SysMenu.Update, L("Permission:Update"));
        menuPermission.AddChild(SysResourcePermissions.SysMenu.Enable, L("Permission:Enable"));
        menuPermission.AddChild(SysResourcePermissions.SysMenu.Delete, L("Permission:Delete"));


        var saleVersionPermission = myGroup.AddPermission(SysResourcePermissions.SaleVersion.Default, L("Permission:Default"));
        saleVersionPermission.AddChild(SysResourcePermissions.SaleVersion.Update, L("Permission:Update"));
        saleVersionPermission.AddChild(SysResourcePermissions.SaleVersion.Enable, L("Permission:Enable"));
        saleVersionPermission.AddChild(SysResourcePermissions.SaleVersion.Delete, L("Permission:Delete"));
        saleVersionPermission.AddChild(SysResourcePermissions.SaleVersion.Create, L("Permission:Create"));
        saleVersionPermission.AddChild(SysResourcePermissions.SaleVersion.Menu, L("Permission:Menu"));

        var roleMenuPermission = myGroup.AddPermission(SysResourcePermissions.RoleMenu.Default, L("Permission:Default"));
        roleMenuPermission.AddChild(SysResourcePermissions.RoleMenu.Authorization, L("Permission:Authorization"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<SysResourceResource>(name);
    }
}
