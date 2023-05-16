using ONE.Abp.Pagination.Contracts.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ONE.Abp.SysResource.SysMenus
{
    public interface ISysMenuAppService : IApplicationService
    {
        public Task CreateAsync(SysMenuCreateInput input);

        public Task UpdateAsync( Guid id, SysMenuUpdateInput input);


        public Task<SysMenuDto> GetAsync(Guid id);

        public Task DeleteAsync(Guid id);

        public Task<PagedResult<SysMenuDto>> QueryPageAsync(SysMenuQuery input);

        public Task<ListResultDto<SysMenuDto>> GetListByAppIdAsync(Guid appId);

        public Task<ListResultDto<SysMenuDto>> GetListByAppCodeAsync(string appCode);
    }
}
