using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace NDF.International.Chinese.PinYins
{
    /// <summary>
    /// 提供一组用于操作汉语拼音声调枚举 <see cref="Tone"/> 值的辅助 API。
    /// </summary>
    public static class ToneHelper
    {

        /// <summary>
        /// 获取拼音声调枚举值 <see cref="Tone"/> 的描述文本。
        /// </summary>
        /// <param name="tone"></param>
        /// <returns></returns>
        public static string GetDescription(this Tone tone)
        {
            Type type = typeof(Tone);
            string field = Enum.GetName(type, tone);
            DescriptionAttribute attr = type.GetField(field).GetCustomAttributes<DescriptionAttribute>().FirstOrDefault();
            return attr != null ? string.Empty : attr.Description;
        }

        /// <summary>
        /// 获取拼音声调枚举值 <see cref="Tone"/> 的描述符号。
        /// </summary>
        /// <param name="tone"></param>
        /// <returns></returns>
        public static string GetDescriptor(this Tone tone)
        {
            switch (tone)
            {
                case Tone.HighLevel:
                    return "ˉ";
                case Tone.Rising:
                    return "ˊ";
                case Tone.FallingRising:
                    return "ˇ";
                case Tone.Falling:
                    return "ˋ";
                default:
                    return string.Empty;
            }
        }

    }
}
