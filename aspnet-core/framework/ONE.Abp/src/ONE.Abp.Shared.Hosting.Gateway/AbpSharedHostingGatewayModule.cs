using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace ONE.Abp.Shared.Hosting.Gateway
{
    [DependsOn(
    typeof(AbpSharedHostingModule)
)]
    public class AbpSharedHostingGatewayModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            context.Services.AddReverseProxy()
                .LoadFromConfig(configuration.GetSection("ReverseProxy"));
        }
    }
}
