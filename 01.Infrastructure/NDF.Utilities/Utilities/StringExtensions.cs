using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace NDF.Utilities
{
    /// <summary>
    /// 提供一组对字符串对象 <see cref="System.String"/> 操作方法的扩展。
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 将 GUID 的字符串表示形式转换为等效的 <see cref="System.Guid"/> 结构。
        /// </summary>
        /// <param name="input">要转换的 GUID。</param>
        /// <returns>一个包含已分析的值的结构。</returns>
        public static Guid ToGuid(this string input)
        {
            return Guid.Parse(input);
        }

        /// <summary>
        /// 将 GUID 的字符串表示形式转换为等效的 <see cref="System.Guid"/> 结构，前提是该字符串采用的是指定格式。
        /// </summary>
        /// <param name="input">要转换的 GUID。</param>
        /// <param name="format">下列说明符之一，指示当解释 <paramref name="input"/> 时要使用的确切格式：“N”、“D”、“B”、“P”或“X”。</param>
        /// <returns>一个包含已分析的值的结构。</returns>
        public static Guid ToGuid(this string input, string format)
        {
            return Guid.ParseExact(input, format);
        }

        /// <summary>
        /// 将 GUID 的字符串表示形式转换为等效的 <see cref="System.Guid"/> 结构。
        /// </summary>
        /// <param name="input">要转换的 GUID。</param>
        /// <param name="result">将包含已分析的值的结构。 如果此方法返回 true，result 将包含有效的 <see cref="System.Guid"/>。 如果 <paramref name="result"/> 等于 <seealso cref="System.Guid.Empty"/>，则此方法将返回 false。</param>
        /// <returns>如果分析操作成功，则为 true；否则为 false。</returns>
        public static bool TryToGuid(this string input, out Guid result)
        {
            return Guid.TryParse(input, out result);
        }

        /// <summary>
        /// 将 GUID 的字符串表示形式转换为等效的 <see cref="System.Guid"/> 结构，前提是该字符串采用的是指定格式。
        /// </summary>
        /// <param name="input">要转换的 GUID。</param>
        /// <param name="format">下列说明符之一，指示当解释 input 时要使用的确切格式：“N”、“D”、“B”、“P”或“X”。</param>
        /// <param name="result">将包含已分析的值的结构。 如果此方法返回 true，result 将包含有效的 <see cref="System.Guid"/>。 如果 <paramref name="result"/> 等于 <seealso cref="System.Guid.Empty"/>，则此方法将返回 false。</param>
        /// <returns>如果分析操作成功，则为 true；否则为 false。</returns>
        public static bool TryToGuid(string input, string format, out Guid result)
        {
            return Guid.TryParseExact(input, format, out result);
        }



        /// <summary>
        /// 将日期和时间的字符串表示形式转换为其等效的 <see cref="System.DateTime"/>。
        /// </summary>
        /// <param name="s">包含要转换的日期和时间的字符串。</param>
        /// <returns>一个对象，它等效于 <paramref name="s"/> 中包含的日期和时间。</returns>
        public static DateTime ToDateTime(this string s)
        {
            return DateTime.Parse(s);
        }

        /// <summary>
        /// 使用区域性特定格式信息，将日期和时间的字符串表示形式转换为其等效的 <see cref="System.DateTime"/>。
        /// </summary>
        /// <param name="s">包含要转换的日期和时间的字符串。</param>
        /// <param name="provider">一个对象，提供有关 <paramref name="s"/> 的区域性特定格式信息。</param>
        /// <returns>一个对象，它等效于 <paramref name="s"/> 中包含的日期和时间，由 <paramref name="provider"/> 指定。</returns>
        public static DateTime ToDateTime(this string s, IFormatProvider provider)
        {
            return DateTime.Parse(s, provider);
        }

        /// <summary>
        /// 使用区域性特定格式信息和格式设置样式将日期和时间的字符串表示形式转换为其等效的 <see cref="System.DateTime"/>。
        /// </summary>
        /// <param name="s">包含要转换的日期和时间的字符串。</param>
        /// <param name="provider">一个对象，提供有关 <paramref name="s"/> 的区域性特定格式设置信息。</param>
        /// <param name="styles">枚举值的按位组合，用于指示 <paramref name="s"/> 成功执行分析操作所需的样式元素以及定义如何根据当前时区或当前日期解释已分析日期的样式元素。 一个用来指定的典型值为 <seealso cref="System.Globalization.DateTimeStyles.None"/>。</param>
        /// <returns>一个对象，它等效于 <paramref name="s"/> 中包含的日期和时间，由 <paramref name="provider"/> 和 <paramref name="styles"/> 指定。</returns>
        public static DateTime ToDateTime(this string s, IFormatProvider provider, DateTimeStyles styles)
        {
            return DateTime.Parse(s, provider, styles);
        }


        /// <summary>
        /// 将日期和时间的指定字符串表示形式转换为其 System.DateTime 等效项，并返回一个指示转换是否成功的值。
        /// </summary>
        /// <param name="s">包含要转换的日期和时间的字符串。</param>
        /// <param name="result">
        /// 当此方法返回时，如果转换成功，则包含与 s 中包含的日期和时间等效的 System.DateTime 值；如果转换失败，则为 System.DateTime.MinValue。
        /// 如果 s 参数为 null，是空字符串 ("") 或者不包含日期和时间的有效字符串表示形式，则转换失败。 该参数未经初始化即被传递。</param>
        /// <returns>如果 s 参数成功转换，则为 true；否则为 false。</returns>
        public static bool TryToDateTime(this string s, out DateTime result)
        {
            return DateTime.TryParse(s, out result);
        }

        /// <summary>
        /// 使用指定的区域性特定格式信息和格式设置样式，将日期和时间的指定字符串表示形式转换为其 System.DateTime 等效项，并返回一个指示转换是否成功的值。
        /// </summary>
        /// <param name="s">包含要转换的日期和时间的字符串。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <param name="styles">枚举值的按位组合，该组合定义如何根据当前时区或当前日期解释已分析日期。 一个要指定的典型值为 System.Globalization.DateTimeStyles.None。</param>
        /// <param name="result">
        /// 当此方法返回时，如果转换成功，则包含与 s 中包含的日期和时间等效的 System.DateTime 值；如果转换失败，则为 System.DateTime.MinValue。
        /// 如果 s 参数为 null，是空字符串 ("") 或者不包含日期和时间的有效字符串表示形式，则转换失败。 该参数未经初始化即被传递。</param>
        /// <returns>如果 s 参数成功转换，则为 true；否则为 false。</returns>
        public static bool TryToDateTime(this string s, IFormatProvider provider, DateTimeStyles styles, out DateTime result)
        {
            return DateTime.TryParse(s, provider, styles, out result);
        }

        /// <summary>
        /// 使用指定的格式、区域性特定的格式信息和样式将日期和时间的指定字符串表示形式转换为其等效的 System.DateTime。 字符串表示形式的格式必须与指定的格式完全匹配。该方法返回一个指示转换是否成功的值。
        /// </summary>
        /// <param name="s">包含要转换的日期和时间的字符串。</param>
        /// <param name="format">所需的 s 格式。 有关更多信息，请参见备注部分。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式设置信息。</param>
        /// <param name="style">一个或多个枚举值的按位组合，指示 s 允许使用的格式。</param>
        /// <param name="result">
        /// 当此方法返回时，如果转换成功，则包含与 s 中包含的日期和时间等效的 System.DateTime 值；如果转换失败，则为 System.DateTime.MinValue。
        /// 如果 s 或 format 参数为 null，或者为空字符串，或者未包含对应于 format 中指定的模式的日期和时间，则转换失败。 该参数未经初始化即被传递。
        /// </param>
        /// <returns>如果成功转换了 s，则为 true；否则为 false。</returns>
        public static bool TryToDateTime(this string s, string format, IFormatProvider provider, DateTimeStyles style, out DateTime result)
        {
            return DateTime.TryParseExact(s, format, provider, style, out result);
        }

        /// <summary>
        /// 使用指定的格式数组、区域性特定格式信息和样式，将日期和时间的指定字符串表示形式转换为其等效的 System.DateTime。 字符串表示形式的格式必须至少与指定的格式之一完全匹配。该方法返回一个指示转换是否成功的值。
        /// </summary>
        /// <param name="s">包含要转换的日期和时间的字符串。</param>
        /// <param name="formats">s 的允许格式的数组。 有关更多信息，请参见备注部分。</param>
        /// <param name="provider">一个对象，提供有关 s 的区域性特定格式信息。</param>
        /// <param name="style">枚举值的一个按位组合，指示 s 所允许的格式。 一个用来指定的典型值为 System.Globalization.DateTimeStyles.None。</param>
        /// <param name="result">
        /// 当此方法返回时，如果转换成功，则包含与 s 中包含的日期和时间等效的 System.DateTime 值；如果转换失败，则为 System.DateTime.MinValue。
        /// 如果 s 或 formats 为 null，s 或 formats 的一个元素为空字符串，或者 s 的格式与 formats 中的格式模式所指定的格式都不完全匹配，则转换失败。
        /// 该参数未经初始化即被传递。
        /// </param>
        /// <returns>如果 s 参数成功转换，则为 true；否则为 false。</returns>
        public static bool TryToDateTime(this string s, string[] formats, IFormatProvider provider, DateTimeStyles style, out DateTime result)
        {
            return DateTime.TryParseExact(s, formats, provider, style, out result);
        }


        /// <summary>
        /// 尝试将字符串转换成其等效的 <typeparamref name="TResult"/> 类型对象，并返回一个指示是否转换成功的 bool 值。
        /// </summary>
        /// <typeparam name="TResult">一个 <see cref="IConvertible"/> 接口的实现类，表示转换的目标类型。</typeparam>
        /// <param name="str">被转换的字符串。</param>
        /// <param name="result">
        /// 当此方法返回时，如果转换成功，则包含与 <paramref name="str"/> 中等效的 <typeparamref name="TResult"/> 对象值；
        /// 如果转换失败，则为类型 <typeparamref name="TResult"/> 的默认值。
        /// 如果 <paramref name="str"/> 参数为 null，是空字符串 ("") 或者不是 <typeparamref name="TResult"/> 类型对象的等效字符串表示形式，则转换失败。该参数未经初始化即被传递。
        /// </param>
        /// <returns></returns>
        public static bool TryConvert<TResult>(this string str, out TResult result) where TResult : IConvertible
        {
            Type conversionType = typeof(TResult);
            try
            {
                object ret = Convert.ChangeType(str, conversionType);
                result = (TResult)ret;
                return true;
            }
            catch
            {
                result = default(TResult);
                return false;
            }
        }



        /// <summary>
        /// 判断传入的字符串是否为 HTML 代码段。
        /// </summary>
        /// <param name="str">被判断的字符串对象。</param>
        /// <returns>如果传入的字符串为 HTML 代码段(以 '&gt;' 开头并且以 '&lt;' 结尾)，则返回 true，否则返回 false。</returns>
        public static bool IsHtmlText(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return false;
            }
            return str.Length >= 3 && str[0] == '<' && str[str.Length - 1] == '>';
        }


        /// <summary>
        /// 将指定字符串中的格式项替换为指定数组中相应对象的字符串表示形式。
        /// </summary>
        /// <param name="str">复合格式字符串。</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象。</param>
        /// <returns>format 的一个副本，其中格式项已替换为 args 中相应对象的字符串表示形式。</returns>
        public static string Format(this string str, params object[] args)
        {
            return string.Format(str, args);
        }

        /// <summary>
        /// 将指定字符串中的格式项替换为指定数组中相应对象的字符串表示形式。 参数提供区域性特定的格式设置信息。
        /// </summary>
        /// <param name="str">复合格式字符串。</param>
        /// <param name="provider">一个提供区域性特定的格式设置信息的对象。</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象。</param>
        /// <returns>format 的一个副本，其中格式项已替换为 args 中相应对象的字符串表示形式。</returns>
        public static string Format(this string str, IFormatProvider provider, params object[] args)
        {
            return string.Format(provider, str, args);
        }



        /// <summary>
        /// 判断指定的字符串值是否为 Null 或者 System.String.Empty 空字符串值。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string str)
        {
            return String.IsNullOrEmpty(str);
        }

        /// <summary>
        /// 判断指定的字符串值是否为 Null 、 System.String.Empty 空字符串值或者仅由空格组成。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEmptyOrWhiteSpace(this string str)
        {
            return String.IsNullOrWhiteSpace(str);
        }


        /// <summary>
        /// 将字符串转换为其所表示的 int 值。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(this string value)
        {
            return ToInt(value, 0);
        }

        /// <summary>
        /// 将字符串转换为其所表示的 int 值。参数表示转换不成功时返回的默认值。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ToInt(this string value, int defaultValue)
        {
            int ret;
            return int.TryParse(value, out ret) ? ret : defaultValue;
        }

        /// <summary>
        /// 将字符串转换为其所表示的 decimal 值。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string value)
        {
            return To<Decimal>(value);
        }

        /// <summary>
        /// 将字符串转换为其所表示的 decimal 值。参数表示转换不成功时返回的默认值。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string value, decimal defaultValue)
        {
            return To<Decimal>(value, defaultValue);
        }

        /// <summary>
        /// 将字符串转换为其所表示的 float 值。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float ToFloat(this string value)
        {
            return ToFloat(value, default(float));
        }

        /// <summary>
        /// 将字符串转换为其所表示的 float 值。参数表示转换不成功时返回的默认值。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static float ToFloat(this string value, float defaultValue)
        {
            float ret;
            return Single.TryParse(value, out ret) ? ret : defaultValue;
        }

        /// <summary>
        /// 将字符串转换为其所表示的 bool 值。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBoolean(this string value)
        {
            return ToBoolean(value, default(bool), false);
        }

        /// <summary>
        /// 将字符串转换为其所表示的 bool 值。转换时识别友好的条件 true 字符（如 "true", "1", "yes", "genuine", "right", "r", "checked"）。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBooleanWithFriendly(this string value)
        {
            return ToBoolean(value, default(bool), true);
        }

        /// <summary>
        /// 将字符串转换为其表示的 bool 值。参数表示转换不成功时返回的默认值。
        /// 参数 <paramref name="friendly"/> 指示是否识别友好的条件 true 字符（如 "true", "1", "yes", "genuine", "right", "r", "checked"）。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="friendly"></param>
        /// <returns></returns>
        public static bool ToBoolean(this string value, bool defaultValue, bool friendly = true)
        {
            if (friendly)
            {
                string[] array = { "true", "t", "1", "yes", "y", "genuine", "right", "r", "checked" };
                if (array.Contains(value, StringComparer.InvariantCultureIgnoreCase))
                    return true;
            }
            bool ret;
            return Boolean.TryParse(value, out ret) ? ret : defaultValue;
        }


        /// <summary>
        /// 将字符串转换为其表示的另一类型 <typeparamref name="TValue"/> 的值。
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TValue To<TValue>(this string value)
        {
            return To<TValue>(value, default(TValue));
        }

        /// <summary>
        /// 将字符串转换为其表示的另一类型 <typeparamref name="TValue"/> 的值。参数表示转换不成功时返回的默认值。
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TValue To<TValue>(this string value, TValue defaultValue)
        {
            return (TValue)To(value, defaultValue, typeof(TValue));
        }

        /// <summary>
        /// 将字符串转换为其表示的另一类型 <paramref name="type"/> 的值。参数表示转换不成功时返回的默认值。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object To(this string value, object defaultValue, Type type)
        {
            Check.NotNull(type);
            if (!type.IsInstanceOfType(defaultValue))
                throw new ArgumentException("传入的参数 defaultValue 不是类型参数 type 所示类型的实例。");

            try
            {
                TypeConverter converter = TypeDescriptor.GetConverter(type);
                if (converter.CanConvertFrom(typeof(string)))
                {
                    return converter.ConvertFrom(value);
                }
                // try the other direction
                converter = TypeDescriptor.GetConverter(typeof(string));
                if (converter.CanConvertTo(type))
                {
                    return converter.ConvertTo(value, type);
                }
            }
            catch
            {
                // eat all exceptions and return the defaultValue, assumption is that its always a parse/format exception
            }
            return defaultValue;
        }


        /// <summary>
        /// 判断字符串值是否能正确的表示一个 bool 值。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsBoolean(this string value)
        {
            bool ret;
            return Boolean.TryParse(value, out ret);
        }

        /// <summary>
        /// 判断字符串值是否能正确的表示一个 int 值。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInt(this string value)
        {
            int ret;
            return Int32.TryParse(value, out ret);
        }

        /// <summary>
        /// 判断字符串值是否能正确的表示一个 decimal 值。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDecimal(this string value)
        {
            return Is<Decimal>(value);
        }

        /// <summary>
        /// 判断字符串值是否能正确的表示一个 float 值。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsFloat(this string value)
        {
            float ret;
            return Single.TryParse(value, out ret);
        }

        /// <summary>
        /// 判断字符串值是否能正确的表示一个 DateTime 值。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDateTime(this string value)
        {
            DateTime ret;
            return DateTime.TryParse(value, out ret);
        }



        /// <summary>
        /// 判断指定的字符串值是否可以转换为指定的类型。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool Is(this string value, Type type)
        {
            Check.NotNull(type);
            TypeConverter converter = TypeDescriptor.GetConverter(type);
            if (converter != null)
            {
                try
                {
                    if ((value == null) || converter.CanConvertFrom(null, value.GetType()))
                    {
                        // TypeConverter.IsValid essentially does this - a try catch - but uses InvariantCulture to convert. 
                        converter.ConvertFrom(null, CultureInfo.CurrentCulture, value);
                        return true;
                    }
                }
                catch
                {
                }
            }
            return false;
        }

        /// <summary>
        /// 判断指定的字符串值是否可以转换为指定的类型。
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Is<TValue>(this string value)
        {
            return Is(value, typeof(TValue));
        }



        /// <summary>
        /// 获取字符串包含的所有字节所构成的一个 <see cref="System.Byte"/> 数组。
        /// </summary>
        /// <param name="str">被处理的字符串对象。</param>
        /// <returns>返回字符串包含的所有字节所构成的一个 <see cref="System.Byte"/> 数组。</returns>
        public static byte[] ToBytes(this string str)
        {
            return ToBytes(str, Encoding.UTF8);
        }

        /// <summary>
        /// 根据指定的字符集信息获取字符串包含的所有字节所构成的一个 <see cref="System.Byte"/> 数组。
        /// </summary>
        /// <param name="str">被处理的字符串对象。</param>
        /// <param name="encoding">字符集名称。</param>
        /// <returns>返回字符串包含的所有字节所构成的一个 <see cref="System.Byte"/> 数组。</returns>
        public static byte[] ToBytes(this string str, string encoding)
        {
            Encoding e = Encoding.GetEncoding(encoding);
            return ToBytes(str, e);
        }

        /// <summary>
        ///  根据指定的字符集信息获取字符串包含的所有字节所构成的一个 <see cref="System.Byte"/> 数组。
        /// </summary>
        /// <param name="str">被处理的字符串对象。</param>
        /// <param name="encoding">字符集。</param>
        /// <returns>返回字符串包含的所有字节所构成的一个 <see cref="System.Byte"/> 数组。</returns>
        public static byte[] ToBytes(this string str, Encoding encoding)
        {
            Check.NotNull(encoding);
            return encoding.GetBytes(str);
        }


        /// <summary>
        /// 返回的字符串数组包含此字符串中的子字符串（由指定字符串元素分隔）。
        /// </summary>
        /// <param name="str">要进行元素分隔的字符串。</param>
        /// <param name="separator">分隔此字符串中子字符串的字符串数组、不包含分隔符的空数组或 null。</param>
        /// <returns></returns>
        public static string[] Split(this string str, params string[] separator)
        {
            return str.Split(separator, StringSplitOptions.None);
        }

        /// <summary>
        /// 返回的字符串数组包含此字符串中的子字符串（由指定字符串元素分隔）。参数指定是否返回空数组元素。
        /// </summary>
        /// <param name="str">要进行元素分隔的字符串。</param>
        /// <param name="separator">分隔此字符串中子字符串的字符串数组、不包含分隔符的空数组或 null。</param>
        /// <param name="options">要省略返回的数组中的空数组元素，则为 System.StringSplitOptions.RemoveEmptyEntries；要包含返回的数组中的空数组元素，则为 System.StringSplitOptions.None。</param>
        /// <returns></returns>
        public static string[] Split(this string str, string separator, StringSplitOptions options)
        {
            string[] seps = { separator };
            return str.Split(seps, options);
        }

        /// <summary>
        /// 返回的字符串数组包含此字符串中的子字符串（由指定字符串元素分隔）。参数指定要返回子字符串的最大数量。
        /// </summary>
        /// <param name="str">要进行元素分隔的字符串。</param>
        /// <param name="separator">分隔此字符串中子字符串的字符串数组、不包含分隔符的空数组或 null。</param>
        /// <param name="count">要返回的子字符串的最大数量。</param>
        /// <returns></returns>
        public static string[] Split(this string str, string separator, int count)
        {
            string[] seps = { separator };
            return str.Split(seps, count, StringSplitOptions.None);
        }

        /// <summary>
        /// 返回的字符串数组包含此字符串中的子字符串（由指定字符串元素分隔）。 参数指定要返回子字符串的最大数量，以及是否返回空数组元素。
        /// </summary>
        /// <param name="str">要进行元素分隔的字符串。</param>
        /// <param name="separator">分隔此字符串中子字符串的字符串数组、不包含分隔符的空数组或 null。</param>
        /// <param name="count">要返回的子字符串的最大数量。</param>
        /// <param name="options">要省略返回的数组中的空数组元素，则为 System.StringSplitOptions.RemoveEmptyEntries；要包含返回的数组中的空数组元素，则为 System.StringSplitOptions.None。</param>
        /// <returns></returns>
        public static string[] Split(this string str, string separator, int count, StringSplitOptions options)
        {
            string[] seps = { separator };
            return str.Split(seps, count, options);
        }



        /// <summary>
        /// 将字符串按指定的分隔符分割成一个数组，然后将分割后的结果数组中的每个元素转换成指定的类型后投射到一个新的数组并返回。
        /// </summary>
        /// <typeparam name="TResult">分隔后的字符串数组中元素被转换的目标类型。</typeparam>
        /// <param name="str">被分隔处理的字符串。</param>
        /// <param name="separator">用户分隔字符串 <paramref name="str"/> 的分隔符。</param>
        /// <param name="containsConvertFaildEntries">
        /// <para>该 bool 值指示返回的数组中是否包含未成功转换的项。该参数默认值为 false。</para>
        /// <para>如果该参数为 true，则返回的数组长度将等于 <paramref name="str"/> 分隔成字符串数组的长度。并且当 <paramref name="str"/> 分隔成字符串数组后，数组中的任何元素如果未成功转换为 <typeparamref name="TResult"/> 类型值，则以类型 <typeparamref name="TResult"/> 的默认值来填充该位置。</para>
        /// <para>如果该参数为 false，则返回的数组长度将小于或等于 等于 <paramref name="str"/> 分隔成字符串数组的长度。并且当 <paramref name="str"/> 分隔成字符串数组后，数组中的任何元素如果未成功转换为 <typeparamref name="TResult"/> 类型值，则该位置的元素将会被不会包含在返回的数组中。</para>
        /// </param>
        /// <returns></returns>
        public static TResult[] SplitTo<TResult>(this string str, char[] separator, bool containsConvertFaildEntries = false) where TResult : IConvertible
        {
            string[] array = str.Split(separator);
            return InternalSplitTo<TResult>(array, containsConvertFaildEntries);
        }

        /// <summary>
        /// 将字符串按指定的分隔符分割成一个数组，然后将分割后的结果数组中的每个元素转换成指定的类型后投射到一个新的数组并返回。
        /// </summary>
        /// <typeparam name="TResult">分隔后的字符串数组中元素被转换的目标类型。</typeparam>
        /// <param name="str">被分隔处理的字符串。</param>
        /// <param name="separator">用户分隔字符串 <paramref name="str"/> 的分隔符。</param>
        /// <param name="count">要返回的子字符串的最大数量。</param>
        /// <param name="containsConvertFaildEntries">
        /// <para>该 bool 值指示返回的数组中是否包含未成功转换的项。该参数默认值为 false。</para>
        /// <para>如果该参数为 true，则返回的数组长度将等于 <paramref name="str"/> 分隔成字符串数组的长度。并且当 <paramref name="str"/> 分隔成字符串数组后，数组中的任何元素如果未成功转换为 <typeparamref name="TResult"/> 类型值，则以类型 <typeparamref name="TResult"/> 的默认值来填充该位置。</para>
        /// <para>如果该参数为 false，则返回的数组长度将小于或等于 等于 <paramref name="str"/> 分隔成字符串数组的长度。并且当 <paramref name="str"/> 分隔成字符串数组后，数组中的任何元素如果未成功转换为 <typeparamref name="TResult"/> 类型值，则该位置的元素将会被不会包含在返回的数组中。</para>
        /// </param>
        /// <returns></returns>
        public static TResult[] SplitTo<TResult>(this string str, char[] separator, int count, bool containsConvertFaildEntries = false) where TResult : IConvertible
        {
            string[] array = str.Split(separator, count);
            return InternalSplitTo<TResult>(array, containsConvertFaildEntries);
        }

        /// <summary>
        /// 将字符串按指定的分隔符分割成一个数组，然后将分割后的结果数组中的每个元素转换成指定的类型后投射到一个新的数组并返回。
        /// </summary>
        /// <typeparam name="TResult">分隔后的字符串数组中元素被转换的目标类型。</typeparam>
        /// <param name="str">被分隔处理的字符串。</param>
        /// <param name="separator">用户分隔字符串 <paramref name="str"/> 的分隔符。</param>
        /// <param name="options">要省略返回的数组中的空数组元素，则为 System.StringSplitOptions.RemoveEmptyEntries；要包含返回的数组中的空数组元素，则为 System.StringSplitOptions.None。</param>
        /// <param name="containsConvertFaildEntries"></param>
        /// <returns></returns>
        public static TResult[] SplitTo<TResult>(this string str, char[] separator, StringSplitOptions options, bool containsConvertFaildEntries = false) where TResult : IConvertible
        {
            string[] array = str.Split(separator, options);
            return InternalSplitTo<TResult>(array, containsConvertFaildEntries);
        }

        /// <summary>
        /// 将字符串按指定的分隔符分割成一个数组，然后将分割后的结果数组中的每个元素转换成指定的类型后投射到一个新的数组并返回。
        /// </summary>
        /// <typeparam name="TResult">分隔后的字符串数组中元素被转换的目标类型。</typeparam>
        /// <param name="str">被分隔处理的字符串。</param>
        /// <param name="separator">用户分隔字符串 <paramref name="str"/> 的分隔符。</param>
        /// <param name="options">要省略返回的数组中的空数组元素，则为 System.StringSplitOptions.RemoveEmptyEntries；要包含返回的数组中的空数组元素，则为 System.StringSplitOptions.None。</param>
        /// <param name="containsConvertFaildEntries">
        /// <para>该 bool 值指示返回的数组中是否包含未成功转换的项。该参数默认值为 false。</para>
        /// <para>如果该参数为 true，则返回的数组长度将等于 <paramref name="str"/> 分隔成字符串数组的长度。并且当 <paramref name="str"/> 分隔成字符串数组后，数组中的任何元素如果未成功转换为 <typeparamref name="TResult"/> 类型值，则以类型 <typeparamref name="TResult"/> 的默认值来填充该位置。</para>
        /// <para>如果该参数为 false，则返回的数组长度将小于或等于 等于 <paramref name="str"/> 分隔成字符串数组的长度。并且当 <paramref name="str"/> 分隔成字符串数组后，数组中的任何元素如果未成功转换为 <typeparamref name="TResult"/> 类型值，则该位置的元素将会被不会包含在返回的数组中。</para>
        /// </param>
        /// <returns></returns>
        public static TResult[] SplitTo<TResult>(this string str, string[] separator, StringSplitOptions options, bool containsConvertFaildEntries = false) where TResult : IConvertible
        {
            string[] array = str.Split(separator, options);
            return InternalSplitTo<TResult>(array, containsConvertFaildEntries);
        }

        /// <summary>
        /// 将字符串按指定的分隔符分割成一个数组，然后将分割后的结果数组中的每个元素转换成指定的类型后投射到一个新的数组并返回。
        /// </summary>
        /// <typeparam name="TResult">分隔后的字符串数组中元素被转换的目标类型。</typeparam>
        /// <param name="str">被分隔处理的字符串。</param>
        /// <param name="separator">用户分隔字符串 <paramref name="str"/> 的分隔符。</param>
        /// <param name="count">要返回的子字符串的最大数量。</param>
        /// <param name="options">要省略返回的数组中的空数组元素，则为 System.StringSplitOptions.RemoveEmptyEntries；要包含返回的数组中的空数组元素，则为 System.StringSplitOptions.None。</param>
        /// <param name="containsConvertFaildEntries">
        /// <para>该 bool 值指示返回的数组中是否包含未成功转换的项。该参数默认值为 false。</para>
        /// <para>如果该参数为 true，则返回的数组长度将等于 <paramref name="str"/> 分隔成字符串数组的长度。并且当 <paramref name="str"/> 分隔成字符串数组后，数组中的任何元素如果未成功转换为 <typeparamref name="TResult"/> 类型值，则以类型 <typeparamref name="TResult"/> 的默认值来填充该位置。</para>
        /// <para>如果该参数为 false，则返回的数组长度将小于或等于 等于 <paramref name="str"/> 分隔成字符串数组的长度。并且当 <paramref name="str"/> 分隔成字符串数组后，数组中的任何元素如果未成功转换为 <typeparamref name="TResult"/> 类型值，则该位置的元素将会被不会包含在返回的数组中。</para>
        /// </param>
        /// <returns></returns>
        public static TResult[] SplitTo<TResult>(this string str, char[] separator, int count, StringSplitOptions options, bool containsConvertFaildEntries = false) where TResult : IConvertible
        {
            string[] array = str.Split(separator, count, options);
            return InternalSplitTo<TResult>(array, containsConvertFaildEntries);
        }

        /// <summary>
        /// 将字符串按指定的分隔符分割成一个数组，然后将分割后的结果数组中的每个元素转换成指定的类型后投射到一个新的数组并返回。
        /// </summary>
        /// <typeparam name="TResult">分隔后的字符串数组中元素被转换的目标类型。</typeparam>
        /// <param name="str">被分隔处理的字符串。</param>
        /// <param name="separator">用户分隔字符串 <paramref name="str"/> 的分隔符。</param>
        /// <param name="count">要返回的子字符串的最大数量。</param>
        /// <param name="options">要省略返回的数组中的空数组元素，则为 System.StringSplitOptions.RemoveEmptyEntries；要包含返回的数组中的空数组元素，则为 System.StringSplitOptions.None。</param>
        /// <param name="containsConvertFaildEntries">
        /// <para>该 bool 值指示返回的数组中是否包含未成功转换的项。该参数默认值为 false。</para>
        /// <para>如果该参数为 true，则返回的数组长度将等于 <paramref name="str"/> 分隔成字符串数组的长度。并且当 <paramref name="str"/> 分隔成字符串数组后，数组中的任何元素如果未成功转换为 <typeparamref name="TResult"/> 类型值，则以类型 <typeparamref name="TResult"/> 的默认值来填充该位置。</para>
        /// <para>如果该参数为 false，则返回的数组长度将小于或等于 等于 <paramref name="str"/> 分隔成字符串数组的长度。并且当 <paramref name="str"/> 分隔成字符串数组后，数组中的任何元素如果未成功转换为 <typeparamref name="TResult"/> 类型值，则该位置的元素将会被不会包含在返回的数组中。</para>
        /// </param>
        /// <returns></returns>
        public static TResult[] SplitTo<TResult>(this string str, string[] separator, int count, StringSplitOptions options, bool containsConvertFaildEntries = false) where TResult : IConvertible
        {
            string[] array = str.Split(separator, count, options);
            return InternalSplitTo<TResult>(array, containsConvertFaildEntries);
        }


        private static TResult[] InternalSplitTo<TResult>(string[] array, bool containsConvertFaildEntries) where TResult : IConvertible
        {
            if (array == null || array.Length == 0)
                return Enumerable.Empty<TResult>().ToArray();

            List<TResult> list = new List<TResult>();
            foreach (string item in array)
            {
                TResult ret;
                if (TryConvert<TResult>(item, out ret))
                {
                    list.Add(ret);
                }
                else if (containsConvertFaildEntries)
                {
                    ret = default(TResult);
                    list.Add(ret);
                }
            }
            return list.ToArray();
        }




        /// <summary>
        /// 获取字符串包含非 ASCII 码字符(例如中文、日文、俄文等)的 byte 字节长度。
        /// </summary>
        /// <param name="str">被处理的字符串对象。</param>
        /// <returns>返回字符串包含非 ASCII 码字符(例如中文、日文、俄文等)的 byte 字节长度。</returns>
        public static int GetByteLength(this string str)
        {
            return str.ToBytes().Length;
        }



        /// <summary>
        /// 将字符串反转；该方法将返回源字符串处理后的一个副本，而不会改变源字符串的值。
        /// </summary>
        /// <param name="str">被处理的字符串对象。</param>
        /// <returns>返回源字符串处理后的一个副本，而不会改变源字符串的值。</returns>
        public static string Reverse(this string str)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = str.Length - 1; i > -1; i--)
            {
                stringBuilder.Append(str[i]);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 从当前 System.String 对象移除数组中指定的一组字符的所有前导匹配项。
        /// 同方法 <seealso cref="System.String.TrimStart"/>
        /// </summary>
        /// <param name="str">被处理的字符串对象。</param>
        /// <param name="trimChars">要删除的 Unicode 字符的数组，或 null。</param>
        /// <returns>从当前字符串的开头移除所出现的所有 trimChars 参数中的字符后剩余的字符串。 如果 trimChars 为 null 或空数组，则改为移除空白字符。</returns>
        public static string LeftTrim(this string str, params char[] trimChars)
        {
            return str.TrimStart(trimChars);
        }

        /// <summary>
        /// 从当前 System.String 对象移除数组中指定的一组字符的所有尾部匹配项。
        /// 同方法 <seealso cref="System.String.TrimEnd"/>
        /// </summary>
        /// <param name="str">被处理的字符串对象。</param>
        /// <param name="trimChars">要删除的 Unicode 字符的数组，或 null。</param>
        /// <returns>从当前字符串的结尾移除所出现的所有 trimChars 参数中的字符后剩余的字符串。 如果 trimChars 为 null 或空数组，则改为删除 Unicode 空白字符。</returns>
        public static string RightTrim(this string str, params char[] trimChars)
        {
            return str.TrimEnd(trimChars);
        }

        /// <summary>
        /// 从此实例检索子字符串。 子字符串在指定的字符位置开始并一直到该字符串的末尾。
        /// 同方法 String.Substring。
        /// </summary>
        /// <param name="str">被处理的字符串对象。</param>
        /// <param name="startIndex">此实例中子字符串的起始字符位置（从零开始）。</param>
        /// <returns>与此实例中在 startIndex 处开头的子字符串等效的一个字符串；如果 startIndex 等于此实例的长度，则为 System.String.Empty。</returns>
        public static string Mid(this string str, int startIndex)
        {
            return str.Substring(startIndex);
        }

        /// <summary>
        /// 从此实例检索子字符串。 子字符串从指定的字符位置开始且具有指定的长度。
        /// 同方法 String.Substring。
        /// </summary>
        /// <param name="str">被处理的字符串对象。</param>
        /// <param name="startIndex">此实例中子字符串的起始字符位置（从零开始）。</param>
        /// <param name="length">子字符串中的字符数。</param>
        /// <returns>与此实例中在 startIndex 处开头、长度为 length 的子字符串等效的一个字符串，如果 startIndex 等于此实例的长度且 length 为零，则为 System.String.Empty。</returns>
        public static string Mid(this string str, int startIndex, int length)
        {
            return str.Substring(startIndex, length);
        }

        /// <summary>
        /// 截取当前字符串左边的指定长度内容。
        /// </summary>
        /// <param name="str">被处理的字符串对象。</param>
        /// <param name="length">子字符串中的字符数，即被截取的长度。</param>
        /// <returns>返回截取当前字符串左边的指定长度的内容。</returns>
        public static string Left(this string str, int length)
        {
            return str.Substring(0, length);
        }

        /// <summary>
        /// 截取当前字符串右边的指定长度内容。
        /// </summary>
        /// <param name="str">被处理的字符串对象。</param>
        /// <param name="length">子字符串中的字符数，即被截取的长度。</param>
        /// <returns>返回截取当前字符串右边的指定长度的内容。</returns>
        public static string Right(this string str, int length)
        {
            return str.Substring(str.Length - length);
        }

        /// <summary>
        /// 截取当前字符串左边的指定长度的字节内容。
        /// </summary>
        /// <param name="str">被处理的字符串对象。</param>
        /// <param name="length">子字符串中的字节数，即被截取的字节长度。</param>
        /// <returns>返回截取当前字符串左边的指定长度的字节内容所构成的一个数组。</returns>
        public static byte[] LeftBytes(this string str, int length)
        {
            return str.ToBytes().Take(length).ToArray();
        }

        /// <summary>
        /// 截取当前字符串右边的指定长度的字节内容。
        /// </summary>
        /// <param name="str">被处理的字符串对象。</param>
        /// <param name="length">子字符串中的字节数，即被截取的字节长度。</param>
        /// <returns>返回截取当前字符串右边的指定长度的字节内容所构成的一个数组。</returns>
        public static byte[] RightBytes(this string str, int length)
        {
            byte[] bytes = str.ToBytes();
            return bytes.Skip(bytes.Length - length).Take(length).ToArray();
        }

        /// <summary>
        /// 截取当前字符串左边的指定长度的字节内容所构成的一个字符串。
        /// </summary>
        /// <param name="str">被处理的字符串对象。</param>
        /// <param name="length">子字符串中的字节数，即被截取的字节长度。</param>
        /// <returns>返回截取当前字符串左边的指定长度的字节内容数组所构成的一个字符串。</returns>
        public static string LeftBytesString(this string str, int length)
        {
            return System.Text.Encoding.Default.GetString(str.LeftBytes(length));
        }

        /// <summary>
        /// 截取当前字符串右边的指定长度的字节内容所构成的一个字符串。
        /// </summary>
        /// <param name="str">被处理的字符串对象。</param>
        /// <param name="length">子字符串中的字节数，即被截取的字节长度。</param>
        /// <returns>返回截取当前字符串右边的指定长度的字节内容数组所构成的一个字符串。</returns>
        public static string RightBytesString(this string str, int length)
        {
            return System.Text.Encoding.Default.GetString(str.RightBytes(length));
        }



        /// <summary>
        /// 判断字符串是否符合将其转换为 System.DateTime 等效项的格式要求。
        /// </summary>
        /// <param name="str">被判断的字符串对象。</param>
        /// <returns>如果字符串符合将其转换为 System.DateTime 等效项的格式要求，则返回 true，否则返回 false。</returns>
        public static bool IsDate(this string str)
        {
            DateTimeStyles[] styles = Enums.GetFields<DateTimeStyles>();
            foreach (var item in styles)
            {
                if (str.IsDate(CultureInfo.InvariantCulture, item))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 使用指定的区域性特定格式信息和格式设置样式判断字符串是否符合将其转换为 System.DateTime 等效项的格式要求。
        /// </summary>
        /// <param name="str">被判断的字符串对象。</param>
        /// <param name="provider">一个对象，提供有关 str 的区域性特定格式设置信息。</param>
        /// <param name="style">枚举值的按位组合，该组合定义如何根据当前时区或当前日期解释已分析日期。 一个要指定的典型值为 System.Globalization.DateTimeStyles.None。</param>
        /// <returns>如果字符串符合将其转换为 System.DateTime 等效项的格式要求，则返回 true，否则返回 false。</returns>
        public static bool IsDate(this string str, IFormatProvider provider, DateTimeStyles style)
        {
            DateTime datetime;
            return DateTime.TryParse(str, provider, style, out datetime);
        }

        /// <summary>
        /// 判断当前 String 对象是否是正确的电话号码格式(中国)。
        /// 格式如：(2-3位数字)|
        /// </summary>
        /// <param name="str">被判断的字符串对象。</param>
        /// <returns>如果当前 String 对象是否是正确的电话号码格式(中国)，则返回 true，否则返回 false。</returns>
        public static bool IsTel(this string str)
        {
            return str.IsMatch(@"^((\(\d{2,3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-)?[1-9]\d{6,7}(\-\d{1,4})?$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 判断当前 String 对象是否是正确的手机号码格式(中国)。
        /// </summary>
        /// <param name="str">被判断的字符串对象。</param>
        /// <returns>如果当前 String 对象是否是正确的手机号码格式(中国)，则返回 true，否则返回 false。</returns>
        public static bool IsMobile(this string str)
        {
            return str.IsMatch(@"^(13|14|15|17|18)\d{9}$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 判断当前 String 对象是否是正确的电话号码或者手机号码格式(中国)。
        /// </summary>
        /// <param name="str">被判断的字符串对象。</param>
        /// <returns>如果当前 String 对象是否是正确的电话号码或者手机号码格式(中国)，则返回 true，否则返回 false。</returns>
        public static bool IsTelOrMobile(this string str)
        {
            return str.IsTel() || str.IsMobile();
        }

        /// <summary>
        /// 判断当前 String 对象是否是正确的传真号码格式。
        /// </summary>
        /// <param name="str">被判断的字符串对象。</param>
        /// <returns>如果当前 String 对象是否是正确的传真号码格式，则返回 true，否则返回 false。</returns>
        public static bool IsFax(this string str)
        {
            //return str.IsMatch(@"^((\(\d{2,3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-)?[1-9]\d{6,7}(\-\d{1,4})?$", RegexOptions.IgnoreCase);
            return str.IsTel();
        }

        /// <summary>
        /// 判断当前 String 对象是否是正确的 电子邮箱地址(Email) 格式。
        /// </summary>
        /// <param name="str">被判断的字符串对象。</param>
        /// <returns>如果当前 String 对象是否是正确的 电子邮箱地址(Email) 格式，则返回 true，否则返回 false。</returns>
        public static bool IsEmail(this string str)
        {
            return str.IsMatch(@"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 判断当前 String 对象是否是正确的 邮政编码(中国) 格式。
        /// </summary>
        /// <param name="str">被判断的字符串对象。</param>
        /// <returns>如果当前 String 对象是否是正确的 邮政编码(中国) 格式，则返回 true，否则返回 false。</returns>
        public static bool IsZipCode(this string str)
        {
            return str.IsMatch(@"^[\d]{6}$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 判断当前 String 对象是否是否存在汉字字符。
        /// </summary>
        /// <param name="str">被判断的字符串对象。</param>
        /// <returns>如果当前 String 对象是否是否存在汉字字符，则返回 true，否则返回 false。</returns>
        public static bool ExistChinese(this string str)
        {
            // [\u4E00-\u9FA5]為漢字﹐[\uFE30-\uFFA0]為全角符號
            return str.IsMatch("!^[\x00-\xff]*$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 判断当前 String 对象是否为汉字字符或汉字字符集中的字符元素。
        /// </summary>
        /// <param name="str">被判断的字符串对象。</param>
        /// <returns>如果当前 String 对象是否为纯汉字字符，则返回 true，否则返回 false。</returns>
        public static bool IsFullChinese(this string str)
        {
            return str.IsMatch("^[\u0391-\uFFE5]+$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 判断当前 String 对象是否为纯英文字符。
        /// </summary>
        /// <param name="str">被判断的字符串对象。</param>
        /// <returns>如果当前 String 对象是否为纯英文字符，则返回 true，否则返回 false。</returns>
        public static bool IsEnglish(this string str)
        {
            return str.IsMatch("^[A-Za-z]+$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 判断当前 String 对象是否是正确的 文件名称(路径) 格式。
        /// </summary>
        /// <param name="str">被判断的字符串对象。</param>
        /// <returns>如果当前 String 对象是否是正确的 文件名称(路径) 格式，则返回 true，否则返回 false。</returns>
        public static bool IsFileName(this string str)
        {
            return str.IsMatch("!^[A-Za-z]+$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 判断字符串对象是否为良好的 Uri 格式。
        /// 此方法将调用 <seealso cref="System.UriKind.RelativeOrAbsolute"/> 进行判断。
        /// </summary>
        /// <param name="str">被判断的字符串对象。</param>
        /// <returns>如果字符串对象 str 符合 Uri 格式规范，则返回 true，否则返回 false。</returns>
        public static bool IsUri(this string str)
        {
            return str.IsUri(UriKind.RelativeOrAbsolute);
        }

        /// <summary>
        /// 判断字符串对象是否为良好的 Uri 格式。
        /// </summary>
        /// <param name="str">被判断的字符串对象。</param>
        /// <param name="uriKind"><paramref name="str"/> 中的 <see cref="System.Uri"/> 类型。</param>
        /// <returns>如果字符串对象 str 符合 Uri 格式规范，则返回 true，否则返回 false。</returns>
        public static bool IsUri(this string str, UriKind uriKind)
        {
            return Uri.IsWellFormedUriString(str, uriKind);
        }

        /// <summary>
        /// 判断字符串对象是否为 IPv4(0.0.0.0) 地址格式。
        /// </summary>
        /// <param name="str">被判断的字符串对象。</param>
        /// <returns>如果字符串 str 为 IPv4(0.0.0.0) 地址格式，则返回 true，否则返回 false。</returns>
        public static bool IsIPv4(this string str)
        {
            if (string.IsNullOrWhiteSpace(str) || str.Length < 7 || str.Length > 15)
            {
                return false;
            }
            string[] chars = str.Split(new char[] { '.' }, StringSplitOptions.None);
            if (chars.Length != 4)
            {
                return false;
            }
            int a, b, c, d;
            return int.TryParse(chars[0], out a) && a > 0 && a <= 255
                && int.TryParse(chars[1], out b) && b >= 0 && b <= 255
                && int.TryParse(chars[2], out c) && c >= 0 && c <= 255
                && int.TryParse(chars[3], out d) && d > 0 && d <= 255;
        }

        /// <summary>
        /// 判断当前 String 对象是否是正确的 url 格式。
        /// </summary>
        /// <param name="str">被判断的字符串对象。</param>
        /// <returns>如果当前 String 对象是否是正确的 url 格式，则返回 true，否则返回 false。</returns>
        public static bool IsUrl(this string str)
        {
            return str.IsMatch(@"^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 判断当前 String 对象是否为合法的 ipv4 或者 url 格式
        /// </summary>
        /// <param name="str">被判断的字符串对象。</param>
        /// <returns>如果当前 String 对象是否为合法的 ipv4 或者 url 格式，则返回 true，否则返回 false。</returns>
        public static bool IsUrlOrIPv4(this string str)
        {
            return str.IsUrl() || str.IsIPv4();
        }

        /// <summary>
        /// 判断是否为合法的货币格式
        /// </summary>
        /// <param name="str">被判断的字符串对象。</param>
        /// <returns>如果指定的 String 对象是为合法的货币格式，则返回 true，否则返回 false。</returns>
        public static bool IsCurrency(this string str)
        {
            return str.IsMatch(@"^\d{0,}(\.\d+)?$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 判断是否为合法的 QQ 号码格式
        /// </summary>
        /// <param name="str">被判断的字符串对象。</param>
        /// <returns>如果指定的 String 对象为合法的 QQ 号码格式，则返回 true，否则返回 false。</returns>
        public static bool IsQQ(this string str)
        {
            return str.IsMatch(@"^[1-9]\d{4,11}$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 判断是否为合法的 MSN 帐号格式
        /// </summary>
        /// <param name="str">被判断的字符串对象。</param>
        /// <returns>如果指定的 String 对象为合法的 MSN 帐号格式，则返回 true，否则返回 false。</returns>
        public static bool IsMSN(this string str)
        {
            return str.IsEmail();
        }

        /// <summary>
        /// 验证是否包含空格和非法字符Z
        /// </summary>
        /// <param name="str">被判断的字符串对象。</param>
        /// <returns>如果指定的 String 对象包含空格和非法字符Z，则返回 true，否则返回 false。</returns>
        public static bool IsUnNormal(this string str)
        {
            return str.IsMatch(@".+", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 验证是否为合法的车牌号码
        /// </summary>
        /// <param name="str">被判断的字符串对象。</param>
        /// <returns>如果指定的 String 对象为合法的车牌号码格式，则返回 true，否则返回 false。</returns>
        public static bool IsCarNo(this string str)
        {
            return str.IsMatch(@"^[\u4E00-\u9FA5][\da-zA-Z]{6}$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 验证是否为合法的汽车发动机序列号
        /// </summary>
        /// <param name="str">被判断的字符串对象。</param>
        /// <returns>如果指定的 String 对象为合法的汽车发动机序列号格式，则返回 true，否则返回 false。</returns>
        public static bool IsCarEngineNo(this string str)
        {
            return str.IsMatch(@"^[a-zA-Z0-9]{16}$");
        }

        /// <summary>
        /// 验证是否可以作为合法的用户名字符(字母开头，允许6-16字节，允许字母数字下划线)
        /// </summary>
        /// <param name="str">被判断的字符串对象。</param>
        /// <returns>如果指定的 String 对象为合法的用户名字符(字母开头，允许6-16字节，允许字母数字下划线)，则返回 true，否则返回 false。</returns>
        public static bool IsUserName(this string str)
        {
            return str.IsMatch(@"^[a-zA-Z][a-zA-Z0-9_]{5,15}$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 验证是否可以作为合法的名称标识符字符
        /// </summary>
        /// <param name="str">被判断的字符串对象。</param>
        /// <returns>如果指定的 String 对象为合法的名称标识符字符，则返回 true，否则返回 false。</returns>
        public static bool IsNameToken(this string str)
        {
            return str.IsUserName();
        }

        /// <summary>
        /// 判断当前 String 对象是否是正确的 身份证号码(中国) 格式(15或18位)。
        /// </summary>
        /// <param name="str">被判断的字符串对象。</param>
        /// <returns>如果指定的 String 对象是正确的 身份证号码(中国) 格式(15或18位)，则返回 true，否则返回 false。</returns>
        public static bool IsIDCard(this string str)
        {
            string sId = str;
            if (sId.IsMatch(@"!^\d{17}(\d|x)$", RegexOptions.IgnoreCase))
            {
                return false;
            }
            sId = Regex.Replace(sId, "x$", "a", RegexOptions.IgnoreCase);
            if (Dictionaries.IDCardAreas.Select(pair => pair.Key).Contains(sId.Substring(0, 2), StringComparer.InvariantCultureIgnoreCase))
            {
                return false;
            }
            string sBirthday = sId.Substring(6, 4) + "/" + sId.Substring(10, 2) + "/" + sId.Substring(12, 2);
            DateTime d;
            if (!DateTime.TryParse(sBirthday, out d))
            {
                return false;
            }
            double iSum = 0;
            for (int i = 17; i >= 0; i--)
            {
                iSum += (Math.Pow(2, i) % 11) * int.Parse(sId[17 - i].ToString(), NumberStyles.HexNumber);
            }
            if (iSum % 11 != 1)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 验证是否为整数格式
        /// </summary>
        /// <param name="str">被判断的字符串对象。</param>
        /// <returns>如果指定的 String 对象为整数格式，则返回 true，否则返回 false。</returns>
        public static bool IsInteger(this string str)
        {
            int i;
            return int.TryParse(str, out i);
        }

        /// <summary>
        /// 判断当前 String 对象是否是正确的 数字 格式。
        /// </summary>
        /// <param name="str">被判断的字符串对象。</param>
        /// <returns>如果指定的 String 对象是正确的 数字 格式，则返回 true，否则返回 false。</returns>
        public static bool IsNumeric(this string str)
        {
            decimal d;
            return decimal.TryParse(str, out d);
        }

        /// <summary>
        /// 判断当前 String 对象是否是正确的 颜色(#FFFFFF 或 #FFF 形式) 格式。
        /// </summary>
        /// <param name="str">被判断的字符串对象。</param>
        /// <returns>如果指定的 String 对象是正确的 颜色(#FFFFFF 或 #FFF 形式) 格式，则返回 true，否则返回 false。</returns>
        public static bool IsColor(this string str)
        {
            return str.IsMatch(@"^\#([a-fA-F0-9]{3}|[a-fA-F0-9]{6})$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 判断当前 String 对象是否可以作为安全密码字符(由字符、数字和符号三种中至少两种组成，长度至少(含) 6 位).
        /// </summary>
        /// <param name="str">被判断的字符串对象。</param>
        /// <returns>如果当前 String 对象可以作为安全密码字符(由字符、数字和符号三种中至少两种组成，长度至少(含) 6 位)，则返回 true，否则返回 false。</returns>
        public static bool IsSafePassword(this string str)
        {
            return !str.IsMatch(@"^(([A-Z]*|[a-z]*|\d*|[-_\~!@#\$%\^&\*\.\(\)\[\]\{\}<>\?\\\/\'\" + "\"" + @"]*)|.{0,5})$|\s");
        }


        /// <summary>
        /// 判断字符串是否由纯空格组成。
        /// 如果字符串为 null 或空字符串值、或者含有空格外的其他任意字符，则返回 false；
        /// 如果字符串不为 null 则其中所有的字符都为空格，则返回 true。
        /// </summary>
        /// <param name="str">被判断的字符串。</param>
        /// <returns>如果字符串不为 null 则其中所有的字符都为空格，则返回 true；如果字符串为 null 或空字符串值、或者含有空格外的其他任意字符，则返回 false；</returns>
        public static bool IsWhiteSpace(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            for (int i = 0; i < str.Length; i++)
            {
                if (!char.IsWhiteSpace(str[i]))
                    return false;
            }
            return true;
        }



        /// <summary>
        /// 将字符串对象转换成全角字符串后返回转换后的结果。
        /// </summary>
        /// <param name="str">被处理的字符串对象。</param>
        /// <returns>返回字符串对象转换成全角字符串后的结果。</returns>
        public static string ToCaseFullChar(this string str)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (var i = 0; i < str.Length; i++)
            {
                char c = str[i];
                stringBuilder.Append((c > 0 && c < 255) ? Convert.ToChar(c + 65248) : c);
            }
            return stringBuilder.ToString();
        }


        /// <summary>
        /// 判断字符串对象是否为 javascript 语言环境中的 function 格式。
        /// </summary>
        /// <param name="str">被判断的字符串对象。</param>
        /// <returns>如果 str 不为空且以 "function" 开头和以 "}" 结尾，则返回 true，否则返回 false。</returns>
        public static bool IsJavaScriptFunction(this string str)
        {
            bool b = string.IsNullOrWhiteSpace(str);
            if (!b)
            {
                var tmp = str.Trim();
                b = tmp.StartsWith("function", StringComparison.InvariantCulture) && tmp.EndsWith("}", StringComparison.InvariantCulture);
            }
            return b;
        }

        /// <summary>
        /// 指示所指定的正则表达式在指定的输入字符串中是否找到了匹配项。
        /// 该函数是方法 System.Text.RegularExpressions.Regex.IsMatch 的一个快速引用。
        /// </summary>
        /// <param name="str">要搜索匹配项的字符串。</param>
        /// <param name="pattern">要匹配的正则表达式模式。</param>
        /// <returns>如果正则表达式找到匹配项，则为 true；否则，为 false。</returns>
        public static bool IsMatch(this string str, string pattern)
        {
            return Regex.IsMatch(str, pattern);
        }

        /// <summary>
        /// 指示所指定的正则表达式是否使用指定的匹配选项在指定的输入字符串中找到了匹配项。
        /// 该函数是方法 System.Text.RegularExpressions.Regex.IsMatch 的一个快速引用。
        /// </summary>
        /// <param name="str">要搜索匹配项的字符串。</param>
        /// <param name="pattern">要匹配的正则表达式模式。</param>
        /// <param name="regexOptions">枚举值的一个按位组合，这些枚举值提供匹配选项。</param>
        /// <returns>如果正则表达式找到匹配项，则为 true；否则，为 false。</returns>
        public static bool IsMatch(this string str, string pattern, RegexOptions regexOptions)
        {
            return Regex.IsMatch(str, pattern, regexOptions);
        }

        /// <summary>
        /// 指示所指定的正则表达式是否使用指定的匹配选项在指定的输入字符串中找到了匹配项和超时间隔。
        /// 该函数是方法 System.Text.RegularExpressions.Regex.IsMatch 的一个快速引用。
        /// </summary>
        /// <param name="str">要搜索匹配项的字符串。</param>
        /// <param name="pattern">要匹配的正则表达式模式。</param>
        /// <param name="regexOptions">枚举值的一个按位组合，这些枚举值提供匹配选项。</param>
        /// <param name="matchTimeout">超时间隔，或 System.Text.RegularExpressions.Regex.InfiniteMatchTimeout 指示该方法不应超时。</param>
        /// <returns>如果正则表达式找到匹配项，则为 true；否则，为 false。</returns>
        public static bool IsMatch(this string str, string pattern, RegexOptions regexOptions, TimeSpan matchTimeout)
        {
            return Regex.IsMatch(str, pattern, regexOptions, matchTimeout);
        }
    }
}
