/*
 * 该类型中所有代码反编译（通过 .NET Reflector 工具）自 Microsoft Visual Studio International Pack 1.0 中的
 * East Asia Numeric Formatting Library（EastAsiaNumericFormatter.dll 亚洲语系数值字符串格式化类库），关于
 * 该 EastAsiaNumericFormatter.dll 库的更多信息，请参阅：http://www.microsoft.com/zh-cn/download/details.aspx?id=15251
 */

namespace NDF.International.Formatting
{
    using NDF.International.Formatting.Formatters;
    using System;
    using System.Globalization;
    using System.Text;

    internal abstract class EastAsiaFormatter
    {
        protected EastAsiaFormatter()
        {
        }

        protected internal virtual bool CheckOutOfRange(double num)
        {
            if (((num <= 1.8446744073709552E+19) && (num >= -9.2233720368547758E+18)) && (!double.IsNaN(num) && !double.IsInfinity(num)))
            {
                return false;
            }
            return true;
        }

        protected abstract void ConvertIntergralStackToText(StackWithIndex stack, StringBuilder text);
        internal string ConvertToLocalizedText(decimal num)
        {
            StringBuilder text = new StringBuilder();
            StackWithIndex stack = new StackWithIndex();
            if (num < 0M)
            {
                text.Append(this.Minus);
                num = Math.Abs(num);
            }
            num = this.Initialize(num);
            if (((ulong)num) == 0L)
            {
                text.Append(this.Zero);
            }
            else
            {
                this.GetIntegralStack(Math.Truncate(num), 1L, stack);
                this.ConvertIntergralStackToText(stack, text);
            }
            num -= Math.Truncate(num);
            if (num != 0M)
            {
                text.Append(this.DecimalPoint);
                this.GetDecimalText(num, text);
            }
            return text.ToString();
        }

        internal static EastAsiaFormatter Create(CultureInfo culture, string format)
        {
            while (!culture.IsNeutralCulture)
            {
                if (culture.Parent == null)
                {
                    break;
                }
                culture = culture.Parent;
            }
            if (culture.Equals(new CultureInfo("zh-CHS")))
            {
                switch (format)
                {
                    case "L":
                        return new CHSStdFmt();

                    case "Ln":
                        return new CHSNorFmt();

                    case "Lc":
                        return new CHSCurFmt();
                }
                return null;
            }
            if (culture.Equals(new CultureInfo("zh-CHT")))
            {
                switch (format)
                {
                    case "L":
                        return new CHTStdFmt();

                    case "Ln":
                        return new CHTNorFmt();

                    case "Lc":
                        return new CHTCurFmt();
                }
                return null;
            }
            if (culture.Equals(new CultureInfo("ja")))
            {
                switch (format)
                {
                    case "L":
                        return new JPNStdFmt();

                    case "Ln":
                        return new JPNNorFmt();

                    case "Lt":
                        return new JPNTraFmt();
                }
                return null;
            }
            if (culture.Equals(new CultureInfo("ko")))
            {
                switch (format)
                {
                    case "L":
                        return new KORStdFmt();

                    case "Lc":
                        return new KORCurFmt();
                }
            }
            return null;
        }

        protected virtual void GetDecimalText(decimal num, StringBuilder text)
        {
            while (num != 0M)
            {
                decimal d = num * 10M;
                int index = Convert.ToInt32(Math.Truncate(d));
                text.Append(this.Digits[index]);
                num = d - Math.Truncate(d);
            }
        }

        protected abstract string GetDigitText(int digit, ulong position);
        protected virtual void GetIntegralStack(decimal num, ulong position, StackWithIndex stack)
        {
            if (num < 10000M)
            {
                if ((num != 0M) || (position == 0xe8d4a51000L))
                {
                    stack.Push(this.GetPositionText(position));
                }
                for (int i = 0; i < 4; i++)
                {
                    int digit = (int)(num % 10M);
                    ulong num4 = (ulong)Math.Pow(10.0, (double)i);
                    num /= 10M;
                    stack.Push(this.GetDigitText(digit, num4));
                }
            }
            else
            {
                this.GetIntegralStack(Math.Truncate((decimal)(num % 10000M)), position, stack);
                this.GetIntegralStack(Math.Truncate((decimal)(num / 10000M)), position * ((ulong)0x2710L), stack);
            }
        }

        protected virtual string GetPositionText(ulong position)
        {
            switch (position)
            {
                case 100L:
                    return this.Hundred;

                case 0x3e8L:
                    return this.Thousand;

                case 1L:
                    return string.Empty;

                case 10L:
                    return this.Ten;

                case 0x2710L:
                    return this.TenThousand;

                case 0x5f5e100L:
                    return this.HundredMillion;

                case 0xe8d4a51000L:
                    return this.ThousandBillion;

                case 0x2386f26fc10000L:
                    return this.TenThousand;
            }
            throw new InvalidOperationException();
        }

        protected virtual decimal Initialize(decimal num)
        {
            num = Math.Round(num, 15);
            return num;
        }

        protected abstract string DecimalPoint { get; }

        protected abstract string[] Digits { get; }

        protected abstract string Hundred { get; }

        protected abstract string HundredMillion { get; }

        protected abstract string Minus { get; }

        protected abstract string Ten { get; }

        protected abstract string TenThousand { get; }

        protected abstract string Thousand { get; }

        protected abstract string ThousandBillion { get; }

        protected abstract string Zero { get; }
    }
}
