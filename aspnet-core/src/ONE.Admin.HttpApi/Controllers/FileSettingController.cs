using Microsoft.AspNetCore.Mvc;
using ONE.Admin.Settings;
using ONE.Abp.FileManagement;
using System.Threading.Tasks;
using Volo.Abp;

namespace ONE.Admin.Controllers
{
    [RemoteService(Name = FileManagementRemoteServiceConsts.RemoteServiceName)]
    [Area(FileManagementRemoteServiceConsts.ModuleName)]
    [ControllerName("Setting")] //控制器组名
    [Route("api/file/setting")]
    public class FileSettingController : AdminController, IFileSettingAppService  //实现接口IIdentityUserAppService 远程调用服务获取路由信息时需要看是否实现了该接口来判断。
    {
        protected IFileSettingAppService SettingAppService { get; }

        public FileSettingController(IFileSettingAppService settingAppService)
        {
            SettingAppService = settingAppService;
        }

        /// <summary>
        /// 更新设置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public Task UpdateAsync(FileSettingDto input)
        {
            return SettingAppService.UpdateAsync(input);
        }

        /// <summary>
        /// 获取设置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<FileSettingDto> GetAsync()
        {
            return SettingAppService.GetAsync();
        }
    }
}
