using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace ONE.Abp.FileManagement;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class AbpFileManagementInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpFileManagementInstallerModule>();
        });
    }
}
