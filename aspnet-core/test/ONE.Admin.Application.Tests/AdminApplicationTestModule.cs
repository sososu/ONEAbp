using Volo.Abp.Modularity;

namespace ONE.Admin;

[DependsOn(
    typeof(AdminApplicationModule),
    typeof(AdminDomainTestModule)
    )]
public class AdminApplicationTestModule : AbpModule
{

}
