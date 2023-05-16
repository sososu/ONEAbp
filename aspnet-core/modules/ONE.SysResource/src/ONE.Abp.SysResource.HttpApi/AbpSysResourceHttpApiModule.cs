using Localization.Resources.AbpUi;
using ONE.Abp.SysResource.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace ONE.Abp.SysResource;

[DependsOn(
    typeof(AbpSysResourceApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class AbpSysResourceHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpSysResourceHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<SysResourceResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
