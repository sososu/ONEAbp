using Microsoft.AspNetCore.Mvc;
using ONE.Abp.Pagination.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;

namespace ONE.Abp.DataPermission.Rules
{
    /// <summary>
    /// 权限规则
    /// </summary>
    [Area(DataPermissionRemoteServiceConsts.ModuleName)]
    [RemoteService(Name = DataPermissionRemoteServiceConsts.RemoteServiceName)]
    [Route("api/data-permission/rule")]
    public class UserDataRuleController : DataPermissionController, IUserDataRuleAppService
    {
        private readonly IUserDataRuleAppService _userDataRuleAppService;

        public UserDataRuleController(IUserDataRuleAppService userDataRuleAppService)
        {
            _userDataRuleAppService = userDataRuleAppService;
        }

        /// <summary>
        /// 创建规则
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public Task CreateAsync(UserDataRuleCreateInput input)
        {
            return _userDataRuleAppService.CreateAsync(input);
        }

        /// <summary>
        /// 更新规则
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>

        [HttpPut("{id}")]
        public Task UpdateAsync(Guid id, UserDataRuleCreateInput input)
        {
            return _userDataRuleAppService.UpdateAsync(id, input);
        }

        /// <summary>
        /// 删除规则
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public Task DeleteAsync(Guid id)
        {
            return _userDataRuleAppService.DeleteAsync(id);
        }

        /// <summary>
        /// 设置启用
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("{id}/enable")]
        public Task SetEnableAsync(Guid id, UserDataRuleEnableInput input)
        {
            return _userDataRuleAppService.SetEnableAsync(id, input);
        }


        /// <summary>
        /// 获取规则
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public Task<UserDataRuleDto> GetAsync(Guid id)
        {
            return _userDataRuleAppService.GetAsync(id);
        }

        /// <summary>
        /// 根据数据源获取规则
        /// </summary>
        /// <param name="dataTargetName"></param>
        /// <returns></returns>

        [HttpGet("by-targetname")]
        public Task<List<UserDataRuleDto>> GetRules(string dataTargetName)
        {
            return _userDataRuleAppService.GetRules(dataTargetName);
        }


        /// <summary>
        /// 分页查询规则
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("page")]
        public Task<PagedResult<UserDataRuleDto>> QueryAsync(UserDataRuleQueryInput input)
        {
            return _userDataRuleAppService.QueryAsync(input);
        }

    }
}
