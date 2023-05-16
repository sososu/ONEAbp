using ONE.Abp.Pagination.Contracts.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ONE.Abp.DataPermission.Rules
{
    public interface IUserRuleAppService : IApplicationService
    {
        public Task<ListResultDto<DataTargetFieldDto>> GetUserTargetFieldsAsync();
        public Task<ListResultDto<IdName<Guid>>> GetIdNamesAsync();
        public Task<UserRuleDto> CreateAsync(UserRuleCreateInput input);
        public Task<UserRuleDto> UpdateAsync(Guid id, UserRuleCreateInput input);

        public Task<UserRuleDto> GetAsync(Guid id);
        public Task DeleteAsync(Guid id);
        public Task<PagedResult<UserRuleMini>> QueryAsync(UserRuleQueryInput input);
    }
}
