using ONE.Abp.Pagination.Contracts.Attributes;
using ONE.Abp.Pagination.Contracts.Dtos;
using ONE.Abp.Shared;

namespace ONE.Abp.DataPermission.Rules
{
    public class DataRuleQueryInput : PagedQuery
    {
        /// <summary>
        /// 用户规则名称
        /// </summary>
        [Query(Compare = QueryCompare.Like)]
        public string Name { get; set; }

        /// <summary>
        /// 数据对象 指需要进行权限控制的数据实体，如表、视图等。
        /// </summary>
        [Query(Compare = QueryCompare.Equal)]
        public string DataTargetName { get; set; }
    }
}
