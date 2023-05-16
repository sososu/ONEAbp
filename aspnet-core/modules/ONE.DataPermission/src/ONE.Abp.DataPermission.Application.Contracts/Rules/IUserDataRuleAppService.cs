using ONE.Abp.Pagination.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ONE.Abp.DataPermission.Rules
{
    public interface IUserDataRuleAppService:IApplicationService
    {
        public Task CreateAsync(UserDataRuleCreateInput input);

        public Task UpdateAsync(Guid id, UserDataRuleCreateInput input);

        public Task SetEnableAsync(Guid id, UserDataRuleEnableInput input);

        public Task DeleteAsync(Guid id);
        public Task<UserDataRuleDto> GetAsync(Guid id);

        public Task<PagedResult<UserDataRuleDto>> QueryAsync(UserDataRuleQueryInput input);

        public Task<List<UserDataRuleDto>> GetRules(string dataTargetName);
    }
}
