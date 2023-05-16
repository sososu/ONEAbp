using Microsoft.AspNetCore.Mvc;
using ONE.Abp.Pagination.Contracts.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace ONE.Abp.DataPermission.Rules
{
    /// <summary>
    /// 用户规则
    /// </summary>
    [Area(DataPermissionRemoteServiceConsts.ModuleName)]
    [RemoteService(Name = DataPermissionRemoteServiceConsts.RemoteServiceName)]
    [Route("api/data-permission/user-rule")]
    public class UserRuleController : DataPermissionController, IUserRuleAppService
    {
        private readonly IUserRuleAppService _userRuleAppService;

        public UserRuleController(IUserRuleAppService userRuleAppService)
        {
            _userRuleAppService = userRuleAppService;
        }

        /// <summary>
        /// 创建规则
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<UserRuleDto> CreateAsync(UserRuleCreateInput input)
        {
            return _userRuleAppService.CreateAsync(input);
        }

        /// <summary>
        /// 更新规则
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>

        [HttpPut("{id}")]
        public Task<UserRuleDto> UpdateAsync(Guid id, UserRuleCreateInput input)
        {
            return _userRuleAppService.UpdateAsync(id, input);
        }

        /// <summary>
        /// 删除规则
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public Task DeleteAsync(Guid id)
        {
            return _userRuleAppService.DeleteAsync(id);
        }

 
        /// <summary>
        /// 获取规则
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public Task<UserRuleDto> GetAsync(Guid id)
        {
            return _userRuleAppService.GetAsync(id);
        }

        /// <summary>
        /// 分页查询规则
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("page")]
        public Task<PagedResult<UserRuleMini>> QueryAsync(UserRuleQueryInput input)
        {
            return _userRuleAppService.QueryAsync(input);
        }


        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        [HttpGet("kv")]
        public Task<ListResultDto<IdName<Guid>>> GetIdNamesAsync()
        {
            return _userRuleAppService.GetIdNamesAsync();
        }

        /// <summary>
        /// 获取对象字段
        /// </summary>
        /// <returns></returns>
        [HttpGet("fields")]
        public Task<ListResultDto<DataTargetFieldDto>> GetUserTargetFieldsAsync()
        {
            return _userRuleAppService.GetUserTargetFieldsAsync();
        }
    }
}
