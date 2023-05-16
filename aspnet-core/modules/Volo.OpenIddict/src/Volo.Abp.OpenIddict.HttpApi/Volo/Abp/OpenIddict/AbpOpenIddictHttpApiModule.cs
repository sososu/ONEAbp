using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Volo.Abp.OpenIddict;


[DependsOn(typeof(AbpOpenIddictApplicationContractsModule), typeof(AbpAspNetCoreMvcModule))]
public class AbpOpenIddictHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(AbpOpenIddictHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<OpenIddictResources>()
                .AddBaseTypes(
                    typeof(AbpUiResource)
                );
        });
    }

}
