using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Serialization.Json.Converters
{
    /// <summary>
    /// 表示日期对象序列化为字符串的格式。
    /// </summary>
    public enum DateTimeFormatType
    {
        /// <summary>
        /// 定义计算机的区域设置中指定的标准日期时间格式。为："yyyy/MM/dd hh:mm:ss.fff"。
        /// </summary>
        [DateTimeConverterType(typeof(DateTimeConverter))]
        Normal,

        /// <summary>
        /// 表示计算机的区域设置中的通用日期/时间格式；默认值。
        ///     如果日期时间对象存在日期部分而没有时间部分，则只显示日期/短日期格式。
        ///     如果日期时间对象存在时间部分而没有日期部分，则只显示时间/短时间格式。
        ///     如果日期时间对象既存在日期部分又存在时间部分，则按照 DateTimeConverter 默认格式 "yyyy/MM/dd hh:mm:ss.fff" 显示。
        /// </summary>
        [DateTimeConverterType(typeof(GeneralDateConverter))]
        GeneralDate,

        /// <summary>
        /// 表示计算机的区域设置中指定的长日期格式。一般为："yyyy'年'MM'月'dd'日', dddd"。
        /// </summary>
        [DateTimeConverterType(typeof(LongDateConverter))]
        LongDate,

        /// <summary>
        /// 表示计算机的区域设置中指定的长时间格式。一般为："HH:mm:ss"。
        /// </summary>
        [DateTimeConverterType(typeof(LongTimeConverter))]
        LongTime,

        /// <summary>
        /// 表示计算机的区域设置中指定的完整日期时间格式。一般为："yyyy'年'MM'月'dd'日', dddd HH:mm:ss"。
        /// </summary>
        [DateTimeConverterType(typeof(FullDateTimeConverter))]
        FullDateTime,

        /// <summary>
        /// 表示计算机的区域设置中指定的长时间格式。 一般为："yyyy/MM/dd"。
        /// </summary>
        [DateTimeConverterType(typeof(ShortDateConverter))]
        ShortDate,

        /// <summary>
        /// 表示计算机的区域设置中指定的长时间格式。 一般为："HH:mm"。
        /// </summary>
        [DateTimeConverterType(typeof(ShortTimeConverter))]
        ShortTime,

        /// <summary>
        /// 表示计算机的区域设置中指定的月份和日期格式。 一般为："M'月'd'日'"。
        /// </summary>
        [DateTimeConverterType(typeof(MonthDayConverter))]
        MonthDay,

        /// <summary>
        /// 表示计算机的区域设置中指定的格式。 一般为："yyyy'年'M'月'"。
        /// </summary>
        [DateTimeConverterType(typeof(YearMonthConverter))]
        YearMonth,

        /// <summary>
        /// 表示计算机的区域设置中指定的自定义格式，该字符串用于基于 Internet 工程任务组 (IETF) 征求意见文档 (RFC) 1123 规范的时间值。 
        /// 一般为："ddd, dd MMM yyyy HH':'mm':'ss 'GMT'"。
        /// </summary>
        [DateTimeConverterType(typeof(RFC1123DateTimeConverter))]
        RFC1123DateTime,

        /// <summary>
        /// 表示计算机的区域设置中指定的可排序数据和时间值格式。 一般为："yyyy'-'MM'-'dd'T'HH':'mm':'ss"。
        /// </summary>
        [DateTimeConverterType(typeof(SortableDateTimeConverter))]
        SortableDateTime,

        /// <summary>
        /// 表示计算机的区域设置中指定的通用可排序数据和时间字符串格式。 一般为："yyyy'-'MM'-'dd HH':'mm':'ss'Z'"。
        /// </summary>
        [DateTimeConverterType(typeof(UniversalSortableDateTimeConverter))]
        UniversalSortableDateTime,

        /// <summary>
        /// 表示计算机的区域设置中指定的自定义字符串格式，该格式基于 ISO 8601 规范。 一般为："yyyy-MM-dd'T'hh:mm'Z'"。
        /// </summary>
        [DateTimeConverterType(typeof(IsoDateTimeConverter))]
        IsoDateTime,

        /// <summary>
        /// 表示 JavaScript 日期对象 "new Date(milliseconds)" 表示法。
        /// </summary>
        [DateTimeConverterType(typeof(JavaScriptDateTimeConverter))]
        JavaScriptDateTime,
    }
}
