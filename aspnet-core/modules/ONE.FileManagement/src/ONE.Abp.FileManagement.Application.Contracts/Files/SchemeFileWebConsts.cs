using System;

namespace ONE.Abp.FileManagement.Files
{
    public class SchemeFileWebConsts
    {
        public class FileUploading
        {
            /// <summary>
            /// Default value: 5242880
            /// </summary>
            public static int MaxFileSize { get; set; } = 5242880; //5MB
            public static long TotalMaxFileSize { get; set; } = 10995116277760; //10g

            public static int MaxFileSizeAsMegabytes => Convert.ToInt32((MaxFileSize / 1024f) / 1024f);
        }
    }
}