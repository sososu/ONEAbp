using ONE.Abp.Pagination.Contracts.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ONE.Abp.DataPermission.Rules
{
    public interface IDataRuleAppService:IApplicationService
    {
        public Task<ListResultDto<string>> GetPredefineFieldValuesAsync();
        public Task<ListResultDto<IdName<Guid>>> GetIdNamesAsync(string targetName);
        public Task<DataRuleDto> CreateAsync(DataRuleCreateInput input);

        public Task<DataRuleDto> UpdateAsync(Guid id, DataRuleCreateInput input);

        public Task<DataRuleDto> GetAsync(Guid id);
        public Task DeleteAsync(Guid id);

        public Task<PagedResult<DataRuleMini>> QueryAsync(DataRuleQueryInput input);
    }
}
