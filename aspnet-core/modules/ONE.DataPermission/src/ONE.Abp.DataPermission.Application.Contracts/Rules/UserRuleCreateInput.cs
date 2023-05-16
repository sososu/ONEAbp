using ONE.Abp.Shared.Rules;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ONE.Abp.DataPermission.Rules
{
    public class UserRuleCreateInput
    {
        /// <summary>
        /// 用户规则名称
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 指用于过滤符合条件的用户
        /// </summary>
        public List<ConditionGroupUnit> ConditionGroups { get; set; }
    }
}
