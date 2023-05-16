using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace ONE.Abp.DataDictionary;

[DependsOn(
    typeof(AbpDataDictionaryApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class AbpDataDictionaryHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(AbpDataDictionaryApplicationContractsModule).Assembly,
            DataDictionaryRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpDataDictionaryHttpApiClientModule>();
        });

    }
}
