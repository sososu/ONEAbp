using Localization.Resources.AbpUi;
using MyCompanyName.MyProjectName.Localization;
using ONE.Abp.DataDictionary;
using ONE.Abp.DataPermission;
using ONE.Abp.FileManagement;
using ONE.Abp.SysResource;
using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement.HttpApi;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace MyCompanyName.MyProjectName;

[DependsOn(
    typeof(MyProjectNameApplicationContractsModule),
    typeof(AbpAccountHttpApiModule),
    typeof(AbpIdentityHttpApiModule),
    typeof(AbpPermissionManagementHttpApiModule),
    typeof(AbpTenantManagementHttpApiModule),
    typeof(AbpFeatureManagementHttpApiModule),
    typeof(AbpSettingManagementHttpApiModule),
    typeof(AbpSysResourceHttpApiModule),
    typeof(AbpDataDictionaryHttpApiModule),
    typeof(AbpDataPermissionHttpApiModule),
    typeof(AbpFileManagementHttpApiModule)
    )]
public class MyProjectNameHttpApiModule : AbpModule
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
                .Get<MyProjectNameResource>()
                .AddBaseTypes(
                    typeof(AbpUiResource)
                );
        });
    }
}
