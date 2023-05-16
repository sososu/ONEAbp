using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace ONE.Abp.DataPermission.EntityFrameworkCore;

[DependsOn(
    typeof(AbpDataPermissionDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class AbpDataPermissionEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<DataPermissionDbContext>(options =>
        {
            options.AddDefaultRepositories();
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, EfCoreQuestionRepository>();
                 */
        });
    }
}
