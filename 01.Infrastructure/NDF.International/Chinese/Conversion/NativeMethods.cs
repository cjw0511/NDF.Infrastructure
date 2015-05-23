/*
 * 该类型中所有代码反编译（通过 .NET Reflector 工具）自 Microsoft Visual Studio International Pack 1.0 中的
 * Traditional Chinese to Simplified Chinese Conversion Library（ChineseConverter.dll 中文繁简转换类库），关于
 * 该 ChineseConverter.dll 库的更多信息，请参阅：http://www.microsoft.com/zh-cn/download/details.aspx?id=15251
 */

namespace NDF.International.Chinese.Conversion
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// 本类型代码反编译自 ChineseConverter.dll。
    /// </summary>
    internal static class NativeMethods
    {
        public const uint LCMAP_SIMPLIFIED_CHINESE = 0x2000000;
        public const uint LCMAP_TRADITIONAL_CHINESE = 0x4000000;
        public const int zh_CN = 0x804;

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("KERNEL32.DLL", SetLastError = true)]
        internal static extern bool FreeLibrary(HandleRef hModule);


        [DllImport("KERNEL32.DLL", CharSet = CharSet.Unicode)]
        internal static extern int LCMapString(int Locale, uint dwMapFlags, [MarshalAs(UnmanagedType.LPTStr)] string lpSrcStr, int cchSrc, IntPtr lpDestStr, int cchDest);
        
        
        [DllImport("KERNEL32.DLL", CharSet = CharSet.Unicode)]
        internal static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPTStr)] string lpFileName);
        
        
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("MSTR2TSC.DLL", CharSet = CharSet.Unicode)]

        internal static extern bool TCSCConvertText([MarshalAs(UnmanagedType.LPTStr)] string pwszInput, int cchInput, out IntPtr ppwszOutput, out int pcchOutput, ChineseConversionDirection dwDirection, [MarshalAs(UnmanagedType.Bool)] bool fCharBase, [MarshalAs(UnmanagedType.Bool)] bool fLocalTerm);
        
        
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("MSTR2TSC.DLL")]
        internal static extern bool TCSCFreeConvertedText(IntPtr pv);
        
        
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("MSTR2TSC.DLL")]
        internal static extern bool TCSCInitialize();
        
        
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("MSTR2TSC.DLL")]
        internal static extern bool TCSCUninitialize();

    }
}
