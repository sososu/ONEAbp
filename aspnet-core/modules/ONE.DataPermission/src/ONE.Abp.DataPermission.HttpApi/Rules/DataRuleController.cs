using Microsoft.AspNetCore.Mvc;
using ONE.Abp.Pagination.Contracts.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace ONE.Abp.DataPermission.Rules
{
    /// <summary>
    /// 数据规则
    /// </summary>
    [Area(DataPermissionRemoteServiceConsts.ModuleName)]
    [RemoteService(Name = DataPermissionRemoteServiceConsts.RemoteServiceName)]
    [Route("api/data-permission/data-rule")]
    public class DataRuleController : DataPermissionController, IDataRuleAppService
    {
        private readonly IDataRuleAppService _dataRuleAppService;

        public DataRuleController(IDataRuleAppService dataRuleAppService)
        {
            _dataRuleAppService = dataRuleAppService;
        }

        /// <summary>
        /// 创建规则
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<DataRuleDto> CreateAsync(DataRuleCreateInput input)
        {
            return _dataRuleAppService.CreateAsync(input);
        }

        /// <summary>
        /// 更新规则
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>

        [HttpPut("{id}")]
        public Task<DataRuleDto> UpdateAsync(Guid id, DataRuleCreateInput input)
        {
            return _dataRuleAppService.UpdateAsync(id, input);
        }

        /// <summary>
        /// 删除规则
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public Task DeleteAsync(Guid id)
        {
            return _dataRuleAppService.DeleteAsync(id);
        }


        /// <summary>
        /// 获取规则
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public Task<DataRuleDto> GetAsync(Guid id)
        {
            return _dataRuleAppService.GetAsync(id);
        }

        /// <summary>
        /// 分页查询规则
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("page")]
        public Task<PagedResult<DataRuleMini>> QueryAsync(DataRuleQueryInput input)
        {
            return _dataRuleAppService.QueryAsync(input);
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="targetName"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet("kv")]
        public Task<ListResultDto<IdName<Guid>>> GetIdNamesAsync(string targetName)
        {
            return _dataRuleAppService.GetIdNamesAsync(targetName);
        }

        /// <summary>
        /// 获取预定义值
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpGet("define-value")]
        public Task<ListResultDto<string>> GetPredefineFieldValuesAsync()
        {
            return _dataRuleAppService.GetPredefineFieldValuesAsync();
        }
    }
}
