using ONE.Abp.Shared;
using ONE.Abp.SysResource.RoleMenus;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.Identity;

namespace ONE.Admin.EventHandler
{
    public class TenantCreatedEventHandler : IDistributedEventHandler<TenantCreatedEto>,
    ITransientDependency
    {
        protected IRoleMenuService RoleMenuService { get; }
        protected RolePermissionManagementProvider RolePermissionManagementProvider { get; }
        protected IPermissionGrantRepository PermissionGrantRepository { get; }
        protected ICurrentTenant CurrentTenant { get; }
        public TenantCreatedEventHandler(IRoleMenuService roleMenuService,
            IPermissionGrantRepository permissionGrantRepository,
            RolePermissionManagementProvider rolePermissionManagementProvider, ICurrentTenant currentTenant)
        {
            RoleMenuService = roleMenuService;
            RolePermissionManagementProvider = rolePermissionManagementProvider;
            PermissionGrantRepository = permissionGrantRepository;
            CurrentTenant = currentTenant;
        }

        public async Task HandleEventAsync(TenantCreatedEto eventData)
        {
            if (!Guid.TryParse(eventData.Properties["SaleVersionId"], out var saleVersionId))
                return;

            using (CurrentTenant.Change(eventData.Id))
            {
                var granteds = await RoleMenuService.GrantTenantAdminPermsToRoleAsync(eventData.Id, saleVersionId);


                var currentPermisGrants = await PermissionGrantRepository.GetListAsync(RolePermissionManagementProvider.Name, SpecialUserConsts.TenantAdminRole);
                var currentPermis = currentPermisGrants.Select(s => s.Name);

                var removePerms = currentPermis.Except(granteds.NewPerms);
                var addPerms = granteds.NewPerms.Except(currentPermis);

                if (removePerms != null)
                {
                    foreach (var perms in removePerms)
                    {
                        await RolePermissionManagementProvider.SetAsync(perms, SpecialUserConsts.TenantAdminRole, false);
                    }
                }

                if (addPerms != null)
                {
                    foreach (var perms in addPerms)
                    {
                        await RolePermissionManagementProvider.SetAsync(perms, SpecialUserConsts.TenantAdminRole, true);
                    }
                }
            }
        }
    }
}
