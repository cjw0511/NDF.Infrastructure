/*
 * 该类型中所有代码反编译（通过 .NET Reflector 工具）自 Microsoft Visual Studio International Pack 1.0 中的
 * East Asia Numeric Formatting Library（EastAsiaNumericFormatter.dll 亚洲语系数值字符串格式化类库），关于
 * 该 EastAsiaNumericFormatter.dll 库的更多信息，请参阅：http://www.microsoft.com/zh-cn/download/details.aspx?id=15251
 */

namespace NDF.International.Formatting.Formatters
{
    using System;
    using System.Text;

    internal class KORStdFmt : KORFmt
    {
        protected override void ConvertIntergralStackToText(StackWithIndex stack, StringBuilder text)
        {
            while (stack.Count > 0)
            {
                if (!string.IsNullOrEmpty(stack.Peek()))
                {
                    break;
                }
                stack.Pop();
            }
            bool flag = false;
            if (stack.Peek().Equals(this.Digits[1]))
            {
                for (int i = 2; i < stack.Count; i++)
                {
                    if (!string.IsNullOrEmpty(stack[i]))
                    {
                        break;
                    }
                    if (i == (stack.Count - 1))
                    {
                        flag = true;
                    }
                }
            }
            else
            {
                flag = false;
            }
            while (stack.Count > 0)
            {
                string str = stack.Pop();
                if (!string.IsNullOrEmpty(str))
                {
                    if (flag)
                    {
                        flag = false;
                    }
                    else
                    {
                        if (!str.StartsWith(this.Digits[1]) || ((!str.EndsWith(this.Ten) && !str.EndsWith(this.Hundred)) && !str.EndsWith(this.Thousand)))
                        {
                            text.Append(str);
                            continue;
                        }
                        text.Append(str.Substring(this.Digits[1].Length));
                    }
                }
            }
            if (text[text.Length - 1] == ' ')
            {
                text.Remove(text.Length - 1, 1);
            }
        }
    }
}
