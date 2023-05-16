using System.Security.Claims;
using System.Security.Principal;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Security.Claims;
using Volo.Abp.SimpleStateChecking;
using Volo.Abp.Threading;

namespace ONE.Abp.Shared.Hosting.Microservice
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(PermissionChecker), typeof(SuperAdminRolePermissionChecker), typeof(IPermissionChecker))]
    public class SuperAdminRolePermissionChecker : PermissionChecker, IPermissionChecker, ITransientDependency
    {
        public SuperAdminRolePermissionChecker(ICurrentPrincipalAccessor principalAccessor, IPermissionDefinitionManager permissionDefinitionManager, ICurrentTenant currentTenant, IPermissionValueProviderManager permissionValueProviderManager, ISimpleStateCheckerManager<PermissionDefinition> stateCheckerManager) : base(principalAccessor, permissionDefinitionManager, currentTenant, permissionValueProviderManager, stateCheckerManager)
        {
        }

        public override Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string name)
        {
            var roles = claimsPrincipal?.FindAll(AbpClaimTypes.Role).Select(c => c.Value).ToArray();

            if (roles != null && CurrentTenant.GetMultiTenancySide() == MultiTenancySides.Host && roles.Contains(SpecialUserConsts.SuperAdminRole))
            {
                return TaskCache.TrueResult;
            }

            return base.IsGrantedAsync(claimsPrincipal, name);
        }
    }
}
