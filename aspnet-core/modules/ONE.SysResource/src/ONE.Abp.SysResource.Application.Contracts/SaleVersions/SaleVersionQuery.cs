using ONE.Abp.Pagination.Contracts.Attributes;
using ONE.Abp.Pagination.Contracts.Dtos;
using ONE.Abp.Shared;

namespace ONE.Abp.SysResource.SaleVersions
{
    public class SaleVersionQuery:PagedQuery
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Query(Compare =QueryCompare.Like)]
        public string Name { get; set; }
    }
}
