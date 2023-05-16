using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace ONE.Abp.DataDictionary;

[DependsOn(
    typeof(AbpVirtualFileSystemModule)
    )]
public class AbpDataDictionaryInstallerModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<AbpDataDictionaryInstallerModule>();
        });
    }
}
