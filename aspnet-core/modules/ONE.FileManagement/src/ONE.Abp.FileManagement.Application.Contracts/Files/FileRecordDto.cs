using System;
using Volo.Abp.Application.Dtos;

namespace ONE.Abp.FileManagement.Files
{
    public class FileRecordDto:CreationAuditedEntityDto<Guid>
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get;set; }

        /// <summary>
        /// 文件原名
        /// </summary>
        public string OriginalFileName { get;set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; set; }

    }
}
