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

namespace MyCompanyName.MyProjectName;

[DependsOn(
    typeof(MyProjectNameDomainModule),
    typeof(AbpAccountApplicationModule),
    typeof(MyProjectNameApplicationContractsModule),
    typeof(AbpIdentityApplicationModule),
    typeof(AbpPermissionManagementApplicationModule),
    typeof(AbpTenantManagementApplicationModule),
    typeof(AbpFeatureManagementApplicationModule),
    typeof(AbpSettingManagementApplicationModule),
    typeof(AbpPaginationModule),
    typeof(AbpSysResourceApplicationModule),
    typeof(AbpDataDictionaryApplicationModule),
    typeof(AbpDataPermissionApplicationModule),
    typeof(AbpFileManagementApplicationModule)
    )]
    public class MyProjectNameApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<MyProjectNameApplicationModule>();
        });
    }
}
