using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace ONE.Abp.Shared.Hosting.Microservice;

public static class JwtBearerConfigurationHelper
{
    public static void Configure(
        ServiceConfigurationContext context,string? authority=null,bool? requireHttpsMetadata=null,string? audience=null)
    {
        var configuration = context.Services.GetConfiguration();

        context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = authority??configuration["AuthServer:Authority"];
                options.RequireHttpsMetadata = requireHttpsMetadata??Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
                options.Audience = audience?? configuration["AuthServer:ApiName"];
            });
    }
}