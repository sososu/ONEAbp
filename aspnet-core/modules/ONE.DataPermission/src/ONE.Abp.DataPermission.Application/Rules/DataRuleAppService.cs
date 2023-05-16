using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ONE.Abp.Pagination.Contracts.Dtos;
using ONE.Abp.Data.Rules;
using ONE.Abp.Shared.Utils;
using ONE.Abp.Pagination;
using ONE.Abp.DataPermission.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;
using ONE.Abp.Shared.Rules;

namespace ONE.Abp.DataPermission.Rules
{
    [Authorize]
    public class DataRuleAppService:ApplicationService, IDataRuleAppService
    {
        private readonly IRepository<DataTarget> _dataTargetRepository;

        private readonly IRepository<DataRule> _dataRuleRepository;
        protected IDistributedCache<DataRuleResult> DataRuleCache { get; }

        protected AbpRuleOptions RuleOptions { get; }
        public DataRuleAppService(IRepository<DataRule> dataRuleRepository, IDistributedCache<DataRuleResult> dataRuleCache,
            IOptions<AbpRuleOptions> ruleOptions, IRepository<DataTarget> dataTargetRepository)
        {
            _dataRuleRepository = dataRuleRepository;
            DataRuleCache = dataRuleCache;
            RuleOptions = ruleOptions.Value;
            _dataTargetRepository = dataTargetRepository;
        }
        public async Task<ListResultDto<string>> GetPredefineFieldValuesAsync()
        {
            var keys = RuleOptions.RuleExtraFieldManager.GetPredefineNames();

            return new ListResultDto<string>(keys.ToList());
        }


        public async Task<ListResultDto<IdName<Guid>>> GetIdNamesAsync(string targetName)
        {
            return new ListResultDto<IdName<Guid>>(await (await _dataRuleRepository.WithDetailsAsync())
                .WhereIf(targetName.IsNotNullOrWhiteSpace(),x => x.DataTargetName == targetName)
                .Select(u => new IdName<Guid> { Id = u.Id, Name = u.Name }).ToListAsync());
        }

        [Authorize(Policy = DataPermissionPermissions.DataRule.Create)]
        public async Task<DataRuleDto> CreateAsync(DataRuleCreateInput input)
        {
            //检查名称是否唯一
            await ValidateNameAsync(input.Name);

            var dataRule = new DataRule(GuidGenerator.Create(),input.DataTargetName,input.Name);

            if (input.HideDataTargetFields != null && input.HideDataTargetFields.Any())
            {
                var fieldDisplayNames = await GetFieldsDisplayNameAsync(input.DataTargetName, input.HideDataTargetFields);
                dataRule.SetDataTargetFields(input.HideDataTargetFields, fieldDisplayNames);
            }
              

            if (input.ConditionGroups != null && input.ConditionGroups.Any())
                dataRule.SetCondition(input.ConditionGroups.ToJson());

            if (input.DataOperations != null && input.DataOperations.Any())
                dataRule.SetRuleDataOperation((RuleDataOperation)input.DataOperations.Sum(d=>(int)d));
            dataRule= await _dataRuleRepository.InsertAsync(dataRule);
            return ObjectMapper.Map<DataRule, DataRuleDto>(dataRule);
        }

        [Authorize(Policy = DataPermissionPermissions.DataRule.Update)]
        public async Task<DataRuleDto> UpdateAsync(Guid id, DataRuleCreateInput input)
        {
            var dataRule = await _dataRuleRepository.GetAsync(u => u.Id == id);

            //检查名称是否唯一
            await ValidateNameAsync(input.Name,dataRule.Id);

            dataRule.SetName(input.Name);


            var fieldDisplayNames = await GetFieldsDisplayNameAsync(input.DataTargetName, input.HideDataTargetFields);
            dataRule.SetDataTargetFields(input.HideDataTargetFields, fieldDisplayNames);

            if (input.ConditionGroups != null && input.ConditionGroups.Any())
                dataRule.SetCondition(input.ConditionGroups.ToJson());

            dataRule.SetRuleDataOperation((RuleDataOperation)input.DataOperations?.Sum(d => (int)d));
            dataRule=await _dataRuleRepository.UpdateAsync(dataRule);

            var key = string.Format(RuleCacheKeys.DataRuleKey, id);
            await DataRuleCache.RemoveAsync(key);

            return ObjectMapper.Map<DataRule, DataRuleDto>(dataRule);
        }


        [Authorize(Policy = DataPermissionPermissions.DataRule.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _dataRuleRepository.DeleteAsync(u => u.Id == id);

            var key = string.Format(RuleCacheKeys.DataRuleKey, id);
            await DataRuleCache.RemoveAsync(key);
        }


        [Authorize(Policy = DataPermissionPermissions.DataRule.Default)]
        public async Task<PagedResult<DataRuleMini>> QueryAsync(DataRuleQueryInput input)
        {
            return await (await _dataRuleRepository.WithDetailsAsync()).ToPagedResultAsync<DataRule, DataRuleMini>(input);
        }

        [Authorize(Policy = DataPermissionPermissions.DataRule.Default)]
        public async Task<DataRuleDto> GetAsync(Guid id)
        {
            var dataRule = await _dataRuleRepository.GetAsync(u => u.Id == id);
            return ObjectMapper.Map<DataRule, DataRuleDto>(dataRule);
        }

        protected virtual async Task<IList<string>> GetFieldsDisplayNameAsync(string targetName,IList<string> fieldsName)
        {
            if (fieldsName == null || !fieldsName.Any())
                return null;
            var dataTarget = await (await _dataTargetRepository.WithDetailsAsync()).Include(d => d.Fields).Where(r => r.Name == targetName)
                 .FirstOrDefaultAsync();
            if (dataTarget == null)
                return null;
            return dataTarget.Fields?.Where(f => fieldsName.Contains(f.Name)).Select(f => f.DisplayName??f.Name).ToList();
        }

        protected virtual async Task ValidateNameAsync(string name, Guid? expectedId = null)
        {
            var dataRule = await _dataRuleRepository.FindAsync(d => d.Name == name);
            if (dataRule!=null && dataRule.Name == name && dataRule.Id != expectedId)
                throw new BusinessException(DataPermissionErrorCodes.ExistDataRuleName).WithData("Name",name);

        }
    }
}
