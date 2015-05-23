using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Serialization.Json.Converters
{
    /// <summary>
    /// 定义计算机的区域设置中指定的标准日期时间格式。为："yyyy/MM/dd HH:mm:ss.fff"。
    /// </summary>
    public class StandardDateTimeConverter : DateTimeConverter
    {
        private const string _dateTimeFormat = "yyyy/MM/dd HH:mm:ss.fff";


        /// <summary>
        /// 初始化类型 <see cref="StandardDateTimeConverter"/> 的新实例。
        /// </summary>
        public StandardDateTimeConverter()
        {
        }


        /// <summary>
        /// 定义用于将 <see cref="System.DateTime"/> 日期时间对象转换为字符串的格式。
        /// </summary>
        public override string DateTimeFormat
        {
            get { return _dateTimeFormat; }
        }

    }
}
