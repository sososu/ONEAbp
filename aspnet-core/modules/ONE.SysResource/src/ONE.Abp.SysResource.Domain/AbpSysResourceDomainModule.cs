using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace ONE.Abp.SysResource;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpSysResourceDomainSharedModule)
)]
public class AbpSysResourceDomainModule : AbpModule
{

}
