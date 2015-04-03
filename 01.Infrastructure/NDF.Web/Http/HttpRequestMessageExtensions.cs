using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NDF.Web.Http
{
    /// <summary>
    /// 提供一组用于操作 HTTP 请求消息 <see cref="HttpRequestMessage"/> 对象的扩展方法。
    /// </summary>
    public static class HttpRequestMessageExtensions
    {
        /// <summary>
        /// 获取当前 HTTP 请求消息的上下文信息。
        /// </summary>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static HttpContextBase GetHttpContext(this HttpRequestMessage _this)
        {
            object property = _this.Properties != null ? _this.Properties["MS_HttpContext"] : null;
            HttpContextBase context = property as HttpContextBase;
            return context;
        }






    }
}
