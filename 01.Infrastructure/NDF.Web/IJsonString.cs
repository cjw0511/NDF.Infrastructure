using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Web
{
    /// <summary>
    /// 表示不应再次进行 JSON 解析的 JSON 字符串。
    /// </summary>
    public interface IJsonString
    {
        /// <summary>
        /// 返回 JSON 解析的字符串。
        /// </summary>
        /// <returns>经过 JSON 格式解析的字符串。</returns>
        string ToJsonString();
    }
}
