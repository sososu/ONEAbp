using Microsoft.Extensions.DependencyInjection;
using ONE.Abp.DataPermission;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace ONE.Abp.DataPermission;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpDataPermissionModule),
    typeof(AbpDataPermissionDomainSharedModule)
)]
public class AbpDataPermissionDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<AbpDataPermissionDomainModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<AbpDataPermissionDomainModule>(validate: true);
        });
    }
}
