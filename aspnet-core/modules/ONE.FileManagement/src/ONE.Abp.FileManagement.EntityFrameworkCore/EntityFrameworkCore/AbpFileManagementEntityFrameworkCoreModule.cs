using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace ONE.Abp.FileManagement.EntityFrameworkCore;

[DependsOn(
    typeof(AbpFileManagementDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class AbpFileManagementEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<FileManagementDbContext>(options =>
        {
            /* Add custom repositories here. Example:
             * options.AddRepository<Question, EfCoreQuestionRepository>();
             */
            options.AddDefaultRepositories<IFileManagementDbContext>();
        });
    }
}
