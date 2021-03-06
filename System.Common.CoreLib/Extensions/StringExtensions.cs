﻿using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

// ReSharper disable once CheckNamespace
namespace System
{
    public static partial class StringExtensions
    {
        #region System.String 内置函数增强型扩展(用于取代原有函数)

        #region IndexOf

        /// <summary>
        /// 报告指定 Unicode 字符在此字符串中的第一个匹配项的从零开始的索引。
        /// <para>注意：如果 value 仅为英文字符或数字符号，则使用原函数 <see cref="string.IndexOf(char)"/></para>
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value">要查找的 Unicode 字符。</param>
        /// <returns>如果找到该字符，则为 value 的从零开始的索引位置；如果未找到，则为 -1。</returns>
        public static int IndexOf_(this string str, char value) => DI.IsRunningOnMono ? str.IndexOf(value) : str.IndexOf(value, StringComparison.Ordinal);

        /// <summary>
        /// 报告指定 Unicode 字符在此字符串中的第一个匹配项的从零开始的索引。
        /// <para>注意：如果 value 仅为英文字符或数字符号，则使用原函数 <see cref="string.IndexOf(char)"/></para>
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value">要查找的 Unicode 字符。</param>
        /// <returns>如果找到该字符，则为 value 的从零开始的索引位置；如果未找到，则为 -1。</returns>
        public static int IndexOf_Nullable(this string? str, char value)
        {
            if (str == null) return -1;
            return IndexOf_(str, value);
        }

        /// <summary>
        /// 报告指定字符串在此实例中的第一个匹配项的从零开始的索引。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value">要搜寻的字符串。</param>
        /// <returns>如果找到该字符串，则为 value 的从零开始的索引位置；如果未找到该字符串，则为 -1。 如果 value 为 Empty，则返回值为 0。</returns>
        public static int IndexOf_(this string str, string value) => str.IndexOf(value, StringComparison.Ordinal);

        /// <summary>
        /// 报告指定字符串在此实例中的第一个匹配项的从零开始的索引。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value">要搜寻的字符串。</param>
        /// <returns>如果找到该字符串，则为 value 的从零开始的索引位置；如果未找到该字符串，则为 -1。 如果 value 为 Empty，则返回值为 0。</returns>
        public static int IndexOf_Nullable(this string? str, string value)
        {
            if (str == null) return -1;
            return IndexOf_(str, value);
        }

        #endregion

        #region Contains

        /// <summary>
        /// 返回一个值，该值指示指定的字符是否出现在此字符串中。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value">要查找的字符。</param>
        /// <returns>如果 value 参数在此字符串中出现，则为 true；否则为 false。</returns>
        public static bool Contains_(this string str, char value) => str.Contains(value, StringComparison.Ordinal);

        /// <summary>
        /// 返回一个值，该值指示指定的字符是否出现在此字符串中。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value">要查找的字符。</param>
        /// <returns>如果 value 参数在此字符串中出现，则为 true；否则为 false。</returns>
        public static bool Contains_Nullable(this string? str, char value)
        {
            if (str == null) return false;
            return Contains_(str, value);
        }

        /// <summary>
        /// 返回一个值，该值指示指定的子串是否出现在此字符串中。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value">要搜寻的字符串。</param>
        /// <returns>如果 true 参数出现在此字符串中，或者 value 为空字符串 ("")，则为 value；否则为 false。</returns>
        public static bool Contains_(this string str, string value) => str.Contains(value, StringComparison.Ordinal);

        /// <summary>
        /// 返回一个值，该值指示指定的子串是否出现在此字符串中。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value">要搜寻的字符串。</param>
        /// <returns>如果 true 参数出现在此字符串中，或者 value 为空字符串 ("")，则为 value；否则为 false。</returns>
        public static bool Contains_Nullable(this string? str, string value)
        {
            if (str == null) return false;
            return Contains_(str, value);
        }

        #endregion

        #endregion

        /// <summary>
        /// 将版本号字符串转换为 <see cref="Version"/> 对象，传入的字符串不能为 <see langword="null"/>
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static Version? VersionTryParse(string version)
        {
            var versionChars = GetVersion(version).ToArray();
            if (versionChars.Any() && Version.TryParse(new string(versionChars), out var versionObj))
            {
                return versionObj;
            }
            static IEnumerable<char> GetVersion(string s)
            {
                var dianCount = 0;
                var findChar = false;
                foreach (var item in s)
                {
                    var isDian = item == '.';
                    if (isDian)
                    {
                        if (dianCount++ >= 3)
                        {
                            yield break;
                        }
                    }
                    if (isDian || item >= '0' && item <= '9')
                    {
                        findChar = true;
                        yield return item;
                    }
                    else if (findChar)
                    {
                        yield break;
                    }
                }
            }
            return null;
        }

        #region TryParse

        /// <summary>
        /// 尝试将数字的字符串表示形式转换为它的等效 32 位有符号整数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int? TryParseInt32(this string value) => int.TryParse(value, out var temp) ? temp as int? : null;

        #endregion

        #region 数字字母判断

        /// <summary>
        /// 字符串是否是一个数字（可以零开头 无符号不能+-.）
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsDigital(this string input) => input?.All(IsDigital) ?? false;

        public static bool IsDigital(this char input) => input >= '0' && input <= '9';

        /// <summary>
        /// 字符串内含有数字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool HasDigital(this string input) => input?.Any(x => x >= '0' && x <= '9') ?? false;

        /// <summary>
        /// 字符串内含有小写字母
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool HasLowerLetter(this string input) => input?.Any(x => x >= 'a' && x <= 'z') ?? false;

        /// <summary>
        /// 字符串内含有大写字母
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool HasUpperLetter(this string input) => input?.Any(x => x >= 'A' && x <= 'Z') ?? false;

        /// <summary>
        /// 字符串内含有其他字符
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool HasOther(this string input) => input?.Any(x => !(x >= 'a' && x <= 'z' || x >= 'A' && x <= 'Z' || x >= '0' && x <= '9')) ?? false;

        #endregion

        /// <summary>
        /// 返回当前最后相对URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetLastRelativeUrl(this string url)
        {
            int index = url.LastIndexOf("/");

            if (index != -1)
            {
                return url.Substring(0, index) + "/";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 移除字符串内所有\r \n \t
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemovePattern(this string s)
            => s.Replace("\t", "").Replace("\r", "").Replace("\n", "");

        /// <summary>
        /// 获取两个字符串中间的字符串
        /// </summary>
        /// <param name="s">要处理的字符串，例 ABCD</param>
        /// <param name="left">第1个字符串，例 AB</param>
        /// <param name="right">第2个字符串，例 D</param>
        /// <param name="isContains">是否包含标志字符串</param>
        /// <returns>例 返回C</returns>
        public static string Substring(this string s, string left, string right, bool isContains = false)
        {
            int i1 = s.IndexOf(left);
            if (i1 < 0) // 找不到返回空
            {
                return "";
            }

            int i2 = s.IndexOf(right, i1 + left.Length); // 从找到的第1个字符串后再去找
            if (i2 < 0) // 找不到返回空
            {
                return "";
            }

            if (isContains) return s.Substring(i1, i2 - i1 + left.Length);
            else return s.Substring(i1 + left.Length, i2 - i1 - left.Length);
        }

        /// <summary>
        /// Converts a wildcard to a regex.
        /// </summary>
        /// <param name="pattern">The wildcard pattern to convert.</param>
        /// <returns>A regex equivalent of the given wildcard.</returns>
        public static bool IsWildcard(this string str, string pattern)
            => Regex.IsMatch(str, "^" + Regex.Escape(pattern)
                .Replace("\\*", ".*")
                .Replace("\\?", ".") + "$");

        #region Compressor

        /// <summary>
        /// Compresses the string.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string? CompressString(this string? text)
        {
            if (string.IsNullOrEmpty(text)) return null;
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            var memoryStream = new MemoryStream();
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                gZipStream.Write(buffer, 0, buffer.Length);
            }

            memoryStream.Position = 0;

            var compressedData = new byte[memoryStream.Length];
            memoryStream.Read(compressedData, 0, compressedData.Length);

            var gZipBuffer = new byte[compressedData.Length + 4];
            Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
            return Convert.ToBase64String(gZipBuffer);
        }

        /// <summary>
        /// Decompresses the string.
        /// </summary>
        /// <param name="compressedText">The compressed text.</param>
        /// <returns></returns>
        public static string? DecompressString(this string? compressedText)
        {
            if (string.IsNullOrEmpty(compressedText)) return null;
            byte[] gZipBuffer = Convert.FromBase64String(compressedText);
            using var memoryStream = new MemoryStream();
            int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
            memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

            var buffer = new byte[dataLength];

            memoryStream.Position = 0;
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
            {
                gZipStream.Read(buffer, 0, buffer.Length);
            }

            return Encoding.UTF8.GetString(buffer);
        }

        #endregion

#if DEBUG

        [Obsolete("use GetLastRelativeUrl", true)]
        public static string GetCurrentPath(this string url) => GetCurrentPath(url);

#endif
    }
}