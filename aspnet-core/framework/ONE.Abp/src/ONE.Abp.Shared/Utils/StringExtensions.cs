﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using Volo.Abp;
using Formatting = Newtonsoft.Json.Formatting;

namespace  ONE.Abp.Shared.Utils
{
    public static partial class StringExtension
    {
        #region 正则表达式

        /// <summary>
        /// 指示所指定的正则表达式在指定的输入字符串中是否找到了匹配项
        /// </summary>
        /// <param name="value">要搜索匹配项的字符串</param>
        /// <param name="pattern">要匹配的正则表达式模式</param>
        /// <param name="isContains">是否包含，否则全匹配</param>
        /// <returns>如果正则表达式找到匹配项，则为 true；否则，为 false</returns>
        public static bool IsMatch(this string value, string pattern, bool isContains = true)
        {
            if (value == null)
            {
                return false;
            }

            return isContains
                ? Regex.IsMatch(value, pattern)
                : Regex.Match(value, pattern).Success;
        }

        /// <summary>
        /// 在指定的输入字符串中搜索指定的正则表达式的第一个匹配项
        /// </summary>
        /// <param name="value">要搜索匹配项的字符串</param>
        /// <param name="pattern">要匹配的正则表达式模式</param>
        /// <returns>一个对象，包含有关匹配项的信息</returns>
        public static string Match(this string value, string pattern)
        {
            if (value == null)
            {
                return null;
            }

            return Regex.Match(value, pattern).Value;
        }

        /// <summary>
        /// 在指定的输入字符串中匹配并替换符合指定正则表达式的子串
        /// </summary>
        public static string ReplaceRegex(this string value, string pattern, string replacement)
        {
            if (value == null)
            {
                return null;
            }

            return Regex.Replace(value, pattern, replacement);
        }

        /// <summary>
        /// 在指定的输入字符串中搜索指定的正则表达式的所有匹配项的字符串集合
        /// </summary>
        /// <param name="value"> 要搜索匹配项的字符串 </param>
        /// <param name="pattern"> 要匹配的正则表达式模式 </param>
        /// <returns> 一个集合，包含有关匹配项的字符串值 </returns>
        public static IEnumerable<string> Matches(this string value, string pattern)
        {
            if (value == null)
            {
                return new string[] { };
            }

            var matches = Regex.Matches(value, pattern);
            return from Match match in matches select match.Value;
        }

        /// <summary>
        /// 在指定的输入字符串中匹配第一个数字字符串
        /// </summary>
        public static string MatchFirstNumber(this string value)
        {
            var matches = Regex.Matches(value, @"\d+");
            if (matches.Count == 0)
            {
                return string.Empty;
            }

            return matches[0].Value;
        }

        /// <summary>
        /// 在指定字符串中匹配最后一个数字字符串
        /// </summary>
        public static string MatchLastNumber(this string value)
        {
            var matches = Regex.Matches(value, @"\d+");
            if (matches.Count == 0)
            {
                return string.Empty;
            }

            return matches[matches.Count - 1].Value;
        }

        /// <summary>
        /// 在指定字符串中匹配所有数字字符串
        /// </summary>
        public static IEnumerable<string> MatchNumbers(this string value)
        {
            return Matches(value, @"\d+");
        }

        /// <summary>
        /// 检测指定字符串中是否包含数字
        /// </summary>
        public static bool IsMatchNumber(this string value)
        {
            return IsMatch(value, @"\d");
        }

        /// <summary>
        /// 检测指定字符串是否全部为数字并且长度等于指定长度
        /// </summary>
        public static bool IsMatchNumber(this string value, int length)
        {
            var regex = new Regex(@"^\d{" + length + "}$");
            return regex.IsMatch(value);
        }

        /// <summary>
        /// 截取指定字符串之间的字符串
        /// </summary>
        /// <param name="source"></param>
        /// <param name="startString">起始字符串</param>
        /// <param name="endStrings">结束字符串，可多个</param>
        /// <returns>返回的中间字符串</returns>
        public static string Substring(this string source, string startString, params string[] endStrings)
        {
            if (source.IsMissing())
            {
                return string.Empty;
            }

            var startIndex = 0;
            if (!string.IsNullOrEmpty(startString))
            {
                startIndex = source.IndexOf(startString, StringComparison.OrdinalIgnoreCase);
                if (startIndex < 0)
                {
                    throw new InvalidOperationException($"在源字符串中无法找到“{startString}”的子串位置");
                }

                startIndex += startString.Length;
            }

            var endIndex = source.Length;
            endStrings = endStrings.OrderByDescending(m => m.Length).ToArray();
            foreach (var endString in endStrings)
            {
                if (string.IsNullOrEmpty(endString))
                {
                    endIndex = source.Length;
                    break;
                }

                endIndex = source.IndexOf(endString, startIndex, StringComparison.OrdinalIgnoreCase);
                if (endIndex < 0 || endIndex < startIndex)
                {
                    continue;
                }

                break;
            }

            if (endIndex < 0 || endIndex < startIndex)
            {
                throw new InvalidOperationException($"在源字符串中无法找到“{string.Join(",",endStrings)}”的子串位置");
            }

            var length = endIndex - startIndex;

            return source.Substring(startIndex, length);
        }

        /// <summary>
        /// 用正则表达式截取字符串
        /// </summary>
        public static string Substring2(this string source, string startString, string endString)
        {
            return source.Substring2(startString, endString, false);
        }

        /// <summary>
        /// 用正则表达式截取字符串
        /// </summary>
        public static string Substring2(this string source, string startString, string endString, bool containsEmpty)
        {
            if (source.IsMissing())
            {
                return string.Empty;
            }

            var inner = containsEmpty ? "\\s\\S" : "\\S";
            var result = source.Match($"(?<={startString})([{inner}]+?)(?={endString})");
            return result.IsMissing() ? null : result;
        }

        /// <summary>
        /// 是否电子邮件
        /// </summary>
        public static bool IsEmail(this string value)
        {
            const string pattern = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
            return value.IsMatch(pattern);
        }

        /// <summary>
        /// 是否是IP地址
        /// </summary>
        public static bool IsIpAddress(this string value)
        {
            const string pattern =
                @"^((?:(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d))))$";
            return value.IsMatch(pattern);
        }

        /// <summary>
        /// 是否是整数
        /// </summary>
        public static bool IsNumeric(this string value)
        {
            const string pattern = @"^\-?[0-9]+$";
            return value.IsMatch(pattern);
        }

        /// <summary>
        /// 是否是Unicode字符串
        /// </summary>
        public static bool IsUnicode(this string value)
        {
            const string pattern = @"^[\u4E00-\u9FA5\uE815-\uFA29]+$";
            return value.IsMatch(pattern);
        }

        /// <summary>
        /// 是否Url字符串
        /// </summary>
        public static bool IsUrl(this string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value) || value.Contains(' '))
                {
                    return false;
                }

                var uri = new Uri(value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 是否身份证号，验证如下3种情况：
        /// 1.身份证号码为15位数字；
        /// 2.身份证号码为18位数字；
        /// 3.身份证号码为17位数字+1个字母
        /// </summary>
        public static bool IsIdentityCardId(this string value)
        {
            if (value.Length != 15 && value.Length != 18)
            {
                return false;
            }

            Regex regex;
            string[] array;
            if (value.Length == 15)
            {
                regex = new Regex(@"^(\d{6})(\d{2})(\d{2})(\d{2})(\d{3})_");
                if (!regex.Match(value).Success)
                {
                    return false;
                }

                array = regex.Split(value);
                return DateTime.TryParse(string.Format("{0}-{1}-{2}", "19" + array[2], array[3], array[4]), out _);
            }

            regex = new Regex(@"^(\d{6})(\d{4})(\d{2})(\d{2})(\d{3})([0-9Xx])$");
            if (!regex.Match(value).Success)
            {
                return false;
            }

            array = regex.Split(value);
            if (!DateTime.TryParse(string.Format("{0}-{1}-{2}", array[2], array[3], array[4]), out _))
            {
                return false;
            }

            //校验最后一位
            var chars = value.ToCharArray().Select(m => m.ToString()).ToArray();
            int[] weights = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
            var sum = 0;
            for (var i = 0; i < 17; i++)
            {
                var num = int.Parse(chars[i]);
                sum += num * weights[i];
            }

            var mod = sum % 11;
            var vCode = "10X98765432"; // 检验码字符串
            var last = vCode.ToCharArray().ElementAt(mod).ToString();
            return chars.Last().ToUpper() == last;
        }

        /// <summary>
        /// 是否手机号码
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isRestrict">是否按严格格式验证</param>
        public static bool IsMobileNumber(this string value, bool isRestrict = false)
        {
            var pattern = isRestrict ? @"^[1][3-8]\d{9}$" : @"^[1]\d{10}$";
            return value.IsMatch(pattern);
        }

        #endregion

        #region 其他操作

        /// <summary>
        /// 判断指定的字符串不是 null、空。
        /// </summary>
        public static bool IsNotNullOrEmpty(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// 判断指定的字符串不是 null、空或者仅由空白字符组成
        /// </summary>
        public static bool IsNotNullOrWhiteSpace(this string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 指示指定的字符串是 null、空或者仅由空白字符组成
        /// </summary>
        public static bool IsMissing(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// 为指定格式的字符串填充相应对象来生成字符串
        /// </summary>
        /// <param name="format">字符串格式，占位符以{n}表示</param>
        /// <param name="args">用于填充占位符的参数</param>
        /// <returns>格式化后的字符串</returns>
        public static string FormatWith(this string format, params object[] args)
        {
            Check.NotNull(format, nameof(format));
            return string.Format(CultureInfo.CurrentCulture, format, args);
        }

        /// <summary>
        /// 将字符串反转
        /// </summary>
        /// <param name="value">要反转的字符串</param>
        public static string ReverseString(this string value)
        {
            Check.NotNull(value, nameof(value));
            return new string(value.Reverse().ToArray());
        }

        /// <summary>
        /// 单词复数变成单数形式
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string ToSingular(this string word)
        {
            var plural1 = new Regex("(?<keep>[^aeiou])ies$");
            var plural2 = new Regex("(?<keep>[aeiou]y)s$");
            var plural3 = new Regex("(?<keep>[sxzh])es$");
            var plural4 = new Regex("(?<keep>[^sxzhyu])s$");

            if (plural1.IsMatch(word))
            {
                return plural1.Replace(word, "${keep}y");
            }

            if (plural2.IsMatch(word))
            {
                return plural2.Replace(word, "${keep}");
            }

            if (plural3.IsMatch(word))
            {
                return plural3.Replace(word, "${keep}");
            }

            if (plural4.IsMatch(word))
            {
                return plural4.Replace(word, "${keep}");
            }

            return word;
        }

        /// <summary>
        /// 单词单数变成复数形式
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public static string ToPlural(this string word)
        {
            var plural1 = new Regex("(?<keep>[^aeiou])y$");
            var plural2 = new Regex("(?<keep>[aeiou]y)$");
            var plural3 = new Regex("(?<keep>[sxzh])$");
            var plural4 = new Regex("(?<keep>[^sxzhy])$");

            if (plural1.IsMatch(word))
            {
                return plural1.Replace(word, "${keep}ies");
            }

            if (plural2.IsMatch(word))
            {
                return plural2.Replace(word, "${keep}s");
            }

            if (plural3.IsMatch(word))
            {
                return plural3.Replace(word, "${keep}es");
            }

            if (plural4.IsMatch(word))
            {
                return plural4.Replace(word, "${keep}s");
            }

            return word;
        }

        /// <summary>
        /// 以指定字符串作为分隔符将指定字符串分隔成数组
        /// </summary>
        /// <param name="value">要分割的字符串</param>
        /// <param name="strSplit">字符串类型的分隔符</param>
        /// <param name="removeEmptyEntries">是否移除数据中元素为空字符串的项</param>
        /// <returns>分割后的数据</returns>
        public static string[] Split(this string value, string strSplit, bool removeEmptyEntries = false)
        {
            return value.Split(new[] { strSplit },
                removeEmptyEntries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
        }

        /// <summary>
        /// 支持汉字的字符串长度，汉字长度计为2
        /// </summary>
        /// <param name="value">参数字符串</param>
        /// <returns>当前字符串的长度，汉字长度为2</returns>
        public static int TextLength(this string value)
        {
            var ascii = new ASCIIEncoding();
            var tempLen = 0;
            var bytes = ascii.GetBytes(value);
            foreach (var b in bytes)
            {
                if (b == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }
            }

            return tempLen;
        }

        /// <summary>
        /// 从JSON中解析对像, 如果字符串为 null, 则返回 <typeparamref name="T"/> 的默认值.
        /// </summary>
        public static T FromJson<T>(this string json, JsonSerializerSettings settings = null)
        {
            if (json == null) return default;

            return JsonConvert.DeserializeObject<T>(json, settings);
        }


        /// <summary>
        /// 从JSON中解析对像, 如果字符串为 null, 则返回 <typeparamref name="T"/> 的默认值.
        /// </summary>
        public static object FromJson(this string json, Type t, JsonSerializerSettings settings = null)
        {
            if (json == null) return default;

            return JsonConvert.DeserializeObject(json, t, settings);
        }

        /// <summary>
        /// 将对像转换为Json字符串, 如果对像为 null, 则返回 null
        /// </summary>
        public static string ToJson(this object json, Formatting formatting = Formatting.None, JsonSerializerSettings settings = null)
        {
            if (json == null) return null;

            return JsonConvert.SerializeObject(json, formatting, settings);
        }

        /// <summary>
        /// 给URL添加查询参数
        /// </summary>
        /// <param name="url">URL字符串</param>
        /// <param name="queries">要添加的参数，形如："id=1,cid=2"</param>
        /// <returns></returns>
        public static string AddUrlQuery(this string url, params string[] queries)
        {
            foreach (var query in queries)
            {
                if (!url.Contains("?"))
                {
                    url += "?";
                }
                else if (!url.EndsWith("&"))
                {
                    url += "&";
                }

                url += query;
            }

            return url;
        }

        /// <summary>
        /// 获取URL中指定参数的值，不存在返回空字符串
        /// </summary>
        public static string GetUrlQuery(this string url, string key)
        {
            var uri = new Uri(url);
            var query = uri.Query;
            if (string.IsNullOrEmpty(query))
            {
                return string.Empty;
            }

            query = query.TrimStart('?');
            var dict = (from m in query.Split("&", true)
                        let strs = m.Split("=")
                        select new KeyValuePair<string, string>(strs[0], strs[1]))
                .ToDictionary(m => m.Key, m => m.Value);
            if (dict.ContainsKey(key))
            {
                return dict[key];
            }

            return string.Empty;
        }

        /// <summary>
        /// 给URL添加 # 参数
        /// </summary>
        /// <param name="url">URL字符串</param>
        /// <param name="query">要添加的参数</param>
        /// <returns></returns>
        public static string AddHashFragment(this string url, string query)
        {
            Check.NotNull(url, nameof(url));
            Check.NotNull(query, nameof(query));

            if (!url.Contains("#"))
            {
                url += "#";
            }

            return url + query;
        }



        /// <summary>
        /// 将<see cref="byte"/>[]数组转换为Base64字符串
        /// </summary>
        public static string ToBase64String(this byte[] bytes)
        {
            Check.NotNull(bytes, nameof(bytes));

            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 将字符串转换为Base64字符串，默认编码为<see cref="Encoding.UTF8"/>
        /// </summary>
        /// <param name="source">正常的字符串</param>
        /// <param name="encoding">编码</param>
        /// <returns>Base64字符串</returns>
        public static string ToBase64String(this string source, Encoding encoding = null)
        {
            Check.NotNull(source, nameof(source));

            if (encoding == null) encoding = Encoding.UTF8;

            return Convert.ToBase64String(encoding.GetBytes(source));
        }

        /// <summary>
        /// 将Base64字符串转换为正常字符串，默认编码为<see cref="Encoding.UTF8"/>
        /// </summary>
        /// <param name="base64String">Base64字符串</param>
        /// <param name="encoding">编码</param>
        /// <returns>正常字符串</returns>
        public static string FromBase64String(this string base64String, Encoding encoding = null)
        {
            Check.NotNull(base64String, nameof(base64String));

            if (encoding == null) encoding = Encoding.UTF8;

            var bytes = Convert.FromBase64String(base64String);
            return encoding.GetString(bytes);
        }

        /// <summary>
        /// 将字符串进行UrlDecode解码
        /// </summary>
        /// <param name="source">待UrlDecode解码的字符串</param>
        /// <returns>UrlDecode解码后的字符串</returns>
        public static string ToUrlDecode(this string source)
        {
            Check.NotNull(source, nameof(source));

            return HttpUtility.UrlDecode(source);
        }

        /// <summary>
        /// 将字符串进行UrlEncode编码
        /// </summary>
        /// <param name="source">待UrlEncode编码的字符串</param>
        /// <returns>UrlEncode编码后的字符串</returns>
        public static string ToUrlEncode(this string source)
        {
            Check.NotNull(source, nameof(source));

            return HttpUtility.UrlEncode(source);
        }

        /// <summary>
        /// 将字符串进行HtmlDecode解码
        /// </summary>
        /// <param name="source">待HtmlDecode解码的字符串</param>
        /// <returns>HtmlDecode解码后的字符串</returns>
        public static string ToHtmlDecode(this string source)
        {
            Check.NotNull(source, nameof(source));

            return HttpUtility.HtmlDecode(source);
        }

        /// <summary>
        /// 将字符串进行HtmlEncode编码
        /// </summary>
        /// <param name="source">待HtmlEncode编码的字符串</param>
        /// <returns>HtmlEncode编码后的字符串</returns>
        public static string ToHtmlEncode(this string source)
        {
            Check.NotNull(source, nameof(source));

            return HttpUtility.HtmlEncode(source);
        }

        /// <summary>
        /// 将字符串转换为十六进制字符串，默认编码为<see cref="Encoding.UTF8"/>
        /// </summary>
        public static string ToHexString(this string source, Encoding encoding = null)
        {
            Check.NotNull(source, nameof(source));

            if (encoding == null) encoding = Encoding.UTF8;

            byte[] bytes = encoding.GetBytes(source);
            return bytes.ToHexString();
        }

        /// <summary>
        /// 将十六进制字符串转换为常规字符串，默认编码为<see cref="Encoding.UTF8"/>
        /// </summary>
        public static string FromHexString(this string hexString, Encoding encoding = null)
        {
            Check.NotNull(hexString, nameof(hexString));

            if (encoding == null) encoding = Encoding.UTF8;

            var bytes = hexString.ToHexBytes();
            return encoding.GetString(bytes);
        }

        /// <summary>
        /// 将byte[]编码为十六进制字符串
        /// </summary>
        /// <param name="bytes">byte[]数组</param>
        /// <returns>十六进制字符串</returns>
        public static string ToHexString(this byte[] bytes)
        {
            Check.NotNull(bytes, nameof(bytes));

            return bytes.Aggregate(string.Empty, (current, t) => current + t.ToString("X2"));
        }

        /// <summary>
        /// 将十六进制字符串转换为byte[]
        /// </summary>
        /// <param name="hexString">十六进制字符串</param>
        /// <returns>byte[]数组</returns>
        public static byte[] ToHexBytes(this string hexString)
        {
            hexString = hexString ?? "";
            hexString = hexString.Replace(" ", "");
            byte[] bytes = new byte[hexString.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return bytes;
        }

        /// <summary>
        /// 将字符串进行Unicode编码，变成形如“\u7f16\u7801”的形式
        /// </summary>
        /// <param name="source">要进行编号的字符串</param>
        public static string ToUnicodeString(this string source)
        {
            Check.NotNull(source, nameof(source));

            var regex = new Regex(@"[^\u0000-\u00ff]");
            return regex.Replace(source, m => string.Format(@"\u{0:x4}", (short)m.Value[0]));
        }

        /// <summary>
        /// 将形如“\u7f16\u7801”的Unicode字符串解码
        /// </summary>
        public static string FromUnicodeString(this string source)
        {
            var regex = new Regex(@"\\u([0-9a-fA-F]{4})", RegexOptions.Compiled);
            return regex.Replace(source,
                m =>
                {
                    short s;
                    if (short.TryParse(m.Groups[1].Value, NumberStyles.HexNumber, CultureInfo.InstalledUICulture,
                        out s))
                    {
                        return "" + (char)s;
                    }

                    return m.Value;
                });
        }

        /// <summary>
        /// 将驼峰字符串的第一个字符小写
        /// </summary>
        public static string LowerFirstChar(this string str)
        {
            if (string.IsNullOrEmpty(str) || !char.IsUpper(str[0]))
            {
                return str;
            }

            if (str.Length == 1)
            {
                return char.ToLower(str[0]).ToString();
            }

            return char.ToLower(str[0]) + str.Substring(1, str.Length - 1);
        }

        /// <summary>
        /// 将小驼峰字符串的第一个字符大写
        /// </summary>
        public static string UpperFirstChar(this string str)
        {
            if (string.IsNullOrEmpty(str) || !char.IsLower(str[0]))
            {
                return str;
            }

            if (str.Length == 1)
            {
                return char.ToUpper(str[0]).ToString();
            }

            return char.ToUpper(str[0]) + str.Substring(1, str.Length - 1);
        }

        /// <summary>
        /// 计算当前字符串与指定字符串的编辑距离(相似度)
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="target">目标字符串</param>
        /// <param name="similarity">输出相似度</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns>编辑距离</returns>
        public static int LevenshteinDistance(this string source, string target, out double similarity,
            bool ignoreCase = false)
        {
            if (string.IsNullOrEmpty(source))
            {
                if (string.IsNullOrEmpty(target))
                {
                    similarity = 1;
                    return 0;
                }

                similarity = 0;
                return target.Length;
            }

            if (string.IsNullOrEmpty(target))
            {
                similarity = 0;
                return source.Length;
            }

            string from, to;
            if (ignoreCase)
            {
                from = source;
                to = target;
            }
            else
            {
                from = source.ToLower();
                to = source.ToLower();
            }

            int m = from.Length, n = to.Length;
            int[,] mn = new int[m + 1, n + 1];
            for (int i = 0; i <= m; i++)
            {
                mn[i, 0] = i;
            }

            for (int j = 1; j <= n; j++)
            {
                mn[0, j] = j;
            }

            for (int i = 1; i <= m; i++)
            {
                char c = from[i - 1];
                for (int j = 1; j <= n; j++)
                {
                    if (c == to[j - 1])
                    {
                        mn[i, j] = mn[i - 1, j - 1];
                    }
                    else
                    {
                        mn[i, j] = Math.Min(mn[i - 1, j - 1], Math.Min(mn[i - 1, j], mn[i, j - 1])) + 1;
                    }
                }
            }

            int maxLength = Math.Max(m, n);
            similarity = (double)(maxLength - mn[m, n]) / maxLength;
            return mn[m, n];
        }

        /// <summary>
        /// 计算两个字符串的相似度，应用公式：相似度=kq*q/(kq*q+kr*r+ks*s)(kq>0,kr>=0,ka>=0)
        /// 其中，q是字符串1和字符串2中都存在的单词的总数，s是字符串1中存在，字符串2中不存在的单词总数，r是字符串2中存在，字符串1中不存在的单词总数. kq,kr和ka分别是q,r,s的权重，根据实际的计算情况，我们设kq=2，kr=ks=1.
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="target">目标字符串</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns>字符串相似度</returns>
        public static double GetSimilarityWith(this string source, string target, bool ignoreCase = false)
        {
            if (string.IsNullOrEmpty(source) && string.IsNullOrEmpty(target))
            {
                return 1;
            }

            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(target))
            {
                return 0;
            }

            const double kq = 2, kr = 1, ks = 1;
            char[] sourceChars = source.ToCharArray(), targetChars = target.ToCharArray();

            //获取交集数量
            int q = sourceChars.Intersect(targetChars).Count(), s = sourceChars.Length - q, r = targetChars.Length - q;
            return kq * q / (kq * q + kr * r + ks * s);
        }

        /// <summary>
        /// 标准化Path字符串,将 \\ 转换为 /
        /// </summary>
        /// <param name="path">Path字符串</param>
        public static string NormalizePath(this string path)
        {
            return path.Replace('\\', '/');
        }

        /// <summary>
        /// (Pascal) 命名法 的字符串 改为 短横线分隔式命名
        /// 例如UserName => user-name
        /// </summary>
        public static string PascalToKebabCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return Regex.Replace(
                    value,
                    "(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z])",
                    "-$1",
                    RegexOptions.Compiled)
                .Trim()
                .ToLower();
        }

        #endregion
    }
}
