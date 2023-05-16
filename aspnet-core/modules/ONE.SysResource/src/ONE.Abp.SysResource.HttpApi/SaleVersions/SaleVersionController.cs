using Microsoft.AspNetCore.Mvc;
using ONE.Abp.Pagination.Contracts.Dtos;
using ONE.Abp.SysResource.RoleMenus;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace ONE.Abp.SysResource.SaleVersions;

/// <summary>
/// 系统销售版本服务
/// </summary>
[Area(SysResourceRemoteServiceConsts.ModuleName)]
[RemoteService(Name = SysResourceRemoteServiceConsts.RemoteServiceName)]
[Route("api/sys-resource/sale-version")]
public class SaleVersionController : SysResourceController, ISaleVersionAppService
{
    private readonly ISaleVersionAppService _saleVersionAppService;

    public SaleVersionController(ISaleVersionAppService saleVersionAppService)
    {
        _saleVersionAppService = saleVersionAppService;
    }

    /// <summary>
    /// 创建销售版本
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public Task CreateAsync(SaleVersionCreateInput input)
    {
        return _saleVersionAppService.CreateAsync(input);
    }

    /// <summary>
    /// 更新销售版本
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public Task UpdateAsync(Guid id, SaleVersionUpdateInput input)
    {
        return _saleVersionAppService.UpdateAsync(id,input);
    }

    /// <summary>
    /// 删除销售版本
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public Task DeleteAsync(Guid id)
    {
        return _saleVersionAppService.DeleteAsync(id);
    }

    /// <summary>
    /// 获取销售版本
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public Task<SaleVersionDto> GetAsync(Guid id)
    {
        return _saleVersionAppService.GetAsync(id);
    }

    /// <summary>
    /// 分页查询销售版本
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("page")]
    public Task<PagedResult<SaleVersionDto>> QueryPageAsync(SaleVersionQuery input)
    {
        return _saleVersionAppService.QueryPageAsync(input);
    }

    /// <summary>
    /// 获取销售版本的菜单
    /// </summary>
    /// <param name="id"></param>
    /// <param name="appId"></param>
    /// <returns></returns>
    [HttpGet("{id}/menus")]
    public Task<GrantMenus> GetMenuTreeSelected(Guid id, Guid appId)
    {
        return _saleVersionAppService.GetMenuTreeSelected(id, appId);
    }

    /// <summary>
    /// 设置销售版本的菜单
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut("{id}/menus")]
    public Task SetMenusAsync(Guid id,SaleVersionMenuEditInput input)
    {
        return _saleVersionAppService.SetMenusAsync(id,input);
    }


    /// <summary>
    /// 获取销售版本的应用
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}/apps")]
    public Task<ListResultDto<GrantApps>> GetAppsAysnc(Guid id)
    {
        return _saleVersionAppService.GetAppsAysnc(id);
    }
}
