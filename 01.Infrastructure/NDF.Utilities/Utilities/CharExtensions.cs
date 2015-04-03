using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Utilities
{
    /// <summary>
    /// 提供一组对 UTF-16 字符对象 <see cref="System.Char"/> 操作方法的扩展。
    /// </summary>
    public static class CharExtensions
    {
        /// <summary>
        /// 判断指定的字符为空但不是换行符。
        /// </summary>
        /// <param name="_this">被判断的字符。</param>
        /// <returns>如果 <paramref name="_this"/> 为空字符、Null 并且不是 回车符、换行符、行分隔符和段分隔符，则返回 true，否则返回 false。</returns>
        public static bool IsNonNewLineWhitespace(this char _this)
        {
            return Char.IsWhiteSpace(_this) && !IsNewLine(_this);
        }

        /// <summary>
        /// 判断指定的字符是否为一个换行符。
        /// </summary>
        /// <param name="_this">被判断的字符。</param>
        /// <returns>如果 <paramref name="_this"/> 为 回车符、换行符、行分隔符和段分隔符，则返回 true，否则返回 false。</returns>
        public static bool IsNewLine(this char _this)
        {
            return _this == 0x000d          // 回车
                      || _this == 0x000a    // 换行
                      || _this == 0x2028    // 行分隔符
                      || _this == 0x2029;   // 段分隔符
        }

        /// <summary>
        /// 判断指定的字符是否为一个普通字符(大写字母、小写字母、数字或者下划线)。
        /// </summary>
        /// <param name="_this"></param>
        /// <returns>如果 <paramref name="_this"/> 为一个普通字符(大写字母、小写字母、数字或者下划线)，则返回 true，否则返回 false。</returns>
        public static bool IsNormal(this char _this)
        {
            return (_this >= 48 && _this <= 57) || (_this >= 65 && _this <= 90) || _this == 95 || (_this >= 97 && _this <= 122);
        }
    }
}
