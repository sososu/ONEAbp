using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict.Application.Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.Applications;

namespace Volo.Abp.OpenIddict;


[DependsOn(
    typeof(OpenIddictApplicationModel),
    typeof(AbpOpenIddictApplicationContractsModule),
    typeof(AbpAutoMapperModule)
    )]
public class AbpOpenIddictApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<AbpOpenIddictApplicationModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<AbpOpeniddictApplicationModuleAutoMapperProfile>(validate: true);
        });
    }
}
