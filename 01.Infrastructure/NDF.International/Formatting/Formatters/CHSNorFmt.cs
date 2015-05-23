/*
 * 该类型中所有代码反编译（通过 .NET Reflector 工具）自 Microsoft Visual Studio International Pack 1.0 中的
 * East Asia Numeric Formatting Library（EastAsiaNumericFormatter.dll 亚洲语系数值字符串格式化类库），关于
 * 该 EastAsiaNumericFormatter.dll 库的更多信息，请参阅：http://www.microsoft.com/zh-cn/download/details.aspx?id=15251
 */

namespace NDF.International.Formatting.Formatters
{
    using System;

    internal class CHSNorFmt : CHFmt
    {
        protected override string DecimalPoint
        {
            get
            {
                return "点";
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
                return "百";
            }
        }

        protected override string HundredMillion
        {
            get
            {
                return "亿";
            }
        }

        protected override string Minus
        {
            get
            {
                return "负";
            }
        }

        protected override string Ten
        {
            get
            {
                return "十";
            }
        }

        protected override string TenThousand
        {
            get
            {
                return "万";
            }
        }

        protected override string Thousand
        {
            get
            {
                return "千";
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
                return "〇";
            }
        }
    }
}
