using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace MyCompanyName.MyProjectName.Settings
{
    public interface IAccountSettingAppService : IApplicationService
    {
        public Task UpdateAsync(AccountSettingDto input);


        public Task<AccountSettingDto> GetAsync();
    }
}
