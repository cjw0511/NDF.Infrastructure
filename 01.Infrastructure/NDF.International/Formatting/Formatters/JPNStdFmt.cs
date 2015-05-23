/*
 * 该类型中所有代码反编译（通过 .NET Reflector 工具）自 Microsoft Visual Studio International Pack 1.0 中的
 * East Asia Numeric Formatting Library（EastAsiaNumericFormatter.dll 亚洲语系数值字符串格式化类库），关于
 * 该 EastAsiaNumericFormatter.dll 库的更多信息，请参阅：http://www.microsoft.com/zh-cn/download/details.aspx?id=15251
 */

namespace NDF.International.Formatting.Formatters
{
    using System;
    using System.Text;

    internal class JPNStdFmt : JPNFmt
    {
        protected override void ConvertIntergralStackToText(StackWithIndex stack, StringBuilder text)
        {
            while (stack.Count > 0)
            {
                string str = stack.Pop();
                text.Append(str);
            }
        }

        protected override string GetDigitText(int digit, ulong position)
        {
            if (digit == 0)
            {
                return string.Empty;
            }
            return (this.Digits[digit] + this.GetPositionText(position));
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
                return new string[] { "零", "壱", "弐", "参", "四", "伍", "六", "七", "八", "九" };
            }
        }

        protected override string Hundred
        {
            get
            {
                return "百";
            }
        }

        protected override string HundredMillion
        {
            get
            {
                return "億";
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
                return "拾";
            }
        }

        protected override string TenThousand
        {
            get
            {
                return "萬";
            }
        }

        protected override string Thousand
        {
            get
            {
                return "阡";
            }
        }

        protected override string ThousandBillion
        {
            get
            {
                return "兆";
            }
        }

        protected override string Zero
        {
            get
            {
                return "零";
            }
        }
    }
}
