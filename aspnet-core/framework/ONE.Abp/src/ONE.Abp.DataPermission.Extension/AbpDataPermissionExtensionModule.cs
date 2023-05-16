using ONE.Abp.Pagination;
using Volo.Abp.Modularity;

namespace ONE.Abp.DataPermission.Extension
{
    [DependsOn(typeof(AbpDataPermissionModule),
      typeof(AbpPaginationModule))]
    public class AbpDataPermissionExtensionModule:AbpModule
    {
    }
}
