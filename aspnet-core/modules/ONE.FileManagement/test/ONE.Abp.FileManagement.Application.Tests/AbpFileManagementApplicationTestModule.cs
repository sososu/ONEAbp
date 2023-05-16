using Volo.Abp.Modularity;

namespace ONE.Abp.FileManagement;

[DependsOn(
    typeof(AbpFileManagementApplicationModule),
    typeof(AbpFileManagementDomainTestModule)
    )]
public class AbpFileManagementApplicationTestModule : AbpModule
{

}
