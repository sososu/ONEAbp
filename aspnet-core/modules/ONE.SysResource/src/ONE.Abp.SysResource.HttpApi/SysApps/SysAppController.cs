using Microsoft.AspNetCore.Mvc;
using ONE.Abp.Pagination.Contracts.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp;

namespace ONE.Abp.SysResource.SysApps;

/// <summary>
/// 系统应用服务
/// </summary>
[Area(SysResourceRemoteServiceConsts.ModuleName)]
[RemoteService(Name = SysResourceRemoteServiceConsts.RemoteServiceName)]
[Route("api/sys-resource/app")]
public class SysAppController : SysResourceController, ISysAppAppService
{
    private readonly ISysAppAppService _sysAppService;

    public SysAppController(ISysAppAppService sysAppService)
    {
        _sysAppService = sysAppService;
    }

    /// <summary>
    /// 创建系统应用
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public Task CreateAsync(SysAppCreateInput input)
    {
        return _sysAppService.CreateAsync(input);
    }

    /// <summary>
    /// 更新系统应用
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public Task UpdateAsync(Guid id, SysAppUpdateInput input)
    {
        return _sysAppService.UpdateAsync(id,input);
    }

    /// <summary>
    /// 删除系统应用
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public Task DeleteAsync(Guid id)
    {
        return _sysAppService.DeleteAsync(id);
    }

    /// <summary>
    /// 获取系统应用
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public Task<SysAppDto> GetAsync(Guid id)
    {
        return _sysAppService.GetAsync(id);
    }

    /// <summary>
    /// 分页获取系统应用
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("page")]
    public Task<PagedResult<SysAppDto>> QueryPageAsync(SysAppQuery input)
    {
       return _sysAppService.QueryPageAsync(input);
    }


}
