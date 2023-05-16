using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ONE.Abp.Data;
using ONE.Abp.Shared;
using ONE.Abp.Shared.Utils;
using ONE.Abp.SysResource.Menus;
using ONE.Abp.SysResource.Permissions;
using ONE.Abp.SysResource.SaleVersions;
using ONE.Abp.SysResource.SysApps;
using ONE.Abp.SysResource.SysMenus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;

namespace ONE.Abp.SysResource.RoleMenus
{
    public class RoleMenuService : SysResourceAppService, IRoleMenuService
    {
        protected ISysAppRepository SysAppRepository { get; }
        protected ISysMenuRepository SysMenuRepository { get; }
        protected IRepository<RoleMenu,Guid> RoleMenuRepository { get; }

        protected ISaleVersionRepository SaleVersionRepository { get; }

        protected ISaleVersionManager SaleVersionManager { get; }
        public RoleMenuService(ISysAppRepository sysAppRepository, ISysMenuRepository sysMenuRepository, IRepository<RoleMenu, Guid> roleMenuRepository, ISaleVersionRepository saleVersionRepository, ISaleVersionManager saleVersionManager)
        {
            SysAppRepository = sysAppRepository;
            SysMenuRepository = sysMenuRepository;
            RoleMenuRepository = roleMenuRepository;
            SaleVersionRepository = saleVersionRepository;
            SaleVersionManager = saleVersionManager;
        }


        /// <summary>
        /// 租户管理员角色授权
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        [Authorize]
        public async Task<GrantedPermsOutput> GrantTenantAdminPermsToRoleAsync(Guid tenantId,Guid saleVersionId)
        {
            var granted = new GrantedPermsOutput();

            //获取销售版本关联的菜单
            var saleMenus =await SaleVersionManager.GetMenusAsync(saleVersionId);

            var menuCodes = saleMenus.Select(m => m.MenuCode);

            var menus = await SysMenuRepository.GetListAsync(m=>menuCodes.Contains(m.Code));

            var menuCodeAppIdKV = menus.ToDictionary(m => m.Code, m => m.SysAppId);

            var formerCodes = await (await RoleMenuRepository.WithDetailsAsync())
                .Where(r=>r.TenantId== tenantId)
                .Where(r =>r.Role ==SpecialUserConsts.TenantAdminRole)
                .Select(r => r.MenuCode).ToListAsync();

            var removeCodes = formerCodes.Except(menuCodes);
            var addCodes = menuCodes.Except(formerCodes);

            //关联角色菜单 
            if (removeCodes.Any())
            {
                await RoleMenuRepository.DeleteAsync(r =>r.Role == SpecialUserConsts.TenantAdminRole && removeCodes.Contains(r.MenuCode));
                //granted.RemovePerms = menus.Where(m => removeCodes.Contains(m.Code)).Where(m => m.Perms.IsNotNullOrWhiteSpace()).Select(m => m.Perms).Distinct().ToList();
            }

            if (addCodes.Any())
            {
                var srms = new List<RoleMenu>();
                foreach (var code in addCodes)
                {
                    var srm = new RoleMenu(GuidGenerator.Create(), SpecialUserConsts.TenantAdminRole, menuCodeAppIdKV[code], code);
                    srms.Add(srm);
                }
                await RoleMenuRepository.InsertManyAsync(srms);

                //granted.AddPerms = menus.Where(m => addCodes.Contains(m.Code)).Where(m => m.Perms.IsNotNullOrWhiteSpace()).Select(m => m.Perms).Distinct().ToList();
            }

            granted.NewPerms= menus.Where(m => m.Perms.IsNotNullOrWhiteSpace()).Select(m => m.Perms).Distinct().ToList();

            return granted;
        }


        /// <summary>
        /// 角色授权
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        [Authorize(Policy = SysResourcePermissions.RoleMenu.Authorization)]
        public async Task<GrantedPermsOutput> GrantPermsToRoleAsync(RoleMenuCreateInput input)
        {
            var granted = new GrantedPermsOutput();

            //获取当前登录用户的菜单


            var menus =await GetMyMenus(input.AppId);
            var menuCodes = menus.Select(m => m.Code);
            if (input.MenuCodes.Any() && !input.MenuCodes.All(menuCodes.Contains))
                 throw new BusinessException(SysResourceErrorCodes.OutOfRangeOfAuthorization);


            var formerCodes = await (await RoleMenuRepository.WithDetailsAsync()).Where(r => r.AppId == input.AppId && r.Role == input.Role).Select(r => r.MenuCode).ToListAsync();
            var removeCodes = formerCodes.Except(input.MenuCodes);
            var addCodes = input.MenuCodes.Except(formerCodes);

            //关联角色菜单 
            if (removeCodes.Any())
            {
                await RoleMenuRepository.DeleteAsync(r => r.AppId == input.AppId && r.Role == input.Role && removeCodes.Contains(r.MenuCode));
                //granted.RemovePerms= menus.Where(m => removeCodes.Contains(m.Code)).Where(m => m.Perms.IsNotNullOrWhiteSpace()).Select(m => m.Perms).ToList();
               
            }

            if (addCodes.Any())
            {
                var srms = new List<RoleMenu>();
                foreach (var code in addCodes)
                {
                    var srm = new RoleMenu(GuidGenerator.Create(), input.Role, input.AppId, code);
                    srms.Add(srm);
                }
                await RoleMenuRepository.InsertManyAsync(srms);

                //granted.AddPerms = menus.Where(m => addCodes.Contains(m.Code)).Where(m => m.Perms.IsNotNullOrWhiteSpace()).Select(m => m.Perms).ToList();
            }

            granted.NewPerms = menus.Where(m=>input.MenuCodes.Contains(m.Code)).Where(m => m.Perms.IsNotNullOrWhiteSpace()).Select(m => m.Perms).Distinct().ToList();

            return granted;
        }


        /// <summary>
        /// 加载对应角色应用列表
        /// </summary>
        /// <remarks>按应用名称排序</remarks>
        /// <param name="appId"></param>    
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<ListResultDto<GrantApps>> GetRoleAppsSelected(string role)
        {
            var apps = await GetMyApps();
            var checkAppIds = await GetAppIdByRole(role);
            var grantApps= apps.Select(a => new GrantApps { Id = a.Id, Label = a.AppName, IsCehcked = checkAppIds.Contains(a.Id) }).OrderBy(a => a.Label).ToList();
            return new ListResultDto<GrantApps>(grantApps);
        }


        /// <summary>
        /// 加载对应角色菜单列表树
        /// </summary>
        /// <remarks>按菜单顺序排序</remarks>
        /// <param name="appId"></param>    
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<GrantMenus> GetRoleMenuTreeSelected(Guid appId, string role)
        {
            List<SysMenuDto> menus = await GetMyMenus(appId);
            List<string> chekcedMenuCodes = await GetMenuCodesByRole(role, appId);

            List<MenuTreeLabel> tree = MenuTreeHelper.BuildMenuTreeLabel(menus);
            return new GrantMenus
            {
                CheckedKeys = chekcedMenuCodes,
                Menus = tree,
            };
        }


        /// <summary>
        /// 获取登录的路由信息
        /// </summary>
        /// <returns></returns>

        [Authorize]
        public async Task<ListResultDto<RouterVo>> GetRouters(string appCode)
        {
            var menus = await GetLoginMenusExceptBtn(appCode);
            var trees = MenuTreeHelper.BuildMenuTree(menus.Items.ToList(),ObjectMapper);
            return new ListResultDto<RouterVo>(BuildRouters(trees));
        }


        /// <summary>
        /// 根据应用编码获取我的登录菜单
        /// </summary>
        /// <remarks>排除按钮</remarks>
        /// <returns></returns>

        [Authorize]
        public async Task<ListResultDto<SysMenuDto>> GetLoginMenusExceptBtn(string appCode)
        {
            var app = await SysAppRepository.FindByCodeAsync(appCode);
            if (app == null)
                throw new BusinessException(SysResourceErrorCodes.NotExistSysAppCode).WithData("appCode", appCode);

            return await GetLoginMenusExceptBtn(app.Id);
        }

        /// <summary>
        /// 根据应用Id获取我的登录菜单
        /// </summary>
        /// <remarks>排除按钮</remarks>
        /// <returns></returns>
        [Authorize]
        public virtual async Task<ListResultDto<SysMenuDto>> GetLoginMenusExceptBtn(Guid appId)
        {
            var menus = await GetMyMenus(appId);
            return new ListResultDto<SysMenuDto>(menus.Where(m => m.MenuType != MenuType.F).ToList());
        }


        /// <summary>
        /// 获取我的应用菜单
        /// </summary>
        /// <remarks>包括按钮</remarks>
        /// <param name="appId"></param>
        /// <returns></returns>
        protected virtual async Task<List<SysMenuDto>> GetMyMenus(Guid appId)
        {
            //宿主机超级管理员拥有所有菜单
            if (CurrentTenant.GetMultiTenancySide() == MultiTenancySides.Host && CurrentUser.IsInRole(SpecialUserConsts.SuperAdminRole))
                return ObjectMapper.Map<List<SysMenu>, List<SysMenuDto>>(await SysMenuRepository.GetListAsync(m => m.IsEnable && m.SysAppId == appId));

            //其他用户
            var menuCodes = await (await RoleMenuRepository.WithDetailsAsync())
                .Where(r => r.AppId == appId)
                .Where(r => CurrentUser.Roles.Contains(r.Role))
                .Select(r => r.MenuCode).ToListAsync();

            var menus = (await SysMenuRepository.GetListAsync(m => menuCodes.Contains(m.Code))).Where(m => m.IsEnable).ToList();

            return ObjectMapper.Map<List<SysMenu>, List<SysMenuDto>>(menus);
        }


        /// <summary>
        /// 获取我的应用
        /// </summary>
        /// <remarks>包括按钮</remarks>
        /// <param name="appId"></param>
        /// <returns></returns>
        protected virtual async Task<List<SysAppDto>> GetMyApps()
        {
            //宿主机超级管理员拥有所有应用
            if (CurrentTenant.GetMultiTenancySide() == MultiTenancySides.Host && CurrentUser.IsInRole(SpecialUserConsts.SuperAdminRole))
               return ObjectMapper.Map<List<SysApp>, List<SysAppDto>>(await SysAppRepository.GetListAsync());

            //其他用户
            var menuCodes = await (await RoleMenuRepository.WithDetailsAsync())
                .Where(r => CurrentUser.Roles.Contains(r.Role))
                .Select(r => r.MenuCode).ToListAsync();

            var menus = (await SysMenuRepository.GetListAsync(m => menuCodes.Contains(m.Code))).Where(m => m.IsEnable).ToList();
            var appIds= menus.Select(m => m.SysAppId).Distinct().ToList();

            return ObjectMapper.Map<List<SysApp>, List<SysAppDto>>(await SysAppRepository.GetListAsync(s=>appIds.Contains(s.Id)));
        }


        /// <summary>
        /// 获取角色的菜单编码
        /// </summary>
        /// <returns></returns>

        protected virtual async Task<List<string>> GetMenuCodesByRole(string role, Guid? appId = null)
        {
            var menuCodes = await (await RoleMenuRepository.WithDetailsAsync())
                .Where(r =>role == r.Role)
                .WhereIf(appId.HasValue,r=> r.AppId == appId.Value)
                .Select(r => r.MenuCode).ToListAsync();

            //过滤掉禁用的菜单编码
            var menus = await SysMenuRepository.GetListAsync(m =>m.IsEnable && menuCodes.Contains(m.Code));
            return menus.Select(m => m.Code).ToList();
        }


        /// <summary>
        /// 获取角色的应用
        /// </summary>
        /// <returns></returns>

        protected virtual async Task<List<Guid>> GetAppIdByRole(string role)
        {
            var menuCodes = await (await RoleMenuRepository.WithDetailsAsync())
                .Where(r => role == r.Role)
                .Select(r => r.MenuCode).ToListAsync();

            //过滤掉禁用的菜单编码
            var menus = await SysMenuRepository.GetListAsync(m => m.IsEnable && menuCodes.Contains(m.Code));
            return  menus.Select(m => m.SysAppId).Distinct().ToList();
        }

        #region 私有方法

        private List<RouterVo> BuildRouters(IList<MenuTree> menus)
        {
            List<RouterVo> routers = new List<RouterVo>();
            foreach (var menu in menus)
            {
                RouterVo router = new RouterVo();
                router.Hidden = !menu.Visible;
                router.Name =RouterVoHelper.GetRouteName(menu);
                router.Path = RouterVoHelper.GetRouterPath(menu);
                router.Component = RouterVoHelper.GetComponent(menu);
                router.Query = menu.Query;


                router.Meta = new MetaVo(menu.Name, menu.Icon, !menu.IsCache, menu.Path);
                IList<MenuTree> cMenus = menu.Children;
                if (cMenus != null && cMenus.Any() && menu.MenuType == MenuType.M)
                {
                    router.AlwaysShow = true;
                    router.Redirect = "noRedirect";
                    router.Children = BuildRouters(cMenus);
                }
                else if (RouterVoHelper.IsMenuFrame(menu))
                {
                    router.Meta = null;
                    List<RouterVo> childrenList = new List<RouterVo>();
                    RouterVo children = new RouterVo();
                    children.Path = menu.Path;
                    children.Component = menu.Component;
                    children.Name = menu.Path.UpperFirstChar();
                    children.Meta = new MetaVo(menu.Name, menu.Icon, !menu.IsCache, menu.Path);
                    children.Query = menu.Query;
                    childrenList.Add(children);
                    router.Children = childrenList;
                }
                else if (menu.ParentCode.IsNullOrWhiteSpace() && RouterVoHelper.IsInnerLink(menu))
                {
                    router.Meta = new MetaVo(menu.Name, menu.Icon);
                    router.Path = "/";
                    List<RouterVo> childrenList = new List<RouterVo>();
                    RouterVo children = new RouterVo();
                    var routerPath = RouterVoHelper.InnerLinkReplaceEach(menu.Path);
                    children.Path = routerPath;
                    children.Component = "InnerLink";
                    children.Name = routerPath.UpperFirstChar();
                    children.Meta = new MetaVo(menu.Name, menu.Icon, menu.Path);
                    childrenList.Add(children);
                    router.Children = childrenList;
                }
                routers.Add(router);
            }
            return routers;
        }
     
        #endregion
    }
}
