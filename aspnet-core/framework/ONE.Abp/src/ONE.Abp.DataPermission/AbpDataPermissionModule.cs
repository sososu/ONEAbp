using ONE.Abp.Data;
using Volo.Abp.Auditing;
using Volo.Abp.Modularity;

namespace ONE.Abp.DataPermission
{
    [DependsOn(typeof(AbpAuditingModule),
        typeof(ONEAbpDataModule))]
    public class AbpDataPermissionModule : AbpModule
    {
       
    }
}
