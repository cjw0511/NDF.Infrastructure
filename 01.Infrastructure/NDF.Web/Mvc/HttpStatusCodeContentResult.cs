using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NDF.Web.Mvc
{
    /// <summary>
    /// 表示用户定义的内容类型，该类型是操作方法的结果，同时提供一种用于返回带特定 HTTP 响应状态代码和说明的操作结果的方法。
    /// </summary>
    public class HttpStatusCodeContentResult : HttpStatusCodeResult
    {
        private HttpStatusCodeContent _statusCodeContent;


        /// <summary>
        /// 以指定的初始参数创建一个 <see cref="NDF.Web.Mvc.HttpStatusCodeContentResult"/> 对象。
        /// </summary>
        /// <param name="statusCode">表示 HTTP 状态码。</param>
        public HttpStatusCodeContentResult(int statusCode)
            : base(statusCode)
        {
            this.StatusCodeContent.StatusCode = statusCode;
        }

        /// <summary>
        /// 以指定的初始参数创建一个 <see cref="NDF.Web.Mvc.HttpStatusCodeContentResult"/> 对象。
        /// </summary>
        /// <param name="statusCode">表示 HTTP 状态码。</param>
        public HttpStatusCodeContentResult(HttpStatusCode statusCode)
            : base(statusCode)
        {
            this.StatusCodeContent.StatusCode = (int)HttpStatusCode.OK;
        }

        /// <summary>
        /// 以指定的初始参数创建一个 <see cref="NDF.Web.Mvc.HttpStatusCodeContentResult"/> 对象。
        /// </summary>
        /// <param name="statusCode">表示 HTTP 状态码。</param>
        /// <param name="statusDescription">表示 HTTP 状态描述。</param>
        public HttpStatusCodeContentResult(int statusCode, string statusDescription)
            : base(statusCode, statusDescription)
        {
            this.StatusCodeContent.StatusCode = statusCode;
            this.StatusCodeContent.StatusDescription = statusDescription;
        }

        /// <summary>
        /// 以指定的初始参数创建一个 <see cref="NDF.Web.Mvc.HttpStatusCodeContentResult"/> 对象。
        /// </summary>
        /// <param name="statusCode">表示 HTTP 状态码。</param>
        /// <param name="statusDescription">表示 HTTP 状态描述。</param>
        public HttpStatusCodeContentResult(HttpStatusCode statusCode, string statusDescription)
            : base(statusCode, statusDescription)
        {
            this.StatusCodeContent.StatusCode = (int)HttpStatusCode.OK;
            this.StatusCodeContent.StatusDescription = statusDescription;
        }


        /// <summary>
        /// 以指定的初始参数创建一个 <see cref="NDF.Web.Mvc.HttpStatusCodeContentResult"/> 对象。
        /// </summary>
        /// <param name="content">HTTP 响应内容。</param>
        public HttpStatusCodeContentResult(string content)
            : this(HttpStatusCode.OK)
        {
            this.StatusCodeContent.Content = content;
        }

        /// <summary>
        /// 以指定的初始参数创建一个 <see cref="NDF.Web.Mvc.HttpStatusCodeContentResult"/> 对象。
        /// </summary>
        /// <param name="content">HTTP 响应内容。</param>
        /// <param name="statusCode">HTTP 响应状态代码。</param>
        public HttpStatusCodeContentResult(string content, int statusCode)
            : this(statusCode)
        {
            this.StatusCodeContent.Content = content;
        }

        /// <summary>
        /// 以指定的初始参数创建一个 <see cref="NDF.Web.Mvc.HttpStatusCodeContentResult"/> 对象。
        /// </summary>
        /// <param name="content">HTTP 响应内容。</param>
        /// <param name="statusCode">HTTP 响应状态代码。</param>
        public HttpStatusCodeContentResult(string content, HttpStatusCode statusCode)
            : this(statusCode)
        {
            this.StatusCodeContent.Content = content;
        }

        /// <summary>
        /// 以指定的初始参数创建一个 <see cref="NDF.Web.Mvc.HttpStatusCodeContentResult"/> 对象。
        /// </summary>
        /// <param name="content">HTTP 响应内容。</param>
        /// <param name="statusCode">HTTP 响应状态代码。</param>
        /// <param name="statusDescription">HTTP 响应状态说明。</param>
        /// <param name="status">返回到客户端的 Status 值。</param>
        /// <param name="subStatusCode">HTTP 响应的二级状态代码。</param>
        /// <param name="charset">当前 HTTP 响应的 HTTP 字符集。</param>
        /// <param name="contentEncoding">HTTP 响应内容编码。</param>
        /// <param name="contentType"> HTTP 响应内容类型。</param>
        /// <param name="headerEncoding">当前 HTTP 响应的标头的编码。</param>
        public HttpStatusCodeContentResult(string content, int statusCode, string statusDescription, string status, int? subStatusCode, string charset, Encoding contentEncoding, string contentType, Encoding headerEncoding)
            : this(content, statusCode)
        {
            this.StatusCodeContent.StatusDescription = statusDescription;
            this.StatusCodeContent.Status = status;
            this.StatusCodeContent.SubStatusCode = subStatusCode;
            this.StatusCodeContent.Charset = charset;
            this.StatusCodeContent.ContentEncoding = contentEncoding;
            this.StatusCodeContent.ContentType = contentType;
            this.StatusCodeContent.HeaderEncoding = headerEncoding;
        }

        /// <summary>
        /// 以指定的初始参数创建一个 <see cref="NDF.Web.Mvc.HttpStatusCodeContentResult"/> 对象。
        /// </summary>
        /// <param name="content">HTTP 响应内容。</param>
        /// <param name="statusCode">HTTP 响应状态代码。</param>
        /// <param name="statusDescription">HTTP 响应状态说明。</param>
        /// <param name="status">返回到客户端的 Status 值。</param>
        /// <param name="subStatusCode">HTTP 响应的二级状态代码。</param>
        /// <param name="charset">当前 HTTP 响应的 HTTP 字符集。</param>
        /// <param name="contentEncoding">HTTP 响应内容编码。</param>
        /// <param name="contentType"> HTTP 响应内容类型。</param>
        /// <param name="headerEncoding">当前 HTTP 响应的标头的编码。</param>
        public HttpStatusCodeContentResult(string content, HttpStatusCode statusCode, string statusDescription, string status, int? subStatusCode, string charset, Encoding contentEncoding, string contentType, Encoding headerEncoding)
            : this(content, statusCode)
        {
            this.StatusCodeContent.StatusDescription = statusDescription;
            this.StatusCodeContent.Status = status;
            this.StatusCodeContent.SubStatusCode = subStatusCode;
            this.StatusCodeContent.Charset = charset;
            this.StatusCodeContent.ContentEncoding = contentEncoding;
            this.StatusCodeContent.ContentType = contentType;
            this.StatusCodeContent.HeaderEncoding = headerEncoding;
        }


        /// <summary>
        /// 以指定的初始参数创建一个 <see cref="HttpStatusCodeContentResult"/> 对象。
        /// </summary>
        /// <param name="content">表示用于定义的 HTTP 请求响应。</param>
        public HttpStatusCodeContentResult(HttpStatusCodeContent content)
            : this(content.StatusCode != null ? content.StatusCode.Value : (int)HttpStatusCode.OK, content.StatusDescription)
        {
        }



        /// <summary>
        /// 表示用于定义的 HTTP 请求响应。
        /// </summary>
        public HttpStatusCodeContent StatusCodeContent
        {
            get
            {
                if (this._statusCodeContent == null)
                {
                    this._statusCodeContent = new HttpStatusCodeContent();
                }
                return this._statusCodeContent;
            }
            protected internal set { this._statusCodeContent = value; }
        }



        /// <summary>
        /// 通过从 <see cref="System.Web.Mvc.ActionResult"/> 类继承的自定义类型，启用对操作方法结果的处理。
        /// 该方法会将设定的 <seealso cref="StatusCodeContent"/> 属性通过 HttpResopnse 输出至客户端。
        /// </summary>
        /// <param name="context">用于执行结果的上下文。上下文信息包括控制器、HTTP 内容、请求上下文和路由数据。</param>
        public override void ExecuteResult(ControllerContext context)
        {
            NDF.Web.Utilities.HttpResponseBaseExtensions.Write(context.HttpContext.Response, this.StatusCodeContent);

            context.HttpContext.Response.StatusCode = this.StatusCode;
            if (!string.IsNullOrWhiteSpace(this.StatusDescription))
            {
                context.HttpContext.Response.StatusDescription = this.StatusDescription;
            }
        }
    }
}
