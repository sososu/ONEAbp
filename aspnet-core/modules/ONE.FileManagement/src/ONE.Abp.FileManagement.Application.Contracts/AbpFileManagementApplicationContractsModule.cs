using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace ONE.Abp.FileManagement;

[DependsOn(
    typeof(AbpFileManagementDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class AbpFileManagementApplicationContractsModule : AbpModule
{

}
