using Microsoft.AspNetCore.Http;
using Serilog.Events;

namespace ONE.Abp.Shared.Hosting.Microsoft.AspNetCore
{
    public static class LogLevelHelper
    {

        public static LogEventLevel GetLevel(HttpContext ctx, double elapsedMS, Exception ex)
        {
            var path = ctx.Request.Path;
            switch (path)
            {
                case "/":
                case "/health":
                    return LogEventLevel.Debug;
            }

            return ex != null || ctx.Response.StatusCode > 499 ?
                LogEventLevel.Error :
                LogEventLevel.Information;
        }
    }
}
