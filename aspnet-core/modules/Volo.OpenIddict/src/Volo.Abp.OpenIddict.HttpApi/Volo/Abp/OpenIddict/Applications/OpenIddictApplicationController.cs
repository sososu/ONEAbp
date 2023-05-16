using Microsoft.AspNetCore.Mvc;
using ONE.Abp.Pagination.Contracts.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.OpenIddict.Application.Contracts.Volo.Abp.OpenIddict.Applications;

namespace Volo.Abp.OpenIddict
{
    [RemoteService(Name = OpenIddictRemoteServiceConsts.RemoteServiceName)]
    [Area(OpenIddictRemoteServiceConsts.ModuleName)]
    [ControllerName("Application")]
    [Route("api/openiddict/application")]
    public class OpenIddictApplicationController : AbpControllerBase, IOpenIddictApplicationAppService
    {
        private readonly IOpenIddictApplicationAppService _appService;
        public OpenIddictApplicationController(IOpenIddictApplicationAppService appService)
        {
            _appService = appService;
        }

        [HttpPost]
        public Task CreateAsync(OpenIddictApplicationCreateInput input)
        {
            return _appService.CreateAsync(input);
        }
      
        [HttpPut("{clientId}")]
        public Task UpdateAsync(string clientId, OpenIddictApplicationUpdateInput input)
        {
            return _appService.UpdateAsync(clientId, input);
        }
      
        [HttpGet("page")]
        public Task<PagedResult<OpenIddictApplicationDto>> PageAsync(OpenIddictApplicationQueryInput input)
        {
            return _appService.PageAsync(input);
        }

        [HttpDelete]
        public Task DeleteAsync(string name)
        {
            return _appService.DeleteAsync(name);
        }
    }
}
