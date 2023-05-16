﻿using ONE.Abp.Pagination.Contracts.Dtos;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Volo.Abp.Identity;

public interface IIdentityUserAppService
    : ICrudAppService<
        IdentityUserDto,
        Guid,
        GetIdentityUsersInput,
        IdentityUserCreateDto,
        IdentityUserUpdateDto>
{
    Task<ListResultDto<IdentityRoleDto>> GetRolesAsync(Guid id);

    Task<ListResultDto<IdentityRoleDto>> GetAssignableRolesAsync();

    Task UpdateRolesAsync(Guid id, IdentityUserUpdateRolesDto input);

    Task<IdentityUserDto> FindByUsernameAsync(string userName);

    Task<IdentityUserDto> FindByEmailAsync(string email);

    Task<PagedResult<IdentityUserExtDto>> QueryAsync(IdentityUserQuery input);

    Task ResetPasswordAsync(Guid id);

}
