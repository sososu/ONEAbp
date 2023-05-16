using System.ComponentModel.DataAnnotations;

namespace ONE.Abp.DataDictionary
{
    /// <summary>
    /// 字典来源
    /// </summary>
    public enum DataItemSource
    {
        [Display(Name = "枚举", Description = "枚举来源的字典项，不能修改值和编码")]
        Enum,

        [Display(Name = "输入", Description = "输入来源的字典序，可以修改任意字段")]
        Input
    }
}
