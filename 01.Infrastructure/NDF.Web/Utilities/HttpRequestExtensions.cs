using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NDF.Web.Utilities
{
    /// <summary>
    /// 提供一组对 HTTP 请求对象 <see cref="HttpRequest"/> 和 <see cref="HttpRequestBase"/> 操作方法的扩展。
    /// </summary>
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// 获取当前 <see cref="System.Web.HttpRequest"/> 请求路径的文件扩展名。
        /// </summary>
        /// <param name="_this">表示当前 HTTP 请求的 <see cref="System.Web.HttpRequest"/> 对象。</param>
        /// <returns>返回当前 <see cref="System.Web.HttpRequest"/> 请求路径的文件扩展名。</returns>
        public static string GetExtension(this HttpRequest _this)
        {
            Check.NotNull(_this, "_this");
            return VirtualPathUtility.GetExtension(_this.Path);
        }

        /// <summary>
        /// 确定指定的 HTTP 请求是否为 AJAX 请求。
        /// </summary>
        /// <param name="_this">待确定的 HTTP 请求。</param>
        /// <returns>如果指定的 HTTP 请求是 AJAX 请求，则为 true；否则为 false。</returns>
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            return (request["X-Requested-With"] == "XMLHttpRequest") || ((request.Headers != null) && (request.Headers["X-Requested-With"] == "XMLHttpRequest"));
        }


    }
}
