using Microsoft.AspNetCore.Mvc;
using MyCompanyName.MyProjectName.Settings;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Account;

namespace MyCompanyName.MyProjectName.Controllers
{
    [RemoteService(Name = AccountRemoteServiceConsts.RemoteServiceName)]
    [Area(AccountRemoteServiceConsts.ModuleName)]
    [ControllerName("Setting")] //控制器组名
    [Route("api/account/setting")]
    public class AccountSettingController : MyProjectNameController, IAccountSettingAppService  //实现接口IIdentityUserAppService 远程调用服务获取路由信息时需要看是否实现了该接口来判断。
    {
        protected IAccountSettingAppService SettingAppService { get; }

        public AccountSettingController(IAccountSettingAppService settingAppService)
        {
            SettingAppService = settingAppService;
        }

        /// <summary>
        /// 更新设置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        public Task UpdateAsync(AccountSettingDto input)
        {
            return SettingAppService.UpdateAsync(input);
        }

        /// <summary>
        /// 获取设置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public Task<AccountSettingDto> GetAsync()
        {
            return SettingAppService.GetAsync();
        }
    }
}
