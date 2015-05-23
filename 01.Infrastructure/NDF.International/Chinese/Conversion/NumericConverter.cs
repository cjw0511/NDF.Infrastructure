using NDF.International.Formatting;
using System;
using System.Globalization;

namespace NDF.International.Chinese.Formatting
{
    /// <summary>
    /// 提供一个工具类进行数值和中文之间的相互转换。
    /// </summary>
    public static class NumericConverter
    {
        /// <summary>
        /// 在将数值转换成区域语言表现形式的字符串时所使用的区域语言名称。
        /// </summary>
        public const string _CultureInfo_Name = "zh-CN";

        private static CultureInfo _culture = new CultureInfo(_CultureInfo_Name);


        /// <summary>
        /// 将数值转换成指定格式的中文字符串表现形式。
        /// </summary>
        /// <param name="numeric">被转换的数值。</param>
        /// <param name="direction">返回结果的格式。</param>
        /// <returns>返回 <paramref name="numeric"/> 在 <paramref name="direction"/> 格式下等效的中文字符串。</returns>
        /// <exception cref="ArgumentOutOfRangeException">当参数 <paramref name="numeric"/> 值小于 -9.2233720368547758E+18 或大于 1.8446744073709552E+19 时，将抛出该异常。</exception>
        public static string Convert(double numeric, NumericConversionDirection direction)
        {
            return InternalConvert(numeric, direction);
        }

        /// <summary>
        /// 将数值转换成指定格式的中文字符串表现形式。
        /// </summary>
        /// <param name="numeric">被转换的数值。</param>
        /// <param name="direction">返回结果的格式。</param>
        /// <returns>返回 <paramref name="numeric"/> 在 <paramref name="direction"/> 格式下等效的中文字符串。</returns>
        /// <exception cref="ArgumentOutOfRangeException">当参数 <paramref name="numeric"/> 值小于 -9.2233720368547758E+18 或大于 1.8446744073709552E+19 时，将抛出该异常。</exception>
        public static string Convert(float numeric, NumericConversionDirection direction)
        {
            return InternalConvert(numeric, direction);
        }

        /// <summary>
        /// 将数值转换成指定格式的中文字符串表现形式。
        /// </summary>
        /// <param name="numeric">被转换的数值。</param>
        /// <param name="direction">返回结果的格式。</param>
        /// <returns>返回 <paramref name="numeric"/> 在 <paramref name="direction"/> 格式下等效的中文字符串。</returns>
        public static string Convert(int numeric, NumericConversionDirection direction)
        {
            return InternalConvert(numeric, direction);
        }

        /// <summary>
        /// 将数值转换成指定格式的中文字符串表现形式。
        /// </summary>
        /// <param name="numeric">被转换的数值。</param>
        /// <param name="direction">返回结果的格式。</param>
        /// <returns>返回 <paramref name="numeric"/> 在 <paramref name="direction"/> 格式下等效的中文字符串。</returns>
        public static string Convert(uint numeric, NumericConversionDirection direction)
        {
            return InternalConvert(numeric, direction);
        }

        /// <summary>
        /// 将数值转换成指定格式的中文字符串表现形式。
        /// </summary>
        /// <param name="numeric">被转换的数值。</param>
        /// <param name="direction">返回结果的格式。</param>
        /// <returns>返回 <paramref name="numeric"/> 在 <paramref name="direction"/> 格式下等效的中文字符串。</returns>
        public static string Convert(long numeric, NumericConversionDirection direction)
        {
            return InternalConvert(numeric, direction);
        }

        /// <summary>
        /// 将数值转换成指定格式的中文字符串表现形式。
        /// </summary>
        /// <param name="numeric">被转换的数值。</param>
        /// <param name="direction">返回结果的格式。</param>
        /// <returns>返回 <paramref name="numeric"/> 在 <paramref name="direction"/> 格式下等效的中文字符串。</returns>
        public static string Convert(ulong numeric, NumericConversionDirection direction)
        {
            return InternalConvert(numeric, direction);
        }

        /// <summary>
        /// 将数值转换成指定格式的中文字符串表现形式。
        /// </summary>
        /// <param name="numeric">被转换的数值。</param>
        /// <param name="direction">返回结果的格式。</param>
        /// <returns>返回 <paramref name="numeric"/> 在 <paramref name="direction"/> 格式下等效的中文字符串。</returns>
        public static string Convert(short numeric, NumericConversionDirection direction)
        {
            return InternalConvert(numeric, direction);
        }

        /// <summary>
        /// 将数值转换成指定格式的中文字符串表现形式。
        /// </summary>
        /// <param name="numeric">被转换的数值。</param>
        /// <param name="direction">返回结果的格式。</param>
        /// <returns>返回 <paramref name="numeric"/> 在 <paramref name="direction"/> 格式下等效的中文字符串。</returns>
        public static string Convert(ushort numeric, NumericConversionDirection direction)
        {
            return InternalConvert(numeric, direction);
        }

        /// <summary>
        /// 将数值转换成指定格式的中文字符串表现形式。
        /// </summary>
        /// <param name="numeric">被转换的数值。</param>
        /// <param name="direction">返回结果的格式。</param>
        /// <returns>返回 <paramref name="numeric"/> 在 <paramref name="direction"/> 格式下等效的中文字符串。</returns>
        public static string Convert(sbyte numeric, NumericConversionDirection direction)
        {
            return InternalConvert(numeric, direction);
        }

        /// <summary>
        /// 将数值转换成指定格式的中文字符串表现形式。
        /// </summary>
        /// <param name="numeric">被转换的数值。</param>
        /// <param name="direction">返回结果的格式。</param>
        /// <returns>返回 <paramref name="numeric"/> 在 <paramref name="direction"/> 格式下等效的中文字符串。</returns>
        public static string Convert(byte numeric, NumericConversionDirection direction)
        {
            return InternalConvert(numeric, direction);
        }

        /// <summary>
        /// 将数值转换成指定格式的中文字符串表现形式。
        /// </summary>
        /// <param name="numeric">被转换的数值。</param>
        /// <param name="direction">返回结果的格式。</param>
        /// <returns>返回 <paramref name="numeric"/> 在 <paramref name="direction"/> 格式下等效的中文字符串。</returns>
        /// <exception cref="ArgumentOutOfRangeException">当参数 <paramref name="numeric"/> 值小于 -9.2233720368547758E+18 或大于 1.8446744073709552E+19 时，将抛出该异常。</exception>
        public static string Convert(decimal numeric, NumericConversionDirection direction)
        {
            return InternalConvert(numeric, direction);
        }


        internal static string InternalConvert(object arg, NumericConversionDirection direction)
        {
            string format = GetFormatString(direction);
            return EastAsiaNumericFormatter.FormatWithCulture(format, arg, null, _culture);
        }

        internal static string GetFormatString(NumericConversionDirection direction)
        {
            switch (direction)
            {
                case NumericConversionDirection.LowerCase:
                    return "L";
                case NumericConversionDirection.UpperCase:
                    return "Ln";
                case NumericConversionDirection.Currency:
                    return "Lc";
                default:
                    return "Lt";
            }
        }

    }
}
