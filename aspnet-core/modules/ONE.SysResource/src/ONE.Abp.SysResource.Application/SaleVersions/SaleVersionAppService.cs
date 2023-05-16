using Microsoft.AspNetCore.Authorization;
using ONE.Abp.Pagination.Contracts.Dtos;
using ONE.Abp.Shared.Utils;
using ONE.Abp.Pagination;
using ONE.Abp.SysResource.Permissions;
using ONE.Abp.SysResource.RoleMenus;
using ONE.Abp.SysResource.SysApps;
using ONE.Abp.SysResource.SysMenus;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.ObjectExtending;

namespace ONE.Abp.SysResource.SaleVersions
{
    /// <summary>
    /// 销售版本服务
    /// </summary>
    public class SaleVersionAppService : SysResourceAppService, ISaleVersionAppService
    {
        protected ISaleVersionRepository SaleVersionRepository { get; }
        protected ISaleVersionManager SaleVersionManager { get; }

        protected ISysAppRepository SysAppRepository { get; }

        protected ISysMenuRepository SysMenuRepository { get; }
        public SaleVersionAppService(ISaleVersionRepository saleVersionRepository, ISaleVersionManager saleVersionManager, ISysAppRepository sysAppRepository, ISysMenuRepository sysMenuRepository)
        {
            SaleVersionRepository = saleVersionRepository;
            SaleVersionManager = saleVersionManager;
            SysAppRepository = sysAppRepository;
            SysMenuRepository = sysMenuRepository;
        }

        [Authorize(Policy = SysResourcePermissions.SaleVersion.Create)]
        public async Task CreateAsync(SaleVersionCreateInput input)
        {
            var saleVersion = await SaleVersionManager.CreateAsync(input.Name);
            if (input.Description.IsNotNullOrWhiteSpace())
                saleVersion.SetDescription(input.Description);
            saleVersion.SetPrice(input.Price);

            input.MapExtraPropertiesTo(saleVersion);
            await SaleVersionRepository.InsertAsync(saleVersion);
        }

        [Authorize(Policy = SysResourcePermissions.SaleVersion.Update)]
        public async Task UpdateAsync(Guid id, SaleVersionUpdateInput input)
        {
            var saleVersion = await SaleVersionRepository.GetAsync(id);

            await SaleVersionManager.ChangeNameAsync(saleVersion, input.Name);

            saleVersion.SetDescription(input.Description ?? "");
            saleVersion.SetPrice(input.Price);

            input.MapExtraPropertiesTo(saleVersion);
            await SaleVersionRepository.UpdateAsync(saleVersion);
        }


        public async Task<SaleVersionDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<SaleVersion, SaleVersionDto>(await SaleVersionRepository.GetAsync(id));
        }


        [Authorize(Policy = SysResourcePermissions.SaleVersion.Delete)]
        public Task DeleteAsync(Guid id)
        {
            return SaleVersionRepository.DeleteAsync(id);
        }

        [Authorize(Policy = SysResourcePermissions.SaleVersion.Default)]
        public async Task<PagedResult<SaleVersionDto>> QueryPageAsync(SaleVersionQuery input)
        {
            return await (await SaleVersionRepository.WithDetailsAsync()).ToPagedResultAsync<SaleVersion, SaleVersionDto>(input);
        }

        [Authorize(Policy = SysResourcePermissions.SaleVersion.Menu)]
        public async Task SetMenusAsync(Guid id, SaleVersionMenuEditInput input)
        {
            var saleVersion = await SaleVersionRepository.GetAsync(id);

            await SaleVersionManager.SetMenuAsync(saleVersion, input.AppId, input.MenuCodes);
            await SaleVersionRepository.UpdateAsync(saleVersion);
        }

        public async Task<ListResultDto<GrantApps>> GetAppsAysnc(Guid id)
        {
            var result = new List<GrantApps>();
            var apps = await SysAppRepository.GetListAsync();
            var selected = await SaleVersionManager.GetAppsAsync(id);

            result = apps.Select(a => new GrantApps
            {
                Id = a.Id,
                Label = a.AppName,
                IsCehcked = selected.Contains(a.Id)
            }).ToList();

            return new ListResultDto<GrantApps>(result);

        }


        public async Task<GrantMenus> GetMenuTreeSelected(Guid id, Guid appId)
        {
            List<SysMenuDto> menus = ObjectMapper.Map<List<SysMenu>, List<SysMenuDto>>(await SysMenuRepository.GetListAsync(m => m.IsEnable && m.SysAppId == appId));
            List<string> chekcedMenuCodes = await GetSelectedMenuCodesBySaleVersion(id, appId);

            List<MenuTreeLabel> tree = MenuTreeHelper.BuildMenuTreeLabel(menus);
            return new GrantMenus
            {
                CheckedKeys = chekcedMenuCodes,
                Menus = tree,
            };
        }


        #region 帮助方法

        /// <summary>
        /// 获取销售版本的应用菜单编码
        /// </summary>
        /// <returns></returns>

        protected virtual async Task<List<string>> GetSelectedMenuCodesBySaleVersion(Guid id, Guid? appId = null)
        {
            var menuCodes = (await SaleVersionManager.GetMenusAsync(id, appId))?.Select(a => a.MenuCode)?.ToList();

            //过滤掉禁用的菜单编码
            var menus = await SysMenuRepository.GetListAsync(m => m.IsEnable && menuCodes.Contains(m.Code));
            return menus.Select(m => m.Code)?.ToList();
        }
        #endregion
    }
}
