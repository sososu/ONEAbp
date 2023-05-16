using ONE.Abp.Pagination.Contracts.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ONE.Abp.SysResource.SysApps
{
    public interface ISysAppAppService : IApplicationService
    {
        public Task CreateAsync(SysAppCreateInput input);

        public Task UpdateAsync(Guid id,SysAppUpdateInput input);


        public Task<SysAppDto> GetAsync(Guid id);

        public Task DeleteAsync(Guid id);

        public Task<PagedResult<SysAppDto>> QueryPageAsync(SysAppQuery input);
    }
}
