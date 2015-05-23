
一、该命名空间（NDF.International.Formatting）及其子命名空间下的所有类型，均反编译（通过 .NET Reflector 工具）自
    Microsoft Visual Studio International Pack 1.0 中的
    East Asia Numeric Formatting Library（EastAsiaNumericFormatter.dll 亚洲语系数值字符串格式化类库），
    关于该 EastAsiaNumericFormatter.dll 库的更多信息，请参阅：http://www.microsoft.com/zh-cn/download/details.aspx?id=15251。


二、功能说明（摘自 East Asia Numeric Formatting Library 的 API 文档）
    这个类支持以下的东亚语言： 
        简体中文 
        繁体中文 
        日语 
        韩语 
    这个类支持以下格式化字符串： 
        标准格式(L)：又称大写。 
        普通格式(Ln)：又称小写。 
        货币格式(Lc)：用来表示货币。 
        字译格式(Lt)：以数字符号字母表示数值型数据，只支持日文。 
        为了解释文化和格式化组合如何工作，我们将以“12345”举例。 
    简体中文 
        标准：壹万贰仟叁佰肆拾伍 
        普通：一万二千三百四十五 
        货币：壹万贰仟叁佰肆拾伍 
        字译：抛出 ArgumentException 异常 
    繁体中文 
        标准：壹萬貳仟參佰肆拾伍 
        普通：一萬二千三百四十五 
        货币：壹萬貳仟參佰肆拾伍 
        字译：抛出 ArgumentException 异常 
    日语 
        标准：壱萬弐阡参百四拾伍 
        普通：一万二千三百四十五 
        货币：抛出 ArgumentException 异常 
        字译：一二三四五 
    韩语 
        标准：일만 이천삼백사십오 
        普通：抛出 ArgumentException 异常 
        货币：일만 이천삼백사십오 
        字译：抛出 ArgumentException 异常 
    其他语言：抛出 ArgumentException 异常 
    被支持的数据类型，包括 double、float、int、uint、long、ulong、short、ushort、sbyte、byte 和 decimal。


三、示例代码（摘自 East Asia Numeric Formatting Library 的 API 文档）
    以下的代码演示了一个把数值转换为东亚的本地数字表示形式的字符串的实例：

    using System;
    using Microsoft.International.Formatters;
    using System.Globalization;
    using System.Diagnostics;

    namespace Example_LocalNumericFormat
    {
        class Program
        {
            static void Main(string[] args)
            {
                Debug.WriteLine(string.Format(new EastAsiaNumericFormatter(), "The representation for number 123.45 in Standard format of current language is {0:L}", 123.45));
                Debug.WriteLine("The representation for number 123.45 in Japanese Standard format is " + EastAsiaNumericFormatter.FormatWithCulture("L", 123.45, null, new CultureInfo("ja")));  
            }
        }
    }

    // The code produces different debug output with different culture settings:
    // 
    // Chinese-Simplified:
    // The representation for number 123.45 in Standard format of current language is 壹佰贰拾叁点肆伍.
    // The representation for number 123.45 in Japanese Standard format is 壱百弐拾参.
    // 
    // Chinese-Traditional:
    // The representation for number 123.45 in Standard format of current language is 壹佰貳拾參點肆伍.
    // The representation for number 123.45 in Japanese Standard format is 壱百弐拾参.
    // 
    // Japanese:
    // The representation for number 123.45 in Standard format of current language is 壱百弐拾参.
    // The representation for number 123.45 in Japanese Standard format is 壱百弐拾参.
    // 
    // Korean:
    // The representation for number 123.45 in Standard format of current language is 백이십삼 점 사오.
    // The representation for number 123.45 in Japanese Standard format is 壱百弐拾参.
    // 
    // Other languages:
    // ArgumentException will be thrown