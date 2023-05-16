// This file is automatically generated by ABP framework to use MVC Controllers from CSharp
using ONE.Abp.Pagination.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Abp.Http.Modeling;
using Volo.Abp.Identity;

// ReSharper disable once CheckNamespace
namespace Volo.Abp.Identity;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IIdentityRoleAppService), typeof(IdentityRoleClientProxy))]
public partial class IdentityRoleClientProxy : ClientProxyBase<IIdentityRoleAppService>, IIdentityRoleAppService
{
    public virtual async Task<ListResultDto<IdentityRoleDto>> GetAllListAsync()
    {
        return await RequestAsync<ListResultDto<IdentityRoleDto>>(nameof(GetAllListAsync));
    }

    public virtual async Task<PagedResultDto<IdentityRoleDto>> GetListAsync(GetIdentityRolesInput input)
    {
        return await RequestAsync<PagedResultDto<IdentityRoleDto>>(nameof(GetListAsync), new ClientProxyRequestTypeValue
        {
            { typeof(GetIdentityRolesInput), input }
        });
    }

    public virtual async Task<IdentityRoleDto> GetAsync(Guid id)
    {
        return await RequestAsync<IdentityRoleDto>(nameof(GetAsync), new ClientProxyRequestTypeValue
        {
            { typeof(Guid), id }
        });
    }

    public virtual async Task<IdentityRoleDto> CreateAsync(IdentityRoleCreateDto input)
    {
        return await RequestAsync<IdentityRoleDto>(nameof(CreateAsync), new ClientProxyRequestTypeValue
        {
            { typeof(IdentityRoleCreateDto), input }
        });
    }

    public virtual async Task<IdentityRoleDto> UpdateAsync(Guid id, IdentityRoleUpdateDto input)
    {
        return await RequestAsync<IdentityRoleDto>(nameof(UpdateAsync), new ClientProxyRequestTypeValue
        {
            { typeof(Guid), id },
            { typeof(IdentityRoleUpdateDto), input }
        });
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        await RequestAsync(nameof(DeleteAsync), new ClientProxyRequestTypeValue
        {
            { typeof(Guid), id }
        });
    }

    public virtual async Task<PagedResult<IdentityRoleDto>> QueryAsync(QueryRoleInput input)
    {
        return await RequestAsync<PagedResult<IdentityRoleDto>>(nameof(QueryAsync), new ClientProxyRequestTypeValue
        {
            { typeof(QueryRoleInput), input }
        });
    }
}
