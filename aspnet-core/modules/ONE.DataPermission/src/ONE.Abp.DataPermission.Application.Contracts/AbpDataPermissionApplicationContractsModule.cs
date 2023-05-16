using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace ONE.Abp.DataPermission;

[DependsOn(
    typeof(AbpDataPermissionDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class AbpDataPermissionApplicationContractsModule : AbpModule
{

}
