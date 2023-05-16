using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace MyCompanyName.MyProjectName.Settings
{
    public interface IFileSettingAppService : IApplicationService
    {
        public Task UpdateAsync(FileSettingDto input);


        public Task<FileSettingDto> GetAsync();
    }
}
