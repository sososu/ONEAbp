using ONE.Abp.Pagination.Contracts.Dtos;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ONE.Abp.DataPermission.Rules
{
    public interface IDataTargetAppService:IApplicationService
    {
        public Task<PagedResult<DataTargetDto>> QueryAsync(DataTargetQueryInput input);

        public Task<ListResultDto<DataTargetDto>> GetListAsync();

        public Task<DataTargetDto> GetAsync(string name);
    }
}
