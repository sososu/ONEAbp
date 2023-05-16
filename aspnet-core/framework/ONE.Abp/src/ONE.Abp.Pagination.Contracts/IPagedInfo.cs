namespace ONE.Abp.Pagination.Contracts
{
    public interface IPagedInfo
    {
        /// <summary>
        /// 页号，从 1 开始
        /// </summary>
        int PageIndex { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        int PageSize { get; set; }
    }
}
