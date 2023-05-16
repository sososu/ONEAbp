using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ONE.Abp.Shared.Rules;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Users;

namespace ONE.Abp.Data.Rules
{
    public interface IRuleExpressionGenerator
    {
        Expression<Func<TEntity, bool>> CreateFilterConditionExpression<TEntity>(List<ConditionGroupUnit> groups) where TEntity : class;

        Expression<Func<TSource, bool>> CreateFilterUserConditionExpression<TSource>(List<ConditionGroupUnit> groups) where TSource : ICurrentUser;

        Expression<Func<TEntity, TResult>> CreateSelectExpression<TEntity, TResult>(string[] fieldNames) where TEntity : class, IEntity where TResult : ExtensibleObject;

        Action<T> BuildResetPropertiesExpression<T>(IEnumerable<string> ignoredProperties);
    }


    public class NullRuleExpressionGenerator : IRuleExpressionGenerator, ITransientDependency
    {
        public Action<T> BuildResetPropertiesExpression<T>(IEnumerable<string> ignoredProperties)
        {
            return null;
        }

        public Expression<Func<TEntity, bool>> CreateFilterConditionExpression<TEntity>(List<ConditionGroupUnit> groups) where TEntity : class
        {
            return null;
        }

        public Expression<Func<TSource, bool>> CreateFilterUserConditionExpression<TSource>(List<ConditionGroupUnit> groups) where TSource : ICurrentUser
        {
            return null;
        }

        Expression<Func<TEntity, TResult>> IRuleExpressionGenerator.CreateSelectExpression<TEntity, TResult>(string[] fieldNames)
        {
            return null;
        }
    }
}
