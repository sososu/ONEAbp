using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace ONE.Abp.SysResource.EntityFrameworkCore;

[DependsOn(
    typeof(AbpSysResourceDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class AbpSysResourceEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<SysResourceDbContext>(options =>
        {
            /* Add custom repositories here. Example:
             * options.AddRepository<Question, EfCoreQuestionRepository>();
             */
            options.AddDefaultRepositories<ISysResourceDbContext>();
        });
    }
}
