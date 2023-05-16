using ONE.Abp.Pagination.Contracts.Dtos;
namespace Volo.Abp.OpenIddict
{
    public class OpenIddictScopeQueryInput:PagedQuery
    {
        public string Name { get; set; }
    }
}
