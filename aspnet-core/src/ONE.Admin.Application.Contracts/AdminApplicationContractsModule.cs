using Volo.Abp.Account;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using ONE.Abp.SysResource;
using ONE.Abp.DataDictionary;
using ONE.Abp.DataPermission;
using ONE.Abp.FileManagement;

namespace ONE.Admin;

[DependsOn(
    typeof(AdminDomainSharedModule),
    typeof(AbpAccountApplicationContractsModule),
    typeof(AbpFeatureManagementApplicationContractsModule),
    typeof(AbpIdentityApplicationContractsModule),
    typeof(AbpPermissionManagementApplicationContractsModule),
    typeof(AbpSettingManagementApplicationContractsModule),
    typeof(AbpTenantManagementApplicationContractsModule),
    typeof(AbpObjectExtendingModule),
    typeof(AbpSysResourceApplicationContractsModule),
    typeof(AbpDataDictionaryApplicationContractsModule),
    typeof(AbpDataPermissionApplicationContractsModule),
    typeof(AbpFileManagementApplicationContractsModule)
    )]
public class AdminApplicationContractsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        AdminDtoExtensions.Configure();
    }
}
