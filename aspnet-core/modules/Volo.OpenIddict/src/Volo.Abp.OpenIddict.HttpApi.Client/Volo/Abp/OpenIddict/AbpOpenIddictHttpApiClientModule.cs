using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.OpenIddict;


[DependsOn(typeof(AbpOpenIddictApplicationContractsModule), typeof(AbpHttpClientModule))]
public class AbpOpenIddictHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(AbpOpenIddictHttpApiClientModule).Assembly,
            OpenIddictRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpOpenIddictHttpApiClientModule>();
        });
    }
}
