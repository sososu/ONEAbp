using ONE.Abp.Data.Rules;
using ONE.Abp.Shared.Rules;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ONE.Abp.DataPermission.Rules
{
    public class DataRuleCreateInput
    {
        /// <summary>
        /// 规则名称
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 数据对象 指需要进行权限控制的数据实体，如表、视图等。
        /// </summary>
        [Required]
        public string DataTargetName { get; set; }

        /// <summary>
        /// 隐藏的数据对象字段
        /// </summary>
        public List<string> HideDataTargetFields { get; set; }

        /// <summary>
        /// 指用于限制数据对象访问范围的条件或表达式，如部门、角色、时间等
        /// </summary>
        public List<ConditionGroupUnit> ConditionGroups { get; set; }

        /// <summary>
        /// 数据操作
        /// </summary>
        [Required]
        public List<RuleDataOperation> DataOperations { get; set; }
    }
}
