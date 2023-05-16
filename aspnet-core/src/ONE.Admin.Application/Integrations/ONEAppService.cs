using Microsoft.AspNetCore.Authorization;
using ONE.Abp.Shared;
using ONE.Abp.SysResource.RoleMenus;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.Uow;

namespace ONE.Admin.Integrations
{
    public class ONEAppService : AdminAppService, IONEAppService
    {
        protected IRoleMenuService RoleMenuService { get; }
        protected IIdentityRoleAppService RoleAppService { get; }
        protected IPermissionGrantRepository PermissionGrantRepository { get; }
        protected IPermissionDefinitionManager PermissionDefinitionManager;
        protected RolePermissionManagementProvider RolePermissionManagementProvider { get; }
        public ONEAppService(IRoleMenuService roleMenuService, RolePermissionManagementProvider rolePermissionManagementProvider, IPermissionGrantRepository permissionGrantRepository,
            IIdentityRoleAppService identityRoleAppService, IPermissionDefinitionManager permissionDefinitionManager)
        { 
            RoleMenuService = roleMenuService;
            RolePermissionManagementProvider = rolePermissionManagementProvider;
            RoleAppService= identityRoleAppService;
            PermissionDefinitionManager = permissionDefinitionManager;
            PermissionGrantRepository = permissionGrantRepository;
        }

        /// <summary>
        /// 角色授权
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(isTransactional:true)]
        public async Task GrantPermsToRoleAsync(RoleGrantingInput input)
        {
            var role = await RoleAppService.GetAsync(input.RoleId);

            if (CurrentUser.IsInRole(role.Name))
                throw new BusinessException(AdminDomainErrorCodes.CannotAuthorizeOwnerRole);

            var granteds=await RoleMenuService.GrantPermsToRoleAsync(new RoleMenuCreateInput { AppId=input.AppId,Role=role.Name,MenuCodes=input.MenuCodes});


            var currentPermisGrants = await PermissionGrantRepository.GetListAsync(RolePermissionManagementProvider.Name, role.Name);
            var currentPermis = currentPermisGrants.Select(s => s.Name);

            var removePerms = currentPermis.Except(granteds.NewPerms);
            var addPerms = granteds.NewPerms.Except(currentPermis);

            if (removePerms != null)
            {
                foreach (var perms in removePerms)
                {
                    await RolePermissionManagementProvider.SetAsync(perms, role.Name, false);
                }
            }

            if (addPerms != null)
            {
                foreach (var perms in addPerms)
                {
                    await RolePermissionManagementProvider.SetAsync(perms, role.Name, true);
                }
            }
        }


        /// <summary>
        /// 加载对应角色应用列表
        /// </summary>
        /// <remarks>按应用名称排序</remarks>
        /// <param name="roleId"></param>    
        /// <returns></returns>
        public async Task<ListResultDto<GrantApps>> GetRoleAppsSelected(Guid roleId)
        {
            var role = await RoleAppService.GetAsync(roleId);
            return await RoleMenuService.GetRoleAppsSelected(role.Name);
        }

        /// <summary>
        /// 加载对应角色菜单列表树
        /// </summary>
        /// <param name="appId"></param>    
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<GrantMenus> GetRoleMenuTreeSelected(Guid appId, Guid roleId)
        {
            var role = await RoleAppService.GetAsync(roleId);
            return await RoleMenuService.GetRoleMenuTreeSelected(appId, role.Name);
        }


        /// <summary>
        /// 获取登录的路由信息
        /// </summary>
        /// <returns></returns>

        [Authorize]
        public async Task<ListResultDto<RouterVo>> GetRouters(string appCode)
        {
            return await RoleMenuService.GetRouters(appCode);
        }

        /// <summary>
        /// 搜索权限资源(前缀匹配)
        /// </summary>
        /// <returns></returns>
        public async Task<ListResultDto<string>> GetSearchAsync(string? name = null)
        {
            return new ListResultDto<string>((await GetPermissionNames()).WhereIf(!string.IsNullOrWhiteSpace(name), s => s.StartsWith(name, true, null)).ToList());
        }

        #region 帮助方法
        /// <summary>
        /// 获取权限资源标识
        /// </summary>
        /// <returns></returns>
        private async Task<IEnumerable<string>> GetPermissionNames()
        {
            return (await PermissionDefinitionManager.GetPermissionsAsync()).Select(p => p.Name);
        }
        #endregion
    }
}
