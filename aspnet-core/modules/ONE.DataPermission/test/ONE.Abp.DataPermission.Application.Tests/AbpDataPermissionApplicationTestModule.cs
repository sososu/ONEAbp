using Volo.Abp.Modularity;

namespace ONE.Abp.DataPermission;

[DependsOn(
    typeof(AbpDataPermissionApplicationModule),
    typeof(AbpDataPermissionDomainTestModule)
    )]
public class AbpDataPermissionApplicationTestModule : AbpModule
{

}
