using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ONE.Admin.Settings
{
    public interface IIdentitySettingAppService : IApplicationService
    {
        public Task UpdateAsync(IdentitySettingDto input);


        public Task<IdentitySettingDto> GetAsync();
    }
}
