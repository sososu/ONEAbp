using ONE.Abp.Data;
using ONE.Abp.Shared;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;

namespace MyCompanyName.MyProjectName.Identity
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IDataSeedContributor), typeof(PermissionDataSeedContributor), typeof(ONEPermissionDataSeedContributor))]
    public class ONEPermissionDataSeedContributor : PermissionDataSeedContributor
    {
        public ONEPermissionDataSeedContributor(IPermissionDefinitionManager permissionDefinitionManager, IPermissionDataSeeder permissionDataSeeder, ICurrentTenant currentTenant) : base(permissionDefinitionManager, permissionDataSeeder, currentTenant)
        {
        }

        public override async Task SeedAsync(DataSeedContext context)
        {
            var multiTenancySide = CurrentTenant.GetMultiTenancySide();

            if(multiTenancySide==MultiTenancySides.Host)  //宿主机超级管理员不需要显式关联权限
                { return; }

            var permissionNames = (await PermissionDefinitionManager.GetPermissionsAsync())
                .Where(p => p.MultiTenancySide.HasFlag(multiTenancySide))
                .Where(p => !p.Providers.Any() || p.Providers.Contains(RolePermissionValueProvider.ProviderName))
                .Select(p => p.Name)
                .ToArray();

            await PermissionDataSeeder.SeedAsync(
                RolePermissionValueProvider.ProviderName,
                SpecialUserConsts.TenantAdminRole,
                permissionNames,
                context?.TenantId
            );
        }
    }
}
