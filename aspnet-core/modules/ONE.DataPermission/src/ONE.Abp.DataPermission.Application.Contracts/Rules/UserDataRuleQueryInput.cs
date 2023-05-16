using ONE.Abp.Pagination.Contracts.Attributes;
using ONE.Abp.Pagination.Contracts.Dtos;
using ONE.Abp.Shared;

namespace ONE.Abp.DataPermission.Rules
{
    public class UserDataRuleQueryInput : PagedQuery
    {
        /// <summary>
        /// 数据对象 指需要进行权限控制的数据实体，如表、视图等。
        /// </summary>
        [Query(Compare = QueryCompare.Equal)]
        public string DataTargetName { get; set; }
    }
}
