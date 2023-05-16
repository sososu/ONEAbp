using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ONE.Abp.Data.Rules;
using ONE.Abp.Shared.Rules;
using  ONE.Abp.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Users;

namespace ONE.Abp.DataPermission
{
    [Dependency(ReplaceServices = true)]
    public class DefaultRuleEngine: IRuleEngine, ITransientDependency
    {
        protected AbpRuleOptions Options { get; }
        protected IRuleStore RuleStore { get; }
        protected IRuleExpressionGenerator RuleExpressionGenerator { get; }

        protected ICurrentUser CurrentUser{ get; }
        public DefaultRuleEngine(IOptions<AbpRuleOptions> options, IRuleStore ruleStore, IRuleExpressionGenerator ruleExpressionGenerator, ICurrentUser currentUser)
        {
            Options = options.Value;
            RuleStore = ruleStore;
            RuleExpressionGenerator = ruleExpressionGenerator;
            CurrentUser = currentUser;
        }

        //todo:性能优化，添加缓存
        public virtual async Task<List<RuleResult<TEntity, TResult>>> ExecuteAsync<TEntity, TResult>(CancellationToken cancellationToken = default) where TEntity : class, IEntity where TResult : ExtensibleObject
        {
            var results = new List<RuleResult<TEntity, TResult>>();

            //获取该数据源所有的规则
            var rules = await RuleStore.GetUserDataRulesAsync(typeof(TEntity).Name, cancellationToken);

            foreach (var rule in rules)
            {
                //获取用户筛选规则
                var userRule = await RuleStore.GetUserRuleAsync(rule.UserRuleId, cancellationToken);
                if (userRule == null) continue;

                //判断当前用户是否符合该用户筛选规则，不符合继续

                var userConditon = RuleExpressionGenerator.CreateFilterUserConditionExpression<ICurrentUser>(userRule.Conditions);

                var pass = userConditon.Compile().Invoke(CurrentUser);

                if (!pass)
                    continue;

                //符合后获取数据规则

                var dataRule = await RuleStore.GetDataRuleAsync(rule.DataRuleId, cancellationToken);
                if (dataRule == null) continue;

                var conditionExpression = RuleExpressionGenerator.CreateFilterConditionExpression<TEntity>(dataRule.Conditions);

                //var selectExpression = RuleExpressionHelper.CreateSelectExpression<TEntity,TResult>(dataRule.DataTargetFields.Split(',',StringSplitOptions.RemoveEmptyEntries));

                Action<TResult> changeResultAction = null;
                if (dataRule.HideDataTargetFields.IsNotNullOrWhiteSpace())
                    changeResultAction = RuleExpressionGenerator.BuildResetPropertiesExpression<TResult>(dataRule.HideDataTargetFields.Split(",", StringSplitOptions.RemoveEmptyEntries));

                results.Add(new RuleResult<TEntity, TResult>(conditionExpression, changeResultAction, dataRule.DataOperation));

                if (rule.RuleType == RuleType.Exclusive) //如果是排他的则直接跳出循环
                    break;
            }

            if (!results.Any())
            {
                //var select = RuleExpressionHelper.CreateSelectExpression<TEntity>(typeof(TEntity).GetProperties().Select(p=>p.Name).ToArray());
                //todo:如果都没有规则满足条件，则放行或者不放行(由配置决定？）
                

                results.Add(new RuleResult<TEntity, TResult>(entity => Options.IsReleasedIfNoRulesAreMatched, null, Options.RuleDataOperationIfNoRulesAreMatched));
            }

            return results;
        }
    }
}
