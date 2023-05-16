using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ONE.Abp.Data.Rules;
using ONE.Abp.Pagination;
using ONE.Abp.Pagination.Contracts.Dtos;
using ONE.Abp.Shared.Rules;
using ONE.Abp.Shared.Utils;
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
using Volo.Abp.Users;

namespace ONE.Abp.DataPermission.Rules
{
    [Authorize]
    public class UserRuleAppService : ApplicationService, IUserRuleAppService
    {
        private readonly IRepository<UserRule> _userRuleRepository;
        protected IDistributedCache<UserRuleResult> UserRuleCache { get; }
        protected AbpRuleOptions RuleOptions { get; }
        public UserRuleAppService(IRepository<UserRule> userRuleRepository, IDistributedCache<UserRuleResult> userRuleCache, IOptions<AbpRuleOptions> ruleOptions)
        {
            _userRuleRepository = userRuleRepository;
            UserRuleCache = userRuleCache;
            RuleOptions = ruleOptions.Value;
        }


        public async Task<ListResultDto<DataTargetFieldDto>> GetUserTargetFieldsAsync()
        {
            var fields = new List<DataTargetFieldDto>
            {
                new DataTargetFieldDto
            {
                Name = nameof(ICurrentUser.IsAuthenticated),
                DisplayName =
                    "是否认证",
                Type = typeof(bool).Name,
                Description="布尔类型"
            },
                   new DataTargetFieldDto
            {
                Name = nameof(ICurrentUser.Email),
                DisplayName =
                    "邮箱",
                Type = typeof(string).Name,
                  Description="字符串类型"
            },
                        new DataTargetFieldDto
                {
                Name = nameof(ICurrentUser.UserName),
                DisplayName =
                    "用户名",
                Type = typeof(string).Name,
                   Description="字符串类型"
            },
                     new DataTargetFieldDto
                {
                Name = nameof(ICurrentUser.Name),
                DisplayName =
                    "姓名",
                Type = typeof(string).Name,
            },
                        new DataTargetFieldDto
                {
                Name = nameof(ICurrentUser.PhoneNumber),
                DisplayName =
                    "手机号",
                Type = typeof(string).Name,
                   Description="字符串类型"
            },
                            new DataTargetFieldDto
            {
                Name = nameof(ICurrentUser.Roles),
                DisplayName =
                    "角色集合",
                Type = typeof(string[]).Name,
                   Description="字符串数组类型"
            },
        };

            var extraFields = RuleOptions.RuleExtraFieldManager.GetRuleExtraFieldForClaims();

            if (extraFields.Any())
            {
                fields = fields.Union(extraFields.Select(x => new DataTargetFieldDto
                {
                    Name = x.ClaimName,
                    DisplayName = x.ClaimName,
                    Type = typeof(string).Name,
                    Description = "字符串数组类型"
                })).ToList();
            }

            return new ListResultDto<DataTargetFieldDto>(fields);
        }

        public async Task<ListResultDto<IdName<Guid>>> GetIdNamesAsync()
        {
            return new ListResultDto<IdName<Guid>>(await (await _userRuleRepository.WithDetailsAsync()).Select(u => new IdName<Guid> { Id = u.Id, Name = u.Name }).ToListAsync());
        }


        [Authorize(Policy = DataPermissionPermissions.UserRule.Create)]
        public async Task<UserRuleDto> CreateAsync(UserRuleCreateInput input)
        {
            //检查名称是否唯一
            await ValidateNameAsync(input.Name);

            var userRule = new UserRule(GuidGenerator.Create(), input.Name);

            if (input.ConditionGroups != null && input.ConditionGroups.Any())
                userRule.SetCondition(input.ConditionGroups.ToJson());

            userRule = await _userRuleRepository.InsertAsync(userRule);
            return ObjectMapper.Map<UserRule, UserRuleDto>(userRule);
        }


        [Authorize(Policy = DataPermissionPermissions.UserRule.Update)]
        public async Task<UserRuleDto> UpdateAsync(Guid id, UserRuleCreateInput input)
        {
            var userRule = await _userRuleRepository.GetAsync(u => u.Id == id);

            //检查名称是否唯一
            await ValidateNameAsync(input.Name, userRule.Id);

            userRule.SetName(input.Name);

            if (input.ConditionGroups != null && input.ConditionGroups.Any())
                userRule.SetCondition(input.ConditionGroups.ToJson());
            userRule = await _userRuleRepository.UpdateAsync(userRule);

            var key = string.Format(RuleCacheKeys.UserRuleKey, id);
            await UserRuleCache.RemoveAsync(key);

            return ObjectMapper.Map<UserRule, UserRuleDto>(userRule);
        }

        [Authorize(Policy = DataPermissionPermissions.UserRule.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _userRuleRepository.DeleteAsync(u => u.Id == id);

            var key = string.Format(RuleCacheKeys.UserRuleKey, id);
            await UserRuleCache.RemoveAsync(key);
        }


        [Authorize(Policy = DataPermissionPermissions.UserRule.Default)]
        public async Task<PagedResult<UserRuleMini>> QueryAsync(UserRuleQueryInput input)
        {
            return await (await _userRuleRepository.WithDetailsAsync()).ToPagedResultAsync<UserRule, UserRuleMini>(input);
        }

        [Authorize(Policy = DataPermissionPermissions.UserRule.Default)]
        public async Task<UserRuleDto> GetAsync(Guid id)
        {
            var userRule = await _userRuleRepository.GetAsync(u => u.Id == id);
            return ObjectMapper.Map<UserRule, UserRuleDto>(userRule);
        }

        protected virtual async Task ValidateNameAsync(string name, Guid? expectedId = null)
        {
            //检查名称是否唯一
            var userRule = await _userRuleRepository.FindAsync(d => d.Name == name);
            if (userRule!=null && userRule.Name == name && userRule.Id != expectedId)
                throw new BusinessException(DataPermissionErrorCodes.ExistUserRuleName).WithData("Name",name);
        }
    }
}
