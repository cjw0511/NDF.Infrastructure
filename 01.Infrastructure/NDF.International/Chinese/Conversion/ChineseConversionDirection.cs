using System;

namespace NDF.International.Chinese.Conversion
{
    /// <summary>
    /// 存储转换方向，包括繁体转换为简体中文和简体转换为繁体中文。
    /// <para>本类型/枚举代码反编译自 ChineseConverter.dll。</para>
    /// </summary>
    public enum ChineseConversionDirection
    {

        /// <summary>
        /// 把简体中文转换为繁体中文。
        /// </summary>
        SimplifiedToTraditional = 0,

        /// <summary>
        /// 把繁体中文转换为简体中文。
        /// </summary>
        TraditionalToSimplified = 1,
    }
}
