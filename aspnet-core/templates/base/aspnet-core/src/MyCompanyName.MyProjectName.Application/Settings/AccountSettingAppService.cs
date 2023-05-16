using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Volo.Abp.Account.Settings;
using Volo.Abp.Features;
using Volo.Abp.SettingManagement;

namespace MyCompanyName.MyProjectName.Settings
{
    [Authorize(Permissions.SettingManagementPermissions.AccountSettings.Default)]
    public class AccountSettingAppService : MyProjectNameAppService, IAccountSettingAppService
    {
        protected ISettingManager SettingManager { get; }

        public AccountSettingAppService(ISettingManager settingManager)
        {
            SettingManager = settingManager;
        }

        public async Task<AccountSettingDto> GetAsync()
        {
            await CheckFeatureAsync();

            var settingsDto = new AccountSettingDto
            {
                IsSelfRegistrationEnabled = Convert.ToBoolean(await SettingProvider.GetOrNullAsync(AccountSettingNames.IsSelfRegistrationEnabled)),
            };

            return settingsDto;
        }

        [Authorize(Permissions.SettingManagementPermissions.AccountSettings.Update)]
        public async Task UpdateAsync(AccountSettingDto input)
        {
            await CheckFeatureAsync();

            await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, AccountSettingNames.IsSelfRegistrationEnabled, input.IsSelfRegistrationEnabled.ToString());
        }


        protected virtual async Task CheckFeatureAsync()
        {
            await FeatureChecker.CheckEnabledAsync(SettingManagementFeatures.Enable);
        }
    }
}
