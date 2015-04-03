using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Utilities
{
    /// <summary>
    /// 提供一组对 8 位无符号整数(System.Byte) 操作方法的扩展。
    /// </summary>
    public static class ByteExtensions
    {
        /// <summary>
        /// 判断传入的数值是否是一个奇数。
        /// </summary>
        /// <param name="_this">需要判断的数值。</param>
        /// <returns>如果传入的数值是否是一个奇数，则返回 true，否则返回 false。</returns>
        public static bool IsOdd(this Byte _this)
        {
            return (_this % 2) == 1;
        }

        /// <summary>
        /// 判断传入的数值是否是一个偶数。
        /// </summary>
        /// <param name="_this">需要判断的数值。</param>
        /// <returns>如果传入的数值是否是一个偶数，则返回 true，否则返回 false。</returns>
        public static bool IsEven(this Byte _this)
        {
            return (_this % 2) == 0;
        }
    }
}
