using ONE.Abp.Pagination.Contracts.Attributes;
using ONE.Abp.Pagination.Contracts.Dtos;
using ONE.Abp.Shared;

namespace Volo.Abp.Identity
{
    /// <summary>
    /// 角色查询
    /// </summary>
    public class QueryRoleInput:PagedQuery
    {
        /// <summary>
        /// 角色名
        /// </summary>
        [Query(Compare =QueryCompare.Like)]
        public string Name { get; set; }
    }
}
