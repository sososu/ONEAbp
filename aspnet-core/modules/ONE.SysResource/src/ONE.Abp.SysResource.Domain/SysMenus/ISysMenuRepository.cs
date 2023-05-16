using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace ONE.Abp.SysResource.SysMenus
{
    public interface ISysMenuRepository : IRepository<SysMenu, Guid>
    {
        Task<SysMenu> FindByCodeAsync(
            string code,
            bool includeDetails = true,
            CancellationToken cancellationToken = default);
    }
}
