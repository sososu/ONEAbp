using Microsoft.Extensions.Options;
using ONE.Abp.Data;
using ONE.Abp.Data.Rules;
using ONE.Abp.Shared;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;

namespace ONE.Admin
{
    public class ONEAbpClaimsPrincipalContributor : IAbpClaimsPrincipalContributor, ITransientDependency
    {
        private readonly IdentityUserManager _userManager;
        private readonly AbpRuleOptions _ruleOptions;
        public ONEAbpClaimsPrincipalContributor(IdentityUserManager userManager, IOptions<AbpRuleOptions> ruleOptions)
        { _userManager = userManager; _ruleOptions = ruleOptions.Value; }

        public async Task ContributeAsync(AbpClaimsPrincipalContributorContext context)
        {
            var identity = context.ClaimsPrincipal.Identities.First();

            var sub = identity.Claims.First(x => x.Type == AbpClaimTypes.UserId).Value;
            var user = await _userManager.FindByIdAsync(sub);

            var extraFields = _ruleOptions.RuleExtraFieldManager.GetRuleExtraFieldForClaims();

            if (extraFields.Any())
            {
                foreach (var item in extraFields)
                {
                    if(item.ClaimName== ONEAbpClaimTypes.OrganizationCode) //组织编码
                    {
                        var organization = await _userManager.GetOrganizationUnitsAsync(user);

                        if (organization != null && organization.Any())
                        {
                            identity.AddIfNotContains(new Claim(item.ClaimName, organization.FirstOrDefault().Code));
                        }
                    }
                    else
                    {
                        if (user.ExtraProperties.ContainsKey(item.ExtraPropertyName))
                        {
                            identity.AddIfNotContains(new Claim(item.ClaimName, user.ExtraProperties[item.ExtraPropertyName].ToString()));
                        }
                    }
                }
            }
        }
    }
}
