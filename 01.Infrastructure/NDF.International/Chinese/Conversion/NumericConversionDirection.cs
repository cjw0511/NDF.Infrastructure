using System;

namespace NDF.International.Chinese.Formatting
{
    /// <summary>
    /// 数值转换方向，包括数值与所在区域语言大写、小写以及货币格式字符串之间的转换。
    /// </summary>
    public enum NumericConversionDirection
    {

        /// <summary>
        /// 转换成区域语言的小写格式。
        /// </summary>
        LowerCase = 0,

        /// <summary>
        /// 转换成区域语言的大写格式。
        /// </summary>
        UpperCase = 1,

        /// <summary>
        /// 转换成区域语言的货币格式。
        /// </summary>
        Currency = 2,

    }
}
