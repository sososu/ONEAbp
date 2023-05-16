using Microsoft.AspNetCore.Mvc;
using ONE.Admin.Settings;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Identity;

namespace ONE.Admin.Controllers
{
    [RemoteService(Name = IdentityRemoteServiceConsts.RemoteServiceName)] //给参数.BingSource 自动推断绑定类型
    [Area(IdentityRemoteServiceConsts.ModuleName)] //module名字
    [ControllerName("Setting")] //控制器组名
    [Route("api/identity/setting")]
    public class IdentitySettingController : AdminController, IIdentitySettingAppService
    {
        protected IIdentitySettingAppService SettingAppService { get; }

        public IdentitySettingController(IIdentitySettingAppService settingAppService)
        {
            SettingAppService = settingAppService;
        }

        /// <summary>
        /// 更新设置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public Task UpdateAsync(IdentitySettingDto input)
        {
            return SettingAppService.UpdateAsync(input);
        }

        /// <summary>
        /// 获取设置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<IdentitySettingDto> GetAsync()
        {
            return SettingAppService.GetAsync();
        }
    }
}
