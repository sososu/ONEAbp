using Localization.Resources.AbpUi;
using ONE.Admin.Localization;
using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using ONE.Abp.SysResource;
using ONE.Abp.DataDictionary;
using ONE.Abp.DataPermission;
using ONE.Abp.FileManagement;

namespace ONE.Admin;

[DependsOn(
    typeof(AdminApplicationContractsModule),
    typeof(AbpAccountHttpApiModule),
    typeof(AbpIdentityHttpApiModule),
    typeof(AbpPermissionManagementHttpApiModule),
    typeof(AbpTenantManagementHttpApiModule),
    typeof(AbpFeatureManagementHttpApiModule),
    typeof(AbpSettingManagementHttpApiModule)
    )]
[DependsOn(typeof(AbpSysResourceHttpApiModule))]
    [DependsOn(typeof(AbpDataDictionaryHttpApiModule))]
    [DependsOn(typeof(AbpDataPermissionHttpApiModule))]
    [DependsOn(typeof(AbpFileManagementHttpApiModule))]
    public class AdminHttpApiModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        ConfigureLocalization();
    }

    private void ConfigureLocalization()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<AdminResource>()
                .AddBaseTypes(
                    typeof(AbpUiResource)
                );
        });
    }
}
