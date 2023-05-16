using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Http.Modeling;

namespace ONE.Abp.Cli
{
    public static class ONECliUrls
    {
        public const string WwwAbpIo = "https://abp.io/";

        public const string AccountAbpIo = "https://account.abp.io/";

        public const string NuGetRootPath = "https://nuget.abp.io/";

        public const string WwwAbpIoProduction = "https://abp.io/";

        public const string AccountAbpIoProduction = "https://account.abp.io/";

        public const string NuGetRootPathProduction = "https://nuget.abp.io/";

        public const string WwwAbpIoDevelopment = "https://localhost:44328/";

        public const string AccountAbpIoDevelopment = "https://localhost:44333/";

        public const string NuGetRootPathDevelopment = "https://localhost:44373/";

        public static string GetNuGetServiceIndexUrl(string apiKey)
        {
            return "https://nuget.abp.io/" + apiKey + "/v3/index.json";
        }

        public static string GetNuGetPackageInfoUrl(string apiKey, string packageId)
        {
            return "https://nuget.abp.io/" + apiKey + "/v3/package/" + packageId + "/index.json";
        }

        public static string GetApiDefinitionUrl(string url, ApplicationApiDescriptionModelRequestDto model = null)
        {
            url = url.EnsureEndsWith('/');
            return url + "api/abp/api-definition" + ((model == null) ? string.Empty : (model.IncludeTypes ? "?includeTypes=true" : string.Empty));
        }


        public const string ONEURL = "https://nuget.abp.io/";

        public static string[] ONETempletes = new string[2] { "base", "micro" };
       

        public static string GetRealUrl(string name)
        {
            return ONETempletes.Contains(name)? ONEURL : WwwAbpIo;
        }
    }
}
