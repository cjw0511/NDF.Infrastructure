using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDF.Serialization.Json.Converters
{
    /// <summary>
    /// 表示计算机的区域设置中指定的自定义格式，该字符串用于基于 Internet 工程任务组 (IETF) 征求意见文档 (RFC) 1123 规范的时间值。
    /// 一般为："ddd, dd MMM yyyy HH':'mm':'ss 'GMT'"。
    /// </summary>
    public class RFC1123DateTimeConverter : DateTimeConverter
    {
        /// <summary>
        /// 初始化类型 <see cref="RFC1123DateTimeConverter"/> 的新实例。
        /// </summary>
        public RFC1123DateTimeConverter() { }



        /// <summary>
        /// 定义用于将 <see cref="System.DateTime"/> 日期时间对象转换为字符串的格式。
        /// </summary>
        public override string DateTimeFormat
        {
            get
            {
                return this.Culture.DateTimeFormat.RFC1123Pattern;
            }
        }
    }
}
