using System;
using System.Collections.Generic;

namespace ONE.Abp.Shared.Rules
{
    public class DataRuleResult
    {

        public Guid Id { get; set; }

        /// <summary>
        /// 规则名称
        /// </summary>
        public string Name { get;  set; }

        /// <summary>
        /// 数据对象 指需要进行权限控制的数据实体，如表、视图等。
        /// </summary>
        public string DataTargetName { get;  set; }

        /// <summary>
        /// 数据对象字段
        /// 用","分割
        /// </summary>
        public string HideDataTargetFields { get;  set; }

        /// <summary>
        /// 指用于限制数据对象访问范围的条件或表达式，如部门、角色、时间等
        /// </summary>
        public List<ConditionGroupUnit> Conditions { get; set; }

        /// <summary>
        /// 数据操作
        /// </summary>
        public RuleDataOperation DataOperation { get; set; }

    }
}
