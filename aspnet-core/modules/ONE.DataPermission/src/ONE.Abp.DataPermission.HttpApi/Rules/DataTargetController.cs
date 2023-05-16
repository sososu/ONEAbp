using Microsoft.AspNetCore.Mvc;
using ONE.Abp.Pagination.Contracts.Dtos;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace ONE.Abp.DataPermission.Rules
{
    /// <summary>
    /// 数据源对象
    /// </summary>
    [Area(DataPermissionRemoteServiceConsts.ModuleName)]
    [RemoteService(Name = DataPermissionRemoteServiceConsts.RemoteServiceName)]
    [Route("api/data-permission/target")]
    public class DataTargetController : DataPermissionController, IDataTargetAppService
    {
        private readonly IDataTargetAppService _dataTargetAppService;

        public DataTargetController(IDataTargetAppService dataTargetAppService)
        {
            _dataTargetAppService = dataTargetAppService;
        }

        [HttpGet("{name}")]
        public Task<DataTargetDto> GetAsync(string name)
        {
            return _dataTargetAppService.GetAsync(name);
        }

        [HttpGet("list")]
        public Task<ListResultDto<DataTargetDto>> GetListAsync()
        {
            return _dataTargetAppService.GetListAsync();
        }

        [HttpGet("page")]
        public Task<PagedResult<DataTargetDto>> QueryAsync(DataTargetQueryInput input)
        {
            return _dataTargetAppService.QueryAsync(input);
        }
    }
}
