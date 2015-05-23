/*
 * 该类型中所有代码反编译（通过 .NET Reflector 工具）自 Microsoft Visual Studio International Pack 1.0 中的
 * East Asia Numeric Formatting Library（EastAsiaNumericFormatter.dll 亚洲语系数值字符串格式化类库），关于
 * 该 EastAsiaNumericFormatter.dll 库的更多信息，请参阅：http://www.microsoft.com/zh-cn/download/details.aspx?id=15251
 */

namespace NDF.International.Formatting
{
    using System;
    using System.Globalization;

    /// <summary>
    /// 提供一个格式化类，把数值型的数据转换为东亚的本地数字表示形式的字符串。
    /// </summary>
    /// <remarks>
    /// 这个类支持以下的东亚语言：
    ///     简体中文 繁体中文 日语 韩语 这个类支持以下格式化字符串： 标准格式(L)：又称大写。
    ///     普通格式(Ln)：又称小写。 货币格式(Lc)：用来表示货币。
    ///     字译格式(Lt)：以数字符号字母表示数值型数据，只支持日文。
    ///     
    /// 为了解释文化和格式化组合如何工作，我们将以“12345”举例。
    ///     简体中文
    ///         标准：壹万贰仟叁佰肆拾伍
    ///         普通：一万二千三百四十五
    ///         货币：壹万贰仟叁佰肆拾伍
    ///         字译：抛出 ArgumentException 异常
    ///     繁体中文
    ///         标准：壹萬貳仟參佰肆拾伍
    ///         普通：一萬二千三百四十五
    ///         货币：壹萬貳仟參佰肆拾伍
    ///         字译：抛出 ArgumentException 异常
    ///     日语
    ///         标准：壱萬弐阡参百四拾伍
    ///         普通：一万二千三百四十五
    ///         货币：抛出 ArgumentException 异常
    ///         字译：一二三四五
    ///     韩语
    ///         标准：일만 이천삼백사십오
    ///         普通：抛出 ArgumentException 异常
    ///         货币：일만 이천삼백사십오
    ///         字译：抛出 ArgumentException 异常
    ///     其他语言：
    ///         抛出 ArgumentException 异常
    /// 被支持的数据类型，包括 double、float、int、uint、long、ulong、short、ushort、sbyte、byte 和 decimal。
    /// </remarks>
    public class EastAsiaNumericFormatter : ICustomFormatter, IFormatProvider
    {
        /// <summary>
        /// 初始化EastAsiaNumericFormatter类的一个新实例。
        /// </summary>
        public EastAsiaNumericFormatter()
        {
        }

        /// <summary>
        /// 将对象格式化为东亚文化表示形式。
        /// </summary>
        /// <param name="format">格式类型。</param>
        /// <param name="arg">被格式化的数据。</param>
        /// <param name="formatProvider">格式提供者。</param>
        /// <returns>用东亚语言格式化过的本地化字符串。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="format"/>，<paramref name="arg"/> 或者 <paramref name="formatProvider"/> 是一个空引用。 </exception>
        /// <exception cref="ArgumentException">localFmt 在此区域语言中不被支持。 </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="arg"/> 超出范围。</exception>
        /// <exception cref="ArgumentException"><paramref name="arg"/> 是一个无效类型。</exception>
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            return FormatWithCulture(format, arg, formatProvider, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// 将对象格式化为指定的东亚文化表示形式。
        /// </summary>
        /// <param name="format">格式类型。</param>
        /// <param name="arg">将被格式化的数据。</param>
        /// <param name="formatProvider">格式提供者。</param>
        /// <param name="culture">区域语言类型。</param>
        /// <returns>用东亚语言格式化过的本地化字符串。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="format"/>，<paramref name="arg"/> 或者 <paramref name="culture"/> 是一个空引用。 </exception>
        /// <exception cref="ArgumentException">localFmt 在此区域语言中不被支持。 </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="arg"/> 超出范围。</exception>
        /// <exception cref="ArgumentException"><paramref name="arg"/> 是一个无效类型。</exception>
        public static string FormatWithCulture(string format, object arg, IFormatProvider formatProvider, CultureInfo culture)
        {
            if (format == null)
            {
                throw new ArgumentNullException("format");
            }
            if (arg == null)
            {
                throw new ArgumentNullException("arg");
            }
            if (culture == null)
            {
                throw new ArgumentNullException("culture");
            }
            if ((!format.Equals("L") && !format.Equals("Ln")) && (!format.Equals("Lt") && !format.Equals("Lc")))
            {
                IFormattable formattable = arg as IFormattable;
                if (formattable == null)
                {
                    return arg.ToString();
                }
                return formattable.ToString(format, formatProvider);
            }
            EastAsiaFormatter formatter = EastAsiaFormatter.Create(culture, format);
            if (formatter == null)
            {
                throw new ArgumentException("参数 format 的格式不正确。");
            }
            Type type = arg.GetType();
            if ((((type != typeof(double)) && (type != typeof(float))) && ((type != typeof(int)) && (type != typeof(uint)))) && ((((type != typeof(long)) && (type != typeof(ulong))) && ((type != typeof(short)) && (type != typeof(ushort)))) && (((type != typeof(sbyte)) && (type != typeof(byte))) && (type != typeof(decimal)))))
            {
                throw new ArgumentException("参数 arg 的类型不正确。");
            }
            double num = Convert.ToDouble(arg, null);
            if (formatter.CheckOutOfRange(num))
            {
                throw new ArgumentOutOfRangeException("arg");
            }
            return formatter.ConvertToLocalizedText(Convert.ToDecimal(arg, null));
        }

        /// <summary>
        /// 返回一个实现了ICustomFormatter接口的格式化对象。
        /// </summary>
        /// <param name="formatType">格式类型。</param>
        /// <returns>如果参数 <paramref name="formatType"/> 是 <see cref="ICustomFormatter"/>，则返回 <paramref name="formatType"/> 本身，否则返回一个空引用。</returns>
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
            {
                return this;
            }
            return null;
        }
    }
}
