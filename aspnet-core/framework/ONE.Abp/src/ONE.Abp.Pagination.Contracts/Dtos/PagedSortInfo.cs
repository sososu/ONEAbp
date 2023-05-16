using System.Collections.Generic;

namespace ONE.Abp.Pagination.Contracts.Dtos
{
    /// <summary>
    /// 分页排序信息
    /// </summary>
    public class PagedSortInfo : IPagedSortInfo
    {
        /// <summary>
        /// 页号，从 1 开始
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 分页排序信息
        /// </summary>
        public PagedSortInfo()
        {
            SortFields = new List<string>();
        }

        /// <summary>
        /// 分页排序信息
        /// </summary>
        public PagedSortInfo(int pageIndex=1, int pageSize = 10, IEnumerable<string> sort = null)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;

            var sortFields = new List<string>();
            if (sort != null) sortFields.AddRange(sort);

            SortFields = sortFields;
        }

        /// <summary>
        /// 排序字段
        /// </summary>
        /// <example>['SortNo desc', 'CreateTime desc']</example>
        public IList<string> SortFields { get; set; }
    }
}
