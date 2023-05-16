using ONE.Abp.DataPermission.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace ONE.Abp.DataPermission.Permissions;

public class DataPermissionPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(DataPermissionPermissions.GroupName, L("Permission:DataPermission"));

        var rulePermission = myGroup.AddPermission(DataPermissionPermissions.Rule.Default, L("Permission:Default"));
        rulePermission.AddChild(DataPermissionPermissions.Rule.Create, L("Permission:Create"));
        rulePermission.AddChild(DataPermissionPermissions.Rule.Update, L("Permission:Update"));
        rulePermission.AddChild(DataPermissionPermissions.Rule.Enable, L("Permission:Enable"));
        rulePermission.AddChild(DataPermissionPermissions.Rule.Delete, L("Permission:Delete"));


        var userRulePermission = myGroup.AddPermission(DataPermissionPermissions.UserRule.Default, L("Permission:Default"));
        userRulePermission.AddChild(DataPermissionPermissions.UserRule.Create, L("Permission:Create"));
        userRulePermission.AddChild(DataPermissionPermissions.UserRule.Update, L("Permission:Update"));
        userRulePermission.AddChild(DataPermissionPermissions.UserRule.Enable, L("Permission:Enable"));
        userRulePermission.AddChild(DataPermissionPermissions.UserRule.Delete, L("Permission:Delete"));


        var dataRulePermission = myGroup.AddPermission(DataPermissionPermissions.DataRule.Default, L("Permission:Default"));
        dataRulePermission.AddChild(DataPermissionPermissions.DataRule.Create, L("Permission:Create"));
        dataRulePermission.AddChild(DataPermissionPermissions.DataRule.Update, L("Permission:Update"));
        dataRulePermission.AddChild(DataPermissionPermissions.DataRule.Enable, L("Permission:Enable"));
        dataRulePermission.AddChild(DataPermissionPermissions.DataRule.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<DataPermissionResource>(name);
    }
}
