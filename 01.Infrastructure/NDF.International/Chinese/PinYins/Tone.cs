using System;
using System.ComponentModel;

namespace NDF.International.Chinese.PinYins
{
    /// <summary>
    /// 表示现代汉语中拼音的声调。
    /// </summary>
    public enum Tone
    {

        /// <summary>
        /// 表示未定义声调，默认值。
        /// </summary>
        [Description("未定义声调")]
        Undefined = 0,

        /// <summary>
        /// 一声，即平声调中的阴平调（声调符为 ˉ）。
        /// </summary>
        [Description("阴平调")]
        HighLevel= 1,

        /// <summary>
        /// 二声，即平声调中的阳平调（声调符为 ˊ）。
        /// </summary>
        [Description("阳平调")]
        Rising = 2,

        /// <summary>
        /// 三声，也称作上声调（声调符为 ˇ）。
        /// </summary>
        [Description("上声调")]
        FallingRising = 3,

        /// <summary>
        /// 四声，即去声，也称作降升调（声调符为 ˋ）。
        /// </summary>
        [Description("去声调")]
        Falling = 4,

        /// <summary>
        /// 五声，即轻声（无声调符）。
        /// </summary>
        [Description("轻声调")]
        Light = 5,

    }
}
