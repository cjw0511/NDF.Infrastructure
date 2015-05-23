/*
 * 该类型中所有代码反编译（通过 .NET Reflector 工具）自 Microsoft Visual Studio International Pack 1.0 中的
 * East Asia Numeric Formatting Library（EastAsiaNumericFormatter.dll 亚洲语系数值字符串格式化类库），关于
 * 该 EastAsiaNumericFormatter.dll 库的更多信息，请参阅：http://www.microsoft.com/zh-cn/download/details.aspx?id=15251
 */

namespace NDF.International.Formatting.Formatters
{
    using System;

    internal abstract class KORFmt : EastAsiaFormatter
    {
        protected KORFmt()
        {
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
                return " 점 ";
            }
        }

        protected override string[] Digits
        {
            get
            {
                return new string[] { "영", "일", "이", "삼", "사", "오", "육", "칠", "팔", "구" };
            }
        }

        protected override string Hundred
        {
            get
            {
                return "백";
            }
        }

        protected override string HundredMillion
        {
            get
            {
                return "억 ";
            }
        }

        protected override string Minus
        {
            get
            {
                return "음수 ";
            }
        }

        protected override string Ten
        {
            get
            {
                return "십";
            }
        }

        protected override string TenThousand
        {
            get
            {
                return "만 ";
            }
        }

        protected override string Thousand
        {
            get
            {
                return "천";
            }
        }

        protected override string ThousandBillion
        {
            get
            {
                return "조 ";
            }
        }

        protected override string Zero
        {
            get
            {
                return "영";
            }
        }
    }
}
