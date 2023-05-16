using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Volo.Abp.Modularity;

namespace ONE.Abp.Shared.Hosting.Microsoft.AspNetCore;

public static class SwaggerConfigurationHelper
{
    public static void Configure(
        ServiceConfigurationContext context,
        string apiTitle
    )
    {
        context.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = apiTitle, Version = "v1" });
            options.DocInclusionPredicate((docName, description) => true);
            options.CustomSchemaIds(type => type.FullName);
            var xmls = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
            foreach (var xml in xmls)
            {
                options.IncludeXmlComments(xml, true);
            }
        });
    }
    public static void ConfigureWithAuth(
        ServiceConfigurationContext context,
        Dictionary<string, string> scopes,
        string apiTitle,
        string apiVersion = "v1",
        string apiName = "v1",
        string? authority=null
    )
    {
        var configuration = context.Services.GetConfiguration();
        context.Services.AddAbpSwaggerGenWithOAuth(
            authority: authority ?? configuration["AuthServer:Authority"],
            scopes: scopes,
            options =>
            {
                options.SwaggerDoc(apiName, new OpenApiInfo { Title = apiTitle, Version = apiVersion });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
                var xmls = Directory.GetFiles(AppContext.BaseDirectory, "*.xml");
                foreach (var xml in xmls)
                {
                    options.IncludeXmlComments(xml, true);
                }
            });
    }
}