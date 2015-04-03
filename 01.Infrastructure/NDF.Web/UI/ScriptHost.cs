using NDF.Utilities;
using NDF.Web.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace NDF.Web.UI
{
    /// <summary>
    /// 表示 <see cref="System.Web.UI.Page"/> 页面的客户端脚本输出对象，该对象提供一组常见的调用 JavaScript 脚本的方法。
    /// </summary>
    public class ScriptHost : Disposable
    {
        private string _guid;


        /// <summary>
        /// 以指定的 <see cref="System.Web.UI.Page"/> 对象作为 ASP.NET 服务器页面引用初始化 <see cref="NDF.Web.UI.ScriptHost"/> 对象。
        /// </summary>
        /// <param name="page">ASP.NET 服务器页面 <see cref="System.Web.UI.Page"/> 对象。</param>
        public ScriptHost(Page page)
        {
            this.Page = page;
        }

        /// <summary>
        /// 获取 <see cref="NDF.Web.UI.ScriptHost"/> 对象引用的 <see cref="System.Web.UI.Page"/> 服务器页面。
        /// </summary>
        public Page Page
        {
            get;
            private set;
        }


        /// <summary>
        /// 获取 <see cref="NDF.Web.UI.ScriptHost"/> 对象的全局唯一标识符。
        /// </summary>
        public string Guid
        {
            get
            {
                if (this._guid == null)
                {
                    this._guid = System.Guid.NewGuid().ToString();
                }
                return this._guid;
            }
        }

        /// <summary>
        /// 获取用于管理脚本、注册脚本和向页添加脚本的 <see cref="System.Web.UI.ClientScriptManager"/> 对象。
        /// </summary>
        public ClientScriptManager ClientScript
        {
            get
            {
                return this.Page.ClientScript;
            }
        }






        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式让页面弹出(提示/警告类型)消息对话框。
        /// 该脚本将会被添加至 HTML-BODY 的结束位置。
        /// </summary>
        /// <param name="message">表示弹出(提示/警告类型)的消息对话框中的消息文本。</param>
        public virtual void Alert(string message)
        {
            this.ClientScript.Alert(message);
        }

        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式让页面弹出(提示/警告类型)消息对话框。
        /// 该脚本将会被添加至 HTML-BODY 的结束位置。
        /// </summary>
        /// <param name="message">表示弹出(提示/警告类型)的消息对话框中的消息文本。</param>
        /// <param name="startupScript">指示是否将脚本添加为启动脚本。如果该参数为 false，则脚本将会被添加至 HTML-BODY 的起始位置，否则脚本将会呗添加至 HTML-BODY 的结束位置。</param>
        public virtual void Alert(string message, bool startupScript)
        {
            this.ClientScript.Alert(message, startupScript);
        }

        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式让页面弹出(提示/警告类型)消息对话框，并让指定的 HTML 标签控件获取焦点。
        /// 该脚本将会被添加至 HTML-BODY 的结束位置。
        /// </summary>
        /// <param name="message">表示弹出(提示/警告类型)的消息对话框中的消息文本。</param>
        /// <param name="controlId">表示弹出(提示/警告类型)的消息对话框在点击确定后，需要自动获取焦点的 HTML 标签控件的 ID 属性值。</param>
        public virtual void Alert(string message, string controlId)
        {
            this.ClientScript.Alert(message, controlId);
        }

        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式让页面弹出(提示/警告类型)消息对话框，并让指定的 HTML 标签控件获取焦点。
        /// </summary>
        /// <param name="message">表示弹出(提示/警告类型)的消息对话框中的消息文本。</param>
        /// <param name="controlId">表示弹出(提示/警告类型)的消息对话框在点击确定后，需要自动获取焦点的 HTML 标签控件的 ID 属性值。</param>
        /// <param name="startupScript">指示是否将脚本添加为启动脚本。如果该参数为 false，则脚本将会被添加至 HTML-BODY 的起始位置，否则脚本将会呗添加至 HTML-BODY 的结束位置。</param>
        public virtual void Alert(string message, string controlId, bool startupScript)
        {
            this.ClientScript.Alert(message, controlId, startupScript);
        }



        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式让页面重定向到一个新的 URL 地址。
        /// 该脚本将会被添加至 HTML-BODY 的结束位置。
        /// </summary>
        /// <param name="url">表示被重定向的目的页面的 URL。</param>
        public virtual void Redirect(string url)
        {
            this.ClientScript.Redirect(url);
        }

        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式让页面重定向到一个新的 URL 地址。
        /// 该脚本将会被添加至 HTML-BODY 的结束位置。
        /// </summary>
        /// <param name="url">表示被重定向的目的页面的 URL。</param>
        /// <param name="startupScript">指示是否将脚本添加为启动脚本。如果该参数为 false，则脚本将会被添加至 HTML-BODY 的起始位置，否则脚本将会呗添加至 HTML-BODY 的结束位置。</param>
        public virtual void Redirect(string url, bool startupScript)
        {
            this.ClientScript.Redirect(url, startupScript);
        }

        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式让页面重定向到一个新的 URL 地址。
        /// 该脚本将会被添加至 HTML-BODY 的结束位置。
        /// </summary>
        /// <param name="url">表示被重定向的目的页面的 URL。</param>
        /// <param name="parentLevel">
        /// 指示是否重定向当前页面的父级页面。
        /// 如果该参数为 0 则表示重定向当前页面，如果为 1 则表示重定向父级页面，如果为 2 则表示重定向父级页面的父级页面，以此类推。
        /// </param>
        public virtual void Redirect(string url, int parentLevel)
        {
            this.ClientScript.Redirect(url, parentLevel);
        }

        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式让页面重定向到一个新的 URL 地址。
        /// 该脚本将会被添加至 HTML-BODY 的结束位置。
        /// </summary>
        /// <param name="url">表示被重定向的目的页面的 URL。</param>
        /// <param name="isParentFrame">指示是否重定向当前页面的父级页面。</param>
        /// <param name="startupScript">指示是否将脚本添加为启动脚本。如果该参数为 false，则脚本将会被添加至 HTML-BODY 的起始位置，否则脚本将会呗添加至 HTML-BODY 的结束位置。</param>
        public virtual void Redirect(string url, bool isParentFrame, bool startupScript)
        {
            this.ClientScript.Redirect(url, isParentFrame, startupScript);
        }

        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式让页面重定向到一个新的 URL 地址。
        /// </summary>
        /// <param name="url">表示被重定向的目的页面的 URL。</param>
        /// <param name="parentLevel">
        /// 指示是否重定向当前页面的父级页面。
        /// 如果该参数为 0 则表示重定向当前页面，如果为 1 则表示重定向父级页面，如果为 2 则表示重定向父级页面的父级页面，以此类推。
        /// </param>
        /// <param name="startupScript">指示是否将脚本添加为启动脚本。如果该参数为 false，则脚本将会被添加至 HTML-BODY 的起始位置，否则脚本将会呗添加至 HTML-BODY 的结束位置。</param>
        public virtual void Redirect(string url, int parentLevel, bool startupScript)
        {
            this.ClientScript.Redirect(url, parentLevel, startupScript);
        }



        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式来关闭浏览器窗口。
        /// 该脚本将会被添加至 HTML-BODY 的结束位置。
        /// </summary>
        public virtual void CloseWindow()
        {
            this.ClientScript.CloseWindow();
        }

        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式来关闭浏览器窗口。
        /// 该脚本将会被添加至 HTML-BODY 的结束位置。
        /// </summary>
        /// <param name="withNoConfirm">该参数指示当在浏览器客户端执行关闭窗口的 Javascript 代码时，是否禁止弹出确认提示框。</param>
        public virtual void CloseWindow(bool withNoConfirm)
        {
            this.ClientScript.CloseWindow(withNoConfirm);
        }

        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式来关闭浏览器窗口。
        /// </summary>
        /// <param name="withNoConfirm">该参数指示当在浏览器客户端执行关闭窗口的 Javascript 代码时，是否禁止弹出确认提示框。</param>
        /// <param name="startupScript">指示是否将脚本添加为启动脚本。如果该参数为 false，则脚本将会被添加至 HTML-BODY 的起始位置，否则脚本将会呗添加至 HTML-BODY 的结束位置。</param>
        public virtual void CloseWindow(bool withNoConfirm, bool startupScript)
        {
            this.ClientScript.CloseWindow(withNoConfirm, startupScript);
        }



        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式使浏览器从浏览历史列表中加载指定位置的 URL 记录。
        /// 该脚本将会被添加至 HTML-BODY 的结束位置。
        /// </summary>
        /// <param name="vLocation">要从浏览历史列表中加载的 URL 记录的索引位置。同浏览器 JavaScript 脚本中 window.history.go 方法中的参数 vLocation。</param>
        public virtual void HistoryGo(int vLocation)
        {
            this.ClientScript.HistoryGo(vLocation);
        }

        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式使浏览器从浏览历史列表中加载指定位置的 URL 记录。
        /// </summary>
        /// <param name="vLocation">要从浏览历史列表中加载的 URL 记录的索引位置。同浏览器 JavaScript 脚本中 window.history.go 方法中的参数 vLocation。</param>
        /// <param name="startupScript">指示是否将脚本添加为启动脚本。如果该参数为 false，则脚本将会被添加至 HTML-BODY 的起始位置，否则脚本将会呗添加至 HTML-BODY 的结束位置。</param>
        public virtual void HistoryGo(int vLocation, bool startupScript)
        {
            this.ClientScript.HistoryGo(vLocation, startupScript);
        }



        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式使浏览器从浏览历史列表中加载上一条 URL 记录。
        /// 该脚本将会被添加至 HTML-BODY 的结束位置。
        /// </summary>
        public virtual void HistoryBack()
        {
            this.ClientScript.HistoryBack();
        }

        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式使浏览器从浏览历史列表中加载上一条 URL 记录。
        /// </summary>
        /// <param name="startupScript">指示是否将脚本添加为启动脚本。如果该参数为 false，则脚本将会被添加至 HTML-BODY 的起始位置，否则脚本将会呗添加至 HTML-BODY 的结束位置。</param>
        public virtual void HistoryBack(bool startupScript)
        {
            this.ClientScript.HistoryBack(startupScript);
        }



        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式使浏览器从浏览历史列表中加载下一条 URL 记录。
        /// 该脚本将会被添加至 HTML-BODY 的结束位置。
        /// </summary>
        public virtual void HistoryForward()
        {
            this.ClientScript.HistoryForward();
        }

        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式使浏览器从浏览历史列表中加载下一条 URL 记录。
        /// </summary>
        /// <param name="startupScript">指示是否将脚本添加为启动脚本。如果该参数为 false，则脚本将会被添加至 HTML-BODY 的起始位置，否则脚本将会呗添加至 HTML-BODY 的结束位置。</param>
        public virtual void HistoryForward(bool startupScript)
        {
            this.ClientScript.HistoryForward(startupScript);
        }




        /// <summary>
        /// 向 <see cref="System.Web.UI.Page"/> 对象注册脚本。
        /// </summary>
        /// <param name="script">要注册的脚本文本。</param>
        public virtual void RegisterClientScript(string script)
        {
            this.ClientScript.RegisterClientScript(script);
        }

        /// <summary>
        /// 向 <see cref="System.Web.UI.Page"/> 对象注册脚本。
        /// </summary>
        /// <param name="script">要注册的脚本文本。</param>
        /// <param name="startupScript">指示是否将脚本添加为启动脚本。如果该参数为 false，则脚本将会被添加至 HTML-BODY 的起始位置，否则脚本将会呗添加至 HTML-BODY 的结束位置。</param>
        public virtual void RegisterClientScript(string script, bool startupScript)
        {
            this.ClientScript.RegisterClientScript(script, startupScript);
        }

        /// <summary>
        /// 向 <see cref="System.Web.UI.Page"/> 对象注册脚本。
        /// </summary>
        /// <param name="script">要注册的脚本文本。</param>
        /// <param name="addScriptTags">指示是否添加脚本标记的布尔值。</param>
        /// <param name="startupScript">指示是否将脚本添加为启动脚本。如果该参数为 false，则脚本将会被添加至 HTML-BODY 的起始位置，否则脚本将会呗添加至 HTML-BODY 的结束位置。</param>
        public virtual void RegisterClientScript(string script, bool addScriptTags, bool startupScript)
        {
            this.ClientScript.RegisterClientScript(script, addScriptTags, startupScript);
        }

        /// <summary>
        /// 使用类型、键和脚本文本向 <see cref="System.Web.UI.Page"/> 对象注册脚本。
        /// </summary>
        /// <param name="type">要注册的脚本的类型。</param>
        /// <param name="key">要注册的脚本的键。</param>
        /// <param name="script">要注册的脚本文本。</param>
        public virtual void RegisterClientScript(Type type, string key, string script)
        {
            this.ClientScript.RegisterClientScript(type, key, script);
        }

        /// <summary>
        /// 使用类型、键和脚本文本向 <see cref="System.Web.UI.Page"/> 对象注册脚本。
        /// </summary>
        /// <param name="type">要注册的脚本的类型。</param>
        /// <param name="key">要注册的脚本的键。</param>
        /// <param name="script">要注册的脚本文本。</param>
        /// <param name="startupScript">指示是否将脚本添加为启动脚本。如果该参数为 false，则脚本将会被添加至 HTML-BODY 的起始位置，否则脚本将会呗添加至 HTML-BODY 的结束位置。</param>
        public virtual void RegisterClientScript(Type type, string key, string script, bool startupScript)
        {
            this.ClientScript.RegisterClientScript(type, key, script, startupScript);
        }

        /// <summary>
        /// 使用类型、键、脚本文本和指示是否添加脚本标记的布尔值向 <see cref="System.Web.UI.Page"/> 对象注册脚本。
        /// </summary>
        /// <param name="type">要注册的脚本的类型。</param>
        /// <param name="key">要注册的脚本的键。</param>
        /// <param name="script">要注册的脚本文本。</param>
        /// <param name="addScriptTags">指示是否添加脚本标记的布尔值。</param>
        /// <param name="startupScript">指示是否将脚本添加为启动脚本。如果该参数为 false，则脚本将会被添加至 HTML-BODY 的起始位置，否则脚本将会呗添加至 HTML-BODY 的结束位置。</param>
        public virtual void RegisterClientScript(Type type, string key, string script, bool addScriptTags, bool startupScript)
        {
            this.ClientScript.RegisterClientScript(type, key, script, addScriptTags, startupScript);
        }




        /// <summary>
        /// 用指定的 ASP.NET 服务器页面对象 <see cref="System.Web.UI.Page"/> 创建一个 <see cref="NDF.Web.UI.ScriptHost"/>。
        /// </summary>
        /// <param name="page">指定的 ASP.NET 服务器页面对象 <see cref="System.Web.UI.Page"/>。</param>
        /// <returns>一个 <see cref="NDF.Web.UI.ScriptHost"/>  对象。</returns>
        public static ScriptHost Create(Page page)
        {
            return new ScriptHost(page);
        }
    }
}
