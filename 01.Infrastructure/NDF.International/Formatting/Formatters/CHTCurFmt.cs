/*
 * 该类型中所有代码反编译（通过 .NET Reflector 工具）自 Microsoft Visual Studio International Pack 1.0 中的
 * East Asia Numeric Formatting Library（EastAsiaNumericFormatter.dll 亚洲语系数值字符串格式化类库），关于
 * 该 EastAsiaNumericFormatter.dll 库的更多信息，请参阅：http://www.microsoft.com/zh-cn/download/details.aspx?id=15251
 */

namespace NDF.International.Formatting.Formatters
{
    using System;

    internal class CHTCurFmt : CHFmt
    {
        protected override decimal Initialize(decimal num)
        {
            num = Math.Truncate((decimal)(num * 100M)) / 100M;
            return num;
        }

        protected override string DecimalPoint
        {
            get
            {
                return "點";
            }
        }

        protected override string[] Digits
        {
            get
            {
                return new string[] { "零", "壹", "貳", "參", "肆", "伍", "陸", "柒", "捌", "玖" };
            }
        }

        protected override string Hundred
        {
            get
            {
                return "佰";
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
                return "負";
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
                return "仟";
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
