using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace  ONE.Abp.Shared.Utils
{
    public static class TypeExtensions
    {
        /// <summary>
        /// 获取参数的Attribute
        /// </summary>
        public static TAttr GetAttribute<TAttr>(this ParameterInfo parameter) where TAttr : Attribute
        {
            return Attribute.GetCustomAttribute(parameter, typeof(TAttr)) as TAttr;
        }

        /// <summary>
        /// 获取参数的Attribute
        /// </summary>
        public static TAttr[] GetAttributes<TAttr>(this ParameterInfo parameter) where TAttr : Attribute
        {
            return Attribute.GetCustomAttributes(parameter, typeof(TAttr)) as TAttr[];
        }

        /// <summary>
        /// 获取成员信息的Attribute
        /// </summary>
        public static TAttr GetAttribute<TAttr>(this MemberInfo member) where TAttr : Attribute
        {
            return Attribute.GetCustomAttribute(member, typeof(TAttr)) as TAttr;
        }

        /// <summary>
        /// 获取成员信息的Attribute
        /// </summary>
        public static TAttr[] GetAttributes<TAttr>(this MemberInfo member) where TAttr : Attribute
        {
            return Attribute.GetCustomAttributes(member, typeof(TAttr)) as TAttr[];
        }
        public static bool IsSubclassOfGeneric(this Type type, Type generic)
        {
            if (type == null || generic == null)
            {
                throw new ArgumentNullException();
            }

            if (!generic.IsGenericTypeDefinition)
            {
                throw new ArgumentException(nameof(generic) + " is must be generic type definition.");
            }

            if (!generic.IsGenericType)
            {
                return false;
            }

            if (type == generic)
                return false;

            while (type != null)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == generic)
                    return true;
                type = type.BaseType;
            }

            return false;
        }

        /// <summary>
        /// 获取类型的指定泛型实现类型
        /// </summary>
        public static IEnumerable<Type> GetClosedGenericTypes(this Type type, Type generic)
        {
            if (type == null || generic == null)
            {
                throw new ArgumentNullException();
            }

            var types = new HashSet<Type>();

            foreach (var i in type.GetInterfaces())
            {
                if (i.IsGenericType && i.GetGenericTypeDefinition() == generic)
                {
                    types.Add(i);
                }
            }

            while (type != null)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == generic)
                {
                    types.Add(type);
                }
                type = type.BaseType;
            }

            return types;
        }

        public static Type[] GetGenericArguments(this Type type, Type generic)
        {
            if (type == null || generic == null)
            {
                throw new ArgumentNullException();
            }

            if (!generic.IsGenericTypeDefinition)
            {
                throw new ArgumentException(nameof(generic) + " is must be generic type definition.");
            }

            if (!generic.IsGenericType)
            {
                return Type.EmptyTypes;
            }

            if (type == generic)
                return Type.EmptyTypes;

            while (type != null)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == generic)
                    return type.GetGenericArguments();
                type = type.BaseType;
            }

            return Type.EmptyTypes;
        }

        public static Type[] GetGenericInterfaceArguments(this Type type, Type generic)
        {
            if (type == null || generic == null)
            {
                throw new ArgumentNullException();
            }

            if (!generic.IsInterface)
            {
                throw new ArgumentException(nameof(generic) + " is must be interface.");
            }

            if (!generic.IsGenericTypeDefinition)
            {
                throw new ArgumentException(nameof(generic) + " is must be generic type definition.");
            }

            if (!generic.IsGenericType)
            {
                return Type.EmptyTypes;
            }

            if (type == generic)
                return Type.EmptyTypes;
            while (type != null)
            {
                Type t;
                if (type.IsGenericType && (t = type.GetInterfaces()
                        .FirstOrDefault(o => o.IsGenericType && o.GetGenericTypeDefinition() == generic)) != null)
                    return t.GetGenericArguments();
                type = type.BaseType;
            }

            return Type.EmptyTypes;
        }

        public static Type GetEnumerableElementType(this Type type)
        {
            if (!type.IsEnumerableType())
            {
                throw new ArgumentException(nameof(type) + " is must be iterable");
            }
            return type.IsArray ? type.GetElementType() : type.IsGenericType ? type.GetGenericArguments().FirstOrDefault():type;
        }

        public static Type GetNonNullableType(this Type type)
        {
            if (!type.IsNullableType())
            {
                return type;
            }

            return type.GetGenericArguments()[0];
        }


        /// <summary>
        /// 获取<see cref="Nullable{TValue}"/>范型的构造类型
        /// </summary>
        public static Type GetTypeOfNullable(this Type type)
        {
            return type.GetGenericArguments()[0];
        }

        /// <summary>
        /// 判断类型是否为 集合类型 <see cref="ICollection{TValue}"/>
        /// </summary>
        public static bool IsCollectionType(this Type type)
        {
            return ((type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(ICollection<>))) ||
                    (from t in type.GetInterfaces()
                     where t.IsGenericType
                     select t.GetGenericTypeDefinition()).Any<Type>(t => (t == typeof(ICollection<>))));
        }

        /// <summary>
        /// 判断类型是否为 字典类型 <see cref="IDictionary{TKey,TValue}"/>
        /// </summary>
        public static bool IsDictionaryType(this Type type)
        {
            return ((type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(IDictionary<,>))) ||
                    (from t in type.GetInterfaces()
                     where t.IsGenericType
                     select t.GetGenericTypeDefinition()).Any<Type>(t => (t == typeof(IDictionary<,>))));
        }

        /// <summary>
        /// 判断类型是否为 可迭代类型 <see cref="IEnumerable"/>
        /// </summary>
        public static bool IsEnumerableType(this Type type)
        {
            return type.GetInterfaces().Contains(typeof(IEnumerable));
        }

        /// <summary>
        /// 判断类型是否为 列表类型 <see cref="IList"/> 或 字典类型 <see cref="IDictionary{TKey, TValue}"/>
        /// </summary>
        public static bool IsListOrDictionaryType(this Type type)
        {
            if (!type.IsListType())
            {
                return type.IsDictionaryType();
            }
            return true;
        }
        /// <summary>
        /// 判断类型是否为 列表类型 <see cref="IList"/>
        /// </summary>
        public static bool IsListType(this Type type)
        {
            return type.GetInterfaces().Contains(typeof(IList));
        }

        /// <summary>
        /// 判断类型是否为 可空类型 <see cref="Nullable{TValue}"/>
        /// </summary>
        public static bool IsNullableType(this Type type)
        {
            return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }


        /// <summary>
        /// 是否是基础类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsBaseType(this Type type)
        {
            return Type.GetTypeCode(type) != TypeCode.Object;
        }


        /// <summary>
        /// 获取枚举指定的显示内容
        /// </summary>
        public static object Display(this MemberInfo memberInfo, DisplayProperty property)
        {
            if (memberInfo == null) return null;

            var display = memberInfo.GetAttribute<DisplayAttribute>();

            if (display != null)
            {
                switch (property)
                {
                    case DisplayProperty.Name:
                        return display.GetName();
                    case DisplayProperty.ShortName:
                        return display.GetShortName();
                    case DisplayProperty.GroupName:
                        return display.GetGroupName();
                    case DisplayProperty.Description:
                        return display.GetDescription();
                    case DisplayProperty.Order:
                        return display.GetOrder();
                    case DisplayProperty.Prompt:
                        return display.GetPrompt();
                }
            }

            return null;
        }

        /// <summary>
        /// 获取枚举说明
        /// </summary>
        public static string DisplayName(this MemberInfo val)
        {
            return val.Display(DisplayProperty.Name) as string;
        }

        /// <summary>
        /// 获取枚举说明
        /// </summary>
        public static string DisplayShortName(this MemberInfo val)
        {
            return val.Display(DisplayProperty.ShortName) as string;
        }

        /// <summary>
        /// 获取枚举分组名称
        /// </summary>
        public static string DisplayGroupName(this MemberInfo val)
        {
            return val.Display(DisplayProperty.GroupName) as string;
        }

        /// <summary>
        /// 获取枚举水印信息
        /// </summary>
        public static string DisplayPrompt(this MemberInfo val)
        {
            return val.Display(DisplayProperty.Prompt) as string;
        }

        /// <summary>
        /// 获取枚举备注
        /// </summary>
        public static string DisplayDescription(this MemberInfo val)
        {
            return val.Display(DisplayProperty.Description) as string;
        }

        /// <summary>
        /// 获取枚举显示排序
        /// </summary>
        public static int? DisplayOrder(this MemberInfo val)
        {
            return val.Display(DisplayProperty.Order) as int?;
        }


        /// <summary>
        /// Checks whether <paramref name="givenType"/> implements/inherits <paramref name="genericType"/>.
        /// </summary>
        /// <param name="givenType">Type to check</param>
        /// <param name="genericType">Generic type</param>
        public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
        {
            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }

            foreach (var interfaceType in givenType.GetInterfaces())
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == genericType)
                {
                    return true;
                }
            }

            if (givenType.BaseType == null)
            {
                return false;
            }

            return IsAssignableToGenericType(givenType.BaseType, genericType);
        }

        /// <summary>
        /// Gets a list of attributes defined for a class member and it's declaring type including inherited attributes.
        /// </summary>
        /// <param name="memberInfo">MemberInfo</param>
        public static List<object> GetAttributesOfMemberAndDeclaringType(this MemberInfo memberInfo)
        {
            var attributeList = new List<object>();

            attributeList.AddRange(memberInfo.GetCustomAttributes(true));

            //Add attributes on the class
            if (memberInfo.DeclaringType != null)
            {
                attributeList.AddRange(memberInfo.DeclaringType.GetCustomAttributes(true));
            }

            return attributeList;
        }

        /// <summary>
        /// Gets a list of attributes defined for a class member and it's declaring type including inherited attributes.
        /// </summary>
        /// <typeparam name="TAttribute">Type of the attribute</typeparam>
        /// <param name="memberInfo">MemberInfo</param>
        public static List<TAttribute> GetAttributesOfMemberAndDeclaringType<TAttribute>(this MemberInfo memberInfo)
            where TAttribute : Attribute
        {
            var attributeList = new List<TAttribute>();

            //Add attributes on the member
            if (memberInfo.IsDefined(typeof(TAttribute), true))
            {
                attributeList.AddRange(memberInfo.GetCustomAttributes(typeof(TAttribute), true).Cast<TAttribute>());
            }

            //Add attributes on the class
            if (memberInfo.DeclaringType != null && memberInfo.DeclaringType.IsDefined(typeof(TAttribute), true))
            {
                attributeList.AddRange(memberInfo.DeclaringType.GetCustomAttributes(typeof(TAttribute), true).Cast<TAttribute>());
            }

            return attributeList;
        }

        /// <summary>
        /// Tries to gets an of attribute defined for a class member and it's declaring type including inherited attributes.
        /// Returns default value if it's not declared at all.
        /// </summary>
        /// <typeparam name="TAttribute">Type of the attribute</typeparam>
        /// <param name="memberInfo">MemberInfo</param>
        /// <param name="defaultValue">Default value (null as default)</param>
        public static TAttribute GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<TAttribute>(this MemberInfo memberInfo, TAttribute defaultValue = default)
            where TAttribute : Attribute
        {
            //Get attribute on the member
            if (memberInfo.IsDefined(typeof(TAttribute), true))
            {
                return memberInfo.GetCustomAttributes(typeof(TAttribute), true).Cast<TAttribute>().First();
            }

            //Get attribute from class
            if (memberInfo.DeclaringType != null && memberInfo.DeclaringType.IsDefined(typeof(TAttribute), true))
            {
                return memberInfo.DeclaringType.GetCustomAttributes(typeof(TAttribute), true).Cast<TAttribute>().First();
            }

            return defaultValue;
        }

        /// <summary>
        /// Tries to gets an of attribute defined for a class member and it's declaring type including inherited attributes.
        /// Returns default value if it's not declared at all.
        /// </summary>
        /// <typeparam name="TAttribute">Type of the attribute</typeparam>
        /// <param name="memberInfo">MemberInfo</param>
        /// <param name="defaultValue">Default value (null as default)</param>
        public static TAttribute GetSingleAttributeOrDefault<TAttribute>(this MemberInfo memberInfo, TAttribute defaultValue = default)
            where TAttribute : Attribute
        {
            //Get attribute on the member
            if (memberInfo.IsDefined(typeof(TAttribute), true))
            {
                return memberInfo.GetCustomAttributes(typeof(TAttribute), true).Cast<TAttribute>().First();
            }

            return defaultValue;
        }


        #region TypeString

        /// <summary>
        /// 获取类型名称，优化泛型
        /// </summary>
        public static string GetTypeName(this Type type, bool withNamespace)
        {
            if (!type.GetTypeInfo().IsGenericType)
            {
                return type.GetFullName(withNamespace)
                        .ReplacePlusWithDotInNestedTypeName();
            }

            return type.GetGenericTypeDefinition()
                    .GetFullName(withNamespace)
                    .ReplacePlusWithDotInNestedTypeName()
                    .ReplaceGenericParametersInGenericTypeName(type, withNamespace);
        }

        private static Type[] GetAllGenericArguments(this TypeInfo type)
        {
            return type.GenericTypeArguments.Length > 0 ? type.GenericTypeArguments : type.GenericTypeParameters;
        }
        
        private static string GetFullName(this Type type, bool withNamespace)
        {
            if (withNamespace)
            {
                return type.FullName;
            }

            if (type.IsGenericParameter)
            {
                return type.Name;
            }

            const int dotLength = 1;
            return string.IsNullOrEmpty(type.Namespace)
                ? type.FullName
                : type.FullName.Substring(type.Namespace.Length + dotLength);
        }

        private static string ReplacePlusWithDotInNestedTypeName(this string typeName)
        {
            return typeName.Replace('+', '.');
        }

        private static string ReplaceGenericParametersInGenericTypeName(this string typeName, Type type, bool withnamespace)
        {
            var genericArguments = type.GetTypeInfo().GetAllGenericArguments();

            const string regexForGenericArguments = @"`[1-9]\d*";

            var rgx = new Regex(regexForGenericArguments);

            typeName = rgx.Replace(typeName, match =>
            {
                var currentGenericArgumentNumbers = int.Parse(match.Value.Substring(1));
                var currentArguments = string.Join(",", genericArguments.Take(currentGenericArgumentNumbers).Select(t => t.GetTypeName(withnamespace)));
                genericArguments = genericArguments.Skip(currentGenericArgumentNumbers).ToArray();
                return string.Concat("<", currentArguments, ">");
            });

            return typeName;
        }

        #endregion
    }
}
