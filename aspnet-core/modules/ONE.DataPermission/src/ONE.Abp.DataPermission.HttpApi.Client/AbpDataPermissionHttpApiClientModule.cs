using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace ONE.Abp.DataPermission;

[DependsOn(
    typeof(AbpDataPermissionApplicationContractsModule),
    typeof(AbpHttpClientModule))]
public class AbpDataPermissionHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddStaticHttpClientProxies(
            typeof(AbpDataPermissionApplicationContractsModule).Assembly,
            DataPermissionRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpDataPermissionHttpApiClientModule>();
        });

    }
}
