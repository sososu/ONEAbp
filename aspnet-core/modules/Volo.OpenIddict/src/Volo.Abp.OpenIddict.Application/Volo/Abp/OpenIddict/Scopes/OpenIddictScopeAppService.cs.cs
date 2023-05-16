using ONE.Abp.Pagination.Contracts.Dtos;
using OpenIddict.Abstractions;
using Volo.Abp.OpenIddict.Application.Volo.Abp.OpenIddict;
using Volo.Abp.OpenIddict.Scopes;
namespace Volo.Abp.OpenIddict
{
    public class OpenIddictScopeAppService : AbpOpenIddictAppService, IOpenIddictScopeAppService
    {
        private readonly IOpenIddictScopeManager _scopeManager;
        private readonly IOpenIddictScopeRepository _scopeRepository;
        public OpenIddictScopeAppService(IOpenIddictScopeManager scopeManager, IOpenIddictScopeRepository scopeRepository)
        {
            _scopeManager = scopeManager;
            _scopeRepository = scopeRepository;
        }

        public async Task CreateAsync(OpenIddictScopeCreateInput input)
        {
            if (await _scopeManager.FindByNameAsync(input.Name) != null)
                return;


            await _scopeManager.CreateAsync(new OpenIddictScopeDescriptor
            {
                Name = input.Name,
                DisplayName = input.DisplayName,
                Resources =
                {
                    input.Resources
                }
            });
        }

        public async Task UpdateAsync(string name, OpenIddictScopeUpdateInput input)
        {
            var scope = await _scopeManager.FindByNameAsync(name);
            if (scope == null)
                return;

            await _scopeManager.UpdateAsync(scope, new OpenIddictScopeDescriptor
            {
                Name = name,
                DisplayName = input.DisplayName,
                Resources =
                {
                    input.Resources
                }
            });
        }

        public async Task<PagedResult<OpenIddictScopeDto>> PageAsync(OpenIddictScopeQueryInput input)
        {
            var result = await _scopeRepository.GetListAsync(sorting: nameof(OpenIddictScope.Name), (input.PageIndex- 1)*input.PageSize, input.PageSize,input.Name);
            if (result.Count < 1) { return new PagedResult<OpenIddictScopeDto>(); }

            var total = await _scopeRepository.GetCountAsync(input.Name);

            var items = ObjectMapper.Map<List<OpenIddictScopeModel>, List<OpenIddictScopeDto>>(result.Select(x => x.ToModel()).ToList());
            return new PagedResult<OpenIddictScopeDto>(input.PageIndex,input.PageSize, total,items);
        }


        public async Task DeleteAsync(string name)
        {
            var scope = await _scopeManager.FindByNameAsync(name);
            if (scope == null)
                return;

            await _scopeManager.DeleteAsync(scope);
        }
    }
}
