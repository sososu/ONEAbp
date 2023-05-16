using JetBrains.Annotations;
using ONE.Abp.Data.Rules;
using ONE.Abp.Shared.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ONE.Abp.DataPermission.Rules
{

    /// <summary>
    /// 数据规则
    /// 限制了数据的范围和操作权限
    /// </summary>
    public class DataRule:AuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// 规则名称
        /// </summary>
        public string Name { get;protected set; }

        /// <summary>
        /// 数据对象 指需要进行权限控制的数据实体，如表、视图等。
        /// </summary>
        public string DataTargetName { get; protected set; }

        /// <summary>
        /// 隐藏的数据对象字段
        /// 用","分割
        /// </summary>
        [CanBeNull]
        public string HideDataTargetFields { get; protected set; }

        /// <summary>
        /// 隐藏的数据对象字段显示名称
        /// 用","分割
        /// </summary>
        [CanBeNull]
        public string HideDataTargetFieldDisplayNames { get; protected set; }

        /// <summary>
        /// 指用于限制数据对象访问范围的条件或表达式，如部门、角色、时间等
        /// </summary>
        public string Condition { get; protected set; }

        /// <summary>
        /// 数据操作
        /// </summary>
        public RuleDataOperation DataOperation { get; protected set; }

        public Guid? TenantId { get; set; }

        protected DataRule() { }

        public DataRule(Guid id,string dataTarget, string name)
        {
            Id = id;
            DataTargetName = dataTarget;
            SetName(name);
           
        }

        public void SetName(string name)
        {
            Check.NotNull(name, "name");
            Name = name;
        }

        public void SetCondition(string condition)
        {
            Check.NotNull(condition, "condition");
            Condition = condition;
        }

        public void SetDataTargetFields(IList<string> fields,IList<string>fieldDisplayNames)
        {
            HideDataTargetFields = fields != null && fields.Any()?string.Join(",", fields): string.Empty;
            HideDataTargetFieldDisplayNames = fieldDisplayNames != null && fieldDisplayNames.Any()?string.Join(",", fieldDisplayNames):string.Empty;
        }



        public void SetRuleDataOperation(RuleDataOperation dataOperation)
        {
            DataOperation = dataOperation;
        }
    }







}
