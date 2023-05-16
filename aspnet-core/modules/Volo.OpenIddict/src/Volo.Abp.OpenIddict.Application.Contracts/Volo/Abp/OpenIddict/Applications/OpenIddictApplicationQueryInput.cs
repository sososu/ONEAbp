using ONE.Abp.Pagination.Contracts.Dtos;

namespace Volo.Abp.OpenIddict.Application.Contracts.Volo.Abp.OpenIddict.Applications
{
    public class OpenIddictApplicationQueryInput:PagedQuery
    {
        public string ClientId { get; set; }
    }
}
