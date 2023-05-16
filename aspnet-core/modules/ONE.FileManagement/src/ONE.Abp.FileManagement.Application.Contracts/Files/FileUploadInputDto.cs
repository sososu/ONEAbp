using System.ComponentModel.DataAnnotations;
using Volo.Abp.Content;

namespace ONE.Abp.FileManagement.Files
{
    public class FileUploadInputDto
    {
        [Required]
        public IRemoteStreamContent File { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        public string Tag { get; set; } 
    }
}
