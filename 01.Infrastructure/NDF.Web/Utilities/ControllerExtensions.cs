using NDF.Web.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NDF.Web.Utilities
{
    /// <summary>
    /// 提供一组对 ASP.NET MVC 控制器对象 <see cref="System.Web.Mvc.Controller"/> 操作方法的扩展。
    /// </summary>
    public static class ControllerExtensions
    {

        /// <summary>
        /// 以指定的参数创建一个用于返回用户定义的通用 JavaScript 对象表示法(JSON 数据格式) 内容的 <see cref="NDF.Web.Mvc.JsonStatusResult"/> 对象。
        /// <see cref="NDF.Web.Mvc.JsonStatusResult"/> 会将设定的 <see cref="NDF.Web.Mvc.JsonStatus"/> 结构类型参数解析成 Json 数据格式并通过 HttpResopnse 输出至客户端。
        /// </summary>
        /// <param name="controller">表示 ASP.NET MVC 控制器 对象。</param>
        /// <param name="statusResult">表示 HTTP 请求的处理状态(是否处理成功)。</param>
        /// <param name="jsonRequestBehavior">指示是否允许来自客户端的 HTTP GET 请求。</param>
        /// <returns>一个用于返回用户定义的通用 JavaScript 对象表示法(JSON 数据格式) 内容的 <see cref="NDF.Web.Mvc.JsonStatusResult"/> 对象。</returns>
        public static JsonStatusResult JsonStatus(this Controller controller, bool? statusResult, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
        {
            return new JsonStatusResult(statusResult, jsonRequestBehavior);
        }

        /// <summary>
        /// 以指定的参数创建一个用于返回用户定义的通用 JavaScript 对象表示法(JSON 数据格式) 内容的 <see cref="NDF.Web.Mvc.JsonStatusResult"/> 对象。
        /// <see cref="NDF.Web.Mvc.JsonStatusResult"/> 会将设定的 <see cref="NDF.Web.Mvc.JsonStatus"/> 结构类型参数解析成 Json 数据格式并通过 HttpResopnse 输出至客户端。
        /// </summary>
        /// <param name="controller">表示 ASP.NET MVC 控制器 对象。</param>
        /// <param name="jsonRequestBehavior">指示是否允许来自客户端的 HTTP GET 请求。</param>
        /// <returns>一个用于返回用户定义的通用 JavaScript 对象表示法(JSON 数据格式) 内容的 <see cref="NDF.Web.Mvc.JsonStatusResult"/> 对象。</returns>
        public static JsonStatusResult JsonStatus(this Controller controller, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
        {
            return new JsonStatusResult(jsonRequestBehavior);
        }

        /// <summary>
        /// 以指定的参数创建一个用于返回用户定义的通用 JavaScript 对象表示法(JSON 数据格式) 内容的 <see cref="NDF.Web.Mvc.JsonStatusResult"/> 对象。
        /// <see cref="NDF.Web.Mvc.JsonStatusResult"/> 会将设定的 <see cref="NDF.Web.Mvc.JsonStatus"/> 结构类型参数解析成 Json 数据格式并通过 HttpResopnse 输出至客户端。
        /// </summary>
        /// <param name="controller">表示 ASP.NET MVC 控制器 对象。</param>
        /// <param name="statusCode">表示 HTTP 状态码(仅作为 JSON 结果输出，与 HttpResponse.StatusCode 无关)。</param>
        /// <param name="jsonRequestBehavior">指示是否允许来自客户端的 HTTP GET 请求。</param>
        /// <returns>一个用于返回用户定义的通用 JavaScript 对象表示法(JSON 数据格式) 内容的 <see cref="NDF.Web.Mvc.JsonStatusResult"/> 对象。</returns>
        public static JsonStatusResult JsonStatus(this Controller controller, int? statusCode, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
        {
            return new JsonStatusResult(statusCode, jsonRequestBehavior);
        }

        /// <summary>
        /// 以指定的参数创建一个用于返回用户定义的通用 JavaScript 对象表示法(JSON 数据格式) 内容的 <see cref="NDF.Web.Mvc.JsonStatusResult"/> 对象。
        /// <see cref="NDF.Web.Mvc.JsonStatusResult"/> 会将设定的 <see cref="NDF.Web.Mvc.JsonStatus"/> 结构类型参数解析成 Json 数据格式并通过 HttpResopnse 输出至客户端。
        /// </summary>
        /// <param name="controller">表示 ASP.NET MVC 控制器 对象。</param>
        /// <param name="statusCode">表示 HTTP 状态码(仅作为 JSON 结果输出，与 HttpResponse.StatusCode 无关)。</param>
        /// <param name="jsonRequestBehavior">指示是否允许来自客户端的 HTTP GET 请求。</param>
        /// <returns>一个用于返回用户定义的通用 JavaScript 对象表示法(JSON 数据格式) 内容的 <see cref="NDF.Web.Mvc.JsonStatusResult"/> 对象。</returns>
        public static JsonStatusResult JsonStatus(this Controller controller, HttpStatusCode statusCode, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
        {
            return new JsonStatusResult(statusCode, jsonRequestBehavior);
        }

        /// <summary>
        /// 以指定的参数创建一个用于返回用户定义的通用 JavaScript 对象表示法(JSON 数据格式) 内容的 <see cref="NDF.Web.Mvc.JsonStatusResult"/> 对象。
        /// <see cref="NDF.Web.Mvc.JsonStatusResult"/> 会将设定的 <see cref="NDF.Web.Mvc.JsonStatus"/> 结构类型参数解析成 Json 数据格式并通过 HttpResopnse 输出至客户端。
        /// </summary>
        /// <param name="controller">表示 ASP.NET MVC 控制器 对象。</param>
        /// <param name="message">表示 HTTP 请求处理结果的附加消息。</param>
        /// <param name="jsonRequestBehavior">指示是否允许来自客户端的 HTTP GET 请求。</param>
        /// <returns>一个用于返回用户定义的通用 JavaScript 对象表示法(JSON 数据格式) 内容的 <see cref="NDF.Web.Mvc.JsonStatusResult"/> 对象。</returns>
        public static JsonStatusResult JsonStatus(this Controller controller, string message, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
        {
            return new JsonStatusResult(message, jsonRequestBehavior);
        }

        /// <summary>
        /// 以指定的参数创建一个用于返回用户定义的通用 JavaScript 对象表示法(JSON 数据格式) 内容的 <see cref="NDF.Web.Mvc.JsonStatusResult"/> 对象。
        /// <see cref="NDF.Web.Mvc.JsonStatusResult"/> 会将设定的 <see cref="NDF.Web.Mvc.JsonStatus"/> 结构类型参数解析成 Json 数据格式并通过 HttpResopnse 输出至客户端。
        /// </summary>
        /// <param name="controller">表示 ASP.NET MVC 控制器 对象。</param>
        /// <param name="statusResult">表示 HTTP 请求的处理状态(是否处理成功)。</param>
        /// <param name="message">表示 HTTP 请求处理结果的附加消息。</param>
        /// <param name="jsonRequestBehavior">指示是否允许来自客户端的 HTTP GET 请求。</param>
        /// <returns>一个用于返回用户定义的通用 JavaScript 对象表示法(JSON 数据格式) 内容的 <see cref="NDF.Web.Mvc.JsonStatusResult"/> 对象。</returns>
        public static JsonStatusResult JsonStatus(this Controller controller, bool? statusResult, string message, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
        {
            return new JsonStatusResult(statusResult, message, jsonRequestBehavior);
        }

        /// <summary>
        /// 以指定的参数创建一个用于返回用户定义的通用 JavaScript 对象表示法(JSON 数据格式) 内容的 <see cref="NDF.Web.Mvc.JsonStatusResult"/> 对象。
        /// <see cref="NDF.Web.Mvc.JsonStatusResult"/> 会将设定的 <see cref="NDF.Web.Mvc.JsonStatus"/> 结构类型参数解析成 Json 数据格式并通过 HttpResopnse 输出至客户端。
        /// </summary>
        /// <param name="controller">表示 ASP.NET MVC 控制器 对象。</param>
        /// <param name="statusResult">表示 HTTP 请求的处理状态(是否处理成功)。</param>
        /// <param name="message">表示 HTTP 请求处理结果的附加消息。</param>
        /// <param name="url">表示 HTTP 请求处理结果的附加 URL 地址。</param>
        /// <param name="jsonRequestBehavior">指示是否允许来自客户端的 HTTP GET 请求。</param>
        /// <returns>一个用于返回用户定义的通用 JavaScript 对象表示法(JSON 数据格式) 内容的 <see cref="NDF.Web.Mvc.JsonStatusResult"/> 对象。</returns>
        public static JsonStatusResult JsonStatus(this Controller controller, bool? statusResult, string message, string url, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
        {
            return new JsonStatusResult(statusResult, message, url, jsonRequestBehavior);
        }

        /// <summary>
        /// 以指定的参数创建一个用于返回用户定义的通用 JavaScript 对象表示法(JSON 数据格式) 内容的 <see cref="NDF.Web.Mvc.JsonStatusResult"/> 对象。
        /// <see cref="NDF.Web.Mvc.JsonStatusResult"/> 会将设定的 <see cref="NDF.Web.Mvc.JsonStatus"/> 结构类型参数解析成 Json 数据格式并通过 HttpResopnse 输出至客户端。
        /// </summary>
        /// <param name="controller">表示 ASP.NET MVC 控制器 对象。</param>
        /// <param name="statusResult">表示 HTTP 请求的处理状态(是否处理成功)。</param>
        /// <param name="statusCode">表示 HTTP 状态码(仅作为 JSON 结果输出，与 HttpResponse.StatusCode 无关)。</param>
        /// <param name="statusDescription">表示 HTTP 状态表示(仅作为 JSON 结果输出，与 HttpResponse.StatusDescription 无关)。</param>
        /// <param name="message">表示 HTTP 请求处理结果的附加消息。</param>
        /// <param name="url">表示 HTTP 请求处理结果的附加 URL 地址。</param>
        /// <param name="jsonRequestBehavior">指示是否允许来自客户端的 HTTP GET 请求。</param>
        /// <returns>一个用于返回用户定义的通用 JavaScript 对象表示法(JSON 数据格式) 内容的 <see cref="NDF.Web.Mvc.JsonStatusResult"/> 对象。</returns>
        public static JsonStatusResult JsonStatus(this Controller controller, bool? statusResult, int? statusCode, string statusDescription, string message, string url, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
        {
            return new JsonStatusResult(statusResult, statusCode, statusDescription, message, url, jsonRequestBehavior);
        }

        /// <summary>
        /// 以指定的参数创建一个用于返回用户定义的通用 JavaScript 对象表示法(JSON 数据格式) 内容的 <see cref="NDF.Web.Mvc.JsonStatusResult"/> 对象。
        /// <see cref="NDF.Web.Mvc.JsonStatusResult"/> 会将设定的 <see cref="NDF.Web.Mvc.JsonStatus"/> 结构类型参数解析成 Json 数据格式并通过 HttpResopnse 输出至客户端。
        /// </summary>
        /// <param name="controller">表示 ASP.NET MVC 控制器 对象。</param>
        /// <param name="statusResult">表示 HTTP 请求的处理状态(是否处理成功)。</param>
        /// <param name="statusCode">表示 HTTP 状态码(仅作为 JSON 结果输出，与 HttpResponse.StatusCode 无关)。</param>
        /// <param name="statusDescription">表示 HTTP 状态表示(仅作为 JSON 结果输出，与 HttpResponse.StatusDescription 无关)。</param>
        /// <param name="message">表示 HTTP 请求处理结果的附加消息。</param>
        /// <param name="url">表示 HTTP 请求处理结果的附加 URL 地址。</param>
        /// <param name="jsonRequestBehavior">指示是否允许来自客户端的 HTTP GET 请求。</param>
        /// <returns>一个用于返回用户定义的通用 JavaScript 对象表示法(JSON 数据格式) 内容的 <see cref="NDF.Web.Mvc.JsonStatusResult"/> 对象。</returns>
        public static JsonStatusResult JsonStatus(this Controller controller, bool? statusResult, HttpStatusCode statusCode, string statusDescription, string message, string url, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
        {
            return new JsonStatusResult(statusResult, statusCode, statusDescription, message, url, jsonRequestBehavior);
        }

        /// <summary>
        /// 以指定的参数创建一个用于返回用户定义的通用 JavaScript 对象表示法(JSON 数据格式) 内容的 <see cref="NDF.Web.Mvc.JsonStatusResult"/> 对象。
        /// <see cref="NDF.Web.Mvc.JsonStatusResult"/> 会将设定的 <see cref="NDF.Web.Mvc.JsonStatus"/> 结构类型参数解析成 Json 数据格式并通过 HttpResopnse 输出至客户端。
        /// </summary>
        /// <param name="controller">表示 ASP.NET MVC 控制器 对象。</param>
        /// <param name="jsonStatus">表示用户定义的通用 JavaScript 对象表示法(JSON 数据格式) 数据响应格式。</param>
        /// <param name="jsonRequestBehavior">指示是否允许来自客户端的 HTTP GET 请求。</param>
        /// <returns>一个用于返回用户定义的通用 JavaScript 对象表示法(JSON 数据格式) 内容的 <see cref="NDF.Web.Mvc.JsonStatusResult"/> 对象。</returns>
        public static JsonStatusResult JsonStatus(this Controller controller, JsonStatus jsonStatus, JsonRequestBehavior jsonRequestBehavior = JsonRequestBehavior.DenyGet)
        {
            return new JsonStatusResult(jsonStatus, jsonRequestBehavior);
        }



        /// <summary>
        /// 创建一个将指定对象序列化为 JavaScript 对象表示法 (JSON) 的 <see cref="NDF.Web.Mvc.JsonSerializeResult"/> 对象。
        /// </summary>
        /// <param name="controller">表示一个 <see cref="System.Web.Mvc.Controller"/> 类型的控制器对象。</param>
        /// <param name="data">要序列化的 JavaScript 对象图。</param>
        /// <returns>将指定对象序列化为 JSON 格式的 <see cref="NDF.Web.Mvc.JsonSerializeResult"/> 类型结果对象。在执行此方法所准备的结果对象时，ASP.NET MVC 框架会将该对象写入响应。</returns>
        public static JsonSerializeResult JsonSerialize(this Controller controller, object data)
        {
            return JsonSerialize(controller, data, null, null, null);
        }

        /// <summary>
        /// 创建一个将指定对象序列化为 JavaScript 对象表示法 (JSON) 的 <see cref="NDF.Web.Mvc.JsonSerializeResult"/> 对象。
        /// </summary>
        /// <param name="controller">表示一个 <see cref="System.Web.Mvc.Controller"/> 类型的控制器对象。</param>
        /// <param name="data">要序列化的 JavaScript 对象图。</param>
        /// <param name="behavior">JSON 请求行为。</param>
        /// <returns>将指定对象序列化为 JSON 格式的 <see cref="NDF.Web.Mvc.JsonSerializeResult"/> 类型结果对象。在执行此方法所准备的结果对象时，ASP.NET MVC 框架会将该对象写入响应。</returns>
        public static JsonSerializeResult JsonSerialize(this Controller controller, object data, JsonRequestBehavior behavior)
        {
            return JsonSerialize(controller, data, null, null, null, behavior);
        }

        /// <summary>
        /// 创建一个将指定对象序列化为 JavaScript 对象表示法 (JSON) 的 <see cref="NDF.Web.Mvc.JsonSerializeResult"/> 对象。
        /// </summary>
        /// <param name="controller">表示一个 <see cref="System.Web.Mvc.Controller"/> 类型的控制器对象。</param>
        /// <param name="data">要序列化的 JavaScript 对象图。</param>
        /// <param name="contentType">内容类型（MIME 类型）。</param>
        /// <param name="behavior">JSON 请求行为。</param>
        /// <returns>将指定对象序列化为 JSON 格式的 <see cref="NDF.Web.Mvc.JsonSerializeResult"/> 类型结果对象。在执行此方法所准备的结果对象时，ASP.NET MVC 框架会将该对象写入响应。</returns>
        public static JsonSerializeResult JsonSerialize(this Controller controller, object data, string contentType, JsonRequestBehavior behavior)
        {
            return JsonSerialize(controller, data, contentType, null, null, behavior);
        }

        /// <summary>
        /// 创建一个将指定对象序列化为 JavaScript 对象表示法 (JSON) 的 <see cref="NDF.Web.Mvc.JsonSerializeResult"/> 对象。
        /// </summary>
        /// <param name="controller">表示一个 <see cref="System.Web.Mvc.Controller"/> 类型的控制器对象。</param>
        /// <param name="data">要序列化的 JavaScript 对象图。</param>
        /// <param name="contentType">内容类型（MIME 类型）。</param>
        /// <param name="contentEncoding">内容编码。</param>
        /// <param name="behavior">JSON 请求行为。</param>
        /// <returns>将指定对象序列化为 JSON 格式的 <see cref="NDF.Web.Mvc.JsonSerializeResult"/> 类型结果对象。在执行此方法所准备的结果对象时，ASP.NET MVC 框架会将该对象写入响应。</returns>
        public static JsonSerializeResult JsonSerialize(this Controller controller, object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return JsonSerialize(controller, data, contentType, contentEncoding, null, behavior);
        }

        /// <summary>
        /// 创建一个将指定对象序列化为 JavaScript 对象表示法 (JSON) 的 <see cref="NDF.Web.Mvc.JsonSerializeResult"/> 对象。
        /// </summary>
        /// <param name="controller">表示一个 <see cref="System.Web.Mvc.Controller"/> 类型的控制器对象。</param>
        /// <param name="data">要序列化的 JavaScript 对象图。</param>
        /// <param name="settings">将要序列化的 JavaScript 对象图序列化为 JSON 格式字符串时所采用的参数。</param>
        /// <returns>将指定对象序列化为 JSON 格式的 <see cref="NDF.Web.Mvc.JsonSerializeResult"/> 类型结果对象。在执行此方法所准备的结果对象时，ASP.NET MVC 框架会将该对象写入响应。</returns>
        public static JsonSerializeResult JsonSerialize(this Controller controller, object data, JsonSerializerSettings settings)
        {
            return JsonSerialize(controller, data, null, null, settings);
        }

        /// <summary>
        /// 创建一个将指定对象序列化为 JavaScript 对象表示法 (JSON) 的 <see cref="NDF.Web.Mvc.JsonSerializeResult"/> 对象。
        /// </summary>
        /// <param name="controller">表示一个 <see cref="System.Web.Mvc.Controller"/> 类型的控制器对象。</param>
        /// <param name="data">要序列化的 JavaScript 对象图。</param>
        /// <param name="contentType">内容类型（MIME 类型）。</param>
        /// <param name="settings">将要序列化的 JavaScript 对象图序列化为 JSON 格式字符串时所采用的参数。</param>
        /// <returns>将指定对象序列化为 JSON 格式的 <see cref="NDF.Web.Mvc.JsonSerializeResult"/> 类型结果对象。在执行此方法所准备的结果对象时，ASP.NET MVC 框架会将该对象写入响应。</returns>
        public static JsonSerializeResult JsonSerialize(this Controller controller, object data, string contentType, JsonSerializerSettings settings)
        {
            return JsonSerialize(controller, data, contentType, null, settings);
        }

        /// <summary>
        /// 创建一个将指定对象序列化为 JavaScript 对象表示法 (JSON) 的 <see cref="NDF.Web.Mvc.JsonSerializeResult"/> 对象。
        /// </summary>
        /// <param name="controller">表示一个 <see cref="System.Web.Mvc.Controller"/> 类型的控制器对象。</param>
        /// <param name="data">要序列化的 JavaScript 对象图。</param>
        /// <param name="contentType">内容类型（MIME 类型）。</param>
        /// <param name="contentEncoding">内容编码。</param>
        /// <param name="settings">将要序列化的 JavaScript 对象图序列化为 JSON 格式字符串时所采用的参数。</param>
        /// <returns>将指定对象序列化为 JSON 格式的 <see cref="NDF.Web.Mvc.JsonSerializeResult"/> 类型结果对象。在执行此方法所准备的结果对象时，ASP.NET MVC 框架会将该对象写入响应。</returns>
        public static JsonSerializeResult JsonSerialize(this Controller controller, object data, string contentType, Encoding contentEncoding, JsonSerializerSettings settings)
        {
            return new JsonSerializeResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonSerializerSettings = settings
            };
        }

        /// <summary>
        /// 创建一个将指定对象序列化为 JavaScript 对象表示法 (JSON) 的 <see cref="NDF.Web.Mvc.JsonSerializeResult"/> 对象。
        /// </summary>
        /// <param name="controller">表示一个 <see cref="System.Web.Mvc.Controller"/> 类型的控制器对象。</param>
        /// <param name="data">要序列化的 JavaScript 对象图。</param>
        /// <param name="contentType">内容类型（MIME 类型）。</param>
        /// <param name="contentEncoding">内容编码。</param>
        /// <param name="settings">将要序列化的 JavaScript 对象图序列化为 JSON 格式字符串时所采用的参数。</param>
        /// <param name="behavior">JSON 请求行为。</param>
        /// <returns>将指定对象序列化为 JSON 格式的 <see cref="NDF.Web.Mvc.JsonSerializeResult"/> 类型结果对象。在执行此方法所准备的结果对象时，ASP.NET MVC 框架会将该对象写入响应。</returns>
        public static JsonSerializeResult JsonSerialize(this Controller controller, object data, string contentType, Encoding contentEncoding, JsonSerializerSettings settings, JsonRequestBehavior behavior)
        {
            return new JsonSerializeResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                JsonSerializerSettings = settings
            };
        }




        /// <summary>
        /// 以指定的参数创建一个用于返回带特定 HTTP 响应状态代码和说明的操作结果的 <see cref="System.Web.Mvc.HttpStatusCodeResult"/> 对象。
        /// </summary>
        /// <param name="controller">表示 ASP.NET MVC 控制器 对象。</param>
        /// <param name="statusCode">表示特定 HTTP 响应状态代码</param>
        /// <returns>一个用于返回带特定 HTTP 响应状态代码和说明的操作结果的 <see cref="System.Web.Mvc.HttpStatusCodeResult"/> 对象。</returns>
        public static HttpStatusCodeResult StatusCode(this Controller controller, int statusCode)
        {
            return new HttpStatusCodeResult(statusCode);
        }

        /// <summary>
        /// 以指定的参数创建一个用于返回带特定 HTTP 响应状态代码和说明的操作结果的 <see cref="System.Web.Mvc.HttpStatusCodeResult"/> 对象。
        /// </summary>
        /// <param name="controller">表示 ASP.NET MVC 控制器 对象。</param>
        /// <param name="statusCode">表示特定 HTTP 响应状态代码。</param>
        /// <returns>一个用于返回带特定 HTTP 响应状态代码和说明的操作结果的 <see cref="System.Web.Mvc.HttpStatusCodeResult"/> 对象。</returns>
        public static HttpStatusCodeResult StatusCode(this Controller controller, HttpStatusCode statusCode)
        {
            return new HttpStatusCodeResult(statusCode);
        }

        /// <summary>
        /// 以指定的参数创建一个用于返回带特定 HTTP 响应状态代码和说明的操作结果的 <see cref="System.Web.Mvc.HttpStatusCodeResult"/> 对象。
        /// </summary>
        /// <param name="controller">表示 ASP.NET MVC 控制器 对象。</param>
        /// <param name="statusCode">表示特定 HTTP 响应状态代码。</param>
        /// <param name="statusDescription">表示特定 HTTP 响应状态说明。</param>
        /// <returns>一个用于返回带特定 HTTP 响应状态代码和说明的操作结果的 <see cref="System.Web.Mvc.HttpStatusCodeResult"/> 对象。</returns>
        public static HttpStatusCodeResult StatusCode(this Controller controller, int statusCode, string statusDescription)
        {
            return new HttpStatusCodeResult(statusCode, statusDescription);
        }

        /// <summary>
        /// 以指定的参数创建一个用于返回带特定 HTTP 响应状态代码和说明的操作结果的 <see cref="System.Web.Mvc.HttpStatusCodeResult"/> 对象。
        /// </summary>
        /// <param name="controller">表示 ASP.NET MVC 控制器 对象。</param>
        /// <param name="statusCode">表示特定 HTTP 响应状态代码。</param>
        /// <param name="statusDescription">表示特定 HTTP 响应状态说明。</param>
        /// <returns>一个用于返回带特定 HTTP 响应状态代码和说明的操作结果的 <see cref="System.Web.Mvc.HttpStatusCodeResult"/> 对象。</returns>
        public static HttpStatusCodeResult StatusCode(this Controller controller, HttpStatusCode statusCode, string statusDescription)
        {
            return new HttpStatusCodeResult(statusCode, statusDescription);
        }



        /// <summary>
        /// 以指定的参数创建一个用于表示用户自定义 HttpResponse 输出内容的 <see cref="NDF.Web.Mvc.HttpStatusCodeContentResult"/> 对象。
        /// </summary>
        /// <param name="controller">表示 ASP.NET MVC 控制器 <see cref="System.Web.Mvc.Controller"/> 对象。</param>
        /// <param name="statusCode">表示 HTTP 状态码。</param>
        /// <returns>一个用于表示用户自定义 HttpResponse 输出内容的 <see cref="NDF.Web.Mvc.HttpStatusCodeContentResult"/> 对象。</returns>
        public static HttpStatusCodeContentResult StatusCodeContent(this Controller controller, int statusCode)
        {
            return new HttpStatusCodeContentResult(statusCode);
        }

        /// <summary>
        /// 以指定的参数创建一个用于表示用户自定义 HttpResponse 输出内容的 <see cref="NDF.Web.Mvc.HttpStatusCodeContentResult"/> 对象。
        /// </summary>
        /// <param name="controller">表示 ASP.NET MVC 控制器 <see cref="System.Web.Mvc.Controller"/> 对象。</param>
        /// <param name="statusCode">表示 HTTP 状态码。</param>
        /// <returns>一个用于表示用户自定义 HttpResponse 输出内容的 <see cref="NDF.Web.Mvc.HttpStatusCodeContentResult"/> 对象。</returns>
        public static HttpStatusCodeContentResult StatusCodeContent(this Controller controller, HttpStatusCode statusCode)
        {
            return new HttpStatusCodeContentResult(statusCode);
        }

        /// <summary>
        /// 以指定的参数创建一个用于表示用户自定义 HttpResponse 输出内容的 <see cref="NDF.Web.Mvc.HttpStatusCodeContentResult"/> 对象。
        /// </summary>
        /// <param name="controller">表示 ASP.NET MVC 控制器 <see cref="System.Web.Mvc.Controller"/> 对象。</param>
        /// <param name="statusCode">表示 HTTP 状态码。</param>
        /// <param name="statusDescription">表示 HTTP 状态描述。</param>
        /// <returns>一个用于表示用户自定义 HttpResponse 输出内容的 <see cref="NDF.Web.Mvc.HttpStatusCodeContentResult"/> 对象。</returns>
        public static HttpStatusCodeContentResult StatusCodeContent(this Controller controller, int statusCode, string statusDescription)
        {
            return new HttpStatusCodeContentResult(statusCode, statusDescription);
        }

        /// <summary>
        /// 以指定的参数创建一个用于表示用户自定义 HttpResponse 输出内容的 <see cref="NDF.Web.Mvc.HttpStatusCodeContentResult"/> 对象。
        /// </summary>
        /// <param name="controller">表示 ASP.NET MVC 控制器 <see cref="System.Web.Mvc.Controller"/> 对象。</param>
        /// <param name="statusCode">表示 HTTP 状态码。</param>
        /// <param name="statusDescription">表示 HTTP 状态描述。</param>
        /// <returns>一个用于表示用户自定义 HttpResponse 输出内容的 <see cref="NDF.Web.Mvc.HttpStatusCodeContentResult"/> 对象。</returns>
        public static HttpStatusCodeContentResult StatusCodeContent(this Controller controller, HttpStatusCode statusCode, string statusDescription)
        {
            return new HttpStatusCodeContentResult(statusCode, statusDescription);
        }

        /// <summary>
        /// 以指定的参数创建一个用于表示用户自定义 HttpResponse 输出内容的 <see cref="NDF.Web.Mvc.HttpStatusCodeContentResult"/> 对象。
        /// </summary>
        /// <param name="controller">表示 ASP.NET MVC 控制器 <see cref="System.Web.Mvc.Controller"/> 对象。</param>
        /// <param name="content">HTTP 响应内容。</param>
        /// <returns>一个用于表示用户自定义 HttpResponse 输出内容的 <see cref="NDF.Web.Mvc.HttpStatusCodeContentResult"/> 对象。</returns>
        public static HttpStatusCodeContentResult StatusCodeContent(this Controller controller, string content)
        {
            return new HttpStatusCodeContentResult(content);
        }

        /// <summary>
        /// 以指定的参数创建一个用于表示用户自定义 HttpResponse 输出内容的 <see cref="NDF.Web.Mvc.HttpStatusCodeContentResult"/> 对象。
        /// </summary>
        /// <param name="controller">表示 ASP.NET MVC 控制器 <see cref="System.Web.Mvc.Controller"/> 对象。</param>
        /// <param name="content">HTTP 响应内容。</param>
        /// <param name="statusCode">HTTP 响应状态代码。</param>
        /// <returns>一个用于表示用户自定义 HttpResponse 输出内容的 <see cref="NDF.Web.Mvc.HttpStatusCodeContentResult"/> 对象。</returns>
        public static HttpStatusCodeContentResult StatusCodeContent(this Controller controller, string content, int statusCode)
        {
            return new HttpStatusCodeContentResult(content, statusCode);
        }

        /// <summary>
        /// 以指定的参数创建一个用于表示用户自定义 HttpResponse 输出内容的 <see cref="NDF.Web.Mvc.HttpStatusCodeContentResult"/> 对象。
        /// </summary>
        /// <param name="controller">表示 ASP.NET MVC 控制器 <see cref="System.Web.Mvc.Controller"/> 对象。</param>
        /// <param name="content">HTTP 响应内容。</param>
        /// <param name="statusCode">HTTP 响应状态代码。</param>
        /// <returns>一个用于表示用户自定义 HttpResponse 输出内容的 <see cref="NDF.Web.Mvc.HttpStatusCodeContentResult"/> 对象。</returns>
        public static HttpStatusCodeContentResult StatusCodeContent(this Controller controller, string content, HttpStatusCode statusCode)
        {
            return new HttpStatusCodeContentResult(content, statusCode);
        }

        /// <summary>
        /// 以指定的参数创建一个用于表示用户自定义 HttpResponse 输出内容的 <see cref="NDF.Web.Mvc.HttpStatusCodeContentResult"/> 对象。
        /// </summary>
        /// <param name="controller">表示 ASP.NET MVC 控制器 <see cref="System.Web.Mvc.Controller"/> 对象。</param>
        /// <param name="content">HTTP 响应内容。</param>
        /// <param name="statusCode">HTTP 响应状态代码。</param>
        /// <param name="statusDescription">HTTP 响应状态说明。</param>
        /// <param name="status">返回到客户端的 Status 值。</param>
        /// <param name="subStatusCode">HTTP 响应的二级状态代码。</param>
        /// <param name="charset">当前 HTTP 响应的 HTTP 字符集。</param>
        /// <param name="contentEncoding">HTTP 响应内容编码。</param>
        /// <param name="contentType"> HTTP 响应内容类型。</param>
        /// <param name="headerEncoding">当前 HTTP 响应的标头的编码。</param>
        /// <returns>一个用于表示用户自定义 HttpResponse 输出内容的 <see cref="NDF.Web.Mvc.HttpStatusCodeContentResult"/> 对象。</returns>
        public static HttpStatusCodeContentResult StatusCodeContent(this Controller controller, string content, int statusCode, string statusDescription, string status, int? subStatusCode, string charset, Encoding contentEncoding, string contentType, Encoding headerEncoding)
        {
            return new HttpStatusCodeContentResult(content, statusCode, statusDescription, status, subStatusCode, charset, contentEncoding, contentType, headerEncoding);
        }

        /// <summary>
        /// 以指定的参数创建一个用于表示用户自定义 HttpResponse 输出内容的 <see cref="NDF.Web.Mvc.HttpStatusCodeContentResult"/> 对象。
        /// </summary>
        /// <param name="controller">表示 ASP.NET MVC 控制器 <see cref="System.Web.Mvc.Controller"/> 对象。</param>
        /// <param name="content">HTTP 响应内容。</param>
        /// <param name="statusCode">HTTP 响应状态代码。</param>
        /// <param name="statusDescription">HTTP 响应状态说明。</param>
        /// <param name="status">返回到客户端的 Status 值。</param>
        /// <param name="subStatusCode">HTTP 响应的二级状态代码。</param>
        /// <param name="charset">当前 HTTP 响应的 HTTP 字符集。</param>
        /// <param name="contentEncoding">HTTP 响应内容编码。</param>
        /// <param name="contentType"> HTTP 响应内容类型。</param>
        /// <param name="headerEncoding">当前 HTTP 响应的标头的编码。</param>
        /// <returns>一个用于表示用户自定义 HttpResponse 输出内容的 <see cref="NDF.Web.Mvc.HttpStatusCodeContentResult"/> 对象。</returns>
        public static HttpStatusCodeContentResult StatusCodeContent(this Controller controller, string content, HttpStatusCode statusCode, string statusDescription, string status, int? subStatusCode, string charset, Encoding contentEncoding, string contentType, Encoding headerEncoding)
        {
            return new HttpStatusCodeContentResult(content, statusCode, statusDescription, status, subStatusCode, charset, contentEncoding, contentType, headerEncoding);
        }

        /// <summary>
        /// 以指定的参数创建一个用于表示用户自定义 HttpResponse 输出内容的 <see cref="NDF.Web.Mvc.HttpStatusCodeContentResult"/> 对象。
        /// </summary>
        /// <param name="controller">表示 ASP.NET MVC 控制器 <see cref="System.Web.Mvc.Controller"/> 对象。</param>
        /// <param name="content">表示用于定义的 HTTP 请求响应。</param>
        /// <returns>一个用于表示用户自定义 HttpResponse 输出内容的 <see cref="NDF.Web.Mvc.HttpStatusCodeContentResult"/> 对象。</returns>
        public static HttpStatusCodeContentResult StatusCodeContent(this Controller controller, HttpStatusCodeContent content)
        {
            return new HttpStatusCodeContentResult(content);
        }
    }
}
