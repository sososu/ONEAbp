using Microsoft.EntityFrameworkCore;
using ONE.Abp.SysResource.SysApps;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ONE.Abp.SysResource.EntityFrameworkCore
{
    public class EfCoreSysAppRepository : EfCoreRepository<ISysResourceDbContext, SysApp, Guid>, ISysAppRepository
    {
        public EfCoreSysAppRepository(IDbContextProvider<ISysResourceDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public virtual async Task<SysApp> FindByCodeAsync(string code, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
         .IncludeDetails(includeDetails)
            .OrderBy(t => t.Id)
         .FirstOrDefaultAsync(t => t.AppCode == code, GetCancellationToken(cancellationToken));
        }

        [Obsolete("Use WithDetailsAsync method.")]
        public override IQueryable<SysApp> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }

        public override async Task<IQueryable<SysApp>> WithDetailsAsync()
        {
            return (await GetQueryableAsync()).IncludeDetails();
        }
    }
}
