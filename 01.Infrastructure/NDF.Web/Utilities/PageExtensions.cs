using NDF.Utilities;
using NDF.Web.UI;
using NDF.Web.UI.EasyUi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace NDF.Web.Utilities
{
    /// <summary>
    /// 提供一组对 ASP.NET 页面 <see cref="System.Web.UI.Page"/> 操作方法的扩展。
    /// </summary>
    public static class PageExtensions
    {

        /// <summary>
        /// 将 CSS 文件添加至 ASP.NET WebForm 页面 <see cref="System.Web.UI.Page"/> 的 Header 中。
        /// </summary>
        /// <param name="page">ASP.NET WebForm 页面 <see cref="System.Web.UI.Page"/> 的服务器对象引用。</param>
        /// <param name="path">需要添加的 CSS 文件的相对于 <paramref name="page"/> 页面的路径(也可以是相对于应用程序根目录的带~/开头的相对路径)，也就是其生成的页面标签的 href 属性。</param>
        public static void IncludeCss(this Page page, string path)
        {
            IncludeCss(page, path, null);
        }

        /// <summary>
        /// 将 CSS 文件添加至 ASP.NET WebForm 页面 <see cref="System.Web.UI.Page"/> 的 Header 中。
        /// </summary>
        /// <param name="page">ASP.NET WebForm 页面 <see cref="System.Web.UI.Page"/> 的服务器对象引用。</param>
        /// <param name="path">需要添加的 CSS 文件的相对于 <paramref name="page"/> 页面的路径(也可以是相对于应用程序根目录的带~/开头的相对路径)，也就是其生成的页面标签的 href 属性。</param>
        /// <param name="id">需要添加的 CSS 文件的页面标签 ID 属性。</param>
        public static void IncludeCss(this Page page, string path, string id)
        {
            Check.NotNull(page);
            Check.NotNull(path);
            HtmlLink tag = new HtmlLink();
            tag.Attributes.Add("rel", "stylesheet");
            tag.Attributes.Add("type", "text/css");
            tag.Attributes.Add("href", page.ResolveClientUrl(path));
            if (!string.IsNullOrWhiteSpace(id))
            {
                tag.Attributes.Add("id", id);
            }
            page.Header.Controls.Add(tag);
        }



        /// <summary>
        /// 将 JavaScript 文件添加至 ASP.NET WebForm 页面 <see cref="System.Web.UI.Page"/> 的 Header 中。
        /// </summary>
        /// <param name="page">ASP.NET WebForm 页面 <see cref="System.Web.UI.Page"/> 的服务器对象引用。</param>
        /// <param name="path">需要添加的 JavaScript 文件的相对于 <paramref name="page"/> 页面的路径(也可以是相对于应用程序根目录的带~/开头的相对路径)，也就是其生成的页面标签的 href 属性。</param>
        public static void IncludeScript(this Page page, string path)
        {
            IncludeScript(page, path, null);
        }

        /// <summary>
        /// 将 JavaScript 文件添加至 ASP.NET WebForm 页面 <see cref="System.Web.UI.Page"/> 的 Header 中。
        /// </summary>
        /// <param name="page">ASP.NET WebForm 页面 <see cref="System.Web.UI.Page"/> 的服务器对象引用。</param>
        /// <param name="path">需要添加的 JavaScript 文件的相对于 <paramref name="page"/> 页面的路径(也可以是相对于应用程序根目录的带~/开头的相对路径)，也就是其生成的页面标签的 href 属性。</param>
        /// <param name="id">需要添加的 JavaScript 文件的页面标签 ID 属性。</param>
        public static void IncludeScript(this Page page, string path, string id)
        {
            Check.NotNull(page);
            Check.NotNull(path);
            HtmlGenericControl tag = new HtmlGenericControl("script");
            tag.Attributes.Add("type", "text/javascript");
            tag.Attributes.Add("src", page.ResolveClientUrl(path));
            if (!string.IsNullOrWhiteSpace(id))
            {
                tag.Attributes.Add("id", id);
            }
            page.Header.Controls.Add(tag);
        }




        /// <summary>
        /// 获取当前页面 <see cref="System.Web.UI.Page"/> 的 <see cref="NDF.Web.UI.ScriptHost"/> 对象。 
        /// </summary>
        /// <param name="page">指定的 ASP.NET 服务器页面对象 <see cref="System.Web.UI.Page"/>。</param>
        /// <returns>返回一个新创建的 <see cref="NDF.Web.UI.ScriptHost"/> 对象。</returns>
        public static ScriptHost GetScriptHost(this Page page)
        {
            Check.NotNull(page);
            return ScriptHost.Create(page);
        }

        /// <summary>
        /// 获取当前页面 <see cref="System.Web.UI.Page"/> 的 <see cref="NDF.Web.UI.EasyUi.EasyUiScriptHost"/> 对象。 
        /// </summary>
        /// <param name="page">指定的 ASP.NET 服务器页面对象 <see cref="System.Web.UI.Page"/>。</param>
        /// <returns>返回一个新创建的 <see cref="NDF.Web.UI.EasyUi.EasyUiScriptHost"/> 对象。</returns>
        public static EasyUiScriptHost GetEasyUiScriptHost(this Page page)
        {
            return EasyUiScriptHost.Create(page);
        }
    }
}
