using Microsoft.AspNetCore.Mvc;
using ONE.Admin.Integrations;
using ONE.Abp.SysResource.RoleMenus;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace ONE.Admin.Controllers
{
    /// <summary>
    /// ONE服务
    /// </summary>
    [Route("api")]
    public class ONEController : AdminController, IONEAppService
    {
        protected IONEAppService ONEAppService { get; }

        public ONEController(IONEAppService oNEAppService)
        {
            ONEAppService = oNEAppService;
        }

        /// <summary>
        /// 获取路由
        /// </summary>
        /// <param name="appCode"></param>
        /// <returns></returns>
        [HttpGet("routers")]
        public Task<ListResultDto<RouterVo>> GetRouters(string appCode)
        {
            return ONEAppService.GetRouters(appCode);
        }

        /// <summary>
        /// 获取选择角色应用
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("select/role/apps")]
        public Task<ListResultDto<GrantApps>> GetRoleAppsSelected(Guid roleId)
        {
            return ONEAppService.GetRoleAppsSelected(roleId);
        }

        /// <summary>
        /// 获取选择角色菜单树
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("select/role/menu-tree")]
        public Task<GrantMenus> GetRoleMenuTreeSelected(Guid appId, Guid roleId)
        {
            return ONEAppService.GetRoleMenuTreeSelected(appId, roleId);
        }


        /// <summary>
        /// 选择角色授权
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPost("select/role/grant")]
        public Task GrantPermsToRoleAsync(RoleGrantingInput input)
        {
            return ONEAppService.GrantPermsToRoleAsync(input);
        }

        /// <summary>
        /// 搜索权限资源(前缀匹配)
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("perms/search")]
        public Task<ListResultDto<string>> GetSearchAsync(string? name = null)
        {
            return ONEAppService.GetSearchAsync(name);
        }
    }
}
