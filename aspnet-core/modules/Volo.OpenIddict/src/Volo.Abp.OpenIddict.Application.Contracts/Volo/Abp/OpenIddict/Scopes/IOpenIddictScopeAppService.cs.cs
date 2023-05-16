using ONE.Abp.Pagination.Contracts.Dtos;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Abp.OpenIddict
{
    public interface IOpenIddictScopeAppService : IApplicationService
    {
        public Task CreateAsync(OpenIddictScopeCreateInput input);

        public Task UpdateAsync(string name, OpenIddictScopeUpdateInput input);

        public Task<PagedResult<OpenIddictScopeDto>> PageAsync(OpenIddictScopeQueryInput input);


        public Task DeleteAsync(string name);
    }
}
