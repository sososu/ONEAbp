using System.ComponentModel.DataAnnotations;

namespace ONE.Abp.Shared.Rules
{
    public enum RuleType
    {
      
        /// <summary>
        /// 普通 
        /// </summary>
        [Display(Name = "普通", Description = "优先级低于排他类型，如果命中则会继续匹配其他普通规则，后取其并集")]
        Common = 0,

              /// <summary>
              /// 排他
              /// </summary>
        [Display(Name = "排他", Description = "优先匹配排他规则，如果命中则直接采用")]
        Exclusive=1

    }
}
