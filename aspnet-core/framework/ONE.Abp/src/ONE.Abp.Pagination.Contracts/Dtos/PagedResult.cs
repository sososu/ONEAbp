using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace ONE.Abp.Pagination.Contracts.Dtos
{
    /// <summary>
    /// Implements <see cref="IPagedResult{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of the items in the <see cref="ListResultDto{T}.Items"/> list</typeparam>
    [Serializable]
    public class PagedResult<T> : ListResultDto<T>, IPagedResult<T>,IPagedInfo
    {
        /// <inheritdoc />
        public long TotalCount { get; set; } //TODO: Can be a long value..?

        /// <summary>
        /// 当前页
        /// </summary>
        /// <example>1</example>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页大小
        /// </summary>
        /// <example>30</example>
        public int PageSize { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        /// <example>5</example>
        public int TotalPage => PageSize > 0 ? (int)Math.Ceiling((decimal)TotalCount / PageSize) : 0;


        public PagedResult()
        {

        }

        /// <summary>
        /// Creates a new <see cref="PagedResultDto{T}"/> object.
        /// </summary>
        /// <param name="totalCount">Total count of Items</param>
        /// <param name="items">List of items in current page</param>
        public PagedResult(int pageInex,int pageSize,long totalCount, IReadOnlyList<T> items)
            : base(items)
        {
            PageIndex = pageInex;
            PageSize = pageSize;
            TotalCount = totalCount;
        }
    }


    /// <summary>
    /// Implements <see cref="IPagedResult{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of the items in the <see cref="ListResultDto{T}.Items"/> list</typeparam>
    [Serializable]
    public class ExtensiblePagedResult<T> : ExtensibleListResultDto<T>, IPagedResult<T>, IPagedInfo
    {

        /// <summary>
        /// 当前页
        /// </summary>
        /// <example>1</example>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页大小
        /// </summary>
        /// <example>30</example>
        public int PageSize { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        /// <example>5</example>
        public int TotalPage => PageSize > 0 ? (int)Math.Ceiling((decimal)TotalCount / PageSize) : 0;

        /// <inheritdoc />
        public long TotalCount { get; set; } //TODO: Can be a long value..?

        /// <summary>
        /// Creates a new <see cref="PagedResultDto{T}"/> object.
        /// </summary>
        public ExtensiblePagedResult()
        {

        }

        /// <summary>
        /// Creates a new <see cref="PagedResultDto{T}"/> object.
        /// </summary>
        /// <param name="totalCount">Total count of Items</param>
        /// <param name="items">List of items in current page</param>
        public ExtensiblePagedResult(int pageInex, int pageSize, long totalCount, IReadOnlyList<T> items)
            : base(items)
        {
            PageIndex = pageInex;
            PageSize = pageSize;
            TotalCount = totalCount;
        }
    }

}
