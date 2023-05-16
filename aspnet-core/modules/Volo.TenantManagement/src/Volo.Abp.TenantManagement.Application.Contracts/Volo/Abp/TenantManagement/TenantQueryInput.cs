using ONE.Abp.Pagination.Contracts.Attributes;
using ONE.Abp.Pagination.Contracts.Dtos;
using ONE.Abp.Shared;

namespace Volo.Abp.TenantManagement
{
    public class TenantQueryInput:PagedQuery
    {
        [Query(Compare =QueryCompare.Like)]
        public string Name { get; set; }
    }
}
