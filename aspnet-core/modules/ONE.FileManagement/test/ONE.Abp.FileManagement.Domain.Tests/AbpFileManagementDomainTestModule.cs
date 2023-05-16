using ONE.Abp.FileManagement.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace ONE.Abp.FileManagement;

/* Domain tests are configured to use the EF Core provider.
 * You can switch to MongoDB, however your domain tests should be
 * database independent anyway.
 */
[DependsOn(
    typeof(AbpFileManagementEntityFrameworkCoreTestModule)
    )]
public class AbpFileManagementDomainTestModule : AbpModule
{

}
