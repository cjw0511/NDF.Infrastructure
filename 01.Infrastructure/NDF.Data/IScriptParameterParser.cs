using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data
{
    /// <summary>
    /// 定义解析 SQL 脚本中参数名称列表的方法。
    /// </summary>
    public interface IScriptParameterParser
    {
        /// <summary>
        /// 解析 SQL 脚本中的参数名称列表并返回。
        /// </summary>
        /// <param name="sqlScript">待解析的 SQL 脚本。</param>
        /// <returns>返回 <paramref name="sqlScript"/> 中定义的所有脚本参数名称所构成的一个数组。</returns>
        string[] GetParameterNames(string sqlScript);
    }
}
