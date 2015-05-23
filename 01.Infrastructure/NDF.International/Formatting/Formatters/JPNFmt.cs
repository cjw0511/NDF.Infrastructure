/*
 * 该类型中所有代码反编译（通过 .NET Reflector 工具）自 Microsoft Visual Studio International Pack 1.0 中的
 * East Asia Numeric Formatting Library（EastAsiaNumericFormatter.dll 亚洲语系数值字符串格式化类库），关于
 * 该 EastAsiaNumericFormatter.dll 库的更多信息，请参阅：http://www.microsoft.com/zh-cn/download/details.aspx?id=15251
 */

namespace NDF.International.Formatting.Formatters
{
    using System;

    internal abstract class JPNFmt : EastAsiaFormatter
    {
        protected JPNFmt()
        {
        }

        protected internal override bool CheckOutOfRange(double num)
        {
            if ((num < 1E+16) && (num >= 0.0))
            {
                return base.CheckOutOfRange(num);
            }
            return true;
        }

        protected override decimal Initialize(decimal num)
        {
            return Math.Truncate(num);
        }
    }
}
