using System.Collections.Generic;

namespace Volo.Abp.OpenIddict.Application.Contracts.Volo.Abp.OpenIddict.Applications
{

    public class OpenIddictApplicationCreateInput
    {
        public string ClienId { get; set; }

        public string Type { get; set; }


        public string ConsentType { get; set; }

        public string DisplayName { get; set; }


        public string Secret { get; set; }


        public List<string> GrantTypes { get; set; }
        public List<string> Scopes { get; set; }

        public string RedirectUri { get; set; }
        public string ClientUri { get; set; }
        public string PostLogoutRedirectUri { get; set; }
    }

    public class OpenIddictApplicationUpdateInput
    {
        public string Type { get; set; }


        public string ConsentType { get; set; }

        public string DisplayName { get; set; }


        public string Secret { get; set; }

        public List<string> GrantTypes { get; set; }
        public List<string> Scopes { get; set; }

        public string RedirectUri { get; set; }
        public string ClientUri { get; set; }
        public string PostLogoutRedirectUri { get; set; }
    }
}
