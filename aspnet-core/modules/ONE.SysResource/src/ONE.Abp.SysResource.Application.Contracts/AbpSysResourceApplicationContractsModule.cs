using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace ONE.Abp.SysResource;

[DependsOn(
    typeof(AbpSysResourceDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class AbpSysResourceApplicationContractsModule : AbpModule
{

}
