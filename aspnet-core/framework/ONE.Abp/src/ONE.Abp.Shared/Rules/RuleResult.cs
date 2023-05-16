using System;
using System.Linq.Expressions;

namespace ONE.Abp.Shared.Rules
{
    public class RuleResult<TEntity, TResult>
    {
        public Expression<Func<TEntity, bool>> ConditionExpression { get; set; }
        //public Expression<Func<TEntity, dynamic>> SelectExpression { get; set; }

        public Action<TResult> ChangeTargetAction { get; set; }

        public RuleDataOperation RuleDataOperation { get; set; }

        public RuleResult(Expression<Func<TEntity, bool>> conditionExpression, Action<TResult> changeTargetAction, RuleDataOperation ruleDataOperation)
        {
            ConditionExpression = conditionExpression;
            ChangeTargetAction = changeTargetAction;
            RuleDataOperation = ruleDataOperation;
        }
    }
}
