using NDF.Utilities;
using NDF.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace NDF.Web.Http
{
    /// <summary>
    /// 指定对 WebAPI 控制器或操作方法的访问只限于满足授权要求的用户。
    /// 
    /// 在 NetDirkFramework 应用程序中，基于 WebAPI 机制的所有授权验证服务请勿直接继承于特性类 <see cref="System.Web.Http.AuthorizeAttribute"/>，而应该继承于
    ///     该特性类 <see cref="NDF.Web.Http.HttpAuthorizeAttribute"/> 。
    ///     在该特性类的子类中，请勿直接重写 OnAuthorization 方法以实现相应的验证或其他效果，而应该重写其他相应的
    ///     分支方法： 以实现具体对验证条件的判断或针对具体不同验证结果的操作。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class HttpAuthorizeAttribute : System.Web.Http.AuthorizeAttribute
    {

        /// <summary>
        /// 重写时，提供一个入口点用于指示指定的请求是否已获得授权。
        /// </summary>
        /// <param name="actionContext">包含正在执行的操作信息。</param>
        /// <returns>如果当前请求已经获得授权，则返回 true；否则返回 false。</returns>
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            return base.IsAuthorized(actionContext);
        }

        /// <summary>
        /// 重写时，提供一个入口点用于授权时的调用操作。
        /// 在该类型 <see cref="NDF.Web.Http.HttpAuthorizeAttribute"/> 的子类中需要重写逻辑时，请勿直接重写该方法以实现相应的验证或其他效果，而应该重写其他相应的
        ///     分支方法： 以实现具体对验证条件的判断或针对具体不同验证结果的操作。
        /// </summary>
        /// <param name="actionContext">包含正在执行的操作信息。</param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            Check.NotNull(actionContext, "actionContext");

            bool skip = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                     || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                     || actionContext.ActionDescriptor.GetCustomAttributes<AllowAuthorizeAttribute>().Any(attribute => attribute.Validate(this.GetType()))
                     || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAuthorizeAttribute>().Any(attribute => attribute.Validate(this.GetType()));
            if (skip)
            {
                return;
            }

            if (this.SkipAuthorization(actionContext))
            {
                this.HandleSkipAuthorization(actionContext);
                return;
            }

            if (this.IsAuthorized(actionContext))
            {
                this.HandleAllowedRequest(actionContext);
            }
            else
            {
                this.HandleUnauthorizedRequest(actionContext);
            }
        }



        /// <summary>
        /// 重写时，提供一个入口点用于处理当前请求是否绕过该 <see cref="NDF.Web.Http.HttpAuthorizeAttribute"/> 或其子类的验证。
        /// 默认情况下该方法返回 false，即所有请求都不应该绕过 <see cref="NDF.Web.Http.HttpAuthorizeAttribute"/> 过滤器的验证。
        /// </summary>
        /// <param name="actionContext">封装有关使用 System.Web.Http.HttpAuthorizeAttribute 的信息。filterContext 对象包括控制器、HTTP 上下文、请求上下文、操作结果和路由数据。</param>
        /// <returns>如果当前请求应该绕过对该 <see cref="NDF.Web.Http.HttpAuthorizeAttribute"/> 过滤器的验证，则返回 true，否则返回 false。</returns>
        /// <exception cref="System.ArgumentNullException">filterContext 参数为 null。</exception>
        protected virtual bool SkipAuthorization(HttpActionContext actionContext)
        {
            return false;
        }

        /// <summary>
        /// 重写时，提供一个入口点用于处理被当前 <see cref="NDF.Web.Http.HttpAuthorizeAttribute"/> 过滤器绕过验证的 HTTP 请求。
        /// </summary>
        /// <param name="actionContext">封装有关使用 System.Web.Http.HttpAuthorizeAttribute 的信息。filterContext 对象包括控制器、HTTP 上下文、请求上下文、操作结果和路由数据。</param>
        /// <exception cref="System.ArgumentNullException">filterContext 参数为 null。</exception>
        protected virtual void HandleSkipAuthorization(HttpActionContext actionContext)
        {
        }


        /// <summary>
        /// 重写时，提供一个入口点用于处理经过授权的 HTTP 请求。
        /// </summary>
        /// <param name="actionContext">封装有关使用 System.Web.Http.HttpAuthorizeAttribute 的信息。filterContext 对象包括控制器、HTTP 上下文、请求上下文、操作结果和路由数据。</param>
        /// <exception cref="System.ArgumentNullException">filterContext 参数为 null。</exception>
        protected virtual void HandleAllowedRequest(HttpActionContext actionContext)
        {
        }

        /// <summary>
        /// 重写时，提供一个入口点用于处理未能授权的 HTTP 请求。
        /// </summary>
        /// <param name="actionContext">封装有关使用 System.Web.Http.HttpAuthorizeAttribute 的信息。filterContext 对象包括控制器、HTTP 上下文、请求上下文、操作结果和路由数据。</param>
        /// <exception cref="System.ArgumentNullException">filterContext 参数为 null。</exception>
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            base.HandleUnauthorizedRequest(actionContext);
        }


    }
}
