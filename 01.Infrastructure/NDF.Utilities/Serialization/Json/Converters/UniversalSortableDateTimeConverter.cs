using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDF.Serialization.Json.Converters
{
    /// <summary>
    /// 表示计算机的区域设置中指定的通用可排序数据和时间字符串格式。 一般为："yyyy'-'MM'-'dd HH':'mm':'ss'Z'"。
    /// </summary>
    public class UniversalSortableDateTimeConverter : DateTimeConverter
    {
        /// <summary>
        /// 初始化类型 <see cref="UniversalSortableDateTimeConverter"/> 的新实例。
        /// </summary>
        public UniversalSortableDateTimeConverter() { }



        /// <summary>
        /// 定义用于将 <see cref="System.DateTime"/> 日期时间对象转换为字符串的格式。
        /// </summary>
        public override string DateTimeFormat
        {
            get
            {
                return this.Culture.DateTimeFormat.UniversalSortableDateTimePattern;
            }
        }
    }
}
