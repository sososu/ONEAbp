using ONE.Abp.Pagination.Contracts.Attributes;
using ONE.Abp.Pagination.Contracts.Dtos;
using ONE.Abp.Shared;
using System;

namespace ONE.Abp.FileManagement.Files
{
    public class FileQueryInput : PagedQuery
    {
        [Query(Compare =QueryCompare.Like,OrGroup ="name")]
        public string OriginalFileName { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        [Query(Compare =QueryCompare.Like, OrGroup = "name")]
        public string FileName { get; set; }

        [Query(Compare =QueryCompare.Like)]
        public string Tag { get; set; }

        
        [Query("CreationTime",Compare = QueryCompare.GreaterThanOrEqual)]
        public DateTime? Start { get; set; }

        [Query("CreationTime", Compare = QueryCompare.LessThanOrEqual)]
        public DateTime? End { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        [Query(Compare = QueryCompare.Equal)]
        public string FileType { get;  set; }

    }
}
