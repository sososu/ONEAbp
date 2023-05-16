using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ONE.Abp.FileManagement.Files
{
    public static class FileTypeHelper
    {
        public static List<string> FileTypes = new List<string>()
        {
           "文本",
           "文档",
           "图片",
           "音频",
           "视频",
           "未知"
        };

        static Dictionary<string, string[]> musicMIMEs = new Dictionary<string, string[]>
        {
            {"audio/mpeg", new [] {".mp3" } },
            {"audio/midi", new [] { ".mid", ".midi" } },
            {"audio/x-wav", new [] { ".wav" } },
            {"audio/x-mpegurl", new [] {".m3u" } },
            {"audio/x-m4a", new [] { ".m4a" } },
            {"audio/ogg", new [] { ".ogg" } },
            {"audio/x-realaudio", new [] { ".ra" } },
        };

        static Dictionary<string, string[]> videoMIMEs = new Dictionary<string, string[]>
        {
            {"video/mp4", new [] { ".mp4" } },
            {"video/mpeg", new [] { ".mpg", ".mpe", ".mpeg" } },
            {"video/quicktime", new [] { "qt", "mov" } },
            {"video/x-m4v", new [] { ".m4v" } },
            {"video/x-ms-wmv", new [] { ".wmv" } },
            {"video/x-msvideo", new [] { ".avi" } },
            {"video/webm", new [] { ".webm" } },
            {"video/x-flv", new [] { ".flv" } },
        };

        static Dictionary<string, string[]> imageMIMEs = new Dictionary<string, string[]>
        {
            {"image/gif", new [] { ".gif" } },
            {"image/jpeg", new [] { ".jpg", ".jpeg" } },
            {"image/jp2", new [] { ".jpg2" } },
            {"image/png", new [] { ".png" } },
            {"image/tiff", new [] { ".tiff" } },
            {"image/bmp", new [] { ".bmp" } },
            {"image/svg+xml", new [] { ".svg+xml" } },
            {"image/webp", new [] { ".webp" } },
            {"image/x-icon", new [] { ".x-icon" } }
        };

        static Dictionary<string, string[]> textMIMEs = new Dictionary<string, string[]>
        {
            {"application/msword", new [] { ".doc" } },
            {"application/vnd.openxmlformats-officedocument.wordprocessingml.document", new [] { ".docx"} },
            {"application/vnd.ms-excel", new [] { ".xls" } },
            {"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", new [] { ".xlsx" } },
            {"application/vnd.ms-powerpoint", new [] { ".ppt" } },
            {"application/vnd.openxmlformats-officedocument.presentationml.presentation", new [] { ".pptx" } },
            {"application/x-gzip", new [] { ".tgz", ".gzip" } },
            {"application/zip", new [] { ".zip", ".7zip" } },
            {"application/rar", new [] { ".rar" } },
            {"application/x-tar", new [] { ".tar", ".tgz" } },
            {"application/pdf", new [] { ".pdf" } },
            {"application/rtf", new [] { ".rtf" } },

            {"text/plain", new [] { ".txt" } },
            {"text/xml", new [] { ".xml" } },
            {"text/x-vcard", new [] { ".vcf" } },
            {"text/html", new [] { ".htm", ".html", ".shtml" } },
            {"text/css", new [] { ".css" } },
            {"text/javascript", new [] { ".js" } }
        };


        static Dictionary<string, string> ExtMIMEs;

        public static string GetMIMEByExt(this string ext)
        {
            ext = ext.ToLowerInvariant();
            if (ExtMIMEs== null)
            {
                CreateExtMIMES();
            }

            ext = ext.ToLowerInvariant();
            return ExtMIMEs.ContainsKey(ext) ? ExtMIMEs[ext] : "application/octet-stream";
        }

        public static string GetFileType(this string mime)
        {
            mime = mime.ToLower();

            var type = mime.Split('/')[0];


            switch (type)
            {
                case "text":
                    return "文本";

                case "application":
                    return "文档";

                case "image":
                    return "图片";

                case "audio":
                    return "音频";

                case "video":
                    return "视频";

                default:
                    return "未知";
            }
        }

        public static string GetDefaultExtByMIME(this string mime)
        {
            mime = mime.ToLowerInvariant();

            var type = mime.Split('/')[0];

            switch (type)
            {
                case "text":
                case "application":
                    return textMIMEs.ContainsKey(mime) ? textMIMEs[mime].FirstOrDefault() : "";

                case "image":
                    return imageMIMEs.ContainsKey(mime) ? imageMIMEs[mime].FirstOrDefault() : "";

                case "audio":
                    return musicMIMEs.ContainsKey(mime) ? musicMIMEs[mime].FirstOrDefault() : "";

                case "video":
                    return videoMIMEs.ContainsKey(mime) ? musicMIMEs[mime].FirstOrDefault() : "";

                default:
                    return "";
            }

        }


        private static void CreateExtMIMES()
        {
            ExtMIMEs = new Dictionary<string, string> { };
            var dics = new[] { musicMIMEs, videoMIMEs, imageMIMEs, textMIMEs };
            foreach (var dic in dics)
            {
                foreach (var item in dic)
                {
                    foreach (var ext in item.Value)
                    {
                        ExtMIMEs.Add(ext, item.Key);
                    }
                }
            }
        }

    }
}
