using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace ONE.Abp.DataDictionary;

[DependsOn(
    typeof(AbpDataDictionaryDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationModule)
    )]
public class AbpDataDictionaryApplicationContractsModule : AbpModule
{

}
