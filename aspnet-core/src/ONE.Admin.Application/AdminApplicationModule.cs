using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;
using ONE.Abp.SysResource;
using ONE.Abp.DataDictionary;
using ONE.Abp.Pagination;
using ONE.Abp.DataPermission;
using ONE.Abp.FileManagement;

namespace ONE.Admin;

[DependsOn(
    typeof(AdminDomainModule),
    typeof(AbpAccountApplicationModule),
    typeof(AdminApplicationContractsModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(AbpPaginationModule)
    )]
[DependsOn(typeof(AbpSysResourceApplicationModule))]
    [DependsOn(typeof(AbpDataDictionaryApplicationModule))]
    [DependsOn(typeof(AbpDataPermissionApplicationModule))]
    [DependsOn(typeof(AbpFileManagementApplicationModule))]
    public class AdminApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<AdminApplicationModule>();
        });
    }
}
