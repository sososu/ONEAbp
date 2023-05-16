using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
namespace ONE.Abp.Shared.Hosting.Microsoft.AspNetCore
{
    public static class AbpHostingHostBuilderExtensions
    {
        public const string AppSerilogJsonPath = "serilog.json";

        public static IHostBuilder AddSerilogJson(
            this IHostBuilder hostBuilder,
            bool optional = true,
            bool reloadOnChange = true,
            string path = AppSerilogJsonPath)
        {
            return hostBuilder.ConfigureAppConfiguration((_, builder) =>
            {
                builder.AddJsonFile(
                        path: AppSerilogJsonPath,
                        optional: optional,
                        reloadOnChange: reloadOnChange
                    )
                    .AddEnvironmentVariables();
            });
        }
    }
}
