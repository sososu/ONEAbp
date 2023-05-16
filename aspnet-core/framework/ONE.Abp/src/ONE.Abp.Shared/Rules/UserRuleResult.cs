using System;
using System.Collections.Generic;
using ONE.Abp.Shared.Rules;

namespace ONE.Abp.Shared.Rules
{
    public class UserRuleResult
    {

        public Guid Id { get; set; }

        /// <summary>
        /// 用户规则名称
        /// </summary>
        public string Name { get;  set; }

        /// <summary>
        /// 指用于过滤符合条件的用户
        /// </summary>
        public List<ConditionGroupUnit> Conditions { get; set; }

    }
}
