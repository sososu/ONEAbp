using ONE.Abp.SysResource.SysMenus;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ONE.Abp.SysResource.RoleMenus
{
    public interface IRoleMenuService : IApplicationService
    {
        Task<GrantedPermsOutput> GrantTenantAdminPermsToRoleAsync(Guid tenantId, Guid saleVersionId);
        Task<GrantedPermsOutput> GrantPermsToRoleAsync(RoleMenuCreateInput input);
        Task<ListResultDto<GrantApps>> GetRoleAppsSelected(string role);
        Task<GrantMenus> GetRoleMenuTreeSelected(Guid appId, string role);
        Task<ListResultDto<RouterVo>> GetRouters(string appCode);
        Task<ListResultDto<SysMenuDto>> GetLoginMenusExceptBtn(string appCode);


        Task<ListResultDto<SysMenuDto>> GetLoginMenusExceptBtn(Guid appId);

    }
}
