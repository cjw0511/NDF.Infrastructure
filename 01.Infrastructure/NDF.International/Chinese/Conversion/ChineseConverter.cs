using System;
using System.Runtime.InteropServices;

namespace NDF.International.Chinese.Conversion
{
    /// <summary>
    /// 提供一个工具类进行简体中文和繁体中文之间的相互转换。
    /// </summary>
    public static class ChineseConverter
    {

        /// <summary>
        /// 转换简体中文和繁体中文字符串。
        /// </summary>
        /// <param name="text"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">参数 <paramref name="text"/> 为 null。</exception>
        public static string Convert(string text, ChineseConversionDirection direction)
        {
            // 本方法代码反编译自 ChineseConverter.dll。

            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            OfficeConversionEngine engine = OfficeConversionEngine.Create();
            if (engine != null)
            {
                return engine.TCSCConvert(text, direction);
            }
            uint dwMapFlags = (direction == ChineseConversionDirection.TraditionalToSimplified) ? (uint)0x2000000 : (uint)0x4000000;
            int cb = (text.Length * 2) + 2;
            IntPtr lpDestStr = Marshal.AllocHGlobal(cb);
            NativeMethods.LCMapString(0x804, dwMapFlags, text, -1, lpDestStr, cb);
            string str = Marshal.PtrToStringUni(lpDestStr);
            Marshal.FreeHGlobal(lpDestStr);
            return str;
        }

        /// <summary>
        /// 转换简体中文和繁体中文字符。
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static char Convert(char ch, ChineseConversionDirection direction)
        {
            ChineseChar result;
            return ChineseChar.TryParse(ch, out result)
                ? (direction == ChineseConversionDirection.SimplifiedToTraditional) ? result.Traditional : result.Simplified
                : ch;
        }

    }
}
