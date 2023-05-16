using ONE.Abp.Data.DataDics;
using ONE.Abp.DataDictionary.DataItems;
using ONE.Abp.DataDictionary.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace ONE.Abp.DataDictionary;

/* Domain tests are configured to use the EF Core provider.
 * You can switch to MongoDB, however your domain tests should be
 * database independent anyway.
 */
[DependsOn(
    typeof(AbpDataDictionaryEntityFrameworkCoreTestModule)
    )]
public class AbpDataDictionaryDomainTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpEnumDicOption>(option => { option.Add<DataItemStatus>("状态"); option.Add<DataItemSource>("来源"); });
    }
}
