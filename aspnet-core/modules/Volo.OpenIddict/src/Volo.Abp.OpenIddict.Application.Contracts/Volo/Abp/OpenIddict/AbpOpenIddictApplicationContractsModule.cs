using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace Volo.Abp.OpenIddict
{

    [DependsOn(
        typeof(AbpOpenIddictDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
        )]
    public class AbpOpenIddictApplicationContractsModule : AbpModule
    {
       
    }
}