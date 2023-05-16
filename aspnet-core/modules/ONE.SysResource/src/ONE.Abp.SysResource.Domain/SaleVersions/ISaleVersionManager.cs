using JetBrains.Annotations;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace ONE.Abp.SysResource.SaleVersions
{
    public interface ISaleVersionManager : IDomainService
    {
        [NotNull]
        Task<SaleVersion> CreateAsync([NotNull] string name);

        Task ChangeNameAsync([NotNull] SaleVersion saleVersion, [NotNull] string name);

        Task<List<SaleVersionMenu>> GetMenusAsync(Guid id, Guid? appId = null);

        Task SetMenuAsync(SaleVersion saleVersion, Guid appId, IList<string> menuCodes);

        Task<List<Guid>> GetAppsAsync(Guid id);
    }
}
