using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace ONE.Abp.SysResource.SysApps
{
    public interface ISysAppRepository : IRepository<SysApp, Guid>
    {
        Task<SysApp> FindByCodeAsync(
            string code,
            bool includeDetails = true,
            CancellationToken cancellationToken = default);
    }

}
