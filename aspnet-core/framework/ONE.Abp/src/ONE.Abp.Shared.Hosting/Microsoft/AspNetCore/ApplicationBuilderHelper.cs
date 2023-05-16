using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Volo.Abp.Modularity;

namespace ONE.Abp.Shared.Hosting.Microsoft.AspNetCore;

public static class ApplicationBuilderHelper
{
    public static async Task<WebApplication> BuildApplicationAsync<TStartupModule>(string[] args)
        where TStartupModule : IAbpModule
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Host
            .AddAppSettingsSecretsJson()
            .AddSerilogJson()
            .UseAutofac()
            .UseSerilog((context, loggerConfiguration) =>
             {
                 loggerConfiguration
                 .ReadFrom.Configuration(context.Configuration)
                 .Enrich.FromLogContext();
             },true);

        await builder.AddApplicationAsync<TStartupModule>();
        return builder.Build();
    }
}