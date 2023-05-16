using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ONE.Abp.Pagination.Contracts.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.Identity;

[RemoteService(Name = IdentityRemoteServiceConsts.RemoteServiceName)]
[Area(IdentityRemoteServiceConsts.ModuleName)]
[ControllerName("Role")]
[Route("api/identity/roles")]
public class IdentityRoleController : AbpControllerBase, IIdentityRoleAppService
{
    protected IIdentityRoleAppService RoleAppService { get; }

    public IdentityRoleController(IIdentityRoleAppService roleAppService)
    {
        RoleAppService = roleAppService;
    }

    /// <summary>
    /// 获取所有角色
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("all")]
    public virtual Task<ListResultDto<IdentityRoleDto>> GetAllListAsync()
    {
        return RoleAppService.GetAllListAsync();
    }

    /// <summary>
    /// 分页获取角色
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public virtual Task<PagedResultDto<IdentityRoleDto>> GetListAsync(GetIdentityRolesInput input)
    {
        return RoleAppService.GetListAsync(input);
    }

    /// <summary>
    /// 获取角色
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{id}")]
    public virtual Task<IdentityRoleDto> GetAsync(Guid id)
    {
        return RoleAppService.GetAsync(id);
    }

    /// <summary>
    /// 创建角色
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public virtual Task<IdentityRoleDto> CreateAsync(IdentityRoleCreateDto input)
    {
        return RoleAppService.CreateAsync(input);
    }

    /// <summary>
    /// 更新角色
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("{id}")]
    public virtual Task<IdentityRoleDto> UpdateAsync(Guid id, IdentityRoleUpdateDto input)
    {
        return RoleAppService.UpdateAsync(id, input);
    }

    /// <summary>
    /// 删除角色
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return RoleAppService.DeleteAsync(id);
    }


    /// <summary>
    /// 分页查询角色
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet]
    [Route("page")]
    public Task<PagedResult<IdentityRoleDto>> QueryAsync(QueryRoleInput input)
    {
        return RoleAppService.QueryAsync(input);
    }
}
