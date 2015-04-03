using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NDF.Web.Mvc
{
    /// <summary>
    /// 表示将用户定义的通用 JavaScript 对象表示法(JSON 数据格式) 的内容发送到 ASP.NET MVC 控制器 Action 响应。
    /// </summary>
    /// <remarks>
    /// 该类型的 ExecuteResult 方法会将设定的 <see cref="NDF.Web.Mvc.JsonStatus"/> 类型参数解析成 Json 数据格式并通过 HttpResopnse 输出至客户端。
    /// </remarks>
    public class JsonStatusResult : ActionResult
    {
        private JsonStatus _jsonStatus;
        private JsonRequestBehavior _jsonRequestBehavior = JsonRequestBehavior.DenyGet;


        /// <summary>
        /// 以指定的初始参数创建一个 <see cref="NDF.Web.Mvc.JsonStatusResult"/> 对象。
        /// </summary>
        /// <param name="statusResult">表示 HTTP 请求的处理状态(是否处理成功)。</param>
        /// <param name="jsonRequestBehavior">指定是否允许来自客户端的 HTTP GET 请求。该参数默认为 JsonRequestBehavior.DenyGet。</param>
        public JsonStatusResult(bool? statusResult, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
        {
            this.JsonStatus.StatusResult = statusResult;
            this.JsonRequestBehavior = jsonRequestBehavior;
        }

        /// <summary>
        /// 以指定的初始参数创建一个 <see cref="NDF.Web.Mvc.JsonStatusResult"/> 对象。
        /// </summary>
        /// <param name="jsonRequestBehavior">指定是否允许来自客户端的 HTTP GET 请求。该参数默认为 JsonRequestBehavior.DenyGet。</param>
        public JsonStatusResult(JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
            : this(true)
        {
            this.JsonRequestBehavior = jsonRequestBehavior;
        }

        /// <summary>
        /// 以指定的初始参数创建一个 <see cref="NDF.Web.Mvc.JsonStatusResult"/> 对象。
        /// </summary>
        /// <param name="statusCode">表示 HTTP 状态码(仅作为 JSON 结果输出，与 HttpResponse.StatusCode 无关)。</param>
        /// <param name="jsonRequestBehavior">指定是否允许来自客户端的 HTTP GET 请求。该参数默认为 JsonRequestBehavior.DenyGet。</param>
        public JsonStatusResult(int? statusCode, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
            : this(jsonRequestBehavior)
        {
            this.JsonStatus.StatusCode = statusCode;
        }


        /// <summary>
        /// 以指定的初始参数创建一个 <see cref="NDF.Web.Mvc.JsonStatusResult"/> 对象。
        /// </summary>
        /// <param name="statusCode">表示 HTTP 状态码(仅作为 JSON 结果输出，与 HttpResponse.StatusCode 无关)。</param>
        /// <param name="jsonRequestBehavior">指定是否允许来自客户端的 HTTP GET 请求。该参数默认为 JsonRequestBehavior.DenyGet。</param>
        public JsonStatusResult(HttpStatusCode statusCode, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
            : this((int)statusCode, jsonRequestBehavior)
        {
        }

        /// <summary>
        /// 以指定的初始参数创建一个 <see cref="NDF.Web.Mvc.JsonStatusResult"/> 对象。
        /// </summary>
        /// <param name="message">表示 HTTP 请求处理结果的附加消息。</param>
        /// <param name="jsonRequestBehavior">指定是否允许来自客户端的 HTTP GET 请求。该参数默认为 JsonRequestBehavior.DenyGet。</param>
        public JsonStatusResult(string message, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
            : this(true, jsonRequestBehavior)
        {
            this.JsonStatus.Message = message;
        }

        /// <summary>
        /// 以指定的初始参数创建一个 <see cref="NDF.Web.Mvc.JsonStatusResult"/> 对象。
        /// </summary>
        /// <param name="statusResult">表示 HTTP 请求的处理状态(是否处理成功)。</param>
        /// <param name="message">表示 HTTP 请求处理结果的附加消息。</param>
        /// <param name="jsonRequestBehavior">指定是否允许来自客户端的 HTTP GET 请求。该参数默认为 JsonRequestBehavior.DenyGet。</param>
        public JsonStatusResult(bool? statusResult, string message, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
            : this(statusResult, jsonRequestBehavior)
        {
            this.JsonStatus.Message = message;
        }

        /// <summary>
        /// 以指定的初始参数创建一个 <see cref="NDF.Web.Mvc.JsonStatusResult"/> 对象。
        /// </summary>
        /// <param name="statusResult">表示 HTTP 请求的处理状态(是否处理成功)。</param>
        /// <param name="message">表示 HTTP 请求处理结果的附加消息。</param>
        /// <param name="url">表示 HTTP 请求处理结果的附加 URL 地址。</param>
        /// <param name="jsonRequestBehavior">指定是否允许来自客户端的 HTTP GET 请求。该参数默认为 JsonRequestBehavior.DenyGet。</param>
        public JsonStatusResult(bool? statusResult, string message, string url, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
            : this(statusResult, message, jsonRequestBehavior)
        {
            this.JsonStatus.Url = url;
        }

        /// <summary>
        /// 以指定的初始参数创建一个 <see cref="NDF.Web.Mvc.JsonStatusResult"/> 对象。
        /// </summary>
        /// <param name="statusResult">表示 HTTP 请求的处理状态(是否处理成功)。</param>
        /// <param name="statusCode">表示 HTTP 状态码(仅作为 JSON 结果输出，与 HttpResponse.StatusCode 无关)。</param>
        /// <param name="statusDescription">表示 HTTP 状态表示(仅作为 JSON 结果输出，与 HttpResponse.StatusDescription 无关)。</param>
        /// <param name="message">表示 HTTP 请求处理结果的附加消息。</param>
        /// <param name="url">表示 HTTP 请求处理结果的附加 URL 地址。</param>
        /// <param name="jsonRequestBehavior">指定是否允许来自客户端的 HTTP GET 请求。该参数默认为 JsonRequestBehavior.DenyGet。</param>
        public JsonStatusResult(bool? statusResult, int? statusCode, string statusDescription, string message, string url, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
            : this(statusResult, message, url, jsonRequestBehavior)
        {
            this.JsonStatus.StatusCode = statusCode;
            this.JsonStatus.StatusDescription = statusDescription;
        }

        /// <summary>
        /// 以指定的初始参数创建一个 <see cref="NDF.Web.Mvc.JsonStatusResult"/> 对象。
        /// </summary>
        /// <param name="statusResult">表示 HTTP 请求的处理状态(是否处理成功)。</param>
        /// <param name="statusCode">表示 HTTP 状态码(仅作为 JSON 结果输出，与 HttpResponse.StatusCode 无关)。</param>
        /// <param name="statusDescription">表示 HTTP 状态表示(仅作为 JSON 结果输出，与 HttpResponse.StatusDescription 无关)。</param>
        /// <param name="message">表示 HTTP 请求处理结果的附加消息。</param>
        /// <param name="url">表示 HTTP 请求处理结果的附加 URL 地址。</param>
        /// <param name="jsonRequestBehavior">指定是否允许来自客户端的 HTTP GET 请求。该参数默认为 JsonRequestBehavior.DenyGet。</param>
        public JsonStatusResult(bool? statusResult, HttpStatusCode statusCode, string statusDescription, string message, string url, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
            : this(statusResult, (int)statusCode, statusDescription, message, url, jsonRequestBehavior)
        {
        }

        /// <summary>
        /// 以指定的初始参数创建一个 <see cref="NDF.Web.Mvc.JsonStatusResult"/> 对象。
        /// </summary>
        /// <param name="jsonStatus">表示用户定义的通用 JavaScript 对象表示法(JSON 数据格式) 数据响应格式。</param>
        /// <param name="jsonRequestBehavior">指定是否允许来自客户端的 HTTP GET 请求。该参数默认为 JsonRequestBehavior.DenyGet。</param>
        public JsonStatusResult(JsonStatus jsonStatus, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
            : this(jsonRequestBehavior)
        {
            this.JsonStatus = jsonStatus;
        }




        /// <summary>
        /// 表示用户定义的通用 JavaScript 对象表示法(JSON 数据格式) 数据响应格式。
        /// </summary>
        public JsonStatus JsonStatus
        {
            get
            {
                if (this._jsonStatus == null)
                {
                    this._jsonStatus = new JsonStatus();
                }
                return this._jsonStatus;
            }
            protected internal set { this.JsonStatus = value; }
        }

        /// <summary>
        /// 指定是否允许来自客户端的 HTTP GET 请求。
        /// </summary>
        public JsonRequestBehavior JsonRequestBehavior
        {
            get { return this._jsonRequestBehavior; }
            set { this._jsonRequestBehavior = value; }
        }



        /// <summary>
        /// 通过从 <see cref="System.Web.Mvc.ActionResult"/> 类继承的自定义类型，启用对操作方法结果的处理。
        /// 该方法会将设定的 <see cref="JsonStatus"/> 结构类型参数解析成 Json 数据格式并通过 HttpResopnse 输出至客户端。
        /// </summary>
        /// <param name="context">用于执行结果的上下文。上下文信息包括控制器、HTTP 内容、请求上下文和路由数据。</param>
        public override void ExecuteResult(ControllerContext context)
        {
            Check.NotNull(context);
            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
                String.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(Resources.JsonRequest_GetNotAllowed);
            }

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = "application/json";

            if (this.JsonStatus.StatusResult == null)
            {
                this.JsonStatus.StatusResult = true;
            }
            if (this.JsonStatus.StatusCode == null)
            {
                this.JsonStatus.StatusCode = (int)HttpStatusCode.OK;
            }
            if (string.IsNullOrWhiteSpace(this.JsonStatus.Message))
            {
                this.JsonStatus.Message = Resources.JsonStatusResult_DefaultMessage;
            }
            if (this.JsonStatus.ContentEncoding != null)
            {
                response.ContentEncoding = this.JsonStatus.ContentEncoding;
            }

            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //if (this.JsonStatus.MaxJsonLength.HasValue)
            //{
            //    serializer.MaxJsonLength = this.JsonStatus.MaxJsonLength.Value;
            //}
            //response.Write(serializer.Serialize(this.JsonStatus));

            response.Write(this.JsonStatus.ToJson());
        }
    }
}
