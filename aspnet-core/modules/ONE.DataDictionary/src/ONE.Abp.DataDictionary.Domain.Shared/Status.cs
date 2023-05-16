using System.ComponentModel.DataAnnotations;

namespace ONE.Abp.DataDictionary
{
    /// <summary>
    /// 字典状态
    /// </summary>
  
    public enum DataItemStatus
    {
        [Display(Name = "启用",Description ="")]
        Enable,

        [Display(Name = "停用", Description = "")]
        Disable
    }
}
