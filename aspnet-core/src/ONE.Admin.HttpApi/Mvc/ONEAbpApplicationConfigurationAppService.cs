using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using ONE.Abp.Data;
using ONE.Abp.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations.ObjectExtending;
using Volo.Abp.Authorization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Settings;
using Volo.Abp.Timing;
using Volo.Abp.Users;

namespace ONE.Admin.Mvc
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IAbpApplicationConfigurationAppService), typeof(AbpApplicationConfigurationAppService), typeof(ONEAbpApplicationConfigurationAppService))]
    public class ONEAbpApplicationConfigurationAppService : AbpApplicationConfigurationAppService
    {
        public ONEAbpApplicationConfigurationAppService(IOptions<AbpLocalizationOptions> localizationOptions, IOptions<AbpMultiTenancyOptions> multiTenancyOptions, IServiceProvider serviceProvider, IAbpAuthorizationPolicyProvider abpAuthorizationPolicyProvider, IPermissionDefinitionManager permissionDefinitionManager, DefaultAuthorizationPolicyProvider defaultAuthorizationPolicyProvider, IPermissionChecker permissionChecker, IAuthorizationService authorizationService, ICurrentUser currentUser, ISettingProvider settingProvider, ISettingDefinitionManager settingDefinitionManager, IFeatureDefinitionManager featureDefinitionManager, ILanguageProvider languageProvider, ITimezoneProvider timezoneProvider, IOptions<AbpClockOptions> abpClockOptions, ICachedObjectExtensionsDtoService cachedObjectExtensionsDtoService, IOptions<AbpApplicationConfigurationOptions> options) : base(localizationOptions, multiTenancyOptions, serviceProvider, abpAuthorizationPolicyProvider, permissionDefinitionManager, defaultAuthorizationPolicyProvider, permissionChecker, authorizationService, currentUser, settingProvider, settingDefinitionManager, featureDefinitionManager, languageProvider, timezoneProvider, abpClockOptions, cachedObjectExtensionsDtoService, options)
        {
        }

        protected async override Task<ApplicationAuthConfigurationDto> GetAuthConfigAsync()
        {
            if (CurrentTenant.GetMultiTenancySide() == MultiTenancySides.Host && CurrentUser.IsInRole(SpecialUserConsts.SuperAdminRole)) //超级管理员有所有权限
                return new ApplicationAuthConfigurationDto { GrantedPolicies = new Dictionary<string, bool> { { "*.*.*", true } } };

            return await base.GetAuthConfigAsync();
        }
    }
}
