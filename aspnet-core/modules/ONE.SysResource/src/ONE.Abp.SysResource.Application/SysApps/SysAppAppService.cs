using Microsoft.AspNetCore.Authorization;
using ONE.Abp.Pagination.Contracts.Dtos;
using ONE.Abp.Shared.Utils;
using ONE.Abp.Pagination;
using ONE.Abp.SysResource.Permissions;
using System;
using System.Threading.Tasks;
using Volo.Abp.ObjectExtending;

namespace ONE.Abp.SysResource.SysApps
{
    [Authorize]
    public class SysAppAppService : SysResourceAppService, ISysAppAppService
    {
        protected ISysAppRepository SysAppRepository { get; }
        protected ISysAppManager SysAppManager { get; }

        public SysAppAppService(ISysAppRepository sysAppRepository, ISysAppManager sysAppManager)
        {
            SysAppRepository = sysAppRepository;
            SysAppManager = sysAppManager;
        }

        [Authorize(Policy = SysResourcePermissions.SysApp.Create)]
        public async Task CreateAsync(SysAppCreateInput input)
        {
            var sysApp = await SysAppManager.CreateAsync(input.AppCode);
            sysApp.SetBasicInfo(input.AppName,input.AppVersion);

            if (input.Description.IsNotNullOrWhiteSpace())
                sysApp.SetDescription(input.Description);

            if (input.AppSecret.IsNotNullOrWhiteSpace())
                sysApp.SetSecret(input.AppSecret);

            if (input.AppUrl.IsNotNullOrWhiteSpace())
                sysApp.SetUrl(input.AppUrl);

            input.MapExtraPropertiesTo(sysApp);
            await SysAppRepository.InsertAsync(sysApp);
        }

        [Authorize(Policy = SysResourcePermissions.SysApp.Update)]
        public async Task UpdateAsync(Guid id,SysAppUpdateInput input)
        {
            var sysApp = await SysAppRepository.GetAsync(id);

            await SysAppManager.ChangeCodeAsync(sysApp, input.AppCode);

            sysApp.SetBasicInfo(input.AppName, input.AppVersion);

            sysApp.SetDescription(input.Description??"");
            sysApp.SetSecret(input.AppSecret??"");
            sysApp.SetUrl(input.AppUrl??"");

            input.MapExtraPropertiesTo(sysApp);
            await SysAppRepository.UpdateAsync(sysApp);
        }


        public async Task<SysAppDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<SysApp, SysAppDto>(await SysAppRepository.GetAsync(id));
        }

        [Authorize(Policy = SysResourcePermissions.SysApp.Delete)]

        public Task DeleteAsync(Guid id)
        {
            return SysAppRepository.DeleteAsync(id);
        }

        [Authorize(Policy = SysResourcePermissions.SysApp.Default)]
        public async Task<PagedResult<SysAppDto>> QueryPageAsync(SysAppQuery input)
        {
            return await (await SysAppRepository.WithDetailsAsync()).ToPagedResultAsync<SysApp, SysAppDto>(input);
        }
    }
}
