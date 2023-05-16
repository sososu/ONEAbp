using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Volo.Abp.Account.Settings;
using Volo.Abp.Features;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SettingManagement;

namespace ONE.Admin.Settings
{
    [Authorize(Permissions.SettingManagementPermissions.AccountSettings.Default)]
    public class AccountSettingAppService : AdminAppService, IAccountSettingAppService
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
                //EnableLocalLogin = Convert.ToBoolean(await SettingProvider.GetOrNullAsync(AccountSettingNames.EnableLocalLogin)),
            };

            //if (CurrentTenant.IsAvailable)
            //{
            //    settingsDto.IsSelfRegistrationEnabled = Convert.ToBoolean(await SettingManager.GetOrNullForTenantAsync(AccountSettingNames.IsSelfRegistrationEnabled, CurrentTenant.GetId(), false));
            //    //settingsDto.EnableLocalLogin = Convert.ToBoolean(await SettingManager.GetOrNullForTenantAsync(AccountSettingNames.EnableLocalLogin, CurrentTenant.GetId(), false));
            //}

            return settingsDto;
        }

        [Authorize(Permissions.SettingManagementPermissions.AccountSettings.Update)]
        public async Task UpdateAsync(AccountSettingDto input)
        {
            await CheckFeatureAsync();

            await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, AccountSettingNames.IsSelfRegistrationEnabled, input.IsSelfRegistrationEnabled.ToString());
            //await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, AccountSettingNames.EnableLocalLogin, input.EnableLocalLogin.ToString());

        }


        protected virtual async Task CheckFeatureAsync()
        {
            await FeatureChecker.CheckEnabledAsync(SettingManagementFeatures.Enable);
        }
    }
}
