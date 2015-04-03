using NDF.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NDF.Web.Mvc
{
    /// <summary>
    /// 表示一个类，该类用于将 JSON 序列化字符串格式的内容发送到响应。
    /// </summary>
    public class JsonSerializeResult : ActionResult
    {
        /// <summary>
        /// 初始化类型的 <see cref="JsonSerializeResult"/> 新实例。
        /// </summary>
        public JsonSerializeResult()
        {
            this.JsonRequestBehavior = System.Web.Mvc.JsonRequestBehavior.DenyGet;
        }


        /// <summary>
        /// 获取或设置表示该 JSON 序列化字符串发送到客户端响应时所采用的内容编码方式。
        /// </summary>
        public Encoding ContentEncoding { get; set; }

        /// <summary>
        /// 获取或设置表示该 JSON 序列化字符串发送到客户端响应时所采用的 HTTP MIME 类型。
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 获取或设置将要发送到客户端的被 JSON 序列化为字符串的对象。
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 指定将数据发送至客户端时是否允许来自客户端的 HTTP GET 请求。
        /// </summary>
        public JsonRequestBehavior JsonRequestBehavior { get; set; }

        /// <summary>
        /// 指定将数据发送至客户端时对 <seealso cref="Data"/> 数据进行 JSON 序列化的参数。
        /// </summary>
        public JsonSerializerSettings JsonSerializerSettings { get; set; }



        /// <summary>
        /// 通过从 <see cref="System.Web.Mvc.ActionResult"/> 类继承的自定义类型，启用对操作方法结果的处理。
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            Check.NotNull(context);
            if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
                String.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(Resources.JsonRequest_GetNotAllowed);
            }

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = String.IsNullOrEmpty(this.ContentType) ? "application/json" : this.ContentType;

            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }
            if (this.Data != null)
            {
                response.Write(this.JsonSerializerSettings == null ? this.Data.ToJson() : this.Data.ToJson(this.JsonSerializerSettings));
            }
        }
    }
}
