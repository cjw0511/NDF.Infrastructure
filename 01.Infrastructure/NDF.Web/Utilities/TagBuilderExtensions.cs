using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NDF.Web.Utilities
{
    /// <summary>
    /// 提供一组对 ASP.NET MVC 中关于 HTML 标签创建帮助器对象 <see cref="System.Web.Mvc.TagBuilder"/> 操作方法的扩展。
    /// </summary>
    public static class TagBuilderExtensions
    {
        /// <summary>
        /// 向 HTML 标签创建帮助器对象 <see cref="System.Web.Mvc.TagBuilder"/> 中的 CSS 类列表添加 CSS 类。
        /// </summary>
        /// <param name="builder">被操作的 <see cref="System.Web.Mvc.TagBuilder"/> 对象。</param>
        /// <param name="cssClass">要添加的 CSS 类，如果 css class 可以用空格隔开。</param>
        /// <param name="replaceExisting">
        /// 如果为 true，则无论原 <see cref="System.Web.Mvc.TagBuilder"/> 对象是否存在 cssClass 参数所示的 CSS 类，都将会把 cssClass 添加至其 HTML 标签 class 属性上；
        /// 如果为 false，则仅在原 <see cref="System.Web.Mvc.TagBuilder"/> 对象不存在 cssCLass 参数所示的 CSS 类时，才把 cssClass 添加至其 HTML 标签 class 属性上。
        /// </param>
        public static void AddCssClass(this TagBuilder builder, string cssClass, bool replaceExisting)
        {
            if (!string.IsNullOrWhiteSpace(cssClass))
            {
                var array = cssClass.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var cls in array)
                {
                    if (!replaceExisting || !builder.HasClass(cls))
                    {
                        builder.AddCssClass(cls);
                    }
                }
            }
        }

        /// <summary>
        /// 向 HTML 标签创建帮助器对象 <see cref="System.Web.Mvc.TagBuilder"/> 中的 CSS 类列表添加一个或多个 CSS 类。
        /// </summary>
        /// <param name="builder">被操作的 <see cref="System.Web.Mvc.TagBuilder"/> 对象。</param>
        /// <param name="cssClass">要添加的 CSS 类。</param>
        /// <param name="replaceExisting">
        /// 如果为 true，则无论原 <see cref="System.Web.Mvc.TagBuilder"/> 对象是否存在 cssClass 参数所示的 CSS 类，都将会把 cssClass 添加至其 HTML 标签 class 属性上；
        /// 如果为 false，则仅在原 <see cref="System.Web.Mvc.TagBuilder"/> 对象不存在 cssCLass 参数所示的 CSS 类时，才把 cssClass 添加至其 HTML 标签 class 属性上。
        /// 可选参数，默认为 true。
        /// </param>
        public static void AddCssClass(this TagBuilder builder, string[] cssClass, bool replaceExisting = true)
        {
            foreach (var cls in cssClass)
            {
                builder.AddCssClass(cls, replaceExisting);
            }
        }



        /// <summary>
        /// 判断 HTML 标签创建帮助器对象 <see cref="System.Web.Mvc.TagBuilder"/> 中是否已经存在指定的 CSS 类。
        /// </summary>
        /// <param name="builder">被操作的 <see cref="System.Web.Mvc.TagBuilder"/> 对象。</param>
        /// <param name="cssClss">要添加的 CSS 类。</param>
        /// <returns>如果标记中已经存在指定的 CSS 类，则返回 true，否则返回 false。</returns>
        public static bool HasClass(this TagBuilder builder, string cssClss)
        {
            return builder.GetClass().Contains(cssClss, StringComparer.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// 获取 HTML 标签创建帮助器对象 <see cref="System.Web.Mvc.TagBuilder"/> 中所有的 CSS 类名。
        /// </summary>
        /// <param name="builder">被操作的 <see cref="System.Web.Mvc.TagBuilder"/> 对象。</param>
        /// <returns>返回当前 <see cref="System.Web.Mvc.TagBuilder"/> 对象中所有 CSS 类型所构成的一个 string 数组。</returns>
        public static string[] GetClass(this TagBuilder builder)
        {
            List<string> list = new List<string>();
            string cls = null;
            if (builder.Attributes.TryGetValue("class", out cls) && !string.IsNullOrWhiteSpace(cls))
            {
                var array = cls.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                list.AddRange(array);
            }
            return list.Distinct().ToArray();
        }
    }
}
