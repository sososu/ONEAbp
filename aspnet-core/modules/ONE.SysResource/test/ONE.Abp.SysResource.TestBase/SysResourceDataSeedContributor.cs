using ONE.Abp.SysResource.Datas;
using ONE.Abp.SysResource.SaleVersions;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace ONE.Abp.SysResource;

public class SysResourceDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IGuidGenerator _guidGenerator;
    private readonly ICurrentTenant _currentTenant;
    private readonly ISaleVersionRepository _saleVersionRepository;

    public SysResourceDataSeedContributor(
        IGuidGenerator guidGenerator, ICurrentTenant currentTenant, ISaleVersionRepository saleVersionRepository)
    {
        _guidGenerator = guidGenerator;
        _currentTenant = currentTenant;
        _saleVersionRepository = saleVersionRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        /* Instead of returning the Task.CompletedTask, you can insert your test data
         * at this point!
         */

        using (_currentTenant.Change(context?.TenantId))
        {

           await _saleVersionRepository.InsertAsync(SaleVersionData.SaleVersionA);
            //return Task.CompletedTask;
        }
    }
}
