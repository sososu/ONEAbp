using ONE.Abp.Pagination.Contracts.Dtos;
using OpenIddict.Abstractions;
using System.Diagnostics.CodeAnalysis;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.OpenIddict.Application.Contracts.Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Application.Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.PermissionManagement;

namespace Volo.Abp.OpenIddict
{
    public class OpenIddictApplicationAppService : AbpOpenIddictAppService, IOpenIddictApplicationAppService
    {
        private readonly IAbpApplicationManager _applicationManager;
        private readonly IPermissionDataSeeder _permissionDataSeeder;
        private readonly IOpenIddictApplicationRepository _applicationRepository;
        public OpenIddictApplicationAppService(IAbpApplicationManager applicationManager, IPermissionDataSeeder permissionDataSeeder, IOpenIddictApplicationRepository applicationRepository)
        {
            _applicationManager = applicationManager;
            _permissionDataSeeder = permissionDataSeeder;
            _applicationRepository = applicationRepository;
        }

        public async Task<PagedResult<OpenIddictApplicationDto>> PageAsync(OpenIddictApplicationQueryInput input)
        {
            var result = await _applicationRepository.GetListAsync(sorting: nameof(OpenIddictApplication.ClientId), (input.PageIndex - 1) * input.PageSize, input.PageSize, input.ClientId);
            if (result.Count < 1) { return new PagedResult<OpenIddictApplicationDto>(); }

            var total = await _applicationRepository.GetCountAsync(input.ClientId);

            var items = ObjectMapper.Map<List<OpenIddictApplicationModel>, List<OpenIddictApplicationDto>>(result.Select(x => x.ToModel()).ToList());
            return new PagedResult<OpenIddictApplicationDto>(input.PageIndex, input.PageSize, total, items);
        }

        public async Task CreateAsync(OpenIddictApplicationCreateInput input)
        {
            var client = await _applicationManager.FindByClientIdAsync(input.ClienId);
            if (client == null)
                return;

            var application = await CreateApplicationAsync(
                 name: input.ClienId,
                 type: input.Type,
                 consentType: input.ConsentType,
                 displayName: input.DisplayName,
                 secret: input.Secret,
                 grantTypes: input.GrantTypes,
                 scopes: input.Scopes,
                 redirectUri: input.RedirectUri,
                 clientUri: input.ClientUri,
                 postLogoutRedirectUri: input.PostLogoutRedirectUri
             );

            await _applicationManager.CreateAsync(application);
        }

        public async Task UpdateAsync(string clientId, OpenIddictApplicationUpdateInput input)
        {
            var client = await _applicationManager.FindByClientIdAsync(clientId);
            if (client == null)
                return;

            var application = await CreateApplicationAsync(
              name: clientId,
              type: input.Type,
              consentType: input.ConsentType,
              displayName: input.DisplayName,
              secret: input.Secret,
              grantTypes: input.GrantTypes,
              scopes: input.Scopes,
              redirectUri: input.RedirectUri,
              clientUri: input.ClientUri,
              postLogoutRedirectUri: input.PostLogoutRedirectUri
             );

            await _applicationManager.UpdateAsync(client, application);
        }

        public async Task DeleteAsync(string clientId)
        {
            var client = await _applicationManager.FindByClientIdAsync(clientId);
            if (client == null)
                return;
            await _applicationManager.DeleteAsync(client);
        }


        private async Task<AbpApplicationDescriptor> CreateApplicationAsync(
     [NotNull] string name,
     [NotNull] string type,
     [NotNull] string consentType,
     string displayName,
     string secret,
     List<string> grantTypes,
     List<string> scopes,
     string clientUri = null,
     string redirectUri = null,
     string postLogoutRedirectUri = null,
     List<string> permissions = null)
        {
            if (!string.IsNullOrEmpty(secret) && string.Equals(type, OpenIddictConstants.ClientTypes.Public, StringComparison.OrdinalIgnoreCase))
            {
                throw new BusinessException(L["NoClientSecretCanBeSetForPublicApplications"]);
            }

            if (string.IsNullOrEmpty(secret) && string.Equals(type, OpenIddictConstants.ClientTypes.Confidential, StringComparison.OrdinalIgnoreCase))
            {
                throw new BusinessException(L["TheClientSecretIsRequiredForConfidentialApplications"]);
            }

            //if (!string.IsNullOrEmpty(name) && await _applicationManager.FindByClientIdAsync(name) != null)
            //{
            //    return null;
            //    //throw new BusinessException(L["TheClientIdentifierIsAlreadyTakenByAnotherApplication"]);
            //}

            var application = new AbpApplicationDescriptor
            {
                ClientId = name,
                Type = type,
                ClientSecret = secret,
                ConsentType = consentType,
                DisplayName = displayName,
                ClientUri = clientUri,
            };

            Check.NotNullOrEmpty(grantTypes, nameof(grantTypes));
            Check.NotNullOrEmpty(scopes, nameof(scopes));

            if (new[] { OpenIddictConstants.GrantTypes.AuthorizationCode, OpenIddictConstants.GrantTypes.Implicit }.All(grantTypes.Contains))
            {
                application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeIdToken);

                if (string.Equals(type, OpenIddictConstants.ClientTypes.Public, StringComparison.OrdinalIgnoreCase))
                {
                    application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeIdTokenToken);
                    application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.CodeToken);
                }
            }

            if (!redirectUri.IsNullOrWhiteSpace() || !postLogoutRedirectUri.IsNullOrWhiteSpace())
            {
                application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Logout);
            }

            foreach (var grantType in grantTypes)
            {
                if (grantType == OpenIddictConstants.GrantTypes.AuthorizationCode)
                {
                    application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode);
                    application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.Code);
                }

                if (grantType == OpenIddictConstants.GrantTypes.AuthorizationCode || grantType == OpenIddictConstants.GrantTypes.Implicit)
                {
                    application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Authorization);
                }

                if (grantType == OpenIddictConstants.GrantTypes.AuthorizationCode ||
                    grantType == OpenIddictConstants.GrantTypes.ClientCredentials ||
                    grantType == OpenIddictConstants.GrantTypes.Password ||
                    grantType == OpenIddictConstants.GrantTypes.RefreshToken ||
                    grantType == OpenIddictConstants.GrantTypes.DeviceCode)
                {
                    application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Token);
                    application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Revocation);
                    application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Introspection);
                }

                if (grantType == OpenIddictConstants.GrantTypes.ClientCredentials)
                {
                    application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.ClientCredentials);
                }

                if (grantType == OpenIddictConstants.GrantTypes.Implicit)
                {
                    application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.Implicit);
                }

                if (grantType == OpenIddictConstants.GrantTypes.Password)
                {
                    application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.Password);
                }

                if (grantType == OpenIddictConstants.GrantTypes.RefreshToken)
                {
                    application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.RefreshToken);
                }

                if (grantType == OpenIddictConstants.GrantTypes.DeviceCode)
                {
                    application.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.DeviceCode);
                    application.Permissions.Add(OpenIddictConstants.Permissions.Endpoints.Device);
                }

                if (grantType == OpenIddictConstants.GrantTypes.Implicit)
                {
                    application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.IdToken);
                    if (string.Equals(type, OpenIddictConstants.ClientTypes.Public, StringComparison.OrdinalIgnoreCase))
                    {
                        application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.IdTokenToken);
                        application.Permissions.Add(OpenIddictConstants.Permissions.ResponseTypes.Token);
                    }
                }
                if (grantType == OpenIddictConstantExtension.GrantTypes.Impersonation)
                {
                    application.Permissions.Add(OpenIddictConstantExtension.Permissions.Impersonation);
                }
            }

            var buildInScopes = new[]
            {
                OpenIddictConstants.Permissions.Scopes.Address,
                OpenIddictConstants.Permissions.Scopes.Email,
                OpenIddictConstants.Permissions.Scopes.Phone,
                OpenIddictConstants.Permissions.Scopes.Profile,
                OpenIddictConstants.Permissions.Scopes.Roles
            };

            foreach (var scope in scopes)
            {
                if (buildInScopes.Contains(scope))
                {
                    application.Permissions.Add(scope);
                }
                else
                {
                    application.Permissions.Add(OpenIddictConstants.Permissions.Prefixes.Scope + scope);
                }
            }

            if (redirectUri != null)
            {
                if (!redirectUri.IsNullOrEmpty())
                {
                    if (!Uri.TryCreate(redirectUri, UriKind.Absolute, out var uri) || !uri.IsWellFormedOriginalString())
                    {
                        throw new BusinessException(L["InvalidRedirectUri", redirectUri]);
                    }

                    if (application.RedirectUris.All(x => x != uri))
                    {
                        application.RedirectUris.Add(uri);
                    }
                }
            }

            if (postLogoutRedirectUri != null)
            {
                if (!postLogoutRedirectUri.IsNullOrEmpty())
                {
                    if (!Uri.TryCreate(postLogoutRedirectUri, UriKind.Absolute, out var uri) || !uri.IsWellFormedOriginalString())
                    {
                        throw new BusinessException(L["InvalidPostLogoutRedirectUri", postLogoutRedirectUri]);
                    }

                    if (application.PostLogoutRedirectUris.All(x => x != uri))
                    {
                        application.PostLogoutRedirectUris.Add(uri);
                    }
                }
            }

            if (permissions != null)
            {
                await _permissionDataSeeder.SeedAsync(
                    ClientPermissionValueProvider.ProviderName,
                    name,
                    permissions,
                    null
                );
            }

            return application;
        }
    }
}
