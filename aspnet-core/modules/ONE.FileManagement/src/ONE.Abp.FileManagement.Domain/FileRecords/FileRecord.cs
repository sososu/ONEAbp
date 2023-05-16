using JetBrains.Annotations;
using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ONE.Abp.FileManagement.FileRecords
{
    /// <summary>
    /// 文件记录
    /// </summary>
    public class FileRecord : CreationAuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        [CanBeNull]
        public string FilePath { get; protected set; }

        /// <summary>
        /// 文件原名
        /// </summary>
        public string OriginalFileName { get; protected set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; protected set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        [CanBeNull]
        public string FileType { get; protected set; }

        /// <summary>
        /// MIME类型
        /// </summary>
        [CanBeNull]
        public string MimeType { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileSize { get; protected set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; set; }

        public Guid? TenantId { get; set; }

        protected FileRecord()
        {

        }
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public FileRecord(
            Guid id,
            string filePath,
            string originalFileName,
            string fileName,
            string fileType,
            string mimeType,
            long fileSize
            )
        {
            Id= id;
            FilePath = filePath;
            OriginalFileName = originalFileName;
            FileName = fileName;
            FileType = fileType;
            MimeType = mimeType;
            FileSize = fileSize;
        }
        #endregion
    }
}
