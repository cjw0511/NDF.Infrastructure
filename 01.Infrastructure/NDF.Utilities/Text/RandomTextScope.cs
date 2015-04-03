using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDF.Text
{
    /// <summary>
    /// 表示随机字符串生成器在生成随机字符时的取值范围。
    /// </summary>
    [Flags]
    public enum RandomTextScope
    {
        /// <summary>
        /// 表示是否十进制数字字符，取值范围为 "0123456789"。
        /// </summary>
        Digit = 1,

        /// <summary>
        /// 表示是否小写字母字符，取值范围为 "abcdefghijklmnopqrstuvwxyz"。
        /// </summary>
        Lower = 2,

        /// <summary>
        /// 表示是否大写字母字符，取值范围为 "ABCDEFGHIJKLMNOPQRSTUVWXYZ"。
        /// </summary>
        Upper = 4,

        /// <summary>
        /// 表示是否包含符号字符，取值范围为 ",./;'\\[]`-=&gt;&lt;?:\"|{}~!@#$%^&amp;*()_+"。
        /// </summary>
        Symbol = 8,

        /// <summary>
        /// 表示是否包含空格字符，取值范围为 " "。
        /// </summary>
        Space = 16,
    }
}
