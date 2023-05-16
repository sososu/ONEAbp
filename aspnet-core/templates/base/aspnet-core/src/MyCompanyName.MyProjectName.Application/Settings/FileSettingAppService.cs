using Microsoft.AspNetCore.Authorization;
using ONE.Abp.FileManagement.Settings;
using System;
using System.Threading.Tasks;
using Volo.Abp.Features;
using Volo.Abp.SettingManagement;

namespace MyCompanyName.MyProjectName.Settings
{

    [Authorize(Permissions.SettingManagementPermissions.FileSettings.Default)]
    public class FileSettingAppService : MyProjectNameAppService, IFileSettingAppService
    {
        protected ISettingManager SettingManager { get; }

        public FileSettingAppService(ISettingManager settingManager)
        {
            SettingManager = settingManager;
        }

        public async Task<FileSettingDto> GetAsync()
        {
            await CheckFeatureAsync();

            var settingsDto = new FileSettingDto
            {
                LimitSize = Convert.ToInt64(await SettingProvider.GetOrNullAsync(FileManagementSettings.LimitSizeSettingName)),
                TotalLimitSize = Convert.ToInt64(await SettingProvider.GetOrNullAsync(FileManagementSettings.TotalLimitSizeSettingName)),
                SupportMimeType = await SettingProvider.GetOrNullAsync(FileManagementSettings.SupportMimeType),
            };

            return settingsDto;
        }

        [Authorize(Permissions.SettingManagementPermissions.FileSettings.Update)]
        public async Task UpdateAsync(FileSettingDto input)
        {
            await CheckFeatureAsync();

            await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, FileManagementSettings.LimitSizeSettingName, input.LimitSize.ToString());
            await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, FileManagementSettings.TotalLimitSizeSettingName, input.TotalLimitSize.ToString());
            await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, FileManagementSettings.SupportMimeType, input.SupportMimeType?.ToString()??"");

        }


        protected virtual async Task CheckFeatureAsync()
        {
            await FeatureChecker.CheckEnabledAsync(SettingManagementFeatures.Enable);
        }
    }
}
