using Volo.Abp.Autofac;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Modularity;

namespace ONE.Abp.DataDictionary;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpDataDictionaryHttpApiClientModule),
    typeof(AbpHttpClientIdentityModelModule)
    )]
public class AbpDataDictionaryConsoleApiClientModule : AbpModule
{

}
