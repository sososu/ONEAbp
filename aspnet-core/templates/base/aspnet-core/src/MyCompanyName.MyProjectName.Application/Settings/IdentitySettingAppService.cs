using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Volo.Abp.Features;
using Volo.Abp.Identity.Settings;
using Volo.Abp.SettingManagement;

namespace MyCompanyName.MyProjectName.Settings
{

    [Authorize(Permissions.SettingManagementPermissions.IdentitySettings.Default)]
    public class IdentitySettingAppService : MyProjectNameAppService, IIdentitySettingAppService
    {
        protected ISettingManager SettingManager { get; }

        public IdentitySettingAppService(ISettingManager settingManager)
        {
            SettingManager = settingManager;
        }

        public async Task<IdentitySettingDto> GetAsync()
        {
            await CheckFeatureAsync();

            var settingsDto = new IdentitySettingDto
            {
                Password = new Password
                {
                    RequiredLength = Convert.ToInt32(await SettingProvider.GetOrNullAsync(IdentitySettingNames.Password.RequiredLength)),
                    RequiredUniqueChars = Convert.ToInt32(await SettingProvider.GetOrNullAsync(IdentitySettingNames.Password.RequiredUniqueChars)),
                    RequireNonAlphanumeric = Convert.ToBoolean(await SettingProvider.GetOrNullAsync(IdentitySettingNames.Password.RequireNonAlphanumeric)),
                    RequireLowercase = Convert.ToBoolean(await SettingProvider.GetOrNullAsync(IdentitySettingNames.Password.RequireLowercase)),
                    RequireUppercase = Convert.ToBoolean(await SettingProvider.GetOrNullAsync(IdentitySettingNames.Password.RequireUppercase)),
                    RequireDigit = Convert.ToBoolean(await SettingProvider.GetOrNullAsync(IdentitySettingNames.Password.RequireDigit)),
                    DefaultPassword = await SettingProvider.GetOrNullAsync(IdentitySettingNames.Password.Default),
                },
               
            };

            return settingsDto;
        }


        [Authorize(Permissions.SettingManagementPermissions.IdentitySettings.Update)]
        public async Task UpdateAsync(IdentitySettingDto input)
        {
            await CheckFeatureAsync();

            await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, IdentitySettingNames.Password.RequiredLength, input.Password.RequiredLength.ToString());
            await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, IdentitySettingNames.Password.RequiredUniqueChars, input.Password.RequiredUniqueChars.ToString());
            await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, IdentitySettingNames.Password.RequireNonAlphanumeric, input.Password.RequireNonAlphanumeric.ToString());
            await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, IdentitySettingNames.Password.RequireLowercase, input.Password.RequireLowercase.ToString());
            await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, IdentitySettingNames.Password.RequireUppercase, input.Password.RequireUppercase.ToString());
            await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, IdentitySettingNames.Password.RequireDigit, input.Password.RequireDigit.ToString());
            await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, IdentitySettingNames.Password.Default, input.Password.DefaultPassword);
        }


        protected virtual async Task CheckFeatureAsync()
        {
            await FeatureChecker.CheckEnabledAsync(SettingManagementFeatures.Enable);
        }
    }
}
