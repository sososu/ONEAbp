using Microsoft.Extensions.Options;
using ONE.Abp.Data.Rules;
using ONE.Abp.Shared.Rules;
using ONE.Abp.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Users;

namespace ONE.Abp.DataPermission
{
    [Dependency(ReplaceServices = true)]
    public class RuleExpressionGenerator: IRuleExpressionGenerator,ITransientDependency
    {
        protected AbpRuleOptions Options { get; }
        protected ICurrentUser CurrentUser { get; }

        protected IUserExtraFieldResolver UserExtraFieldResolver { get; }
        public RuleExpressionGenerator(IOptions<AbpRuleOptions> options,ICurrentUser currentUser, IUserExtraFieldResolver userExtraFieldResolver) 
        {
            Options = options.Value;
            CurrentUser = currentUser;
            UserExtraFieldResolver = userExtraFieldResolver;
        }

        public virtual Expression<Func<TEntity, bool>> CreateFilterConditionExpression<TEntity>(List<ConditionGroupUnit> groups) where TEntity : class
        {
            var entityParam = Expression.Parameter(typeof(TEntity), "o");

            Expression body = null;


            foreach (var group in groups)
            {
                var experssion = HandleGroup(group);

                body = body == null ? experssion : (group.ConditionOperator.HasValue && group.ConditionOperator.Value == ConditionOperator.And) ? Expression.AndAlso(body, experssion) : Expression.OrElse(body, experssion);
            }

            return body != null ? Expression.Lambda<Func<TEntity, bool>>(body, entityParam) : null;


            Expression HandleGroup(ConditionGroupUnit groupUnit)
            {
                Expression sub = null;

                if (groupUnit == null)
                    return null;

                if (groupUnit.ConditionUnits == null)
                    return null;

                foreach (var conditionUnit in groupUnit.ConditionUnits)
                {
                    var experssion = HandleCondition(conditionUnit.Condition);
                    sub = sub == null ? experssion : conditionUnit.ConditionOperator.HasValue && conditionUnit.ConditionOperator.Value == ConditionOperator.And ? Expression.AndAlso(sub, experssion) : Expression.OrElse(sub, experssion);
                }

                return sub;
            }

            Expression HandleCondition(Condition condition)
            {
                object fieldValue = condition.FieldValue;
                //if (Options.DataRulePredefineFieldValueManager.ContainsKey(condition.FieldValue))
                //{
                //    fieldValue = Options.DataRulePredefineFieldValueManager.GetValueFunc(condition.FieldValue).Invoke(CurrentUser);
                //};
                var extraFieldInfo= Options.RuleExtraFieldManager.GetRuleExtraFieldInfoByPredefine(condition.FieldValue);
                if (extraFieldInfo != null&& extraFieldInfo.GetPredefineValueFunc!=null)
                {
                    fieldValue = extraFieldInfo.GetPredefineValueFunc.Invoke(CurrentUser);
                };

                var propertyPath = condition.FieldName.Split(".").ToArray(); //支持嵌套类 Customer.address.city 
                return QueryExtensions.CreateQueryExpression(entityParam, fieldValue, propertyPath, condition.Compare);
            }
        }

        public virtual Expression<Func<TSource, bool>> CreateFilterUserConditionExpression<TSource>(List<ConditionGroupUnit> groups) where TSource : ICurrentUser
        {
            var entityParam = Expression.Parameter(typeof(TSource), "o");

            Expression body = null;

         
            foreach (var group in groups)
            {
                var experssion = HandleGroup(group);

                body = body == null ? experssion : (group.ConditionOperator.HasValue && group.ConditionOperator.Value == ConditionOperator.And) ? Expression.AndAlso(body, experssion) : Expression.OrElse(body, experssion);
            }

            return body != null ? Expression.Lambda<Func<TSource, bool>>(body, entityParam) : null;


            Expression HandleGroup(ConditionGroupUnit groupUnit)
            {
                Expression sub = null;

                if (groupUnit == null)
                    return null;

                if (groupUnit.ConditionUnits == null)
                    return null;

                foreach (var conditionUnit in groupUnit.ConditionUnits)
                {
                    var experssion = HandleCondition(conditionUnit.Condition);
                    sub = sub == null ? experssion : conditionUnit.ConditionOperator.HasValue && conditionUnit.ConditionOperator.Value == ConditionOperator.And ? Expression.AndAlso(sub, experssion) : Expression.OrElse(sub, experssion);
                }

                return sub;
            }

            Expression HandleCondition(Condition condition)
            {
                object fieldValue = condition.FieldValue;
                var propertyPath = condition.FieldName.Split(".").ToArray(); //支持嵌套类 Customer.address.city 


                if (typeof(ICurrentUser).GetProperty(condition.FieldName) == null) //自定义扩展字段
                {
                    var entityParamExtra = UserExtraFieldResolver.GetExpression(condition.FieldName, entityParam);
                    return QueryExtensions.CreateQueryExpression(entityParamExtra, fieldValue, condition.Compare);
                }

                return QueryExtensions.CreateQueryExpression(entityParam, fieldValue, propertyPath, condition.Compare);
            }
  
        }

        public virtual Expression<Func<TEntity, TResult>> CreateSelectExpression<TEntity, TResult>(string[] fieldNames) where TEntity : class, IEntity where TResult : ExtensibleObject
        {
            ParameterExpression pe = Expression.Parameter(typeof(TEntity), "o");// 创建形参 o
            Type resultType = typeof(TResult);
            NewExpression ne = Expression.New(resultType);	// 相当于 new 关键字创建一个对象


            List<MemberAssignment> bindings = new List<MemberAssignment>();
            foreach (var fieldName in fieldNames)
            {
                if (resultType.GetProperty(fieldName) == null)
                {
                    continue;
                }
                MemberExpression meName = Expression.MakeMemberAccess(pe, typeof(TEntity).GetProperty(fieldName));  // 要使用 MakeMemberAccess 方法
                MemberAssignment maName = Expression.Bind(resultType.GetProperty(fieldName), meName);	// 使用 Bind 方法将目标类型的属性与源类型的属性值绑定
                bindings.Add(maName);
            }

            MemberInitExpression mie = Expression.MemberInit(ne, bindings);	// 相当于初始化时赋值操作
            return Expression.Lambda<Func<TEntity, TResult>>(mie, pe);
        }


        public virtual Action<T> BuildResetPropertiesExpression<T>(IEnumerable<string> ignoredProperties)
        {
            ignoredProperties = GetMapperProperties<T>(ignoredProperties);

            var objParam = Expression.Parameter(typeof(T), "obj");

            var propertyExpressions = new List<BinaryExpression>();

            foreach (var ignoredPropery in ignoredProperties)
            {
                var propertyPath = ignoredPropery.Split(".", StringSplitOptions.RemoveEmptyEntries);
                var member = CreatePropertyExpression(objParam, propertyPath);

                var defaultValue = Expression.Default(member.Type);

                // create an assignment expression to set the property's value to the default value
                var propertyAssignment = Expression.Assign(member, defaultValue);
                propertyExpressions.Add(propertyAssignment);
            }

            if (!propertyExpressions.Any()) { return null; }

            // combine all property expressions into one expression
            var body = Expression.Block(propertyExpressions);

            // create a lambda expression from the body and parameter, and compile it into a delegate
            var lambda = Expression.Lambda<Action<T>>(body, objParam);
            var compiled = lambda.Compile();

            return compiled;
        }

        protected virtual IEnumerable<string> GetMapperProperties<T>(IEnumerable<string> properties)
        {
            var resultType = typeof(T);

            var mapKv = resultType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => p.GetCustomAttribute<RuleMapperAttribute>() != null)
                .Select(p => new { p.GetCustomAttribute<RuleMapperAttribute>().SourceProperty, p.Name }).ToDictionary(kv => kv.SourceProperty, kv => kv.Name);


            var list = new List<string>();

            foreach (var property in properties)
            {
                if (property.Contains("."))
                {
                    var nesteds = property.Split(".", StringSplitOptions.RemoveEmptyEntries);
                    if (nesteds.Length > 2)
                        throw new Exception("Nested classes larger than two levels are not supported");

                    var mapName = resultType.GetProperty(nesteds[0]) != null ? nesteds[0] : mapKv.ContainsKey(nesteds[0]) ? mapKv[nesteds[0]] : string.Empty;
                    if (mapName == string.Empty)
                        continue;

                    var childProperty = resultType.GetProperty(mapName);

                    var childMapNmae =
                        childProperty.PropertyType.GetProperty(nesteds[1]) != null ? nesteds[1] :
                        childProperty.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                   .Where(p => p.GetCustomAttribute<RuleMapperAttribute>() != null && p.GetCustomAttribute<RuleMapperAttribute>().SourceProperty == nesteds[1]).Select(p => p?.Name).FirstOrDefault();

                    if (childMapNmae.IsNullOrWhiteSpace())
                        continue;

                    list.Add($"{mapName}.{childMapNmae}");
                }
                else
                {
                    var mapNmae = resultType.GetProperty(property) != null ? property : mapKv.ContainsKey(property) ? mapKv[property] : string.Empty;
                    if (mapNmae != null)
                        list.Add(mapNmae);
                }
            }

            return list;
        }

        private MemberExpression CreatePropertyExpression(Expression param, string[] propertyPath)
        {
            var expression = propertyPath.Aggregate(param, Expression.Property) as MemberExpression;
            return expression;
        }
    }
}
