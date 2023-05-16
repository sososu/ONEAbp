using Volo.Abp.Modularity;

namespace ONE.Abp.DataDictionary;

[DependsOn(
    typeof(AbpDataDictionaryApplicationModule),
    typeof(AbpDataDictionaryDomainTestModule)
    )]
public class AbpDataDictionaryApplicationTestModule : AbpModule
{

}
