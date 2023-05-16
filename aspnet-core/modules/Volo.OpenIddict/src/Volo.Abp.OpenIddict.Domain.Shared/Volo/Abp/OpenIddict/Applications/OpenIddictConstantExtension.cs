using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volo.Abp.OpenIddict.Applications
{
    public static class OpenIddictConstantExtension
    {
        public static class GrantTypes
        {
            public const string Impersonation = "impersonation";
        }



        public static class Permissions
        {
            public const string Impersonation = "gt:impersonation";
        }
    }
}
