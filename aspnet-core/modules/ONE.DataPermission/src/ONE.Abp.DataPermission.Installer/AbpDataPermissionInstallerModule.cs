using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace ONE.Abp.DataPermission;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class AbpDataPermissionInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpDataPermissionInstallerModule>();
        });
    }
}
