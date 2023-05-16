using ONE.Abp.Pagination.Contracts.Attributes;
using ONE.Abp.Pagination.Contracts.Dtos;
using ONE.Abp.Shared;

namespace ONE.Abp.DataPermission.Rules
{
    public class DataTargetQueryInput:PagedQuery
    {
        /// <summary>
        /// 对象名
        /// </summary>
        [Query(Compare =QueryCompare.Like)]
        public string Name { get; set; }

    }
}
