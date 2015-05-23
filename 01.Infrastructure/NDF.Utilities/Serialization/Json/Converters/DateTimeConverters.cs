using NDF.Utilities;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Serialization.Json.Converters
{
    /// <summary>
    /// 表示日期对象序列化为字符串的格式。该类型提供一组常用的日期时间对象转 JSON 格式化字符串转换器。
    /// </summary>
    public class DateTimeConverters
    {
        private static readonly DateTimeConverterBase _normal = new DateTimeConverter();
        private static readonly DateTimeConverterBase _standard = new StandardDateTimeConverter();
        private static readonly DateTimeConverterBase _general = new GeneralDateTimeConverter();
        private static readonly DateTimeConverterBase _longDate = new LongDateConverter();
        private static readonly DateTimeConverterBase _longTime = new LongTimeConverter();
        private static readonly DateTimeConverterBase _fullDateTime = new FullDateTimeConverter();
        private static readonly DateTimeConverterBase _shortDate = new ShortDateConverter();
        private static readonly DateTimeConverterBase _shortTime = new ShortTimeConverter();
        private static readonly DateTimeConverterBase _monthDay = new MonthDayConverter();
        private static readonly DateTimeConverterBase _yearMonth = new YearMonthConverter();
        private static readonly DateTimeConverterBase _rfc1123DateTime = new RFC1123DateTimeConverter();
        private static readonly DateTimeConverterBase _sortableDateTime = new SortableDateTimeConverter();
        private static readonly DateTimeConverterBase _universalSortableDateTime = new UniversalSortableDateTimeConverter();
        private static readonly DateTimeConverterBase _isoDateTime = new IsoDateTimeConverter();
        private static readonly DateTimeConverterBase _javaScriptDateTime = new JavaScriptDateTimeConverter();

        private static readonly Type _normalConverterType = _normal.GetType();
        private static readonly Type _standardConverterType = _standard.GetType();
        private static readonly Type _generalConverterType = _general.GetType();
        private static readonly Type _longDateConverterType = _longDate.GetType();
        private static readonly Type _longTimeConverterType = _longTime.GetType();
        private static readonly Type _fullDateTimeConverterType = _fullDateTime.GetType();
        private static readonly Type _shortDateConverterType = _shortDate.GetType();
        private static readonly Type _shortTimeConverterType = _shortTime.GetType();
        private static readonly Type _monthDayConverterType = _monthDay.GetType();
        private static readonly Type _yearMonthConverterType = _yearMonth.GetType();
        private static readonly Type _rfc1123DateTimeConverterType = _rfc1123DateTime.GetType();
        private static readonly Type _sortableDateTimeConverterType = _sortableDateTime.GetType();
        private static readonly Type _universalSortableDateTimeConverterType = _universalSortableDateTime.GetType();
        private static readonly Type _isoDateTimeConverterType = _isoDateTime.GetType();
        private static readonly Type _javaScriptDateTimeConverterType = _javaScriptDateTime.GetType();


        /// <summary>
        /// 根据指定的日期时间对象序列化类型定义获取一个日期序列化转换器对象。
        /// </summary>
        /// <param name="dateTimeFormatType">指定的日期时间对象序列化类型定义。</param>
        /// <returns>返回一个日期序列化转换器类型的 <see cref="DateTimeConverterBase"/> 实例。</returns>
        public static DateTimeConverterBase Create(DateTimeFormatType dateTimeFormatType)
        {
            IEnumerable<DateTimeConverterTypeAttribute> attrs = dateTimeFormatType.GetCustomeAttributes<DateTimeConverterTypeAttribute>();
            if (attrs.IsNullOrEmpty())
            {
                throw new InvalidOperationException("获取指定的日期时间 JSON 字符串格式转换器出错，传入的枚举值 {0} 没有定义相应的转换器属性。".Format(dateTimeFormatType));
            }
            Type converterType = attrs.First().ConverterType;
            return Activator.CreateInstance(converterType) as DateTimeConverterBase;
        }



        /// <summary>
        /// 定义计算机的区域设置中指定的标准日期时间格式。为："yyyy/MM/dd HH:mm:ss.fff"。
        /// </summary>
        public static DateTimeConverterBase Normal
        {
            get { return _normal; }
        }

        /// <summary>
        /// 定义计算机的区域设置中指定的标准日期时间格式。为："yyyy/MM/dd HH:mm:ss.fff"。
        /// </summary>
        public static Type NormalConverterType
        {
            get { return _normalConverterType; }
        }


        /// <summary>
        /// 定义计算机的区域设置中指定的标准日期时间格式。为："yyyy/MM/dd HH:mm:ss.fff"。
        /// </summary>
        public static DateTimeConverterBase Standard
        {
            get { return _standard; }
        }

        /// <summary>
        /// 定义计算机的区域设置中指定的标准日期时间格式。为："yyyy/MM/dd HH:mm:ss.fff"。
        /// </summary>
        public static Type StandardConverterType
        {
            get { return _standardConverterType; }
        }


        /// <summary>
        /// 表示计算机的区域设置中的通用日期/时间格式；
        ///     如果日期时间对象存在日期部分而没有时间部分，则只显示日期/短日期格式。
        ///     如果日期时间对象存在时间部分而没有日期部分，则只显示时间/短时间格式。
        ///     如果日期时间对象既存在日期部分又存在时间部分，则按照 DateTimeConverter 默认格式 "yyyy/MM/dd HH:mm:ss" 显示。
        /// </summary>
        public static DateTimeConverterBase General
        {
            get { return _general; }
        }

        /// <summary>
        /// 表示计算机的区域设置中的通用日期/时间格式；
        ///     如果日期时间对象存在日期部分而没有时间部分，则只显示日期/短日期格式。
        ///     如果日期时间对象存在时间部分而没有日期部分，则只显示时间/短时间格式。
        ///     如果日期时间对象既存在日期部分又存在时间部分，则按照 DateTimeConverter 默认格式 "yyyy/MM/dd HH:mm:ss.fff" 显示。
        /// </summary>
        public static Type GeneralConverterType
        {
            get { return _generalConverterType; }
        }


        /// <summary>
        /// 表示计算机的区域设置中指定的长日期格式。一般为："yyyy'年'MM'月'dd'日', dddd"。
        /// </summary>
        public static DateTimeConverterBase LongDate
        {
            get { return _longDate; }
        }

        /// <summary>
        /// 表示计算机的区域设置中指定的长日期格式。一般为："yyyy'年'MM'月'dd'日', dddd"。
        /// </summary>
        public static Type LongDateConverterType
        {
            get { return _longDateConverterType; }
        }


        /// <summary>
        /// 表示计算机的区域设置中指定的长时间格式。一般为："HH:mm:ss"。
        /// </summary>
        public static DateTimeConverterBase LongTime
        {
            get { return _longTime; }
        }

        /// <summary>
        /// 表示计算机的区域设置中指定的长时间格式。一般为："HH:mm:ss"。
        /// </summary>
        public static Type LongTimeConverterType
        {
            get { return _longTimeConverterType; }
        }


        /// <summary>
        /// 表示计算机的区域设置中指定的完整日期时间格式。一般为："yyyy'年'MM'月'dd'日', dddd HH:mm:ss"。
        /// </summary>
        public static DateTimeConverterBase FullDateTime
        {
            get { return _fullDateTime; }
        }

        /// <summary>
        /// 表示计算机的区域设置中指定的完整日期时间格式。一般为："yyyy'年'MM'月'dd'日', dddd HH:mm:ss"。
        /// </summary>
        public static Type FullDateTimeConverterType
        {
            get { return _fullDateTimeConverterType; }
        }


        /// <summary>
        /// 表示计算机的区域设置中指定的长时间格式。 一般为："yyyy/MM/dd"。
        /// </summary>
        public static DateTimeConverterBase ShortDate
        {
            get { return _shortDate; }
        }

        /// <summary>
        /// 表示计算机的区域设置中指定的长时间格式。 一般为："yyyy/MM/dd"。
        /// </summary>
        public static Type ShortDateConverterType
        {
            get { return _shortDateConverterType; }
        }


        /// <summary>
        /// 表示计算机的区域设置中指定的长时间格式。 一般为："HH:mm"。
        /// </summary>
        public static DateTimeConverterBase ShortTime
        {
            get { return _shortTime; }
        }

        /// <summary>
        /// 表示计算机的区域设置中指定的长时间格式。 一般为："HH:mm"。
        /// </summary>
        public static Type ShortTimeConverterType
        {
            get { return _shortTimeConverterType; }
        }


        /// <summary>
        /// 表示计算机的区域设置中指定的月份和日期格式。 一般为："M'月'd'日'"。
        /// </summary>
        public static DateTimeConverterBase MonthDay
        {
            get { return _monthDay; }
        }

        /// <summary>
        /// 表示计算机的区域设置中指定的月份和日期格式。 一般为："M'月'd'日'"。
        /// </summary>
        public static Type MonthDayConverterType
        {
            get { return _monthDayConverterType; }
        }


        /// <summary>
        /// 表示计算机的区域设置中指定的格式。 一般为："yyyy'年'M'月'"。
        /// </summary>
        public static DateTimeConverterBase YearMonth
        {
            get { return _yearMonth; }
        }

        /// <summary>
        /// 表示计算机的区域设置中指定的格式。 一般为："yyyy'年'M'月'"。
        /// </summary>
        public static Type YearMonthConverterType
        {
            get { return _yearMonthConverterType; }
        }


        /// <summary>
        /// 表示计算机的区域设置中指定的自定义格式，该字符串用于基于 Internet 工程任务组 (IETF) 征求意见文档 (RFC) 1123 规范的时间值。 
        /// 一般为："ddd, dd MMM yyyy HH':'mm':'ss 'GMT'"。
        /// </summary>
        public static DateTimeConverterBase RFC1123DateTime
        {
            get { return _rfc1123DateTime; }
        }

        /// <summary>
        /// 表示计算机的区域设置中指定的自定义格式，该字符串用于基于 Internet 工程任务组 (IETF) 征求意见文档 (RFC) 1123 规范的时间值。 
        /// 一般为："ddd, dd MMM yyyy HH':'mm':'ss 'GMT'"。
        /// </summary>
        public static Type RFC1123DateTimeConverterType
        {
            get { return _rfc1123DateTimeConverterType; }
        }


        /// <summary>
        /// 表示计算机的区域设置中指定的可排序数据和时间值格式。 一般为："yyyy'-'MM'-'dd'T'HH':'mm':'ss"。
        /// </summary>
        public static DateTimeConverterBase SortableDateTime
        {
            get { return _sortableDateTime; }
        }

        /// <summary>
        /// 表示计算机的区域设置中指定的可排序数据和时间值格式。 一般为："yyyy'-'MM'-'dd'T'HH':'mm':'ss"。
        /// </summary>
        public static Type SortableDateTimeConverterType
        {
            get { return _sortableDateTimeConverterType; }
        }


        /// <summary>
        /// 表示计算机的区域设置中指定的通用可排序数据和时间字符串格式。 一般为："yyyy'-'MM'-'dd HH':'mm':'ss'Z'"。
        /// </summary>
        public static DateTimeConverterBase UniversalSortableDateTime
        {
            get { return _universalSortableDateTime; }
        }

        /// <summary>
        /// 表示计算机的区域设置中指定的通用可排序数据和时间字符串格式。 一般为："yyyy'-'MM'-'dd HH':'mm':'ss'Z'"。
        /// </summary>
        public static Type UniversalSortableDateTimeConverterType
        {
            get { return _universalSortableDateTimeConverterType; }
        }


        /// <summary>
        /// 表示计算机的区域设置中指定的自定义字符串格式，该格式基于 ISO 8601 规范。 一般为："yyyy-MM-dd'T'hh:mm'Z'"。
        /// </summary>
        public static DateTimeConverterBase IsoDateTime
        {
            get { return _isoDateTime; }
        }

        /// <summary>
        /// 表示计算机的区域设置中指定的自定义字符串格式，该格式基于 ISO 8601 规范。 一般为："yyyy-MM-dd'T'hh:mm'Z'"。
        /// </summary>
        public static Type IsoDateTimeConverterType
        {
            get { return _isoDateTimeConverterType; }
        }


        /// <summary>
        /// 表示 JavaScript 日期对象 "new Date(milliseconds)" 表示法。
        /// </summary>
        public static DateTimeConverterBase JavaScriptDateTime
        {
            get { return _javaScriptDateTime; }
        }

        /// <summary>
        /// 表示 JavaScript 日期对象 "new Date(milliseconds)" 表示法。
        /// </summary>
        public static Type JavaScriptDateTimeConverterType
        {
            get { return _javaScriptDateTimeConverterType; }
        }


    }
}
