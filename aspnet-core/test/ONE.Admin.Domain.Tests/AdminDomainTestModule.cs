using ONE.Admin.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace ONE.Admin;

[DependsOn(
    typeof(AdminEntityFrameworkCoreTestModule)
    )]
public class AdminDomainTestModule : AbpModule
{

}
