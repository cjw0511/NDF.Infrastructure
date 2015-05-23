/*
 * 该类型中所有代码反编译（通过 .NET Reflector 工具）自 Microsoft Visual Studio International Pack 1.0 中的
 * East Asia Numeric Formatting Library（EastAsiaNumericFormatter.dll 亚洲语系数值字符串格式化类库），关于
 * 该 EastAsiaNumericFormatter.dll 库的更多信息，请参阅：http://www.microsoft.com/zh-cn/download/details.aspx?id=15251
 */

namespace NDF.International.Formatting
{
    using System;
    using System.Collections.Generic;

    internal class StackWithIndex
    {
        private List<string> data = new List<string>();

        internal StackWithIndex()
        {
        }

        internal string Peek()
        {
            return this.data[this.data.Count - 1];
        }

        internal string Pop()
        {
            string str = this.data[this.data.Count - 1];
            this.data.RemoveAt(this.data.Count - 1);
            return str;
        }

        internal void Push(string item)
        {
            this.data.Add(item);
        }

        internal int Count
        {
            get
            {
                return this.data.Count;
            }
        }

        internal string this[int index]
        {
            get
            {
                return this.data[(this.data.Count - index) - 1];
            }
        }
    }
}
