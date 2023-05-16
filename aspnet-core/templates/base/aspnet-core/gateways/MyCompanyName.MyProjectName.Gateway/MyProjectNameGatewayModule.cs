using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Rewrite;
using ONE.Abp.Shared.Hosting.Gateway;
using ONE.Abp.Shared.Hosting.Microsoft.AspNetCore;
using Volo.Abp;
using Volo.Abp.Modularity;

namespace MyCompanyName.MyProjectName.Gateway
{
    [DependsOn(typeof(AbpSharedHostingGatewayModule))]
    public class MyProjectNameGatewayModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            SwaggerConfigurationHelper.ConfigureWithAuth(
                context: context,
                scopes: new
                    Dictionary<string, string> /* Requested scopes for authorization code request and descriptions for swagger UI only */
                    {
                    {"MyProjectName", "MyProjectName Service API"}
                    },
                apiTitle: "Gateway"
            );


            context.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .WithOrigins(
                            configuration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .WithAbpExposedHeaders()
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCorrelationId();
            app.UseAbpSerilogEnrichers();
            app.UseCors();
            app.UseSwaggerUIWithYarp(context);

            app.UseRewriter(new RewriteOptions()
                // Regex for "", "/" and "" (whitespace)
                .AddRedirect("^(|\\|\\s+)$", "/swagger"));

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapReverseProxy();
            });
        }
    }
}
