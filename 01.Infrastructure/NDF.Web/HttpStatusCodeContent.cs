using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Web
{
    /// <summary>
    /// 表示用于定义的 HTTP 请求响应。
    /// </summary>
    public class HttpStatusCodeContent
    {
        /// <summary>
        /// 获取或设置当前响应的 HTTP 字符集。
        /// </summary>
        public string Charset { get; set; }

        /// <summary>
        /// 获取或设置 HTTP 响应内容。
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 获取或设置 HTTP 响应内容编码。
        /// </summary>
        public Encoding ContentEncoding { get; set; }

        /// <summary>
        /// 获取或设置 HTTP 响应内容类型。
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 获取或设置当前响应的标头的编码。
        /// </summary>
        public Encoding HeaderEncoding { get; set; }

        /// <summary>
        /// 获取或设置返回到客户端的 Status 值。
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 获取或设置 HTTP 响应状态代码。
        /// </summary>
        public int? StatusCode { get; set; }

        /// <summary>
        /// 获取或设一个值，该值限定响应的状态代码。
        /// </summary>
        public int? SubStatusCode { get; set; }

        /// <summary>
        /// 获取或设置 HTTP 响应状态说明。
        /// </summary>
        public string StatusDescription { get; set; }

    }
}
