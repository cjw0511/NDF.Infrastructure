using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Web.Mvc
{
    /// <summary>
    /// 表示用户定义的通用 JavaScript 对象表示法(JSON 数据格式) 数据响应格式。
    /// </summary>
    public class JsonStatus
    {
        /// <summary>
        /// 获取或设置 HTTP 请求的处理状态(是否处理成功)。
        /// </summary>
        public bool? StatusResult { get; set; }

        /// <summary>
        /// 获取或设置 HTTP 状态码(仅作为 JSON 结果输出，与 HttpResponse.StatusCode 无关)。
        /// </summary>
        public int? StatusCode { get; set; }

        /// <summary>
        /// 获取或设置 HTTP 状态表示(仅作为 JSON 结果输出，与 HttpResponse.StatusDescription 无关)。
        /// </summary>
        public string StatusDescription { get; set; }

        /// <summary>
        /// 获取或设置 HTTP 请求处理结果的附加消息。
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 获取或设置 HTTP 请求处理结果的附加 URL 地址。
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 获取或设置 HTTP 响应内容编码。
        /// </summary>
        public Encoding ContentEncoding { get; set; }

        ///// <summary>
        ///// 获取或设置 HTTP 响应数据的最大长度。
        ///// </summary>
        //public int? MaxJsonLength { get; set; }
    }
}
