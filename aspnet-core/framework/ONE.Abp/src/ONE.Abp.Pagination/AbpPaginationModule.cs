using Microsoft.Extensions.DependencyInjection;
using ONE.Abp.Data;
using ONE.Abp.Pagination.Contracts;
using Volo.Abp;
using Volo.Abp.AutoMapper;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Modularity;

namespace ONE.Abp.Pagination
{
    [DependsOn(typeof(AbpAutoMapperModule),
         typeof(ONEAbpDataModule),
         typeof(AbpPaginationContractsModule))]
    public class AbpPaginationModule : AbpModule
    {

        /// <inheritdoc />
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            ServiceContainer.Set(context.ServiceProvider.GetRequiredService<ObjectAccessor<IServiceProvider>>().Value);
        }

    }
}
