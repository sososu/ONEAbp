using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ONE.Abp.DataPermission.Rules
{
    /// <summary>
    /// 用户规则
    /// 筛选符合条件的用户
    /// </summary>
    public class UserRule : AuditedAggregateRoot<Guid>,IMultiTenant
    {
        /// <summary>
        /// 用户规则名称
        /// </summary>
        public string Name { get;protected set; }

        /// <summary>
        /// 指用于过滤符合条件的用户
        /// </summary>
        public string Condition { get; protected set; }

        public Guid? TenantId { get; protected set; }

        protected UserRule() { }

        public UserRule(Guid id,string name)
        {
            Id = id;
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

    }
}
