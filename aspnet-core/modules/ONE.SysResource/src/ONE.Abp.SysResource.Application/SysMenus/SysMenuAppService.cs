using Microsoft.AspNetCore.Authorization;
using ONE.Abp.Pagination.Contracts.Dtos;
using ONE.Abp.Pagination;
using ONE.Abp.SysResource.Permissions;
using ONE.Abp.SysResource.SysApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization.Permissions;

namespace ONE.Abp.SysResource.SysMenus
{
    [Authorize]
    public class SysMenuAppService : SysResourceAppService, ISysMenuAppService
    {
        protected ISysAppRepository SysAppRepository { get; }
        protected ISysMenuRepository SysMenuRepository { get; }
        protected ISysMenuManager SysMenuManager { get; }

        protected IPermissionDefinitionManager PermissionDefinitionManager;
        public SysMenuAppService(ISysMenuRepository sysMenuRepository, ISysMenuManager sysMenuManager, IPermissionDefinitionManager permissionDefinitionManager, ISysAppRepository sysAppRepository)
        {
            SysMenuRepository = sysMenuRepository;
            SysMenuManager = sysMenuManager;
            PermissionDefinitionManager = permissionDefinitionManager;
            SysAppRepository= sysAppRepository;
        }

        [Authorize(Policy = SysResourcePermissions.SysMenu.Create)]
        public async Task CreateAsync(SysMenuCreateInput input)
        {
            var sysMenu = await SysMenuManager.CreateAsync(input.SysAppId, input.Code);
            await UpdateMenuInput(sysMenu, input);
            await SysMenuRepository.InsertAsync(sysMenu);
        }

        [Authorize(Policy = SysResourcePermissions.SysMenu.Update)]
        public async Task UpdateAsync(Guid id,SysMenuUpdateInput input)
        {
            var sysMenu = await SysMenuRepository.GetAsync(id);
            await SysMenuManager.ChangeCodeAsync(sysMenu, input.Code);
            await UpdateMenuInput(sysMenu, input);
            await SysMenuRepository.UpdateAsync(sysMenu);
        }


        public async Task<SysMenuDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<SysMenu, SysMenuDto>(await SysMenuRepository.GetAsync(id));
        }

        [Authorize(Policy = SysResourcePermissions.SysMenu.Delete)]
        public Task DeleteAsync(Guid id)
        {
            return SysMenuRepository.DeleteAsync(id);
        }

        [Authorize(Policy = SysResourcePermissions.SysMenu.Default)]
        public async Task<PagedResult<SysMenuDto>> QueryPageAsync(SysMenuQuery input)
        {
            return await (await SysMenuRepository.WithDetailsAsync()).ToPagedResultAsync<SysMenu, SysMenuDto>(input);
        }


        public async Task<ListResultDto<SysMenuDto>> GetListByAppIdAsync(Guid appId)
        {
            return new ListResultDto<SysMenuDto>(ObjectMapper.Map<List<SysMenu>, List<SysMenuDto>>(await SysMenuRepository.GetListAsync(m => m.SysAppId == appId)));
        }

        public async Task<ListResultDto<SysMenuDto>> GetListByAppCodeAsync(string appCode)
        {
            var app =await SysAppRepository.GetAsync(s=>s.AppCode== appCode);
            return new ListResultDto<SysMenuDto>(ObjectMapper.Map<List<SysMenu>, List<SysMenuDto>>(await SysMenuRepository.GetListAsync(m => m.SysAppId == app.Id)));
        }


        #region 私有方法

        public async Task UpdateMenuInput(SysMenu sysMenu, SysMenuCreateInput input)
        {
            sysMenu.SetBasicInfo(input.Name, input.Order, input.Path, input.Component, input.MenuType, input.ParentCode, input.Icon, input.Query);
            sysMenu.SetEnable(input.IsFrame, input.IsCache, input.IsEnable, input.Visible);

            if (!input.Perms.IsNullOrEmpty())
            {
                //检验权限资源是否存在
                if (await CheckNotExistPermission(input.Perms))
                    throw new BusinessException(SysResourceErrorCodes.NotExistSysMenuPerms).WithData("perms", input.Perms);
              
            }
            sysMenu.SetPerms(input.Perms);
        }

        /// <summary>
        /// 权限资源是否存在
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        private async Task<bool> CheckNotExistPermission(string permission)
        {
            var permissions = await PermissionDefinitionManager.GetPermissionsAsync();
            return !permissions.Any(p => p.Name == permission);
        }

        #endregion
    }
}
