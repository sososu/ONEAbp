using ONE.Abp.Shared.Rules;
using System;
using System.ComponentModel.DataAnnotations;

namespace ONE.Abp.DataPermission.Rules
{
    public class UserDataRuleCreateInput
    {

        /// <summary>
        /// 数据对象 指需要进行权限控制的数据实体，如表、视图等。
        /// </summary>
        [Required]
        public string DataTargetName { get; set; }

        /// <summary>
        /// 用户规则Id
        /// </summary>
        [Required]
        public Guid UserRuleId { get; set; }

        /// <summary>
        /// 数据规则Id
        /// </summary>
        [Required]
        public Guid DataRuleId { get; set; }


        /// <summary>
        /// 规则类型
        /// </summary>
        [Required]
        public RuleType RuleType { get; set; }


        /// <summary>
        /// 优先级 值越大优先级越高
        /// </summary>
        public int Priority { get; set; }

    }
}
