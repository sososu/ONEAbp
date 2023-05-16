using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ONE.Abp.Pagination.Contracts.Dtos;
using ONE.Abp.Data.Rules;
using ONE.Abp.Pagination;
using ONE.Abp.DataPermission.Permissions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;
using ONE.Abp.Shared.Rules;

namespace ONE.Abp.DataPermission.Rules
{
    [Authorize]
    public class UserDataRuleAppService : ApplicationService, IUserDataRuleAppService
    {

        private readonly IRepository<UserDataRule> _userDataRuleRepository;
        private readonly IRepository<DataRule> _dataRuleRepository;
        private readonly IRepository<UserRule> _userRuleRepository;
        private readonly IUserDataRuleManager _userDataRuleManager;
        protected IDistributedCache<List<UserDataRuleResult>> UserDataRuleCache { get; }
        public UserDataRuleAppService(IRepository<UserDataRule> userDataRuleRepository, IRepository<DataRule> dataRuleRepository, 
            IRepository<UserRule> userRuleRepository, IDistributedCache<List<UserDataRuleResult>> userDataRuleCache, IUserDataRuleManager userDataRuleManager)
        {
            _userDataRuleRepository = userDataRuleRepository;
            _dataRuleRepository = dataRuleRepository;
            _userRuleRepository = userRuleRepository;
            UserDataRuleCache = userDataRuleCache;
            _userDataRuleManager = userDataRuleManager;
        }


        [Authorize(Policy = DataPermissionPermissions.Rule.Create)]
        public async Task CreateAsync(UserDataRuleCreateInput input)
        {
            var userDataRule=await _userDataRuleManager.CreateAsync(input.DataTargetName, input.UserRuleId, input.DataRuleId);

            userDataRule.SetRuleType(input.RuleType);
            userDataRule.Priority = input.Priority;

            await _userDataRuleRepository.InsertAsync(userDataRule);

            await ClearCache(input.DataTargetName);
        }


        [Authorize(Policy = DataPermissionPermissions.Rule.Update)]
        public async Task UpdateAsync(Guid id, UserDataRuleCreateInput input)
        {
            var userDataRule = await _userDataRuleRepository.GetAsync(u => u.Id == id);

            await _userDataRuleManager.ChangeeUserDataIdAsync(userDataRule,input.DataTargetName,input.UserRuleId,input.DataRuleId);

            userDataRule.SetRuleType(input.RuleType);
            userDataRule.Priority = input.Priority;
            await _userDataRuleRepository.UpdateAsync(userDataRule);

            await ClearCache(input.DataTargetName);
        }

        [Authorize(Policy = DataPermissionPermissions.Rule.Enable)]
        public async Task SetEnableAsync(Guid id, UserDataRuleEnableInput input)
        {
            var userDataRule = await _userDataRuleRepository.GetAsync(u => u.Id == id);
            userDataRule.IsEnable = input.IsEnable;
            await _userDataRuleRepository.UpdateAsync(userDataRule);

            await ClearCache(userDataRule.DataTargetName);
        }

        [Authorize(Policy = DataPermissionPermissions.Rule.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            var userDataRule = await _userDataRuleRepository.FindAsync(u => u.Id == id);
            if (userDataRule == null)
                return;

            await _userDataRuleRepository.DeleteAsync(u => u.Id == id);

            await ClearCache(userDataRule.DataTargetName);
        }


        [Authorize(Policy = DataPermissionPermissions.Rule.Default)]
        public async Task<PagedResult<UserDataRuleDto>> QueryAsync(UserDataRuleQueryInput input)
        {
            var result = await (await _userDataRuleRepository.WithDetailsAsync()).ToPagedResultAsync<UserDataRule, UserDataRuleDto>(input);
            if (result.Items.Count < 1)
                return result;

            var userRuleIds = result.Items.Select(x => x.UserRuleId);
            var dataRuleIds = result.Items.Select(x => x.DataRuleId);

            var userRuleKv = await (await _userRuleRepository.WithDetailsAsync()).Where(u => userRuleIds.Contains(u.Id)).Select(u => new { u.Id, u.Name }).ToDictionaryAsync(u => u.Id, u => u.Name);
            var dataRuleKv = await (await _dataRuleRepository.WithDetailsAsync()).Where(u => dataRuleIds.Contains(u.Id)).Select(u => new { u.Id, u.Name }).ToDictionaryAsync(u => u.Id, u => u.Name);

            foreach (var item in result.Items)
            {
                item.UserRuleName = userRuleKv.ContainsKey(item.UserRuleId) ? userRuleKv[item.UserRuleId] : string.Empty;
                item.DataRuleName = dataRuleKv.ContainsKey(item.DataRuleId) ? dataRuleKv[item.DataRuleId] : string.Empty;
            }

            return result;
        }

        [Authorize(Policy = DataPermissionPermissions.Rule.Default)]
        public async Task<UserDataRuleDto> GetAsync(Guid id)
        {
            var userDataRule = await _userDataRuleRepository.GetAsync(u => u.Id == id);
            return ObjectMapper.Map<UserDataRule, UserDataRuleDto>(userDataRule);
        }

        public async Task<List<UserDataRuleDto>> GetRules(string dataTargetName)
        {
            var result = await (await _userDataRuleRepository.WithDetailsAsync())
                .Where(r => r.IsEnable && r.DataTargetName == dataTargetName)
                .OrderByDescending(r => r.RuleType).OrderByDescending(r => r.Priority).ToListAsync();

            return ObjectMapper.Map<List<UserDataRule>, List<UserDataRuleDto>>(result);
        }

        private async Task ClearCache(string target)
        {
            var key = string.Format(RuleCacheKeys.UserDataRuleKey, target);
            await UserDataRuleCache.RemoveAsync(key);
        }
    }
}
