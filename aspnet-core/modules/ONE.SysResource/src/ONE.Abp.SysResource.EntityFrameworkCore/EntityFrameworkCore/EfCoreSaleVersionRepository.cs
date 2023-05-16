using Microsoft.EntityFrameworkCore;
using ONE.Abp.SysResource.SaleVersions;
using ONE.Abp.SysResource.SysApps;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace ONE.Abp.SysResource.EntityFrameworkCore
{
    public class EfCoreSaleVersionRepository : EfCoreRepository<ISysResourceDbContext, SaleVersion, Guid>, ISaleVersionRepository
    {
        public EfCoreSaleVersionRepository(IDbContextProvider<ISysResourceDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public virtual async Task<SaleVersion> FindByNameAsync(string name, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            return await(await GetDbSetAsync())
           .IncludeDetails(includeDetails)
           .OrderBy(t => t.Id)
           .FirstOrDefaultAsync(t => t.Name == name, GetCancellationToken(cancellationToken));
        }

        [Obsolete("Use WithDetailsAsync method.")]
        public override IQueryable<SaleVersion> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }

        public override async Task<IQueryable<SaleVersion>> WithDetailsAsync()
        {
            return (await GetQueryableAsync()).IncludeDetails();
        }
    }
}
