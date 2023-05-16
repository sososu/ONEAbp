using Volo.Abp.Autofac;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace ONE.Abp.DataPermission;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpDataPermissionHttpApiClientModule),
    typeof(AbpHttpClientIdentityModelModule)
    )]
public class AbpDataPermissionConsoleApiClientModule : AbpModule
{

}
