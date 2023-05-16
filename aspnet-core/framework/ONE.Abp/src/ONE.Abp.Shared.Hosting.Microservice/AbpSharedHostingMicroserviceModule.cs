using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.Json;
using Volo.Abp.Modularity;

namespace ONE.Abp.Shared.Hosting.Microservice
{
    [DependsOn(
typeof(AbpAspNetCoreMultiTenancyModule),
typeof(AbpSharedHostingModule))]
    public class AbpSharedHostingMicroserviceModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            /* 阻止跨站点请求伪造
              https://docs.microsoft.com/zh-cn/aspnet/core/security/anti-request-forgery?view=aspnetcore-6.0
             */
            Configure<AbpAntiForgeryOptions>(options =>
            {
                options.AutoValidate = false;
            });

            context.Services.Configure<AbpJsonOptions>(options =>
            {
                options.InputDateTimeFormats = new List<string> { "yyyy-MM-dd HH:mm:ss", "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK" };
                options.OutputDateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            });

            ConfigureHealthChecks(context);
        }

    
        /// <summary>
        /// 健康检查
        /// </summary>
        /// <param name="context"></param>
        protected virtual void ConfigureHealthChecks(ServiceConfigurationContext context)
        {
            context.Services.AddHealthChecks();
        }
    }
}
