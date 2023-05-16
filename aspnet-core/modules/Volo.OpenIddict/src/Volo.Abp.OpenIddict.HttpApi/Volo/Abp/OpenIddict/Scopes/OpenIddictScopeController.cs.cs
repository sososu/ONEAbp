using Microsoft.AspNetCore.Mvc;
using ONE.Abp.Pagination.Contracts.Dtos;
using Volo.Abp.AspNetCore.Mvc;
namespace Volo.Abp.OpenIddict
{
    [RemoteService(Name = OpenIddictRemoteServiceConsts.RemoteServiceName)]
    [Area(OpenIddictRemoteServiceConsts.ModuleName)]
    [ControllerName("Scope")]
    [Route("api/openiddict/scope")]
    public class OpenIddictScopeController : AbpControllerBase, IOpenIddictScopeAppService
    {
        private readonly IOpenIddictScopeAppService _scopeAppService;
        public OpenIddictScopeController(IOpenIddictScopeAppService scopeAppService)
        {
            _scopeAppService = scopeAppService;
        }

        [HttpPost]
        public Task CreateAsync(OpenIddictScopeCreateInput input)
        {
            return _scopeAppService.CreateAsync(input); 
        }

        [HttpPut("{name}")]
        public Task UpdateAsync(string name,OpenIddictScopeUpdateInput input)
        {
            return _scopeAppService.UpdateAsync(name,input);
        }

        [HttpGet("page")]
        public Task<PagedResult<OpenIddictScopeDto>> PageAsync(OpenIddictScopeQueryInput input)
        {
            return _scopeAppService.PageAsync(input);
        }

        [HttpDelete]
        public Task DeleteAsync(string name)
        {
            return _scopeAppService.DeleteAsync(name);
        }
    }
}
