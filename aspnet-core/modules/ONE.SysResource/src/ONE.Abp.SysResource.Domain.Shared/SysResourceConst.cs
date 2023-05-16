namespace ONE.Abp.SysResource
{
    public static class SysResourceConst
    {
        /// <summary>
        /// Default value: 64
        /// </summary>
        public static int MaxNameLength { get; set; } = 64;


        /// <summary>
        /// Default value: 256
        /// </summary>
        public static int MaxStrLength { get; set; } = 256;

        /// <summary>
        /// http请求
        /// </summary>
        public static string HTTP = "http://";

        /// <summary>
        ///  https请求
        /// </summary>
        public static string HTTPS = "https://";

        /// <summary>
        ///  www主域
        /// </summary>
        public static string WWW = "www.";
    }
}
