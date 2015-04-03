using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.SessionState;

namespace NDF.Web
{
    /// <summary>
    /// 表示 WEB 应用程序运行环境。
    /// 关于有关当前操作系统环境和 .NET 平台的信息/操作方法可以从 <see cref="System.Environment"/> 静态类获取。
    /// </summary>
    public class Environment
    {
        /// <summary>
        /// 以指定的 HTTP 上下文 <see cref="System.Web.HttpContext"/> 信息初始化 <see cref="NDF.Web.Environment"/> 对象。
        /// </summary>
        /// <param name="context">HTTP 上下文 <see cref="System.Web.HttpContext"/> 信息。</param>
        public Environment(HttpContext context)
        {
            this.HttpContext = context;
        }


        private static Environment _instance;

        /// <summary>
        /// 获取当前的 WEB 应用程序默认运行环境。
        /// </summary>
        public static Environment Default
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Environment(HttpContext.Current);
                }
                else
                {
                    _instance.HttpContext = HttpContext.Current;
                }
                return _instance;
            }
        }



        /// <summary>
        /// 获取 <see cref="NDF.Web.Environment"/> 对象所包含的 HTTP 上下文 <see cref="System.Web.HttpContext"/> 信息。
        /// </summary>
        public HttpContext HttpContext
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取提供用于处理 Web 请求的方法的 <see cref="System.Web.HttpServerUtility"/> 对象。
        /// </summary>
        public virtual HttpServerUtility Server
        {
            get { return this.HttpContext.Server; }
        }

        /// <summary>
        /// 为当前 HTTP 请求获取 <see cref="System.Web.HttpApplicationState"/> 对象。
        /// </summary>
        public virtual HttpApplicationState Application
        {
            get { return this.HttpContext.Application; }
        }

        /// <summary>
        /// 为当前 HTTP 请求获取 <see cref="System.Web.SessionState.HttpSessionState"/> 对象。
        /// </summary>
        public virtual HttpSessionState Session
        {
            get { return this.HttpContext.Session; }
        }

        /// <summary>
        /// 获取当前应用程序域的 <see cref="System.Web.Caching.Cache"/> 对象。
        /// </summary>
        public virtual Cache Cache
        {
            get { return this.HttpContext.Cache; }
        }

        /// <summary>
        /// 获取客户端发送的 Cookie 的集合。
        /// </summary>
        public virtual HttpCookieCollection RequestCookies
        {
            get { return this.Request.Cookies; }
        }

        /// <summary>
        /// 获取响应 Cookie 集合。
        /// </summary>
        public virtual HttpCookieCollection ResponseCookies
        {
            get { return this.Response.Cookies; }
        }

        /// <summary>
        /// 获取当前 HTTP 请求的 <see cref="System.Web.HttpRequest"/> 对象。
        /// </summary>
        public virtual HttpRequest Request
        {
            get { return this.HttpContext.Request; }
        }

        /// <summary>
        /// 获取当前 HTTP 响应的 <see cref="System.Web.HttpResponse"/> 对象。
        /// </summary>
        public virtual HttpResponse Response
        {
            get { return this.HttpContext.Response; }
        }


        /// <summary>
        /// 获取在处理 HTTP 请求的过程中累积的第一个错误（如果有）。
        /// </summary>
        public virtual Exception Error
        {
            get { return this.HttpContext.Error; }
        }

        /// <summary>
        /// 获取在处理 HTTP 请求的过程中累积的错误数组。
        /// </summary>
        public virtual Exception[] AllErrors
        {
            get { return this.HttpContext.AllErrors; }
        }



        /// <summary>
        /// 获取当前 HTTP 请求的初始时间戳。
        /// </summary>
        public virtual DateTime Timestamp
        {
            get { return this.HttpContext.Timestamp; }
        }

        /// <summary>
        /// 为当前 HTTP 响应获取 <see cref="System.Web.TraceContext"/> 对象。
        /// </summary>
        public virtual TraceContext Trace
        {
            get { return this.HttpContext.Trace; }
        }

        /// <summary>
        /// 为当前 HTTP 请求获取或设置安全信息。
        /// </summary>
        public virtual IPrincipal User
        {
            get { return this.HttpContext.User; }
            set { this.HttpContext.User = value; }
        }

        /// <summary>
        /// 获取用于 System.Web.WebSockets.AspNetWebSocket 连接而从服务器发送到客户端的协商协议。
        /// </summary>
        public virtual string WebSocketNegotiatedProtocol
        {
            get { return this.HttpContext.WebSocketNegotiatedProtocol; }
        }

        /// <summary>
        /// 获取客户要求的子协议列表的有序列表。
        /// </summary>
        public virtual IList<string> WebSocketRequestedProtocols
        {
            get { return this.HttpContext.WebSocketRequestedProtocols; }
        }





        private static string _cookieToken = "NDF_";

        /// <summary>
        /// 获取或设置当前 HTTP 请求环境中用于设置 HttpCooike 名称的前缀字符；该值默认为 "NDF_"。
        /// </summary>
        public static string CookieToken
        {
            get { return _cookieToken; }
            set { _cookieToken = value; }
        }



        /// <summary>
        /// 获取 WEB 客户端提交过来请求中 Cookies 集合内指定名称的 Cookie 值。
        /// </summary>
        /// <param name="name">指定的 Cookie 名称。</param>
        /// <returns>一个 <see cref="System.String"/> 值，表示参数 name 所示名称的 Cookie 值。</returns>
        public virtual string GetCookieValue(string name)
        {
            return this.GetCookieValue(name, null);
        }

        /// <summary>
        /// 获取 WEB 客户端提交过来请求中 Cookies 集合内指定名称和键的 Cookie 值。
        /// </summary>
        /// <param name="name">指定的 Cookie 名称。</param>
        /// <param name="key">指定名称的 Cookie 对象中的指定键。</param>
        /// <returns>一个 <see cref="System.String"/> 值，表示参数 name 所示名称的 Cookie 对象中对应的 key 键的值。</returns>
        public virtual string GetCookieValue(string name, string key)
        {
            string ret = null,
                   cookieName = this.BuildCookieName(name);
            var cookie = this.RequestCookies[cookieName];
            if (cookie != null)
            {
                ret = !string.IsNullOrWhiteSpace(key) && cookie.HasKeys ? cookie[key] : cookie.Value;
            }
            return ret;
        }




        /// <summary>
        /// 设置 HTTP 输出响应 Cookie 集合 HttpContext.Current.Response.Cookies 中指定名称的 Cookie 值。
        /// </summary>
        /// <param name="name">指定的 Cookie 名称。</param>
        /// <param name="value">要设置的 Cookie 值。</param>
        public virtual void SetCookieValue(string name, string value)
        {
            this.SetCookieValue(name, value, CookieExpires);
        }

        /// <summary>
        /// 设置 HTTP 输出响应 Cookie 集合 HttpContext.Current.Response.Cookies 中指定名称的 Cookie 值。
        /// 同时限定该 Cookie 的过期时间和日期。
        /// </summary>
        /// <param name="name">指定的 Cookie 名称。</param>
        /// <param name="value">要设置的 Cookie 值。</param>
        /// <param name="expiress">指定 name 所示的 Cookie 对象的过期时间和日期，单位为 分钟。</param>
        public virtual void SetCookieValue(string name, string value, int expiress)
        {
            this.SetCookieValue(name, null, value, expiress);
        }

        /// <summary>
        /// 设置 HTTP 输出响应 Cookie 集合 HttpContext.Current.Response.Cookies 中指定名称和键的 Cookie 值。
        /// </summary>
        /// <param name="name">指定的 Cookie 名称。</param>
        /// <param name="key">指定名称的 Cookie 对象中的指定键，如果该参数为 Null 或空字符串，将直接设置名称为 name 的 Cookie 的 Value 属性值。</param>
        /// <param name="value">要设置的 Cookie 值。</param>
        public virtual void SetCookieValue(string name, string key, string value)
        {
            this.SetCookieValue(name, key, value, CookieExpires);
        }

        /// <summary>
        /// 设置 HTTP 输出响应 Cookie 集合 HttpContext.Current.Response.Cookies 中指定名称和键的 Cookie 值。
        /// 同时限定该 Cookie 的过期时间和日期。
        /// </summary>
        /// <param name="name">指定的 Cookie 名称。</param>
        /// <param name="key">指定名称的 Cookie 对象中的指定键，如果该参数为 Null 或空字符串，将直接设置名称为 name 的 Cookie 的 Value 属性值。</param>
        /// <param name="value">要设置的 Cookie 值。</param>
        /// <param name="expires">指定 name 所示的 Cookie 对象的过期时间和日期，单位为 分钟。</param>
        public virtual void SetCookieValue(string name, string key, string value, int expires)
        {
            string cookieName = this.BuildCookieName(name);
            var cookie = this.ResponseCookies[cookieName];
            if (cookie == null)
            {
                cookie = new HttpCookie(cookieName);
                this.ResponseCookies.Add(cookie);
            }
            if (string.IsNullOrWhiteSpace(key))
            {
                cookie.Value = value;
            }
            else
            {
                cookie[key] = value;
            }
            cookie.Expires = DateTime.Now.AddMinutes(expires);
        }



        /// <summary>
        /// 使得 HTTP 输出响应 Cookie 集合 HttpContext.Current.Response.Cookies 中指定名称和键的 Cookie 立即过期从而让浏览器不能获取到该 Cookie 值。
        /// </summary>
        /// <param name="name">指定的 Cookie 名称。</param>
        public void RemoveCookie(string name)
        {
            this.RemoveCookie(name, null);
        }

        /// <summary>
        /// 使得 HTTP 输出响应 Cookie 集合 HttpContext.Current.Response.Cookies 中指定名称和键的 Cookie 值为空或者整个 Cookie 对象立即过期从而让浏览器不能获取到该 Cookie 值。
        /// </summary>
        /// <param name="name">指定的 Cookie 名称。</param>
        /// <param name="key">指定名称的 Cookie 对象中的指定键，如果该参数为 Null 或空字符串，将直接设置名称为 name 的整个 Cookie 对象过期，否则仅设置该 key 所示的键值为空。</param>
        public void RemoveCookie(string name, string key)
        {
            string cookieName = this.BuildCookieName(name);
            var cookie = this.RequestCookies[cookieName];
            if (cookie != null)
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    cookie[key] = null;
                }
                else
                {
                    cookie.Expires = DateTime.Now.AddDays(-1);
                }
                this.ResponseCookies.Add(cookie);
            }
        }






        /// <summary>
        /// 用于构建 HttpCooike 的名称。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual string BuildCookieName(string name)
        {
            Check.NotEmpty(name, "name");
            return CookieToken + name;
        }



        private static int cookieExpires = 10080;

        /// <summary>
        /// 获取或设置当前 WEB 应用程序环境中的 HttpCookie 值默认有效期(单位为 分钟)，初始值为 10080 min(7 天整)。
        /// 该值范围介于 30 min(半小时) 至 43200 min(1个月)。
        /// </summary>
        public static int CookieExpires
        {
            get { return cookieExpires; }
            set { cookieExpires = Math.Max(43200, Math.Min(30, value)); }
        }
    }
}
