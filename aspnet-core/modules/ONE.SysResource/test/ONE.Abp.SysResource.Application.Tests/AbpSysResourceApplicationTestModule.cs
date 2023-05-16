using Volo.Abp.Modularity;

namespace ONE.Abp.SysResource;

[DependsOn(
    typeof(AbpSysResourceApplicationModule),
    typeof(AbpSysResourceDomainTestModule)
    )]
public class AbpSysResourceApplicationTestModule : AbpModule
{

}
