using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Text
{
    /// <summary>
    /// 表示随机字符串生成器，可用于生成一组指定规格的随机字符。
    /// </summary>
    public class RandomText
    {
        /// <summary>
        /// 初始化类型 <see cref="RandomText"/> 的一个新实例。
        /// </summary>
        public RandomText()
        {
            this.LengthIsRandom = false;
            this.Scope = RandomTextScope.Digit | RandomTextScope.Lower | RandomTextScope.Upper | RandomTextScope.Symbol;
        }



        /// <summary>
        /// 获取或设置一个 int 值，表示当 LengthIsRandom 属性为 true 时，随机计算 Length 时所能取的最大值。
        /// </summary>
        public int MinLength { get; set; }

        /// <summary>
        /// 获取或设置一个 int 值，表示当 LengthIsRandom 属性为 true 时，随机计算 Length 时所能取的最小值。
        /// </summary>
        public int MaxLength { get; set; }

        /// <summary>
        /// 获取或设置一个 int 值，表示当 LengthIsRandom 属性为 false 时，随机生成的字符串长度。
        /// </summary>
        public int Length { get; set; }


        /// <summary>
        /// 获取或设置一个 <see cref="RandomTextScope"/> 枚举值，表示随机生成的字符串的内容范围。
        /// </summary>
        public RandomTextScope Scope { get; set; }

        /// <summary>
        /// 获取或设置一个 bool 值，表示随机生成字符串的长度是取 Length 属性的固定值，还是取 MinLength 和 MaxLength 之间的随机值。
        /// </summary>
        public bool LengthIsRandom { get; set; }
        




        public string GenerateText()
        {
            throw new NotImplementedException();
        }
    }
}
