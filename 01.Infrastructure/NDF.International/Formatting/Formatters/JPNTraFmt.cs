/*
 * 该类型中所有代码反编译（通过 .NET Reflector 工具）自 Microsoft Visual Studio International Pack 1.0 中的
 * East Asia Numeric Formatting Library（EastAsiaNumericFormatter.dll 亚洲语系数值字符串格式化类库），关于
 * 该 EastAsiaNumericFormatter.dll 库的更多信息，请参阅：http://www.microsoft.com/zh-cn/download/details.aspx?id=15251
 */

namespace NDF.International.Formatting.Formatters
{
    using System;
    using System.Text;

    internal class JPNTraFmt : JPNFmt
    {
        protected override void ConvertIntergralStackToText(StackWithIndex stack, StringBuilder text)
        {
            while (stack.Count > 0)
            {
                if (!(stack.Peek() == this.Zero))
                {
                    break;
                }
                stack.Pop();
            }
            while (stack.Count > 0)
            {
                text.Append(stack.Pop());
            }
        }

        protected override string GetDigitText(int digit, ulong position)
        {
            return this.Digits[digit];
        }

        protected override string DecimalPoint
        {
            get
            {
                return string.Empty;
            }
        }

        protected override string[] Digits
        {
            get
            {
                return new string[] { "〇", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
            }
        }

        protected override string Hundred
        {
            get
            {
                return string.Empty;
            }
        }

        protected override string HundredMillion
        {
            get
            {
                return string.Empty;
            }
        }

        protected override string Minus
        {
            get
            {
                return "-";
            }
        }

        protected override string Ten
        {
            get
            {
                return string.Empty;
            }
        }

        protected override string TenThousand
        {
            get
            {
                return string.Empty;
            }
        }

        protected override string Thousand
        {
            get
            {
                return string.Empty;
            }
        }

        protected override string ThousandBillion
        {
            get
            {
                return string.Empty;
            }
        }

        protected override string Zero
        {
            get
            {
                return "〇";
            }
        }
    }
}
