using ONE.Abp.Pagination.Contracts.Attributes;
using  ONE.Abp.Shared.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
namespace ONE.Abp.Pagination.Contracts
{
    /// <summary>
    /// 查询扩展方法
    /// </summary>
    public static class QueryContractExtension
    {
        /// <summary>
        /// 查询指定条件的数据
        /// </summary>
        /// <typeparam name="TEntity">查询的实体</typeparam>
        /// <param name="source"></param>
        /// <param name="sort">排序</param>
        /// <param name="defaultSort">默认排序</param>
        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, ISortInfo sort, string defaultSort = null)
        {
            var sortFields = sort?.SortFields?.Where(p => !string.IsNullOrEmpty(p))
                .Select(p => p.StartsWith("+") ? (p.TrimStart('+') + " ASC") : p.StartsWith("-") ? (p.TrimStart('-') + " DESC") : p).ToArray();
            if (sortFields != null && sortFields.Length > 0)
            {
                source = source.OrderBy(string.Join(" , ", sortFields));
            }
            else if (!string.IsNullOrEmpty(defaultSort))
            {
                source = source.OrderBy(defaultSort);
            }

            return source;
        }


        /// <summary>
        /// 获取查询表达式
        /// </summary>
        /// <typeparam name="TEntity">要查询的实体类型</typeparam>
        public static Expression<Func<TEntity, bool>> GetQueryExpression<TEntity>(this IQuery query)
            where TEntity : class
        {
            if (query == null) return null;

            var queryType = query.GetType();
            var entityParam = Expression.Parameter(typeof(TEntity), "o");

            Expression body = null;

            var groupQuery = GetQueryGroup(queryType);

            foreach (var group in groupQuery.Values)
            {
                Expression sub = null;

                foreach ((var property, var attr) in group)
                {
                    var value = property.GetValue(query);
                    if (value is string str)
                    {
                        str = str.Trim();
                        value = string.IsNullOrEmpty(str) ? null : str;
                    }

                    var experssion = QueryExtensions.CreateQueryExpression(entityParam, value, attr.PropertyPath, attr.Compare);
                    if (experssion != null)
                    {
                        sub = sub == null ? experssion : Expression.OrElse(sub, experssion);
                    }
                }

                if (sub != null)
                {
                    body = body == null ? sub : Expression.AndAlso(body, sub);
                }
            }

            return body != null ? Expression.Lambda<Func<TEntity, bool>>(body, entityParam) : null;
        }

        private static readonly ConcurrentDictionary<Type, Dictionary<string, List<ValueTuple<PropertyInfo, QueryAttribute>>>>
            _queryCache = new ConcurrentDictionary<Type, Dictionary<string, List<(PropertyInfo, QueryAttribute)>>>();

        private static Dictionary<string, List<ValueTuple<PropertyInfo, QueryAttribute>>> GetQueryGroup(Type type)
        {
            return _queryCache.GetOrAdd(type, queryType =>
            {
                var groupIndex = 0;
                var groupQuery = new Dictionary<string, List<ValueTuple<PropertyInfo, QueryAttribute>>>();
                var properties = queryType.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic |
                                                         BindingFlags.Public);
                foreach (var property in properties)
                {
                    var queries = property.GetAttributes<QueryAttribute>();
                    if (queries.Length == 0) continue;

                    foreach (var attr in queries)
                    {
                        if (attr.OrGroup == null)
                            attr.OrGroup = groupIndex.ToString();
                        if (attr.PropertyPath == null || attr.PropertyPath.Length == 0)
                            attr.PropertyPath = new[] { property.Name };

                        var group = groupQuery.GetOrAdd(attr.OrGroup, p => new List<(PropertyInfo, QueryAttribute)>());
                        group.Add((property, attr));
                    }
                    groupIndex++;
                }
                return groupQuery;
            });
        }
    }
}
