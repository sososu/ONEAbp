using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace ONE.Abp.DataPermission;

[DependsOn(
    typeof(AbpDataPermissionModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpDataPermissionDomainModule),
    typeof(AbpDataPermissionApplicationContractsModule),
    typeof(AbpDddApplicationModule)
    )]
public class AbpDataPermissionApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<AbpDataPermissionApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<AbpDataPermissionApplicationModule>(validate: true);
        });
    }
}
