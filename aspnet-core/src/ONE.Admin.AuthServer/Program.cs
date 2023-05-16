using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ONE.Abp.Shared.Hosting.Microsoft.AspNetCore;
using Serilog;
using Serilog.Events;

namespace ONE.Admin;

public class Program
{
    public async static Task<int> Main(string[] args)
    {
        //        Log.Logger = new LoggerConfiguration()
        //#if DEBUG
        //            .MinimumLevel.Debug()
        //#else
        //            .MinimumLevel.Information()
        //#endif
        //            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        //            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
        //            .Enrich.FromLogContext()
        //            .WriteTo.Async(c => c.File("Logs/logs.txt"))
        //            .WriteTo.Async(c => c.Console())
        //            .CreateLogger();

        //        try
        //        {
        //            Log.Information("Starting ONE.Admin.AuthServer.");
        //            var builder = WebApplication.CreateBuilder(args);
        //            builder.Host.AddAppSettingsSecretsJson()
        //                .UseAutofac()
        //                .UseSerilog();
        //            await builder.AddApplicationAsync<AuthServerModule>();
        //            var app = builder.Build();
        //            await app.InitializeApplicationAsync();
        //            await app.RunAsync();
        //            return 0;
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Fatal(ex, "ONE.Admin.AuthServer terminated unexpectedly!");
        //            return 1;
        //        }
        //        finally
        //        {
        //            Log.CloseAndFlush();
        //        }
        var assemblyName = typeof(Program).Assembly.GetName().Name;
        SerilogConfigurationHelper.Configure(assemblyName);
        try
        {
            Log.Information($"Starting {assemblyName}.");
            var app = await ApplicationBuilderHelper
           .BuildApplicationAsync<AuthServerModule>(args);
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
