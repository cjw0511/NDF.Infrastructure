using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Utilities
{
    /// <summary>
    /// 提供一组对 日期时间点对象 <see cref="System.DateTimeOffset"/> 操作方法的扩展。
    /// </summary>
    public static partial class DateTimeOffsetExtensions
    {
        /// <summary>
        /// 获取一个布尔值，表示当前 日期时间点对象 <see cref="System.DateTimeOffset"/> 是否存在日期部分。
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static bool HasDate(this DateTimeOffset _this)
        {
            return _this.DateTime.HasDate();
        }

        /// <summary>
        /// 获取一个布尔值，表示当前 日期时间点对象 <see cref="System.DateTimeOffset"/> 是否存在时间部分。
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static bool HasTime(this DateTimeOffset _this)
        {
            return _this.DateTime.HasTime();
        }
    }
}
