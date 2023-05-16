using Volo.Abp.Autofac;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace ONE.Abp.SysResource;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpSysResourceHttpApiClientModule),
    typeof(AbpHttpClientIdentityModelModule)
    )]
public class AbpSysResourceConsoleApiClientModule : AbpModule
{

}
