using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Autofac;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Client.IdentityModel;
using Volo.Abp.Json;
using Volo.Abp.Modularity;

namespace ONE.Admin.HttpApi.Client.ConsoleTestApp;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AdminHttpApiClientModule),
    typeof(AbpHttpClientIdentityModelModule)
    )]
public class AdminConsoleApiClientModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<AbpHttpClientBuilderOptions>(options =>
        {
            options.ProxyClientBuildActions.Add((remoteServiceName, clientBuilder) =>
            {
                clientBuilder.AddTransientHttpErrorPolicy(
                    policyBuilder => policyBuilder.WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(Math.Pow(2, i)))
                );
            });
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.Configure<AbpJsonOptions>(options =>
        {
            options.InputDateTimeFormats = new List<string> { "yyyy-MM-dd HH:mm:ss", "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK" };
            options.OutputDateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        });
    }
}
