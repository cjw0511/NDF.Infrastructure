using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NDF.Web.Mvc
{
    /// <summary>
    /// 指定对控制器或操作方法的访问只限于满足授权要求的用户。
    /// 
    /// 在 NetDirkFramework 应用程序中，所有的授权验证服务请勿直接继承于特性类 <see cref="System.Web.Mvc.AuthorizeAttribute"/>，而应该继承于
    ///     该特性类 <see cref="NDF.Web.Mvc.AuthorizeAttribute"/> 。
    ///     在该特性类的子类中，请勿直接重写 OnAuthorization 方法以实现相应的验证或其他效果，而应该重写其他相应的
    ///     分支方法： 以实现具体对验证条件的判断或针对具体不同验证结果的操作。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        /// <summary>
        /// 重写时，提供一个入口点用于进行自定义授权检查。
        /// 该方法已经被弃用；如果需要进行自定义授权检查，请重写方法 AuthorizeCore(AuthorizationContext filterContext)。
        /// </summary>
        /// <param name="httpContext">HTTP 上下文，它封装有关单个 HTTP 请求的所有 HTTP 特定的信息。</param>
        /// <returns>如果用户已经过授权，则为 true；否则为 false。</returns>
        /// <exception cref="System.ArgumentNullException">httpContext 参数为 null。</exception>
        [SuppressMessage("Microsoft.Design", "CA1041:ProvideObsoleteAttributeMessage")]
        //[Obsolete("该方法已经被弃用；如果需要进行自定义授权检查，请重写方法 AuthorizeCore(AuthorizationContext filterContext)。", true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return base.AuthorizeCore(httpContext);
        }


        /// <summary>
        /// 重写时，提供一个入口点用于进行自定义授权检查。
        /// </summary>
        /// <param name="filterContext">筛选器上下文，它封装有关使用 System.Web.Mvc.AuthorizeAttribute 的信息。</param>
        /// <returns>如果用户已经过授权，则为 true；否则为 false。</returns>
        /// <exception cref="System.ArgumentNullException">filterContext 参数为 null。</exception>
        protected virtual bool AuthorizeCore(AuthorizationContext filterContext)
        {
            return base.AuthorizeCore(filterContext.HttpContext);
        }


        /// <summary>
        /// 重写时，提供一个入口点用于在过程请求授权时调用。
        /// 在该类型 <see cref="NDF.Web.Mvc.AuthorizeAttribute"/> 的子类中需要重写逻辑时，请勿直接重写该方法以实现相应的验证或其他效果，而应该重写其他相应的
        ///     分支方法： 以实现具体对验证条件的判断或针对具体不同验证结果的操作。
        /// </summary>
        /// <param name="filterContext">筛选器上下文，它封装有关使用 System.Web.Mvc.AuthorizeAttribute 的信息。</param>
        /// <exception cref="System.ArgumentNullException">filterContext 参数为 null。</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            Check.NotNull(filterContext);

            if (OutputCacheAttribute.IsChildActionCacheActive(filterContext))
            {
                // 如果需要验证能够被成功并正确调用，当有子操作缓存处于活动状态时，立即抛出异常。
                // 因为在过滤器上下文对象从缓存中退出之前，没有办法执行相应的过滤器回调函数，不能保证该过滤器能够在后续的请求中能被重新执行。
                throw new InvalidOperationException("MvcResources.AuthorizeAttribute_CannotUseWithinChildActionCache");
            }

            Type allowAnonymousAttribute = typeof(AllowAnonymousAttribute);
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(allowAnonymousAttribute, inherit: true)
                                     || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(allowAnonymousAttribute, inherit: true);
            if (skipAuthorization)
            {
                return;
            }

            Type allowAuthorizeAttribute = typeof(AllowAuthorizeAttribute);
            bool skipAllowAuthorize = filterContext.ActionDescriptor.GetCustomAttributes(allowAuthorizeAttribute, inherit: true).OfType<AllowAuthorizeAttribute>().Any(attribute => attribute.Validate(this.GetType()))
                                     || filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(allowAuthorizeAttribute, inherit: true).OfType<AllowAuthorizeAttribute>().Any(attribute => attribute.Validate(this.GetType()));
            if (skipAllowAuthorize)
            {
                return;
            }

            if (this.SkipAuthorization(filterContext))
            {
                this.HandleSkipAuthorization(filterContext);
                return;
            }

            if (this.AuthorizeCore(filterContext))
            {
                HttpCachePolicyBase cachePolicy = filterContext.HttpContext.Response.Cache;
                cachePolicy.SetProxyMaxAge(new TimeSpan(0));
                cachePolicy.AddValidationCallback(this.CacheValidateHandler, null /* data */);
                this.HandleAllowedRequest(filterContext);
            }
            else
            {
                this.HandleUnauthorizedRequest(filterContext);
            }
        }



        /// <summary>
        /// 重写时，提供一个入口点用于处理当前请求是否绕过该 <see cref="NDF.Web.Mvc.AuthorizeAttribute"/> 或其子类的验证。
        /// 默认情况下该方法返回 false，即所有请求都不应该绕过 <see cref="NDF.Web.Mvc.AuthorizeAttribute"/> 过滤器的验证。
        /// </summary>
        /// <param name="filterContext">封装有关使用 System.Web.Mvc.AuthorizeAttribute 的信息。filterContext 对象包括控制器、HTTP 上下文、请求上下文、操作结果和路由数据。</param>
        /// <returns>如果当前请求应该绕过对该 <see cref="NDF.Web.Mvc.AuthorizeAttribute"/> 过滤器的验证，则返回 true，否则返回 false。</returns>
        /// <exception cref="System.ArgumentNullException">filterContext 参数为 null。</exception>
        protected virtual bool SkipAuthorization(AuthorizationContext filterContext)
        {
            return false;
        }

        /// <summary>
        /// 重写时，提供一个入口点用于处理被当前 <see cref="NDF.Web.Mvc.AuthorizeAttribute"/> 过滤器绕过验证的 HTTP 请求。
        /// </summary>
        /// <param name="filterContext">封装有关使用 System.Web.Mvc.AuthorizeAttribute 的信息。filterContext 对象包括控制器、HTTP 上下文、请求上下文、操作结果和路由数据。</param>
        /// <exception cref="System.ArgumentNullException">filterContext 参数为 null。</exception>
        protected virtual void HandleSkipAuthorization(AuthorizationContext filterContext)
        {
        }


        /// <summary>
        /// 重写时，提供一个入口点用于处理经过授权的 HTTP 请求。
        /// </summary>
        /// <param name="filterContext">封装有关使用 System.Web.Mvc.AuthorizeAttribute 的信息。filterContext 对象包括控制器、HTTP 上下文、请求上下文、操作结果和路由数据。</param>
        /// <exception cref="System.ArgumentNullException">filterContext 参数为 null。</exception>
        protected virtual void HandleAllowedRequest(AuthorizationContext filterContext)
        {
        }

        /// <summary>
        /// 重写时，提供一个入口点用于处理未能授权的 HTTP 请求。
        /// </summary>
        /// <param name="filterContext">封装有关使用 System.Web.Mvc.AuthorizeAttribute 的信息。filterContext 对象包括控制器、HTTP 上下文、请求上下文、操作结果和路由数据。</param>
        /// <exception cref="System.ArgumentNullException">filterContext 参数为 null。</exception>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
        }


        /// <summary>
        /// 重写时，提供一个入口点用于在缓存模块请求授权时调用。
        /// </summary>
        /// <param name="httpContext">HTTP 上下文，它封装有关单个 HTTP 请求的所有 HTTP 特定的信息。</param>
        /// <returns>对验证状态的引用。</returns>
        /// <exception cref="System.ArgumentNullException">httpContext 参数为 null。</exception>
        protected override HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
        {
            return base.OnCacheAuthorization(httpContext);
        }



        /// <summary>
        /// 重写时，提供一个入口点用于请求授权的验证状态。
        /// 默认情况下该方法会调用 <seealso cref="OnCacheAuthorization(HttpContextBase)"/> 方法中执行的缓存封装操作，所以不建议重写该方法。
        /// </summary>
        /// <param name="context">HTTP 上下文，它封装有关单个 HTTP 请求的所有 HTTP 特定的信息。</param>
        /// <param name="data"></param>
        /// <param name="validationStatus">返回的验证状态。</param>
        protected virtual void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = this.OnCacheAuthorization(new HttpContextWrapper(context));
        }


    }
}
