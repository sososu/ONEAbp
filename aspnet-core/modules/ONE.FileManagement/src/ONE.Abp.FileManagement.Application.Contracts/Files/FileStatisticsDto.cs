using System.Collections.Generic;

namespace ONE.Abp.FileManagement.Files
{
    public class FileStatisticsDto
    {
        public List<FileTypeDetailStatistics> FileTypeDetailStatistics { get; set; }
      
        /// <summary>
        /// 总容量
        /// </summary>
        public long TotalSize { get; set; }

        /// <summary>
        /// 已用容量
        /// </summary>
        public long UseSize { get; set; }
    }

    public class FileTypeDetailStatistics
    {
        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType { get; set; }

        public int TotalCount { get; set; }
    }
}
