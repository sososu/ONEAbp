using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ONE.Admin.Settings
{
    public interface IFileSettingAppService : IApplicationService
    {
        public Task UpdateAsync(FileSettingDto input);


        public Task<FileSettingDto> GetAsync();
    }
}
