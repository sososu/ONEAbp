using Localization.Resources.AbpUi;
using ONE.Abp.DataPermission.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace ONE.Abp.DataPermission;

[DependsOn(
    typeof(AbpDataPermissionApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class AbpDataPermissionHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpDataPermissionHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<DataPermissionResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
