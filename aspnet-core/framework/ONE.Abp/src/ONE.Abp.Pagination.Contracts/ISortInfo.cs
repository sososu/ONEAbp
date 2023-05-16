using System;
using System.Collections.Generic;
using System.Text;

namespace ONE.Abp.Pagination.Contracts
{
    /// <summary>
    /// 排序信息
    /// </summary>
    public interface ISortInfo
    {
        /// <summary>
        /// 排序字段
        /// </summary>
        /// <example>['SortNo desc', 'CreateTime desc']</example>
        IList<string> SortFields { get; set; }
    }
}
