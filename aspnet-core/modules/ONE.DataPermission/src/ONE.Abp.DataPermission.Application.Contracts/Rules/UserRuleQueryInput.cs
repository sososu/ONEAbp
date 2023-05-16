using ONE.Abp.Pagination.Contracts.Attributes;
using ONE.Abp.Pagination.Contracts.Dtos;
using ONE.Abp.Shared;

namespace ONE.Abp.DataPermission.Rules
{
    public class UserRuleQueryInput:PagedQuery
    {
        /// <summary>
        /// 用户规则名称
        /// </summary>
        [Query(Compare =QueryCompare.Like)]
        public string Name { get; set; }
    }
}
