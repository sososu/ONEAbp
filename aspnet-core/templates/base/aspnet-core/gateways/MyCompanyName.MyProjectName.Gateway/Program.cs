using MyCompanyName.MyProjectName.Gateway;
using ONE.Abp.Shared.Hosting.Gateway;
using ONE.Abp.Shared.Hosting.Microsoft.AspNetCore;
using Serilog;
public class Program
{
    public async static Task<int> Main(string[] args)
    {
        var assemblyName = typeof(Program).Assembly.GetName().Name;
        SerilogConfigurationHelper.Configure(assemblyName);
        try
        {
            Log.Information($"Starting {assemblyName}.");
            var builder = WebApplication.CreateBuilder(args);
            builder.Host
                .AddAppSettingsSecretsJson()
                .AddSerilogJson()
                .AddYarpJson()
                .UseAutofac()
            .UseSerilog((context, loggerConfiguration) =>
            {
                loggerConfiguration
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext();
            }, true);

            await builder.AddApplicationAsync<MyProjectNameGatewayModule>();
            var app = builder.Build();
            await app.InitializeApplicationAsync();
            await app.RunAsync();

            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, $"{assemblyName} terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
