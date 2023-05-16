using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ONE.Abp.Data;
using ONE.Abp.Shared;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using OpenIddict.Server.AspNetCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.ExtensionGrantTypes;
using Volo.Abp.Security.Claims;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace MyCompanyName.MyProjectName
{
    public class ExchangeTokenExtensionGrant : ITokenExtensionGrant
    {
        public string Name => ExchangeTokenExtensionGrantConsts.GrantType;

        public async virtual Task<IActionResult> HandleAsync(ExtensionGrantContext context)
        {
            var token = context.Request.GetParameter(ExchangeTokenExtensionGrantConsts.AccessToken).ToString();
            if (token == null)
            {
                return new ForbidResult(
              new[] { OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
              properties: new AuthenticationProperties(new Dictionary<string, string>
              {
                  [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidRequest
              }!));
            }

            var transaction = await context.HttpContext.RequestServices.GetRequiredService<IOpenIddictServerFactory>().CreateTransactionAsync();
            transaction.EndpointType = OpenIddictServerEndpointType.Introspection;
            transaction.Request = new OpenIddictRequest
            {
                ClientId = context.Request.ClientId,
                ClientSecret = context.Request.ClientSecret,
                Token = token
            };

            var notification = new OpenIddictServerEvents.ProcessAuthenticationContext(transaction);
            var dispatcher = context.HttpContext.RequestServices.GetRequiredService<IOpenIddictServerDispatcher>();
            await dispatcher.DispatchAsync(notification);

            if (notification.IsRejected)
            {
                return new ForbidResult(
                    new[] { OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
                    properties: new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = notification.Error ?? OpenIddictConstants.Errors.InvalidRequest,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = notification.ErrorDescription,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorUri] = notification.ErrorUri
                    }));
            }

            var principal = notification.GenericTokenPrincipal;
            if (principal == null)
            {
                return new ForbidResult(
                    new[] { OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
                    properties: new AuthenticationProperties(new Dictionary<string, string>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = notification.Error ?? OpenIddictConstants.Errors.InvalidRequest,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = notification.ErrorDescription,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorUri] = notification.ErrorUri
                    }));
            }

            var userId = principal.FindUserId();
            var impersonatorUserId = principal.FindImpersonatorUserId();

            var userManager = context.HttpContext.RequestServices.GetRequiredService<IdentityUserManager>();
            var currentTenant = context.HttpContext.RequestServices.GetRequiredService<ICurrentTenant>();

            if (impersonatorUserId.HasValue)
            {
                using (currentTenant.Change(null))
                {
                    //back to root
                    var user = await userManager.GetByIdAsync(impersonatorUserId.Value);
                    //var users = await userManager.GetUsersInRoleAsync(SpecialUserConsts.SuperAdminRole);
                    //var user = users?.Where(u => u.UserName == SpecialUserConsts.SuperAdmin).FirstOrDefault();
                    if (user == null)
                    {
                        return new ForbidResult(
                            new[] { OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
                            properties: new AuthenticationProperties(new Dictionary<string, string>
                            {
                                [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                                [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The token is no longer valid."
                            }));
                    }


                    var userClaimsPrincipalFactory = context.HttpContext.RequestServices.GetRequiredService<IUserClaimsPrincipalFactory<IdentityUser>>();
                    var claimsPrincipal = await userClaimsPrincipalFactory.CreateAsync(user);
                    claimsPrincipal.SetScopes(principal.GetScopes());
                    claimsPrincipal.SetResources(await GetResourcesAsync(context, principal.GetScopes()));
                    await context.HttpContext.RequestServices.GetRequiredService<AbpOpenIddictClaimDestinationsManager>().SetAsync(claimsPrincipal);
                    return new Microsoft.AspNetCore.Mvc.SignInResult(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, claimsPrincipal);
                }
                //return await SetSuccessResultAsync(context, user);
            }

            var tenantIdStr = context.Request.GetParameter(AbpClaimTypes.TenantId)?.Value.ToString();
            if (!Guid.TryParse(tenantIdStr, out var tenantId))
                return new ForbidResult(
                            new[] { OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
                             properties: new AuthenticationProperties(new Dictionary<string, string>
                             {
                                 [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidRequest
                             }!));


            using (currentTenant.Change(tenantId))
            {
                // Retrieve the user profile corresponding to the authorization code/refresh token.
                // Note: if you want to automatically invalidate the authorization code/refresh token
                // when the user password/roles change, use the following line instead:
                // var user = _signInManager.ValidateSecurityStampAsync(info.Principal);

                var users = await userManager.GetUsersInRoleAsync(SpecialUserConsts.TenantAdminRole);
                var user = users?.Where(u => u.UserName == SpecialUserConsts.TenantAdmin).FirstOrDefault();
                if (user == null)
                {
                    return new ForbidResult(
                        new[] { OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
                        properties: new AuthenticationProperties(new Dictionary<string, string>
                        {
                            [OpenIddictServerAspNetCoreConstants.Properties.Error] = OpenIddictConstants.Errors.InvalidGrant,
                            [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user is no exist."
                        }));
                }



                var userClaimsPrincipalFactory = context.HttpContext.RequestServices.GetRequiredService<IUserClaimsPrincipalFactory<IdentityUser>>();
                var claimsPrincipal = await userClaimsPrincipalFactory.CreateAsync(user);
                claimsPrincipal.SetScopes(principal.GetScopes());
                claimsPrincipal.SetResources(await GetResourcesAsync(context, principal.GetScopes()));
                claimsPrincipal.AddClaim(AbpClaimTypes.ImpersonatorUserId, userId?.ToString());
                claimsPrincipal.AddClaim(AbpClaimTypes.ImpersonatorUserName, SpecialUserConsts.SuperAdmin);
                await context.HttpContext.RequestServices.GetRequiredService<AbpOpenIddictClaimDestinationsManager>().SetAsync(claimsPrincipal);
                return new Microsoft.AspNetCore.Mvc.SignInResult(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, claimsPrincipal);
            }
        }


        private async Task<IEnumerable<string>> GetResourcesAsync(ExtensionGrantContext context, ImmutableArray<string> scopes)
        {
            var resources = new List<string>();
            if (!scopes.Any())
            {
                return resources;
            }

            await foreach (var resource in context.HttpContext.RequestServices.GetRequiredService<IOpenIddictScopeManager>().ListResourcesAsync(scopes))
            {
                resources.Add(resource);
            }
            return resources;
        }

        //protected virtual async Task<IActionResult> SetSuccessResultAsync(ExtensionGrantContext context, IdentityUser user, Action<ClaimsPrincipal> action = null)
        //{
        //    Logger.LogInformation("Credentials validated for username: {username}", user.UserName);

        //    var principal = await SignInManager.CreateUserPrincipalAsync(user);

        //    principal.SetScopes(context.Request.GetScopes());
        //    principal.SetResources(await GetResourcesAsync(context.Request.GetScopes()));

        //    action?.Invoke(principal);

        //    await SetClaimsDestinationsAsync(principal);

        //    await SaveSecurityLogAsync(
        //        context,
        //        user,
        //        OpenIddictSecurityLogActionConsts.LoginSucceeded);

        //    return SignIn(principal, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        //}

        //protected async virtual Task SaveSecurityLogAsync(
        //    ExtensionGrantContext context,
        //    IdentityUser user,
        //    string action)
        //{
        //    var logContext = new IdentitySecurityLogContext
        //    {
        //        Identity = OpenIddictSecurityLogIdentityConsts.OpenIddict,
        //        Action = action,
        //        UserName = user.UserName,
        //        ClientId = await FindClientIdAsync(context)
        //    };
        //    logContext.WithProperty("GrantType", Name);

        //    await IdentitySecurityLogManager.SaveAsync(logContext);
        //}

        //protected virtual Task<string> FindClientIdAsync(ExtensionGrantContext context)
        //{
        //    return Task.FromResult(context.Request.ClientId);
        //}
    }
}
