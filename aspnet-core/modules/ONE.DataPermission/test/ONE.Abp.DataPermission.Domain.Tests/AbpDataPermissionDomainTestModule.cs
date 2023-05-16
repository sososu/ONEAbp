using ONE.Abp.DataPermission.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace ONE.Abp.DataPermission;

/* Domain tests are configured to use the EF Core provider.
 * You can switch to MongoDB, however your domain tests should be
 * database independent anyway.
 */
[DependsOn(
    typeof(AbpDataPermissionEntityFrameworkCoreTestModule)
    )]
public class AbpDataPermissionDomainTestModule : AbpModule
{

}
