using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace ONE.Abp.SysResource;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class AbpSysResourceInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpSysResourceInstallerModule>();
        });
    }
}
