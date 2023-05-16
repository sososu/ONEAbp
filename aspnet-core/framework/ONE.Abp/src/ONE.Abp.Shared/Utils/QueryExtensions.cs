using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace  ONE.Abp.Shared.Utils
{
    /// <summary>
    /// 查询扩展方法
    /// </summary>
    public static class QueryExtensions
    {
        public static Expression CreateQueryExpression(Expression entityParan, object value, string[] propertyPath,
            QueryCompare compare)
        {
            var member = CreatePropertyExpression(entityParan, propertyPath);

            switch (compare)
            {
                case QueryCompare.Equal:
                    return CreateEqualExpression(member, value);
                case QueryCompare.NotEqual:
                    return CreateNotEqualExpression(member, value);
                case QueryCompare.Like:
                    return CreateLikeExpression(member, value);
                case QueryCompare.NotLike:
                    return CreateNotLikeExpression(member, value);
                case QueryCompare.StartWidth:
                    return CreateStartsWithExpression(member, value);
                case QueryCompare.EndsWith:
                    return CreateEndsWithExpression(member, value);
                case QueryCompare.LessThan:
                    return CreateLessThanExpression(member, value);
                case QueryCompare.LessThanOrEqual:
                    return CreateLessThanOrEqualExpression(member, value);
                case QueryCompare.GreaterThan:
                    return CreateGreaterThanExpression(member, value);
                case QueryCompare.GreaterThanOrEqual:
                    return CreateGreaterThanOrEqualExpression(member, value);
                case QueryCompare.Between:
                    return CreateBetweenExpression(member, value);
                case QueryCompare.GreaterEqualAndLess:
                    return CreateGreaterEqualAndLessExpression(member, value);
                case QueryCompare.GreaterEqualAndLessEqual:
                    return CreateGreaterEqualAndLessEqualExpression(member, value);
                case QueryCompare.Include:
                    return CreateIncludeExpression(member, value);
                case QueryCompare.NotInclude:
                    return CreateNotIncludeExpression(member, value);
                case QueryCompare.IsNull:
                    return CreateIsNullExpression(member, value);
                case QueryCompare.HasFlag:
                    return CreateHasFlagExpression(member, value);
                default:
                    return null;
            }
        }


        public static Expression CreateQueryExpression(Expression entityParan, object value,
           QueryCompare compare)
        {
            switch (compare)
            {
                case QueryCompare.Equal:
                    return CreateEqualExpression(entityParan, value);
                case QueryCompare.NotEqual:
                    return CreateNotEqualExpression(entityParan, value);
                case QueryCompare.Like:
                    return CreateLikeExpression(entityParan, value);
                case QueryCompare.NotLike:
                    return CreateNotLikeExpression(entityParan, value);
                case QueryCompare.StartWidth:
                    return CreateStartsWithExpression(entityParan, value);
                case QueryCompare.EndsWith:
                    return CreateEndsWithExpression(entityParan, value);
                case QueryCompare.LessThan:
                    return CreateLessThanExpression(entityParan, value);
                case QueryCompare.LessThanOrEqual:
                    return CreateLessThanOrEqualExpression(entityParan, value);
                case QueryCompare.GreaterThan:
                    return CreateGreaterThanExpression(entityParan, value);
                case QueryCompare.GreaterThanOrEqual:
                    return CreateGreaterThanOrEqualExpression(entityParan, value);
                case QueryCompare.Between:
                    return CreateBetweenExpression(entityParan, value);
                case QueryCompare.GreaterEqualAndLess:
                    return CreateGreaterEqualAndLessExpression(entityParan, value);
                case QueryCompare.GreaterEqualAndLessEqual:
                    return CreateGreaterEqualAndLessEqualExpression(entityParan, value);
                case QueryCompare.Include:
                    return CreateIncludeExpression(entityParan, value);
                case QueryCompare.NotInclude:
                    return CreateNotIncludeExpression(entityParan, value);
                case QueryCompare.IsNull:
                    return CreateIsNullExpression(entityParan, value);
                case QueryCompare.HasFlag:
                    return CreateHasFlagExpression(entityParan, value);
                default:
                    return null;
            }
        }

        #region Private Method

        private static MemberExpression CreatePropertyExpression(Expression param, string[] propertyPath)
        {
            var expression = propertyPath.Aggregate(param, Expression.Property) as MemberExpression;
            return expression;
        }


        private static Expression CreateEqualExpression(Expression member, object value)
        {
            if (value == null) return null;

            var val = CreateQueryParamExpression(ChangeType(value, member.Type), member.Type);

            return Expression.Equal(member, val);
        }


        private static Expression CreateNotEqualExpression(Expression member, object value)
        {
            if (value == null) return null;

            var val = CreateQueryParamExpression(ChangeType(value, member.Type), member.Type);

            return Expression.NotEqual(member, val);
        }

        private static Expression CreateLikeExpression(Expression member, object value)
        {
            if (value == null) return null;
            if (member.Type != typeof(string))
                throw new ArgumentOutOfRangeException(nameof(member), $"Member '{member}' can not use 'Like' compare");

            var str = value.ToString();
            var val = CreateQueryParamExpression(str, member.Type);

            return Expression.Call(member, nameof(string.Contains), null, val);
        }

        private static Expression CreateNotLikeExpression(Expression member, object value)
        {
            var like = CreateLikeExpression(member, value);
            if (like == null) return null;

            return Expression.Not(like);
        }

        private static Expression CreateStartsWithExpression(Expression member, object value)
        {
            if (value == null) return null;
            if (member.Type != typeof(string))
                throw new ArgumentOutOfRangeException(nameof(member), $"Member '{member}' can not use 'Like' compare");

            var str = value.ToString();
            var val = CreateQueryParamExpression(str, member.Type);

            return Expression.Call(member, nameof(string.StartsWith), null, val);
        }

        private static Expression CreateEndsWithExpression(Expression member, object value)
        {
            if (value == null) return null;
            if (member.Type != typeof(string))
                throw new ArgumentOutOfRangeException(nameof(member), $"Member '{member}' can not use 'Like' compare");

            var str = value.ToString();
            var val = CreateQueryParamExpression(str, member.Type);

            return Expression.Call(member, nameof(string.EndsWith), null, val);
        }

        private static Expression CreateLessThanExpression(Expression member, object value)
        {
            if (value == null) return null;

            var val = CheckConvertToEnumUnderlyingType(member.Type, member);
            var right = CreateQueryParamExpression(ChangeType(value, val.Type), val.Type);

            return Expression.LessThan(val, right);
        }

        private static Expression CreateLessThanOrEqualExpression(Expression member, object value)
        {
            if (value == null) return null;

            var val = CheckConvertToEnumUnderlyingType(member.Type, member);
            var right = CreateQueryParamExpression(ChangeType(value, val.Type), val.Type);

            return Expression.LessThanOrEqual(val, right);
        }

        private static Expression CreateGreaterThanExpression(Expression member, object value)
        {
            if (value == null) return null;

            var val = CheckConvertToEnumUnderlyingType(member.Type, member);
            var right = CreateQueryParamExpression(ChangeType(value, val.Type), val.Type);

            return Expression.GreaterThan(val, right);
        }

        private static Expression CreateGreaterThanOrEqualExpression(Expression member, object value)
        {
            if (value == null) return null;

            var val = CheckConvertToEnumUnderlyingType(member.Type, member);
            var right = CreateQueryParamExpression(ChangeType(value, val.Type), val.Type);

            return Expression.GreaterThanOrEqual(val, right);
        }

        private static Expression CreateBetweenExpression(Expression member, object value)
        {
            var val = CheckConvertToEnumUnderlyingType(member.Type, member);
            var list = GetListValue(val.Type, value);
            if (list == null) return null;
            if (list.Count < 2) return null;

            var left = list[0];
            var right = list[list.Count - 1];
            if (left == null || right == null) return null;


            Expression greaterEqual = null;
            Expression less = null;
            if (left != null)
            {
                var leftVal = CreateQueryParamExpression(left, val.Type);
                greaterEqual = Expression.GreaterThanOrEqual(val, leftVal);
            }
            if (right != null)
            {
                if (right is DateTime date && date.TimeOfDay == TimeSpan.Zero)
                {
                    right = date.AddDays(1);
                }
                var rightVal = CreateQueryParamExpression(right, val.Type);
                less = Expression.LessThan(val, rightVal);
            }

            if (greaterEqual == null)
                return less;
            else if (less == null)
                return greaterEqual;
            else
                return Expression.AndAlso(greaterEqual, less);
        }

        private static Expression CreateGreaterEqualAndLessExpression(Expression member, object value)
        {
            var val = CheckConvertToEnumUnderlyingType(member.Type, member);
            var list = GetListValue(val.Type, value);
            if (list == null) return null;
            if (list.Count < 2) return null;

            var left = list[0];
            var right = list[list.Count - 1];

            Expression greaterEqual = null;
            Expression less = null;
            if (left != null)
            {
                var leftVal = CreateQueryParamExpression(left, val.Type);
                greaterEqual = Expression.GreaterThanOrEqual(val, leftVal);
            }
            if (right != null)
            {
                if (right is DateTime date && date.TimeOfDay == TimeSpan.Zero)
                {
                    right = date.AddDays(1);
                }
                var rightVal = CreateQueryParamExpression(right, val.Type);
                less = Expression.LessThan(val, rightVal);
            }

            if (greaterEqual == null)
                return less;
            else if (less == null)
                return greaterEqual;
            else
                return Expression.AndAlso(greaterEqual, less);

        }


        private static Expression CreateGreaterEqualAndLessEqualExpression(Expression member, object value)
        {
            var val = CheckConvertToEnumUnderlyingType(member.Type, member);
            var list = GetListValue(val.Type, value);
            if (list == null) return null;
            if (list.Count < 2) return null;

            var left = list[0];
            var right = list[list.Count - 1];

            Expression greaterEqual = null;
            Expression lessEual = null;
            if (left != null)
            {
                var leftVal = CreateQueryParamExpression(left, val.Type);
                greaterEqual = Expression.GreaterThanOrEqual(val, leftVal);
            }
            if (right != null)
            {
                if (right is DateTime date && date.TimeOfDay == TimeSpan.Zero)
                {
                    right = date.AddDays(1);
                }
                var rightVal = CreateQueryParamExpression(right, val.Type);
                lessEual = Expression.LessThanOrEqual(val, rightVal);
            }

            if (greaterEqual == null)
                return lessEual;
            else if (lessEual == null)
                return greaterEqual;
            else
                return Expression.AndAlso(greaterEqual, lessEual);

        }

        private static Expression CheckConvertToEnumUnderlyingType(Type memberType, Expression val)
        {
            var nonNullableType = memberType.GetNonNullableType();
            if (nonNullableType.IsEnum)
            {
                var underlyingType = nonNullableType.GetEnumUnderlyingType();
                if (memberType.IsNullableType())
                {
                    underlyingType = underlyingType.GetTypeOfNullable();
                }

                return Expression.Convert(val, underlyingType);
            }
            return val;
        }

        private static Expression CreateIncludeExpression(Expression member, object value)
        {
            if (member.Type != typeof(string) && member.Type.IsEnumerableType())
            {
                var elementType = member.Type.GetEnumerableElementType();
                var val = CreateQueryParamExpression(value, elementType);

                return Expression.Call(typeof(Enumerable), nameof(Enumerable.Contains), new[] { elementType }, member, val);
            }
            else
            {
                var list = GetListValue(member.Type, value);
                if (list == null || list.Count == 0) return null;
                if (list.Count == 1)
                {
                    return CreateEqualExpression(member, list[0]);
                }

                var listType = typeof(IEnumerable<>).MakeGenericType(member.Type);
                var vals = CreateQueryParamExpression(list, listType);

                return Expression.Call(typeof(Enumerable), nameof(Enumerable.Contains), new[] { member.Type }, vals, member);

            }
        }

        private static Expression CreateNotIncludeExpression(Expression member, object value)
        {
            var includeExpression = CreateIncludeExpression(member, value);
            if (includeExpression == null) return null;

            return Expression.Not(includeExpression);
        }

        private static Expression CreateIsNullExpression(Expression member, object value)
        {
            if (member.Type.IsValueType && !member.Type.IsNullableType())
                throw new InvalidOperationException($"Member:{member} can not use '{QueryCompare.IsNull}' compare");

            var nullVal = CreateQueryParamExpression(null, member.Type);

            if (value == null || Equals(value, false))
                return Expression.Equal(member, nullVal);

            return Expression.NotEqual(member, nullVal);
        }


        private static Expression CreateHasFlagExpression(Expression member, object value)
        {
            if (!member.Type.GetNonNullableType().IsEnum)
                throw new InvalidOperationException($"Member:{member} is not a Enum type");
            var list = GetListValue(member.Type.GetNonNullableType(), value);
            if (list == null || list.Count == 0) return null;

            var p = member;
            if (member.Type.IsNullableType())
                p = Expression.Property(member, "Value");

            Expression exp = null;
            foreach (var item in list)
            {
                var val = CreateQueryParamExpression(item, typeof(Enum));
                var method = typeof(Enum).GetMethod(nameof(Enum.HasFlag), new[] { typeof(Enum) });
                Expression temp = Expression.Call(p, method, val);
                exp = exp != null ? Expression.OrElse(exp, temp) : temp;
            }

            return exp;
        }

        private static IList GetListValue(Type memberType, object value)
        {
            if (value == null) return null;
            var data = value as IEnumerable;
            if (value is string str)
            {
                data = str.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => p.Trim()).ToArray();
            }

            if (data == null)
            {
                data = new[] { value };
            }

            var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(memberType));

            foreach (var item in data)
            {
                try
                {
                    list.Add(ChangeType(item, memberType));
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.Message);
                }
            }

            return list.Count == 0 ? null : list;
        }

        private static object ChangeType(object value, Type type)
        {
            if (value == null) return null;

            type = type.GetNonNullableType();
            if (type == value.GetType().GetNonNullableType()) return value;

            if (type.IsEnum)
            {
                if (value is string str1)
                    return Enum.Parse(type, str1);
                else
                    return Enum.ToObject(type, value);
            }
            if (value is string str2 && type == typeof(Guid))
                return new Guid(str2);

            return Convert.ChangeType(value, type);
        }

        private static Expression CreateQueryParamExpression(object value, Type type)
        {
            var queryType = _createValueTupleMethod.MakeGenericMethod(type);
            var queryParam = queryType.Invoke(null, new[] { value });
            var param = Expression.Constant(queryParam);

            return Expression.Field(param, "Value");
        }

        private static readonly MethodInfo _createValueTupleMethod =
            typeof(QueryExtensions).GetMethod(nameof(CreateQueryParam), BindingFlags.Static | BindingFlags.NonPublic);
        private static QueryParam<T> CreateQueryParam<T>(T value)
        {
            return new QueryParam<T>(value);
        }

        public struct QueryParam<TValue>
        {
            public TValue Value;

            public QueryParam(TValue value)
            {
                Value = value;
            }

            /// <inheritdoc />
            public override bool Equals(object obj)
            {
                if (obj is QueryParam<TValue>)
                    return this.Equals((QueryParam<TValue>)obj);
                return false;
            }

            public bool Equals(QueryParam<TValue> other)
            {
                return EqualityComparer<TValue>.Default.Equals(this.Value, other.Value);
            }

            /// <inheritdoc />
            public override int GetHashCode()
            {
                return EqualityComparer<TValue>.Default.GetHashCode(this.Value);
            }

            /// <inheritdoc />
            public override string ToString()
            {
                return $"QueryParam<{typeof(TValue).Name}>";
            }
        }

        #endregion
    }
}
