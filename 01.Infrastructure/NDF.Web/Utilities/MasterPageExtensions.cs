using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace NDF.Web.Utilities
{
    /// <summary>
    /// 提供一组对 ASP.NET 页面模板容器 <see cref="System.Web.UI.MasterPage"/> 操作方法的扩展。
    /// </summary>
    public static class MasterPageExtensions
    {
        /// <summary>
        /// 将 CSS 文件添加至 ASP.NET WebForm 页面 <see cref="System.Web.UI.Page"/> 的 Header 中。
        /// </summary>
        /// <param name="masterPage">ASP.NET WebForm 母版页面 <see cref="System.Web.UI.MasterPage"/> 的服务器对象引用。</param>
        /// <param name="path">需要添加的 CSS 文件的相对于 <paramref name="masterPage"/> 中所对应的 Page 属性页面的路径(也可以是相对于应用程序根目录的带~/开头的相对路径)，也就是其生成的页面标签的 href 属性。</param>
        public static void IncludeCss(this MasterPage masterPage, string path)
        {
            PageExtensions.IncludeCss(masterPage.Page, path);
        }

        /// <summary>
        /// 将 CSS 文件添加至 ASP.NET WebForm 页面 <see cref="System.Web.UI.Page"/> 的 Header 中。
        /// </summary>
        /// <param name="masterPage">ASP.NET WebForm 母版页面 <see cref="System.Web.UI.MasterPage"/> 的服务器对象引用。</param>
        /// <param name="path">需要添加的 CSS 文件的相对于 <paramref name="masterPage"/> 中所对应的 Page 属性页面的路径(也可以是相对于应用程序根目录的带~/开头的相对路径)，也就是其生成的页面标签的 href 属性。</param>
        /// <param name="id">需要添加的 CSS 文件的页面标签 ID 属性。</param>
        public static void IncludeCss(this MasterPage masterPage, string path, string id)
        {
            PageExtensions.IncludeCss(masterPage.Page, path, id);
        }



        /// <summary>
        /// 将 JavaScript 文件添加至 ASP.NET WebForm 页面 <see cref="System.Web.UI.Page"/> 的 Header 中。
        /// </summary>
        /// <param name="masterPage">ASP.NET WebForm 母版页面 <see cref="System.Web.UI.MasterPage"/> 的服务器对象引用。</param>
        /// <param name="path">需要添加的 JavaScript 文件的相对于 <paramref name="masterPage"/> 中所对应的 Page 属性页面的路径(也可以是相对于应用程序根目录的带~/开头的相对路径)，也就是其生成的页面标签的 href 属性。</param>
        public static void IncludeScript(this MasterPage masterPage, string path)
        {
            PageExtensions.IncludeScript(masterPage.Page, path);
        }

        /// <summary>
        /// 将 JavaScript 文件添加至 ASP.NET WebForm 页面 <see cref="System.Web.UI.Page"/> 的 Header 中。
        /// </summary>
        /// <param name="masterPage">ASP.NET WebForm 母版页面 <see cref="System.Web.UI.MasterPage"/> 的服务器对象引用。</param>
        /// <param name="path">需要添加的 JavaScript 文件的相对于 <paramref name="masterPage"/> 中所对应的 Page 属性页面的路径(也可以是相对于应用程序根目录的带~/开头的相对路径)，也就是其生成的页面标签的 href 属性。</param>
        /// <param name="id">需要添加的 JavaScript 文件的页面标签 ID 属性。</param>
        public static void IncludeScript(this MasterPage masterPage, string path, string id)
        {
            PageExtensions.IncludeScript(masterPage.Page, path, id);
        }
    }
}
