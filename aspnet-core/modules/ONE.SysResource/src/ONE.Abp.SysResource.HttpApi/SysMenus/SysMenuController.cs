using Microsoft.AspNetCore.Mvc;
using ONE.Abp.Pagination.Contracts.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace ONE.Abp.SysResource.SysMenus;

/// <summary>
/// 系统菜单服务
/// </summary>
[Area(SysResourceRemoteServiceConsts.ModuleName)]
[RemoteService(Name = SysResourceRemoteServiceConsts.RemoteServiceName)]
[Route("api/sys-resource/menu")]
public class SysMenuController : SysResourceController, ISysMenuAppService
{
    private readonly ISysMenuAppService _menuAppService;

    public SysMenuController(ISysMenuAppService menuAppService)
    {
        _menuAppService = menuAppService;
    }

    /// <summary>
    /// 创建菜单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public Task CreateAsync(SysMenuCreateInput input)
    {
       return _menuAppService.CreateAsync(input);
    }

    /// <summary>
    /// 更新菜单
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public Task UpdateAsync(Guid id,SysMenuUpdateInput input)
    {
        return _menuAppService.UpdateAsync(id,input);
    }

    /// <summary>
    /// 删除菜单
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public Task DeleteAsync(Guid id)
    {
        return _menuAppService.DeleteAsync(id);
    }

    /// <summary>
    /// 获取菜单
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public Task<SysMenuDto> GetAsync(Guid id)
    {
        return _menuAppService.GetAsync(id);
    }

    /// <summary>
    /// 分页查询菜单
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("page")]
    public Task<PagedResult<SysMenuDto>> QueryPageAsync(SysMenuQuery input)
    {
        return _menuAppService.QueryPageAsync(input);
    }

    /// <summary>
    /// 根据AppCode获取菜单
    /// </summary>
    /// <param name="appCode"></param>
    /// <returns></returns>
    [HttpGet("by-appcode")]
    public Task<ListResultDto<SysMenuDto>> GetListByAppCodeAsync(string appCode)
    {
        return _menuAppService.GetListByAppCodeAsync(appCode);
    }

    /// <summary>
    /// 根据AppId获取菜单
    /// </summary>
    /// <param name="appId"></param>
    /// <returns></returns>
    [HttpGet("by-appid")]
    public Task<ListResultDto<SysMenuDto>> GetListByAppIdAsync(Guid appId)
    {
        return _menuAppService.GetListByAppIdAsync(appId);
    }
}
