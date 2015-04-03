using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDF.Serialization.Json.Converters
{
    /// <summary>
    /// 表示计算机的区域设置中指定的长时间格式。 一般为："HH:mm"。
    /// </summary>
    public class ShortTimeConverter : DateTimeConverter
    {
        /// <summary>
        /// 初始化类型 <see cref="ShortTimeConverter"/> 的新实例。
        /// </summary>
        public ShortTimeConverter() { }




        /// <summary>
        /// 定义用于将 <see cref="System.DateTime"/> 日期时间对象转换为字符串的格式。
        /// </summary>
        public override string DateTimeFormat
        {
            get
            {
                return this.Culture.DateTimeFormat.ShortTimePattern;
            }
        }
    }
}
