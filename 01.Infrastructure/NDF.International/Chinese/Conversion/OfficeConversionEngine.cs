/*
 * 该类型中所有代码反编译（通过 .NET Reflector 工具）自 Microsoft Visual Studio International Pack 1.0 中的
 * Traditional Chinese to Simplified Chinese Conversion Library（ChineseConverter.dll 中文繁简转换类库），关于
 * 该 ChineseConverter.dll 库的更多信息，请参阅：http://www.microsoft.com/zh-cn/download/details.aspx?id=15251
 */

namespace NDF.International.Chinese.Conversion
{
    using Microsoft.Win32;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    /// <summary>
    /// 本类型代码反编译自 ChineseConverter.dll。
    /// </summary>
    internal class OfficeConversionEngine
    {
        private static string MSOPath;
        private static string Mstr2tscPath;

        static OfficeConversionEngine()
        {
            string str = null;
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Office\12.0\Common\InstallRoot");
            if (key != null)
            {
                str = Convert.ToString(key.GetValue("Path"), null);
            }
            RegistryKey key2 = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Office\12.0\Common\FilesPaths");
            if (key2 != null)
            {
                MSOPath = Convert.ToString(key2.GetValue("mso.dll"), null);
            }
            if (!string.IsNullOrEmpty(str))
            {
                Mstr2tscPath = Path.Combine(str, @"ADDINS\MSTR2TSC.DLL");
            }
            if (string.IsNullOrEmpty(Mstr2tscPath) || !File.Exists(Mstr2tscPath))
            {
                Mstr2tscPath = null;
            }
        }

        private OfficeConversionEngine()
        {
        }

        internal static OfficeConversionEngine Create()
        {
            if (!string.IsNullOrEmpty(MSOPath) && !string.IsNullOrEmpty(Mstr2tscPath))
            {
                return new OfficeConversionEngine();
            }
            return null;
        }

        internal string TCSCConvert(string input, ChineseConversionDirection direction)
        {
            string str2;
            IntPtr zero = IntPtr.Zero;
            IntPtr handle = IntPtr.Zero;
            try
            {
                IntPtr ptr3;
                int num;
                zero = NativeMethods.LoadLibrary(MSOPath);
                handle = NativeMethods.LoadLibrary(Mstr2tscPath);
                if (!NativeMethods.TCSCInitialize())
                {
                    return null;
                }
                string str = null;
                if (NativeMethods.TCSCConvertText(input, input.Length, out ptr3, out num, direction, false, true))
                {
                    str = Marshal.PtrToStringUni(ptr3);
                    NativeMethods.TCSCFreeConvertedText(ptr3);
                }
                NativeMethods.TCSCUninitialize();
                str2 = str;
            }
            finally
            {
                if (handle != IntPtr.Zero)
                {
                    NativeMethods.FreeLibrary(new HandleRef(this, handle));
                }
                if (zero != IntPtr.Zero)
                {
                    NativeMethods.FreeLibrary(new HandleRef(this, zero));
                }
            }
            return str2;
        }
    }
}
