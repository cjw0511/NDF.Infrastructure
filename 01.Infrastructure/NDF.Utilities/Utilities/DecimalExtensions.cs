using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Utilities
{
    /// <summary>
    /// 提供一组对 十进制（128 位）值(System.Decimal) 操作方法的扩展。
    /// </summary>
    public static class DecimalExtensions
    {
        /// <summary>
        /// 判断传入的数值是否是一个奇数。
        /// </summary>
        /// <param name="_this">需要判断的数值。</param>
        /// <returns>如果传入的数值是否是一个奇数，则返回 true，否则返回 false。</returns>
        public static bool IsOdd(this Decimal _this)
        {
            return (_this % 2) == 1;
        }

        /// <summary>
        /// 判断传入的数值是否是一个偶数。
        /// </summary>
        /// <param name="_this">需要判断的数值。</param>
        /// <returns>如果传入的数值是否是一个偶数，则返回 true，否则返回 false。</returns>
        public static bool IsEven(this Decimal _this)
        {
            return (_this % 2) == 0;
        }

        /// <summary>
        /// 将双精度浮点值按指定的小数位舍入。
        /// </summary>
        /// <param name="_this">要舍入的双精度浮点数。</param>
        /// <param name="digits">返回值中的小数位数。</param>
        /// <returns>最接近 value 的 digits 位小数的数字。</returns>
        public static Decimal Round(this Decimal _this, int digits)
        {
            return Math.Round(_this, digits);
        }

        /// <summary>
        /// 获取数值的小数位数。
        /// </summary>
        /// <param name="_this">要获取小数位数的数值。</param>
        /// <returns>返回数值的小数位数。</returns>
        public static int GetDigits(this Decimal _this)
        {
            int digits = _this.ToString().LastIndexOf('.');
            return digits == -1 ? 0 : digits;
        }
    }
}
