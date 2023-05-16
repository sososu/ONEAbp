using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace ONE.Abp.DataDictionary.EntityFrameworkCore;

[DependsOn(
    typeof(AbpDataDictionaryDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class AbpDataDictionaryEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<DataDictionaryDbContext>(options =>
        {
            options.AddDefaultRepositories();
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
        });
    }
}
