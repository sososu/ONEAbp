using ONE.Abp.SysResource.RoleMenus;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ONE.Admin.Integrations
{
    [IntegrationService]
    public interface IONEAppService:IApplicationService
    {
        /// <summary>
        /// 角色授权
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task GrantPermsToRoleAsync(RoleGrantingInput input);


        /// <summary>
        /// 加载对应角色应用列表
        /// </summary>
        /// <remarks>按应用名称排序</remarks>
        /// <param name="appId"></param>    
        /// <param name="role"></param>
        /// <returns></returns>
        Task<ListResultDto<GrantApps>> GetRoleAppsSelected(Guid roleId);

        /// <summary>
        /// 加载对应角色菜单列表树
        /// </summary>
        /// <param name="appId"></param>    
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<GrantMenus> GetRoleMenuTreeSelected(Guid appId, Guid roleId);


        /// <summary>
        /// 获取登录的路由信息
        /// </summary>
        /// <returns></returns>

       Task<ListResultDto<RouterVo>> GetRouters(string appCode);

        Task<ListResultDto<string>> GetSearchAsync(string? name=null);
    }
}
