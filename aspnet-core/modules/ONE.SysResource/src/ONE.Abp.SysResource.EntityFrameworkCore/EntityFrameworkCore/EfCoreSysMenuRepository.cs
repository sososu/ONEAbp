using Microsoft.EntityFrameworkCore;
using ONE.Abp.SysResource.SaleVersions;
using ONE.Abp.SysResource.SysMenus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ONE.Abp.SysResource.EntityFrameworkCore
{
    public class EfCoreSysMenuRepository : EfCoreRepository<ISysResourceDbContext, SysMenu, Guid>, ISysMenuRepository
    {
        public EfCoreSysMenuRepository(IDbContextProvider<ISysResourceDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public virtual async Task<SysMenu> FindByCodeAsync(string code, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
         .IncludeDetails(includeDetails)
            .OrderBy(t => t.Id)
         .FirstOrDefaultAsync(t => t.Code == code, GetCancellationToken(cancellationToken));
        }


        [Obsolete("Use WithDetailsAsync method.")]
        public override IQueryable<SysMenu> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }

        public override async Task<IQueryable<SysMenu>> WithDetailsAsync()
        {
            return (await GetQueryableAsync()).IncludeDetails();
        }
    }
}
