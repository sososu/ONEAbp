using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace ONE.Abp.SysResource.SaleVersions
{
    public interface ISaleVersionRepository : IRepository<SaleVersion, Guid>
    {
        Task<SaleVersion> FindByNameAsync(
            string name,
            bool includeDetails = true,
            CancellationToken cancellationToken = default);
    }

}
