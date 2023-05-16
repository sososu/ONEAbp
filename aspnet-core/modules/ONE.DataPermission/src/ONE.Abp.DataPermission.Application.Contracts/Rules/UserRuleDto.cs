using ONE.Abp.Shared.Rules;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace ONE.Abp.DataPermission.Rules
{
    public class UserRuleDto: AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 用户规则名称
        /// </summary>
        public string Name { get;  set; }

        /// <summary>
        /// 指用于过滤符合条件的用户
        /// </summary>
        public List<ConditionGroupUnit> ConditionGroups { get; set; }

    }

    public class UserRuleMini : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 用户规则名称
        /// </summary>
        public string Name { get; set; }

    }
}
