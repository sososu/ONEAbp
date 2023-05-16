using System.ComponentModel.DataAnnotations;
using Volo.Abp.Content;

namespace Volo.Abp.Account
{
    public class UpdateAdvarInput
    {
        [Required]
        public IRemoteStreamContent AvatarFile { get; set; }
    }
}
