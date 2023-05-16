using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace ONE.Abp.FileManagement;

[DependsOn(
    typeof(AbpFileManagementApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class AbpFileManagementHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(AbpFileManagementApplicationContractsModule).Assembly,
            FileManagementRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpFileManagementHttpApiClientModule>();
        });

    }
}
