using Microsoft.AspNetCore.Mvc;
using ONE.Abp.Pagination.Contracts.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using static Volo.Abp.UI.Navigation.DefaultMenuNames.Application;

namespace Volo.Abp.Identity;

[RemoteService(Name = IdentityRemoteServiceConsts.RemoteServiceName)]
[Area(IdentityRemoteServiceConsts.ModuleName)]
[ControllerName("User")]
[Route("api/identity/users")]
public class IdentityUserController : AbpControllerBase, IIdentityUserAppService
{
    protected IIdentityUserAppService UserAppService { get; }

    public IdentityUserController(IIdentityUserAppService userAppService)
    {
        UserAppService = userAppService;
    }

    /// <summary>
    /// ��ȡ�û�
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{id}")]
    public virtual Task<IdentityUserDto> GetAsync(Guid id)
    {
        return UserAppService.GetAsync(id);
    }

    /// <summary>
    /// ��ҳ��ȡ�û�
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public virtual Task<PagedResultDto<IdentityUserDto>> GetListAsync(GetIdentityUsersInput input)
    {
        return UserAppService.GetListAsync(input);
    }

    /// <summary>
    /// �����û�
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public virtual Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input)
    {
        return UserAppService.CreateAsync(input);
    }

    /// <summary>
    /// �����û�
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("{id}")]
    public virtual Task<IdentityUserDto> UpdateAsync(Guid id, IdentityUserUpdateDto input)
    {
        return UserAppService.UpdateAsync(id, input);
    }

    /// <summary>
    /// ɾ���û�
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return UserAppService.DeleteAsync(id);
    }

    /// <summary>
    /// ��ҳ��ѯ�û�
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet]
    [Route("page")]
    public Task<PagedResult<IdentityUserExtDto>> QueryAsync(IdentityUserQuery input)
    {
        return UserAppService.QueryAsync(input);
    }

    /// <summary>
    /// ��ȡ�û���ɫ
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{id}/roles")]
    public virtual Task<ListResultDto<IdentityRoleDto>> GetRolesAsync(Guid id)
    {
        return UserAppService.GetRolesAsync(id);
    }

    /// <summary>
    /// ��ȡ�ɷ����ɫ
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("assignable-roles")]
    public Task<ListResultDto<IdentityRoleDto>> GetAssignableRolesAsync()
    {
        return UserAppService.GetAssignableRolesAsync();
    }

    /// <summary>
    /// �����û���ɫ
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("{id}/roles")]
    public virtual Task UpdateRolesAsync(Guid id, IdentityUserUpdateRolesDto input)
    {
        return UserAppService.UpdateRolesAsync(id, input);
    }

    /// <summary>
    /// �����û����Ʋ�ѯ�û�
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("by-username/{userName}")]
    public virtual Task<IdentityUserDto> FindByUsernameAsync(string userName)
    {
        return UserAppService.FindByUsernameAsync(userName);
    }

    /// <summary>
    /// ���������ѯ�û�
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("by-email/{email}")]
    public virtual Task<IdentityUserDto> FindByEmailAsync(string email)
    {
        return UserAppService.FindByEmailAsync(email);
    }

    /// <summary>
    /// ��������
    /// </summary>
    /// <param name="id"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("{id}/reset-password")]
    public Task ResetPasswordAsync(Guid id)
    {
        return UserAppService.ResetPasswordAsync(id);
    }
}
