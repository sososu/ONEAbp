using ONE.Abp.Pagination.Contracts.Dtos;
using ONE.Abp.SysResource.RoleMenus;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ONE.Abp.SysResource.SaleVersions
{
    public interface ISaleVersionAppService:IApplicationService
    {
        public Task CreateAsync(SaleVersionCreateInput input);

        public Task UpdateAsync(Guid id,SaleVersionUpdateInput input);


        public Task<SaleVersionDto> GetAsync(Guid id);

        public Task DeleteAsync(Guid id);

        public Task<PagedResult<SaleVersionDto>> QueryPageAsync(SaleVersionQuery input);


        public Task SetMenusAsync(Guid id,SaleVersionMenuEditInput input);

        public Task<GrantMenus> GetMenuTreeSelected(Guid id, Guid appId);

        public Task<ListResultDto<GrantApps>> GetAppsAysnc(Guid id);
    }
}
