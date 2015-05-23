/*
 * 该类型中所有代码反编译（通过 .NET Reflector 工具）自 Microsoft Visual Studio International Pack 1.0 中的
 * East Asia Numeric Formatting Library（EastAsiaNumericFormatter.dll 亚洲语系数值字符串格式化类库），关于
 * 该 EastAsiaNumericFormatter.dll 库的更多信息，请参阅：http://www.microsoft.com/zh-cn/download/details.aspx?id=15251
 */

namespace NDF.International.Formatting.Formatters
{
    using System;
    using System.Text;

    internal class KORCurFmt : KORFmt
    {
        protected override void ConvertIntergralStackToText(StackWithIndex stack, StringBuilder text)
        {
            while (stack.Count > 0)
            {
                string str = stack.Pop();
                text.Append(str);
            }
            if (text[text.Length - 1] == ' ')
            {
                text.Remove(text.Length - 1, 1);
            }
        }

        protected override decimal Initialize(decimal num)
        {
            num = Math.Truncate((decimal)(num * 100M)) / 100M;
            return num;
        }
    }
}
