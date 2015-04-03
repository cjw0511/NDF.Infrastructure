using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Optimization;

namespace NDF.Web.Optimization
{
    /// <summary>
    /// 提供一组工具方法，用于快速将通过 <see cref="NDF.Web.Optimization.BundleCollectionExtensions"/> 绑定的 <see cref="System.Web.Optimization.Bundle"/> 相关 WEB 资源引用发送至 ASP.NET MVC 视图页面。
    /// </summary>
    public static class Bundles
    {
        /// <summary>
        /// 将 <paramref name="key"/> 索引键值所指定的一个/组 WEB 资源文件引用绑定渲染成一组可适用
        /// 于 ASP.NET MVC 视图页面的 HTML 标记。
        /// </summary>
        /// <param name="key">用于查找要渲染 WEB 资源文件引用绑定集合虚拟路径的索引键值。</param>
        /// <returns>一组可适用于 ASP.NET MVC 视图页面的 HTML 标记 <see cref="System.Web.IHtmlString"/>。</returns>
        public static IHtmlString Render(object key)
        {
            return RenderFormat(null, key);
        }

        /// <summary>
        /// 以参数 <paramref name="tagFormat"/> 作为格式化字符串将 <paramref name="key"/> 索引键值所指定
        /// 的一个/组 WEB 资源文件引用绑定渲染成一组可适用于 ASP.NET MVC 视图页面的 HTML 标记。
        /// </summary>
        /// <param name="tagFormat">用于定义渲染生成 HTML 标记格式的格式化字符串。</param>
        /// <param name="key">用于查找要渲染 WEB 资源文件引用绑定集合虚拟路径的索引键值。</param>
        /// <returns>一组可适用于 ASP.NET MVC 视图页面的 HTML 标记 <see cref="System.Web.IHtmlString"/>。</returns>
        public static IHtmlString RenderFormat(string tagFormat, object key)
        {
            Check.NotNull(key);
            List<IHtmlString> list = new List<IHtmlString>();

            if (BundleWrapperFactory.HasKey(key))
            {
                List<BundleWrapper> wrappers = BundleWrapperFactory.GetBundleWrapperList(key);
                foreach (BundleWrapper item in wrappers)
                {
                    string virtualPath = item.VirtualPath;
                    bool tagFormatIsEmpty = string.IsNullOrWhiteSpace(tagFormat);
                    if (item.Bundle is ScriptBundle)
                    {
                        list.Add(tagFormatIsEmpty ? Scripts.Render(virtualPath) : Scripts.RenderFormat(tagFormat, virtualPath));
                    }
                    else if (item.Bundle is StyleBundle)
                    {
                        list.Add(tagFormatIsEmpty ? Styles.Render(virtualPath) : Styles.RenderFormat(tagFormat, virtualPath));
                    }
                }
            }

            StringBuilder stringBuilder = new StringBuilder();
            foreach (IHtmlString item in list)
            {
                stringBuilder.AppendLine(item.ToHtmlString());
            }
            return new HtmlString(stringBuilder.ToString());
        }


        /// <summary>
        /// 如果传入的 <paramref name="virtualPath"/> 所示的虚拟路径已经绑定至 <see cref="System.Web.Optimization"/> 组件中，则
        /// 返回其绑定所使用的可访问虚拟路径，否则返回该 <paramref name="virtualPath"/> 解析后的 URL。
        /// </summary>
        /// <param name="virtualPath">一个表示 WEB 资源文件虚拟路径的字符串。</param>
        /// <returns>一个用来表示 URL 访问路径的 <see cref="System.Web.IHtmlString"/> 对象。</returns>
        public static IHtmlString Url(string virtualPath)
        {
            return BundleUtility.GetBundleType(virtualPath) == BundleType.JavaScript ? Scripts.Url(virtualPath) : Styles.Url(virtualPath);
        }



        /// <summary>
        /// 获取或设置一个 bool 值选项，表示是否启用 Web 资源的绑定压缩功能。
        /// 如果当前在 Web.config/App.config - appSettings 设置了 "BundleTable.EnableOptimizations"（注意大小写），则该属性
        ///     将取 appSettings 中指定的值，否则取默认值 false。
        /// </summary>
        public static bool EnableOptimizations
        {
            get
            {
                return ConfigurationManager.AppSettings["BundleTable.EnableOptimizations"].ToBoolean(false);
            }
        }


    }
}
