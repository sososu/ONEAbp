using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Users;

namespace Volo.Abp.Identity;

[RemoteService(Name = IdentityRemoteServiceConsts.RemoteServiceName)]
[Area(IdentityRemoteServiceConsts.ModuleName)]
[ControllerName("UserLookup")]
[Route("api/identity/users/lookup")]
public class IdentityUserLookupController : AbpControllerBase, IIdentityUserLookupAppService
{
    protected IIdentityUserLookupAppService LookupAppService { get; }

    public IdentityUserLookupController(IIdentityUserLookupAppService lookupAppService)
    {
        LookupAppService = lookupAppService;
    }

    /// <summary>
    /// 根据用户Id查找用户
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("{id}")]
    public virtual Task<UserData> FindByIdAsync(Guid id)
    {
        return LookupAppService.FindByIdAsync(id);
    }

    /// <summary>
    /// 根据用户名查找用户
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("by-username/{userName}")]
    public virtual Task<UserData> FindByUserNameAsync(string userName)
    {
        return LookupAppService.FindByUserNameAsync(userName);
    }

    /// <summary>
    /// 搜索用户
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("search")]
    public Task<ListResultDto<UserData>> SearchAsync(UserLookupSearchInputDto input)
    {
        return LookupAppService.SearchAsync(input);
    }

    /// <summary>
    /// 获取用户数量
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("count")]
    public Task<long> GetCountAsync(UserLookupCountInputDto input)
    {
        return LookupAppService.GetCountAsync(input);
    }
}
