using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ONE.Admin.Settings
{
    public interface IAccountSettingAppService : IApplicationService
    {
        public Task UpdateAsync(AccountSettingDto input);


        public Task<AccountSettingDto> GetAsync();
    }
}
