using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Web
{
    /// <summary>
    /// <para>
    /// 表示一个特性，该特性用于标记在授权期间要跳过指定或所有的 MVC 权限过滤器 <see cref="NDF.Web.Mvc.AuthorizeAttribute"/> ，以及指定
    ///     的或所有的 WebAPI 权限过滤器 <see cref="NDF.Web.Http.HttpAuthorizeAttribute"/>。
    /// </para>
    /// <para>---</para>
    /// <para>
    /// 该特性类似于 <see cref="System.Web.Mvc.AllowAnonymousAttribute"/> 或 <see cref="System.Web.Http.AllowAnonymousAttribute"/>，但不同之处在于：
    ///     <see cref="System.Web.Mvc.AllowAnonymousAttribute"/> 指定 MVC 的 Controller/Action 跳过所有的 <see cref="System.Web.Mvc.AuthorizeAttribute"/> 和所有的 <see cref="NDF.Web.Mvc.AuthorizeAttribute"/> 验证；
    ///     <see cref="System.Web.Http.AllowAnonymousAttribute"/> 指定 WebAPI 的 Controller/Action 跳过所有的 <see cref="System.Web.Http.AuthorizeAttribute"/> 和所有的  <see cref="NDF.Web.Http.HttpAuthorizeAttribute"/> 验证；
    /// </para>
    /// <para>---</para>
    /// <para>
    /// 而特性 <see cref="NDF.Web.AuthorizeAttribute"/> 可以用于指定 MVC 的 Controller/Action 跳过所有的、或指定的 <see cref="System.Web.Mvc.AuthorizeAttribute"/>、<see cref="NDF.Web.Mvc.AuthorizeAttribute"/>；
    ///     也可以用于指定 WebAPI 的 Controller/Action 跳过所有的、或指定的 <see cref="System.Web.Http.AuthorizeAttribute"/>、<see cref="NDF.Web.Http.HttpAuthorizeAttribute"/>；
    /// </para>      
    /// <para>---</para>  
    /// <para>
    /// 当在 MVC 或 WebAPI 的 Controller/Action 上指定该特性类型用于跳过权限验证过滤器时，如果不指定参数，则表示跳过所有的 <see cref="NDF.Web.Mvc.AuthorizeAttribute"/> 和 <see cref="NDF.Web.Http.HttpAuthorizeAttribute"/> 权限验证过滤器。
    ///     在 MVC 或 WebAPI 的 Controller/Action 上标记该特性类时，可以将一个或多个的 <see cref="NDF.Web.Mvc.AuthorizeAttribute"/> 或 <see cref="NDF.Web.Http.HttpAuthorizeAttribute"/> 类型对象 <see cref="System.Type"/>（例如 typeof(<see cref="NDF.Web.Http.HttpAuthorizeAttribute"/>)）作为
    ///     参数传入，则表示仅跳过指定的权限验证过滤器。
    /// </para>
    /// </summary>
    /// <remarks>
    /// 注意：要使该标记该特定的 Controller 或 Action 能够有效跳过验证，被添加至 GlobalFilters.Filters" 的所有 AuthorizeAttribute 特性必须
    ///     继承于类型 <see cref="NDF.Web.Mvc.AuthorizeAttribute"/>。
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AllowAuthorizeAttribute : Attribute
    {
        private Type[] _allowTypes;


        /// <summary>
        /// 初始化类型 <see cref="AllowAuthorizeAttribute"/> 的新实例。
        /// </summary>
        public AllowAuthorizeAttribute() { }

        /// <summary>
        /// 以指定的 <see cref="System.Type"/> 集合作为可绕过的 <see cref="NDF.Web.Mvc.AuthorizeAttribute"/> 验证类型初始化类型 <see cref="NDF.Web.Mvc.AllowAuthorizeAttribute"/> 的新实例。
        /// </summary>
        /// <param name="allowTypes">
        /// 一组 <see cref="System.Type"/>，其中每个元素都必须是 <see cref="NDF.Web.Mvc.AuthorizeAttribute"/> 或其子类；
        /// 表示 Controller 或 Action 可绕过的 <see cref="NDF.Web.Mvc.AuthorizeAttribute"/> 验证服务。
        /// </param>
        public AllowAuthorizeAttribute(IEnumerable<Type> allowTypes)
        {
            Check.NotEmpty(allowTypes);
            this._allowTypes = allowTypes.ToArray();
        }

        /// <summary>
        /// 以指定的 <see cref="System.Type"/> 集合作为可绕过的 <see cref="NDF.Web.Mvc.AuthorizeAttribute"/> 验证类型初始化类型 <see cref="AllowAuthorizeAttribute"/> 的新实例。
        /// </summary>
        /// <param name="allowTypes">
        /// 一组 <see cref="System.Type"/>，其中每个元素都必须是 <see cref="NDF.Web.Mvc.AuthorizeAttribute"/> 或其子类；
        /// 表示 Controller 或 Action 可绕过的 <see cref="NDF.Web.Mvc.AuthorizeAttribute"/> 验证服务。
        /// </param>
        public AllowAuthorizeAttribute(params Type[] allowTypes) : this(allowTypes.AsEnumerable())
        {
        }



        /// <summary>
        /// 获取一个 <see cref="List&lt;T&gt;"/> 列表集合属性。该集合中的
        ///     每个 <see cref="System.Type"/> 元素都是 <see cref="AuthorizeAttribute"/> 类型或其子类，表示一个可以
        ///     在 Controller 或 Action 中被绕过的 <see cref="AuthorizeAttribute"/> 验证服务。
        /// </summary>
        public Type[] AllowTypes
        {
            get { return this._allowTypes ?? System.Linq.Enumerable.Empty<Type>().ToArray(); }
        }


        /// <summary>
        /// 验证当前特性的 <seealso cref="AllowTypes"/> 属性中是否包含参数 <paramref name="attributeType"/> 指定的特性类型定义。
        /// </summary>
        /// <param name="attributeType"></param>
        /// <returns></returns>
        protected internal bool Validate(Type attributeType)
        {
            Type[] types = this.AllowTypes;
            return types.IsNullOrEmpty() || types.Any(type => type == attributeType);
        }


    }
}
