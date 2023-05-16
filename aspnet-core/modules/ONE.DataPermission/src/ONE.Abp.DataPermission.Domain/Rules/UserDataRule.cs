using ONE.Abp.Shared.Rules;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace ONE.Abp.DataPermission.Rules
{
    /// <summary>
    /// 用户数据规则
    /// 关联用户于数据规则
    /// </summary>
    public class UserDataRule : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        /// <summary>
        /// 数据对象 指需要进行权限控制的数据实体，如表、视图等。
        /// </summary>
        public string DataTargetName { get;protected set; }

        /// <summary>
        /// 用户规则Id
        /// </summary>
        public Guid UserRuleId { get; protected set; }

        /// <summary>
        /// 数据规则Id
        /// </summary>
        public Guid DataRuleId { get; protected set; }


        /// <summary>
        /// 规则类型
        /// </summary>
        public RuleType RuleType { get; protected set; }


        /// <summary>
        /// 优先级 值越大优先级越高
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }    

        public Guid? TenantId { get; set; }

        protected UserDataRule() { }

        public UserDataRule(Guid id,string dataTarget,Guid userRuleId, Guid dataRuleId)
        {
            Id = id;

            Check.NotNull(dataTarget, nameof(dataTarget));
            Check.NotNull(userRuleId, nameof(userRuleId));
            Check.NotNull(dataRuleId, nameof(dataRuleId));

            DataTargetName = dataTarget;
            UserRuleId = userRuleId;
            DataRuleId = dataRuleId;
        }



        public void Change(string dataTarget, Guid userRuleId, Guid dataRuleId)
        {
            Check.NotNull(dataTarget, nameof(dataTarget));
            Check.NotNull(userRuleId, nameof(userRuleId));
            Check.NotNull(dataRuleId, nameof(dataRuleId));

            DataTargetName = dataTarget;
            UserRuleId = userRuleId;
            DataRuleId = dataRuleId;
        }


        public void SetRuleType(RuleType ruleType)
        {
            RuleType = ruleType;
        }
    }
}
