using ONE.Abp.Pagination.Contracts.Dtos;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.OpenIddict.Application.Contracts.Volo.Abp.OpenIddict.Applications;

namespace Volo.Abp.OpenIddict
{
    public interface IOpenIddictApplicationAppService : IApplicationService
    {
        public Task<PagedResult<OpenIddictApplicationDto>> PageAsync(OpenIddictApplicationQueryInput input);
               
        public Task CreateAsync(OpenIddictApplicationCreateInput input);
               
        public Task UpdateAsync(string clientId, OpenIddictApplicationUpdateInput input);
        public Task DeleteAsync(string clientId);
    }
}
