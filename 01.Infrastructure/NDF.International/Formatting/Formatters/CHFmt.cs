/*
 * 该类型中所有代码反编译（通过 .NET Reflector 工具）自 Microsoft Visual Studio International Pack 1.0 中的
 * East Asia Numeric Formatting Library（EastAsiaNumericFormatter.dll 亚洲语系数值字符串格式化类库），关于
 * 该 EastAsiaNumericFormatter.dll 库的更多信息，请参阅：http://www.microsoft.com/zh-cn/download/details.aspx?id=15251
 */

namespace NDF.International.Formatting.Formatters
{
    using System.Text;

    internal abstract class CHFmt : EastAsiaFormatter
    {
        protected CHFmt()
        {
        }

        protected override void ConvertIntergralStackToText(StackWithIndex stack, StringBuilder text)
        {
            string zero = string.Empty;
            bool flag = true;
            while (stack.Count > 0)
            {
                string str2 = stack.Pop();
                if (str2 == this.Zero)
                {
                    if (!flag)
                    {
                        zero = this.Zero;
                    }
                    flag = true;
                }
                else
                {
                    if (((str2 == this.TenThousand) || (str2 == this.HundredMillion)) || (str2 == this.ThousandBillion))
                    {
                        text.Append(str2);
                        zero = string.Empty;
                        flag = false;
                        continue;
                    }
                    if (!string.IsNullOrEmpty(str2))
                    {
                        text.Append(zero);
                        text.Append(str2);
                        zero = string.Empty;
                        flag = false;
                    }
                }
            }
        }

        protected override string GetDigitText(int digit, ulong position)
        {
            if (digit == 0)
            {
                return this.Zero;
            }
            if (((ulong)digit * position) != 10L)
            {
                return (this.Digits[digit] + this.GetPositionText(position));
            }
            return this.GetPositionText(position);
        }
    }
}
