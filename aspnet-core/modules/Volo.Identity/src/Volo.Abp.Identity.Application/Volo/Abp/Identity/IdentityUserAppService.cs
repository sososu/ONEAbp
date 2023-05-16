using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ONE.Abp.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;
namespace Volo.Abp.Identity;
using ONE.Abp.Pagination.Contracts.Dtos;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Settings;

public class IdentityUserAppService : IdentityAppServiceBase, IIdentityUserAppService
{
    protected IdentityUserManager UserManager { get; }
    protected IIdentityUserRepository UserRepository { get; }
    protected IIdentityRoleRepository RoleRepository { get; }
    protected IOrganizationUnitRepository OrganizationUnitRepository { get; }
    protected IOptions<IdentityOptions> IdentityOptions { get; }
    protected ISettingProvider SettingProvider { get; }
    public IdentityUserAppService(
        IdentityUserManager userManager,
        IIdentityUserRepository userRepository,
        IIdentityRoleRepository roleRepository,
        IOptions<IdentityOptions> identityOptions,
        ISettingProvider settingProvider,
        IOrganizationUnitRepository organizationUnitRepository)
    {
        UserManager = userManager;
        UserRepository = userRepository;
        RoleRepository = roleRepository;
        IdentityOptions = identityOptions;
        SettingProvider = settingProvider;
        OrganizationUnitRepository = organizationUnitRepository;
    }

    //TODO: [Authorize(IdentityPermissions.Users.Default)] should go the IdentityUserAppService class.
    [Authorize(IdentityPermissions.Users.Default)]
    public virtual async Task<IdentityUserDto> GetAsync(Guid id)
    {
        return ObjectMapper.Map<IdentityUser, IdentityUserDto>(
            await UserManager.GetByIdAsync(id)
        );
    }

    [Authorize(IdentityPermissions.Users.Default)]
    public virtual async Task<PagedResultDto<IdentityUserDto>> GetListAsync(GetIdentityUsersInput input)
    {
        var count = await UserRepository.GetCountAsync(input.Filter);
        var list = await UserRepository.GetListAsync(input.Sorting, input.MaxResultCount, input.SkipCount, input.Filter);

        return new PagedResultDto<IdentityUserDto>(
            count,
            ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(list)
        );
    }

    [Authorize(IdentityPermissions.Users.Default)]
    public virtual async Task<PagedResult<IdentityUserExtDto>> QueryAsync(IdentityUserQuery input)
    {
        
        var query = (await UserRepository.WithDetailsAsync());
        query=query.WhereIf(input.RoleId.HasValue, identityUser => identityUser.Roles.Any(x => x.RoleId == input.RoleId));

        if (input.OrganizationUnitId.HasValue)
        {
            var oids = new List<Guid> { input.OrganizationUnitId.Value };
            if (input.IncludeOrganizationUnitChildren)
            {
                var org = await OrganizationUnitRepository.GetAsync(input.OrganizationUnitId.Value);
                oids.AddRange((await OrganizationUnitRepository.GetAllChildrenWithParentCodeAsync(org.Code, org.Id)).Select(o => o.Id));
            }
            query=query.Where(dentityUser => dentityUser.OrganizationUnits.Any(x => oids.Contains(x.OrganizationUnitId)));
        }

        var users=await query.ToPagedResultAsync(input);

        var list = new List<IdentityUserExtDto>();

        var roleIds = users.Items.SelectMany(u => u.Roles).Select(r => r.RoleId).ToList();
        var organizationUnitIds = users.Items.SelectMany(u => u.OrganizationUnits).Select(o => o.OrganizationUnitId).ToList();

        var roles = await RoleRepository.GetListAsync(roleIds);
        var organizationUnits = await OrganizationUnitRepository.GetListAsync(organizationUnitIds);

        foreach (var user in users.Items)
        {
            var useExtDto = ObjectMapper.Map<IdentityUser, IdentityUserExtDto>(user);
            if (user.Roles.Any())
            {
                var rIds = user.Roles.Select(r => r.RoleId).ToArray();
                useExtDto.RoleNames = roles.Where(r => rIds.Contains(r.Id)).Select(r => r.Name).ToArray();
            }
            if (user.OrganizationUnits.Any())
            {
                useExtDto.OrganizationUnitId = user.OrganizationUnits.Select(r => r.OrganizationUnitId).FirstOrDefault();
                useExtDto.OrganizationUnitName = organizationUnits.FirstOrDefault(o => o.Id == useExtDto.OrganizationUnitId)?.DisplayName;
            }
            list.Add(useExtDto);
        }
        return new PagedResult<IdentityUserExtDto>(users.PageIndex,users.PageSize,users.TotalCount, list);
    }


    [Authorize(IdentityPermissions.Users.Default)]
    public virtual async Task<ListResultDto<IdentityRoleDto>> GetRolesAsync(Guid id)
    {
        //TODO: Should also include roles of the related OUs.

        var roles = await UserRepository.GetRolesAsync(id);

        return new ListResultDto<IdentityRoleDto>(
            ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(roles)
        );
    }

    [Authorize(IdentityPermissions.Users.Default)]
    public virtual async Task<ListResultDto<IdentityRoleDto>> GetAssignableRolesAsync()
    {
        var list = await RoleRepository.GetListAsync();
        return new ListResultDto<IdentityRoleDto>(
            ObjectMapper.Map<List<IdentityRole>, List<IdentityRoleDto>>(list));
    }

    [Authorize(IdentityPermissions.Users.Create)]
    public virtual async Task<IdentityUserDto> CreateAsync(IdentityUserCreateDto input)
    {
        await IdentityOptions.SetAsync();

        var user = new IdentityUser(
            GuidGenerator.Create(),
            input.UserName,
            input.Email,
            CurrentTenant.Id
        );

        input.MapExtraPropertiesTo(user);

        (await UserManager.CreateAsync(user, input.Password)).CheckErrors();
        await UpdateUserByInput(user, input);
        (await UserManager.UpdateAsync(user)).CheckErrors();

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
    }

    [Authorize(IdentityPermissions.Users.Update)]
    public virtual async Task<IdentityUserDto> UpdateAsync(Guid id, IdentityUserUpdateDto input)
    {
        await IdentityOptions.SetAsync();

        var user = await UserManager.GetByIdAsync(id);

        user.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

        (await UserManager.SetUserNameAsync(user, input.UserName)).CheckErrors();

        await UpdateUserByInput(user, input);
        input.MapExtraPropertiesTo(user);

        (await UserManager.UpdateAsync(user)).CheckErrors();

        if (!input.Password.IsNullOrEmpty())
        {
            (await UserManager.RemovePasswordAsync(user)).CheckErrors();
            (await UserManager.AddPasswordAsync(user, input.Password)).CheckErrors();
        }

        await CurrentUnitOfWork.SaveChangesAsync();

        return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
    }

    [Authorize(IdentityPermissions.Users.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        if (CurrentUser.Id == id)
        {
            throw new BusinessException(code: IdentityErrorCodes.UserSelfDeletion);
        }

        var user = await UserManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return;
        }

        (await UserManager.DeleteAsync(user)).CheckErrors();
    }

    [Authorize(IdentityPermissions.Users.Update)]
    public virtual async Task UpdateRolesAsync(Guid id, IdentityUserUpdateRolesDto input)
    {
        var user = await UserManager.GetByIdAsync(id);
        (await UserManager.SetRolesAsync(user, input.RoleNames)).CheckErrors();
        await UserRepository.UpdateAsync(user);
    }

    [Authorize(IdentityPermissions.Users.Default)]
    public virtual async Task<IdentityUserDto> FindByUsernameAsync(string userName)
    {
        return ObjectMapper.Map<IdentityUser, IdentityUserDto>(
            await UserManager.FindByNameAsync(userName)
        );
    }

    [Authorize(IdentityPermissions.Users.Default)]
    public virtual async Task<IdentityUserDto> FindByEmailAsync(string email)
    {
        return ObjectMapper.Map<IdentityUser, IdentityUserDto>(
            await UserManager.FindByEmailAsync(email)
        );
    }


    [Authorize(IdentityPermissions.Users.ResetPassword)]
    public virtual async Task ResetPasswordAsync(Guid id)
    {
        await IdentityOptions.SetAsync();

        var user = await UserManager.GetByIdAsync(id);

        var defaultPwd=await SettingProvider.GetOrNullAsync(IdentitySettingNames.Password.Default);
        (await UserManager.RemovePasswordAsync(user)).CheckErrors();
        (await UserManager.AddPasswordAsync(user, defaultPwd)).CheckErrors();

        await CurrentUnitOfWork.SaveChangesAsync();
    }

    protected virtual async Task UpdateUserByInput(IdentityUser user, IdentityUserCreateOrUpdateDtoBase input)
    {
        if (!string.Equals(user.Email, input.Email, StringComparison.InvariantCultureIgnoreCase))
        {
            (await UserManager.SetEmailAsync(user, input.Email)).CheckErrors();
        }

        if (!string.Equals(user.PhoneNumber, input.PhoneNumber, StringComparison.InvariantCultureIgnoreCase))
        {
            (await UserManager.SetPhoneNumberAsync(user, input.PhoneNumber)).CheckErrors();
        }

        (await UserManager.SetLockoutEnabledAsync(user, input.LockoutEnabled)).CheckErrors();

        user.Name = input.Name;
        user.Surname = input.Surname;
        user.Sex= input.Sex;
        (await UserManager.UpdateAsync(user)).CheckErrors();
        user.SetIsActive(input.IsActive);
        if (input.RoleNames != null)
        {
            (await UserManager.SetRolesAsync(user, input.RoleNames)).CheckErrors();
        }

        if (input.OrganizationUnitId.HasValue)
        {
            await UserManager.SetOrganizationUnitsAsync(user, new[] { input.OrganizationUnitId.Value });
        }
    }
}
