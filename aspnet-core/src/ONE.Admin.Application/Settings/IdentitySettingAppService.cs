using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Volo.Abp.Features;
using Volo.Abp.Identity.Settings;
using Volo.Abp.MultiTenancy;
using Volo.Abp.SettingManagement;

namespace ONE.Admin.Settings
{

    [Authorize(Permissions.SettingManagementPermissions.IdentitySettings.Default)]
    public class IdentitySettingAppService : AdminAppService, IIdentitySettingAppService
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
                //Lockout = new Lockout
                //{
                //    AllowedForNewUsers = Convert.ToBoolean(await SettingProvider.GetOrNullAsync(IdentitySettingNames.Lockout.AllowedForNewUsers)),
                //    LockoutDuration = Convert.ToInt32(await SettingProvider.GetOrNullAsync(IdentitySettingNames.Lockout.LockoutDuration)),
                //    MaxFailedAccessAttempts = Convert.ToInt32(await SettingProvider.GetOrNullAsync(IdentitySettingNames.Lockout.MaxFailedAccessAttempts)),
                //},
                //SignIn = new SignIn
                //{
                //    RequireConfirmedEmail = Convert.ToBoolean(await SettingProvider.GetOrNullAsync(IdentitySettingNames.SignIn.RequireConfirmedEmail)),
                //    EnablePhoneNumberConfirmation = Convert.ToBoolean(await SettingProvider.GetOrNullAsync(IdentitySettingNames.SignIn.EnablePhoneNumberConfirmation)),
                //    RequireConfirmedPhoneNumber = Convert.ToBoolean(await SettingProvider.GetOrNullAsync(IdentitySettingNames.SignIn.RequireConfirmedPhoneNumber)),
                //},

                //User = new User
                //{
                //    IsUserNameUpdateEnabled = Convert.ToBoolean(await SettingProvider.GetOrNullAsync(IdentitySettingNames.User.IsUserNameUpdateEnabled)),
                //    IsEmailUpdateEnabled = Convert.ToBoolean(await SettingProvider.GetOrNullAsync(IdentitySettingNames.User.IsEmailUpdateEnabled)),
                //}
            };

            //if (CurrentTenant.IsAvailable)
            //{
            //    settingsDto.Password = new Password
            //    {
            //        RequiredLength = Convert.ToInt32(await SettingManager.GetOrNullForTenantAsync(IdentitySettingNames.Password.RequiredLength, CurrentTenant.GetId(), false)),
            //        RequiredUniqueChars = Convert.ToInt32(await SettingManager.GetOrNullForTenantAsync(IdentitySettingNames.Password.RequiredUniqueChars, CurrentTenant.GetId(), false)),
            //        RequireNonAlphanumeric = Convert.ToBoolean(await SettingManager.GetOrNullForTenantAsync(IdentitySettingNames.Password.RequireNonAlphanumeric, CurrentTenant.GetId(), false)),
            //        RequireLowercase = Convert.ToBoolean(await SettingManager.GetOrNullForTenantAsync(IdentitySettingNames.Password.RequireLowercase, CurrentTenant.GetId(), false)),
            //        RequireUppercase = Convert.ToBoolean(await SettingManager.GetOrNullForTenantAsync(IdentitySettingNames.Password.RequireUppercase, CurrentTenant.GetId(), false)),
            //        RequireDigit = Convert.ToBoolean(await SettingManager.GetOrNullForTenantAsync(IdentitySettingNames.Password.RequireDigit, CurrentTenant.GetId(), false)),
            //    };
            //    //settingsDto.Lockout = new Lockout
            //    //{
            //    //    AllowedForNewUsers = Convert.ToBoolean(await SettingManager.GetOrNullForTenantAsync(IdentitySettingNames.Lockout.AllowedForNewUsers, CurrentTenant.GetId(), false)),
            //    //    LockoutDuration = Convert.ToInt32(await SettingManager.GetOrNullForTenantAsync(IdentitySettingNames.Lockout.LockoutDuration, CurrentTenant.GetId(), false)),
            //    //    MaxFailedAccessAttempts = Convert.ToInt32(await SettingManager.GetOrNullForTenantAsync(IdentitySettingNames.Lockout.MaxFailedAccessAttempts, CurrentTenant.GetId(), false)),
            //    //};
            //    //settingsDto.SignIn = new SignIn
            //    //{
            //    //    RequireConfirmedEmail = Convert.ToBoolean(await SettingManager.GetOrNullForTenantAsync(IdentitySettingNames.SignIn.RequireConfirmedEmail, CurrentTenant.GetId(), false)),
            //    //    EnablePhoneNumberConfirmation = Convert.ToBoolean(await SettingManager.GetOrNullForTenantAsync(IdentitySettingNames.SignIn.EnablePhoneNumberConfirmation, CurrentTenant.GetId(), false)),
            //    //    RequireConfirmedPhoneNumber = Convert.ToBoolean(await SettingManager.GetOrNullForTenantAsync(IdentitySettingNames.SignIn.RequireConfirmedPhoneNumber, CurrentTenant.GetId(), false)),
            //    //};

            //    //settingsDto.User = new User
            //    //{
            //    //    IsUserNameUpdateEnabled = Convert.ToBoolean(await SettingManager.GetOrNullForTenantAsync(IdentitySettingNames.User.IsUserNameUpdateEnabled, CurrentTenant.GetId(), false)),
            //    //    IsEmailUpdateEnabled = Convert.ToBoolean(await SettingManager.GetOrNullForTenantAsync(IdentitySettingNames.User.IsEmailUpdateEnabled, CurrentTenant.GetId(), false)),
            //    //};
            //}

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


            //await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, IdentitySettingNames.Lockout.AllowedForNewUsers, input.Lockout.AllowedForNewUsers.ToString());
            //await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, IdentitySettingNames.Lockout.LockoutDuration, input.Lockout.LockoutDuration.ToString());
            //await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, IdentitySettingNames.Lockout.MaxFailedAccessAttempts, input.Lockout.MaxFailedAccessAttempts.ToString());


            //await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, IdentitySettingNames.SignIn.RequireConfirmedEmail, input.SignIn.RequireConfirmedEmail.ToString());
            //await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, IdentitySettingNames.SignIn.EnablePhoneNumberConfirmation, input.SignIn.EnablePhoneNumberConfirmation.ToString());
            //await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, IdentitySettingNames.SignIn.RequireConfirmedPhoneNumber, input.SignIn.RequireConfirmedPhoneNumber.ToString());


            //await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, IdentitySettingNames.User.IsUserNameUpdateEnabled, input.User.IsUserNameUpdateEnabled.ToString());
            //await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, IdentitySettingNames.User.IsEmailUpdateEnabled, input.User.IsEmailUpdateEnabled.ToString());
        }


        protected virtual async Task CheckFeatureAsync()
        {
            await FeatureChecker.CheckEnabledAsync(SettingManagementFeatures.Enable);
        }
    }
}
