using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDF.Serialization.Json.Converters
{
    /// <summary>
    /// 表示计算机的区域设置中的通用日期/时间格式；
    ///     如果日期时间对象存在日期部分而没有时间部分，则只显示日期/短日期格式。
    ///     如果日期时间对象存在时间部分而没有日期部分，则只显示时间/短时间格式。
    ///     如果日期时间对象既存在日期部分又存在时间部分，则按照 <see cref="DateTimeConverter"/> 默认格式 "yyyy/MM/dd HH:mm:ss" 显示。
    /// </summary>
    public class GeneralDateTimeConverter : DateTimeConverter
    {
        /// <summary>
        /// 初始化类型 <see cref="GeneralDateTimeConverter"/> 的新实例。
        /// </summary>
        public GeneralDateTimeConverter() { }



        /// <summary>
        /// 定义将时间对象 <see cref="System.DateTime"/> 转换为字符串的方法。
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        protected override string ToString(DateTime datetime)
        {
            if (datetime.HasDate())
                return datetime.HasTime() ? base.ToString(datetime) : datetime.ToString(this.Culture.DateTimeFormat.ShortDatePattern, this.Culture);
            else if (datetime.HasTime())
                return datetime.ToString(this.Culture.DateTimeFormat.ShortTimePattern, this.Culture);

            return base.ToString(datetime);
        }

        /// <summary>
        /// 定义将时间点对象 <see cref="System.DateTimeOffset"/> 转换为字符串的方法。
        /// </summary>
        /// <param name="datetimeOffset"></param>
        /// <returns></returns>
        protected override string ToString(DateTimeOffset datetimeOffset)
        {
            if (datetimeOffset.HasDate())
                return datetimeOffset.HasTime() ? base.ToString(datetimeOffset) : datetimeOffset.ToString(this.Culture.DateTimeFormat.ShortDatePattern, this.Culture);
            else if (datetimeOffset.HasTime())
                return datetimeOffset.ToString(this.Culture.DateTimeFormat.ShortTimePattern, this.Culture);

            return base.ToString(datetimeOffset);
        }
    }
}
