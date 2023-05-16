using System;

namespace ONE.Abp.Shared.Rules
{
    public class UserDataRuleResult
    {

        public Guid Id { get; set; }

        /// <summary>
        /// 数据对象 指需要进行权限控制的数据实体，如表、视图等。
        /// </summary>
        public string DataTargetName { get; set; }

        /// <summary>
        /// 用户规则Id
        /// </summary>
        public Guid UserRuleId { get; set; }

        /// <summary>
        /// 用户规则名称
        /// </summary>
        public string UserRuleName { get; set; }    

        /// <summary>
        /// 数据规则Id
        /// </summary>
        public Guid DataRuleId { get; set; }

        /// <summary>
        /// 数据规则名称
        /// </summary>
        public string DataRuleName { get; set; }

        /// <summary>
        /// 规则类型
        /// </summary>
        public RuleType RuleType { get; set; }


        /// <summary>
        /// 优先级 值越大优先级越高
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }
    }
}
