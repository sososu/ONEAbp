using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ONE.Abp.Shared.Utils
{
    /// <summary>
    /// 枚举工具类
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取指定枚举类型的所有值
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        public static IList<Enum> GetEnums(Type enumType)
        {
            var rs = new List<Enum>();
            
            if (enumType.IsNullableType())
            {
                enumType = enumType.GetTypeOfNullable();
                rs.Add(null);
            }

            var enums = Enum.GetValues(enumType).Cast<Enum>();
            rs.AddRange(enums);
            return rs;
        }
        
        /// <summary>
        /// 获取指定枚举类型的所有值
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        public static IList<TEnum> GetEnums<TEnum>()
        {
            var rs = new List<TEnum>();

            var enumType = typeof(TEnum);
            if (enumType.IsNullableType())
            {
                enumType = enumType.GetTypeOfNullable();
                rs.Add(default);
            }

            var enums = Enum.GetValues(enumType).Cast<TEnum>();
            rs.AddRange(enums);
            return rs;
        }

        /// <summary>
        /// 判断位域是否为指定的值
        /// </summary>
        public static bool HasFlag(this Enum self, ulong value)
        {
            return (Convert.ToUInt64(self) & value) == value;
        }

        /// <summary>
        /// 判断位域是否为指定的值
        /// </summary>
        public static bool HasFlagX(this Enum self, Enum value)
        {
            return self.HasFlag(Convert.ToUInt64(value));
        }

        /// <summary>
        /// 返回枚举包含的位域项
        /// </summary>
        /// <param name="value">要拆分的枚举。</param>
        /// <param name="distinct">是否去除重复的值，当枚举有包含其它枚举的时候。</param>
        public static IEnumerable<TEnum> GetEnumFlags<TEnum>(this Enum value, bool distinct = false)
        {
            return value.GetEnumFlags().Cast<TEnum>().ToList();
        }

        /// <summary>
        /// 返回枚举包含的位域项
        /// </summary>
        /// <param name="value">要拆分的枚举。</param>
        /// <param name="distinct">是否去除重复的值，当枚举有包含其它枚举的时候。</param>
        public static IEnumerable<Enum> GetEnumFlags(this Enum value, bool distinct = false)
        {
            var enumType = value.GetType();
            if (!enumType.IsEnum)
                throw new ArgumentOutOfRangeException($"类型：{enumType.FullName} 不是枚举类型。");

            var values = enumType.GetEnumValues();
            var result = values.Cast<Enum>().Where(value.HasFlagX);

            if (!distinct)
                return result;

            var orderbyList = result.OrderByDescending(p => p).ToList();
            for (var i = orderbyList.Count - 1; i > 0; i--)
            {
                var item = orderbyList[i];
                if (orderbyList.Any(p => !p.Equals(item) && p.HasFlagX(item)))
                    orderbyList.Remove(item);
            }

            orderbyList.Reverse();
            return orderbyList;
        }
        
        /// <summary>
        /// key值是否在枚举中定义
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsDefined<TEnum>(int key)
        {
            return Enum.IsDefined(typeof(TEnum), key);
        }

        /// <summary>
        /// 获取聚合后的枚举值
        /// </summary>
        public static TEnum GetAggregateEnumValue<TEnum>(this IEnumerable<TEnum> source)
            where TEnum : struct
        {
            var value = source.Select(p => Convert.ToInt64(p)).Aggregate(0L, (p, t) => p | t);

            return (TEnum)Enum.ToObject(typeof(TEnum), value);
        }

        /// <summary>
        /// 返回枚举定义的说明 没有则返回 null
        /// </summary>
        public static string Description(this Enum val)
        {
            var enumType = val.GetType();
            var text = val.ToString();
            var field = enumType.GetField(text);

            if (field == null)
                return val.ToString();

            return Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute desc ? desc.Description : text;
        }

        ///// <summary>
        ///// 获取枚举Display数据
        ///// </summary>
        //public static DisplayItem DisplayItem(this Enum self)
        //{
        //    return Mapper.Map<Enum, DisplayItem>(self);
        //}

        /// <summary>
        /// 获取枚举说明
        /// </summary>
        public static string DisplayName(this Enum val)
        {
            return val?.Display()?.GetName();
        }

        /// <summary>
        /// 获取枚举说明
        /// </summary>
        public static string[] DisplayNames(this Enum val)
        {
            return val?.GetEnumFlags().Select(p => p.DisplayName()).ToArray();
        }

        /// <summary>
        /// 获取枚举说明
        /// </summary>
        public static string DisplayShortName(this Enum val)
        {
            return val?.Display()?.GetShortName();
        }

        /// <summary>
        /// 获取枚举分组名称
        /// </summary>
        public static string DisplayGroupName(this Enum val)
        {
            return val?.Display()?.GetGroupName();
        }

        /// <summary>
        /// 获取枚举水印信息
        /// </summary>
        public static string DisplayPrompt(this Enum val)
        {
            return val?.Display()?.GetPrompt();
        }

        /// <summary>
        /// 获取枚举备注
        /// </summary>
        public static string DisplayDescription(this Enum val)
        {
            return val?.Display()?.GetDescription();
        }

        /// <summary>
        /// 获取枚举显示排序
        /// </summary>
        public static int? DisplayOrder(this Enum val)
        {
            return val?.Display()?.GetOrder();
        }

        private static readonly ConcurrentDictionary<Enum, DisplayAttribute> _cacheDisplayAttributes =
            new ConcurrentDictionary<Enum, DisplayAttribute>();

        /// <summary>
        /// 获取枚举指定的显示内容
        /// </summary>
        public static DisplayAttribute Display(this Enum val)
        {
            var display = _cacheDisplayAttributes.GetOrAdd(val, v =>
            {
                var enumType = val.GetType();
                var text = val.ToString();
                var field = enumType.GetField(text);

                return field?.GetAttribute<DisplayAttribute>();
            });

            return display;
        }

        /// <summary>
        /// 返回位域枚举定义的说明
        /// </summary>
        /// <param name="val">枚举值</param>
        /// <param name="split">连接分隔符</param>
        public static string Descriptions(this Enum val, string split = " ")
        {
            if (split == null)
                split = string.Empty;

            if (Convert.ToUInt64(val) == 0)
                return val.Description();

            var enumType = val.GetType();
            var text = val.ToString();
            if (Attribute.IsDefined(enumType, typeof(FlagsAttribute)))
            {
                var vals = Enum.GetValues(enumType);
                var sb = new StringBuilder();

                foreach (Enum e in vals)
                {
                    if (Convert.ToUInt64(e) == 0)
                        continue;

                    if (val.HasFlag(e))
                    {
                        sb.Append(Attribute.GetCustomAttribute(enumType.GetField(e.ToString()), typeof(DescriptionAttribute)) is DescriptionAttribute description ? description.Description : text);
                        sb.Append(split);
                    }
                }
                return sb.ToString().TrimEnd(split.ToCharArray());
            }
            return val.Description();
        }

        /// <summary>
        /// 返回枚举的整型值
        /// </summary>
        public static T To<T>(this Enum val)
            where T : struct, Enum
        {
            var longValue = Convert.ToInt64(val);
            var convertType = typeof(T);
            var longType = typeof(long);

            var converter = TypeDescriptor.GetConverter(longType);
            if (converter.CanConvertTo(convertType))
                return (T)converter.ConvertTo(longValue, convertType);

            converter = TypeDescriptor.GetConverter(convertType);
            if (converter.CanConvertFrom(longType))
                return (T)converter.ConvertFrom(longValue);

            throw new InvalidOperationException($"无法从 {val} 转换为 {convertType}");
        }

        /// <summary>
        /// 转换字符串为指定的枚举
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">要转换的字符串</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <exception cref="ArgumentException"></exception>
        public static T ToEnum<T>(this string value, bool ignoreCase = true)
            where T : struct, Enum
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return (T) Enum.Parse(typeof(T), value, ignoreCase);
        }

        /// <summary>
        /// 转换字符串为指定的枚举，如果失败则使用默认值
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">要转换的字符串</param>
        /// <param name="defaultValue">默认值，如果转换失败使用</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        public static T TryToEnum<T>(this string value, T defaultValue, bool ignoreCase = true)
            where T : struct, Enum
        {
            if (Enum.TryParse<T>(value, ignoreCase, out var val))
                return val;

            return defaultValue;
        }


        /// <summary>
        /// 转换字符串为指定的枚举，如果失败则使用默认值
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">要转换的字符串</param>
        /// <param name="defaultValue">默认值，如果转换失败使用</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        public static T? TryToEnum<T>(this string value, T? defaultValue = null, bool ignoreCase = true)
            where T : struct, Enum
        {
            if (Enum.TryParse<T>(value, ignoreCase, out var val))
                return val;

            return defaultValue;
        }

        /// <summary>
        /// 将具有整数值的指定对象转换为枚举成员。
        /// </summary>
        /// <exception cref="ArgumentNullException">value 为 null。</exception>
        /// <exception cref="ArgumentException">
        /// value 不是 System.SByte、System.Int16、System.Int32、System.Int64、System.Byte、System.UInt16、System.UInt32 和 System.UInt64 类型。
        /// </exception>
        public static TEnum ToEnum<TEnum>(this object value)
            where TEnum : Enum
        {
            var type = typeof(TEnum);

            if (value == null && type.IsNullableType())
                // ReSharper disable once ExpressionIsAlwaysNull
                return (TEnum)value;

            if (type.IsNullableType())
                type = type.GetGenericArguments()[0];

            return (TEnum)Enum.ToObject(type, value);
        }

        /// <summary>
        /// 将具有整数值的指定对象转换为枚举成员。
        /// </summary>
        /// <exception cref="ArgumentNullException">value 为 null。</exception>
        /// <exception cref="ArgumentException">
        /// value 不是 System.SByte、System.Int16、System.Int32、System.Int64、System.Byte、System.UInt16、System.UInt32 和 System.UInt64 类型。
        /// </exception>
        public static string ToEnumDisPlayName<TEnum>(this object value)
        {
            if (value == null)
                return null;

            var type = typeof(TEnum);
            if (type.IsNullableType())
                type = type.GetGenericArguments()[0];

            var enumValue = (Enum)Enum.ToObject(type, value);
            return enumValue.DisplayName();
        }
    }
}
