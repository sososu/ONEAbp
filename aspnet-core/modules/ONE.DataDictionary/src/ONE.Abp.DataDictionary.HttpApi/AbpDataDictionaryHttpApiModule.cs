using Localization.Resources.AbpUi;
using ONE.Abp.DataDictionary.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;

namespace ONE.Abp.DataDictionary;

[DependsOn(
    typeof(AbpDataDictionaryApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule))]
public class AbpDataDictionaryHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpDataDictionaryHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<DataDictionaryResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
