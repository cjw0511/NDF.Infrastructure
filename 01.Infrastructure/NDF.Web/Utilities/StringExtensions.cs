using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NDF.Web.Utilities
{
    /// <summary>
    /// 提供一组对字符串对象 <see cref="System.String"/> 操作方法的扩展。
    /// </summary>
    public static class StringExtensions
    {

        /// <summary>
        /// 对字符串进行Html编码。
        /// </summary>
        /// <param name="str">被处理的字符串对象。</param>
        /// <returns>对传入的 String 对象参数进行 HTML 编码后的结果字符串。</returns>
        public static string ToHtmlEncode(this string str)
        {
            return HttpUtility.HtmlEncode(str);
        }

        /// <summary>
        /// 对字符串进行Html解码。
        /// </summary>
        /// <param name="str">被处理的字符串对象。</param>
        /// <returns>对传入的 String 对象参数进行 HTML 解码后的结果字符串。</returns>
        public static string ToHtmlDecode(this string str)
        {
            return HttpUtility.HtmlDecode(str);
        }
    }
}
