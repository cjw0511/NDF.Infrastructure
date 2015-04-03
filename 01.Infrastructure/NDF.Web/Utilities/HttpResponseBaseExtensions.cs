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
    /// 提供一组对 ASP.NET 操作的 HTTP 响应信息基类 <see cref="System.Web.HttpResponseBase"/> 操作方法的扩展。
    /// </summary>
    public static class HttpResponseBaseExtensions
    {
        /// <summary>
        /// 将指定的 <see cref="HttpStatusCodeContent"/> 写入 HTTP 响应输出流。
        /// </summary>
        /// <param name="_this">一个 <see cref="HttpResponseBase"/> 对象，提供来自 ASP.NET 操作的 HTTP 响应信息。</param>
        /// <param name="content">要被写入 HTTP 响应输出流的 <see cref="HttpStatusCodeContent"/> 对象。</param>
        public static void Write(this HttpResponseBase _this, HttpStatusCodeContent content)
        {
            Check.NotNull(_this);
            if (content != null)
            {
                if(!string.IsNullOrWhiteSpace(content.Charset))
                {
                    _this.Charset = content.Charset;
                }
                if (content.ContentEncoding != null)
                {
                    _this.ContentEncoding = content.ContentEncoding;
                }
                if (!string.IsNullOrWhiteSpace(content.ContentType))
                {
                    _this.ContentType = content.ContentType;
                }
                if (content.HeaderEncoding != null)
                {
                    _this.HeaderEncoding = content.HeaderEncoding;
                }
                if (!string.IsNullOrWhiteSpace(content.Status))
                {
                    _this.Status = content.Status;
                }
                if (content.StatusCode != null && content.StatusCode.HasValue)
                {
                    _this.StatusCode = content.StatusCode.Value;
                }
                if (content.SubStatusCode != null && content.SubStatusCode.HasValue)
                {
                    _this.SubStatusCode = content.SubStatusCode.Value;
                }
                if (!string.IsNullOrWhiteSpace(content.StatusDescription))
                {
                    _this.StatusDescription = content.StatusDescription;
                }
                if (content.Content != null)
                {
                    _this.Write(content.Content);
                }
            }
        }

    }
}
