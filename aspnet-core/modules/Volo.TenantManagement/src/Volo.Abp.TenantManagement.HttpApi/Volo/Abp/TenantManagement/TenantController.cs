using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ONE.Abp.Pagination.Contracts.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.TenantManagement;

/// <summary>
/// 多租户服务
/// </summary>
[Controller]
[RemoteService(Name = TenantManagementRemoteServiceConsts.RemoteServiceName)]
[Area(TenantManagementRemoteServiceConsts.ModuleName)]
[Route("api/multi-tenancy/tenants")]
public class TenantController : AbpControllerBase, ITenantAppService //TODO: Throws exception on validation if we inherit from Controller
{
    protected ITenantAppService TenantAppService { get; }

    public TenantController(ITenantAppService tenantAppService)
    {
        TenantAppService = tenantAppService;
    }

    /// <summary>
    /// 获取租户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{id}")]
    public virtual Task<TenantDto> GetAsync(Guid id)
    {
        return TenantAppService.GetAsync(id);
    }

    /// <summary>
    /// 查询租户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("list")]
    public virtual Task<PagedResultDto<TenantDto>> GetListAsync(GetTenantsInput input)
    {
        return TenantAppService.GetListAsync(input);
    }

    /// <summary>
    /// 创建租户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public virtual Task<TenantDto> CreateAsync(TenantCreateDto input)
    {
        ValidateModel();
        return TenantAppService.CreateAsync(input);
    }

    /// <summary>
    /// 更新租户
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("{id}")]
    public virtual Task<TenantDto> UpdateAsync(Guid id, TenantUpdateDto input)
    {
        return TenantAppService.UpdateAsync(id, input);
    }

    /// <summary>
    /// 删除租户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return TenantAppService.DeleteAsync(id);
    }

    /// <summary>
    /// 获取默认连接字符串
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{id}/default-connection-string")]
    public virtual Task<string> GetDefaultConnectionStringAsync(Guid id)
    {
        return TenantAppService.GetDefaultConnectionStringAsync(id);
    }

    /// <summary>
    /// 更新默认连接字符串
    /// </summary>
    /// <param name="id"></param>
    /// <param name="defaultConnectionString"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("{id}/default-connection-string")]
    public virtual Task UpdateDefaultConnectionStringAsync(Guid id, string defaultConnectionString)
    {
        return TenantAppService.UpdateDefaultConnectionStringAsync(id, defaultConnectionString);
    }

    /// <summary>
    /// 删除默认连接字符串
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("{id}/default-connection-string")]
    public virtual Task DeleteDefaultConnectionStringAsync(Guid id)
    {
        return TenantAppService.DeleteDefaultConnectionStringAsync(id);
    }

    /// <summary>
    /// 获取连接字符串集合
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{id}/connection-strings")]
    public Task<ListResultDto<TenantConnectionStringDto>> GetConnectionStringsAsync(Guid id)
    {
        return TenantAppService.GetConnectionStringsAsync(id);
    }

    /// <summary>
    /// 设置连接字符串
    /// </summary>
    /// <remarks>存在则更新，不存在则新增</remarks>
    /// <param name="id"></param>
    /// <param name="connectionCreateDto"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("{id}/connection-string")]
    public Task UpdateConnectionStringAsync(Guid id, ConnectionStringCreateDto connectionCreateDto)
    {
        return TenantAppService.UpdateConnectionStringAsync(id, connectionCreateDto);
    }

    /// <summary>
    /// 删除连接字符串
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("{id}/connection-string")]
    public Task DeleteConnectionStringAsync(Guid id)
    {
        return TenantAppService.DeleteConnectionStringAsync(id);
    }

    /// <summary>
    /// 设置共享字符串
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("{id}/shared-connection-string")]
    public Task SetSharedConnectionStringAsync(Guid id)
    {
        return TenantAppService.SetSharedConnectionStringAsync(id);
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("{id}/init-data")]
    public Task InitData(Guid id)
    {
        return TenantAppService.InitData(id);
    }

    /// <summary>
    /// 分页查询租户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet("page")]
    public Task<PagedResult<TenantDto>> QueryAsync(TenantQueryInput input)
    {
        return TenantAppService.QueryAsync(input);
    }

    [HttpPut("{id}/sale-version")]
    public Task SetSaleVersionAsync(Guid id, Guid saleVersionId)
    {
        return TenantAppService.SetSaleVersionAsync(id,saleVersionId);
    }

    [HttpPut("{id}/active")]
    public Task SetActiveAsync(Guid id, bool isActive)
    {
        return TenantAppService.SetActiveAsync(id, isActive);
    }
}
