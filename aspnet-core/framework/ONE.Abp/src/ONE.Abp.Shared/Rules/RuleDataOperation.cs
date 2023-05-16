using System;
using System.ComponentModel.DataAnnotations;

namespace ONE.Abp.Shared.Rules
{
    [Flags]
    public enum RuleDataOperation : int
    {
        [Display(Name = "查询")]
        Query = 0b1,

        [Display(Name = "编辑")]
        Edit = 0b10,

        [Display(Name = "删除")]
        Delete = 0b100
    }
}
