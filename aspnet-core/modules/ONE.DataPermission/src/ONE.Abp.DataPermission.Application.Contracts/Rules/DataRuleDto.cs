using ONE.Abp.Shared.Rules;
using ONE.Abp.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Application.Dtos;

namespace ONE.Abp.DataPermission.Rules
{
    public class DataRuleDto: AuditedEntityDto<Guid>
    {
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
        public List<string> HideDataTargetFields { get;  set; }

        /// <summary>
        /// 数据对象字段
        /// 用","分割
        /// </summary>
        public List<string> HideDataTargetFieldDisplayNames { get; set; }
        /// <summary>
        /// 指用于限制数据对象访问范围的条件或表达式，如部门、角色、时间等
        /// </summary>
        public List<ConditionGroupUnit> ConditionGroups { get; set; }

        /// <summary>
        /// 数据操作
        /// </summary>
        public List<RuleDataOperation> DataOperations { get; set; }

    }


    public class DataRuleMini : AuditedEntityDto<Guid>
    {
        /// <summary>
        /// 规则名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 数据对象 指需要进行权限控制的数据实体，如表、视图等。
        /// </summary>
        public string DataTargetName { get; set; }

        /// <summary>
        /// 数据对象字段
        /// 用","分割
        /// </summary>
        public List<string> HideDataTargetFields { get; set; }

        /// <summary>
        /// 数据对象字段
        /// 用","分割
        /// </summary>
        public List<string> HideDataTargetFieldDisplayNames { get; set; }

        
        public string HideDataTargetFieldStr => HideDataTargetFields!=null&& HideDataTargetFields.Any()?string.Join(",", HideDataTargetFields):"";
        public string HideDataTargetFieldDisplayNameStr => HideDataTargetFieldDisplayNames != null&& HideDataTargetFieldDisplayNames.Any()?string.Join(",", HideDataTargetFieldDisplayNames) :"";

        /// <summary>
        /// 数据操作
        /// </summary>
        public List<RuleDataOperation> DataOperations { get; set; }

        public string DataOperationsStr => DataOperations != null && DataOperations.Any()?string.Join("|", DataOperations.Select(d=>d.DisplayName())):"";
    }
}
