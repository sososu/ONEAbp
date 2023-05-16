using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace ONE.Abp.Pagination.Contracts
{
    [DependsOn(typeof(AbpDddApplicationContractsModule))]
    public class AbpPaginationContractsModule : AbpModule
    {
    }
}
