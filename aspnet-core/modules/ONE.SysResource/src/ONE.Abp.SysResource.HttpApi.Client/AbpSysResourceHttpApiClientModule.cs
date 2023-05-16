using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace ONE.Abp.SysResource;

[DependsOn(
    typeof(AbpSysResourceApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class AbpSysResourceHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(AbpSysResourceApplicationContractsModule).Assembly,
            SysResourceRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpSysResourceHttpApiClientModule>();
        });

    }
}
