using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Identity
{
    public enum Sex
    {
        [Display(Name ="未知")]
        Unkonw = 0,

        [Display(Name = "男")]
        Male,

        [Display(Name = "女")]
        Female
    }
}
