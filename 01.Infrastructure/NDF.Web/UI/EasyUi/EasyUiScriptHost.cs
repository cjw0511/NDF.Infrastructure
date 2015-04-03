using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace NDF.Web.UI.EasyUi
{
    /// <summary>
    /// 表示 <see cref="System.Web.UI.Page"/> 页面的客户端脚本输出对象，该对象提供一组常见的调用 JavaScript - jQuery EasyUI 脚本的方法。
    /// </summary>
    public class EasyUiScriptHost : ScriptHost
    {
        /// <summary>
        /// 以指定的 <see cref="System.Web.UI.Page"/> 对象作为 ASP.NET 服务器页面引用初始化 <see cref="NDF.Web.UI.EasyUi.EasyUiScriptHost"/> 对象。
        /// </summary>
        /// <param name="page">ASP.NET 服务器页面 <see cref="System.Web.UI.Page"/> 对象。</param>
        public EasyUiScriptHost(Page page) : base(page)
        { }



        /// <summary>
        /// 用指定的 ASP.NET 服务器页面对象 <see cref="System.Web.UI.Page"/> 创建一个 <see cref="NDF.Web.UI.EasyUi.EasyUiScriptHost"/>。
        /// </summary>
        /// <param name="page">指定的 ASP.NET 服务器页面对象 <see cref="System.Web.UI.Page"/>。</param>
        /// <returns>一个 <see cref="NDF.Web.UI.EasyUi.EasyUiScriptHost"/>  对象。</returns>
        public static new EasyUiScriptHost Create(Page page)
        {
            return new EasyUiScriptHost(page);
        }
    }
}
