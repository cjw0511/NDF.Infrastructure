using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace NDF.Web.Utilities
{
    /// <summary>
    /// 提供一组对 ASP.NET 页面客户端脚本管理器 <see cref="System.Web.UI.ClientScriptManager"/> 操作方法的扩展。
    /// </summary>
    public static class ClientScriptManagerExtensions
    {
        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式让页面弹出(提示/警告类型)消息对话框。
        /// 该脚本将会被添加至 HTML-BODY 的结束位置。
        /// </summary>
        /// <param name="clientScript">用于注册脚本的 <see cref="System.Web.UI.ClientScriptManager"/> 对象。</param>
        /// <param name="message">表示弹出(提示/警告类型)的消息对话框中的消息文本。</param>
        public static void Alert(this ClientScriptManager clientScript, string message)
        {
            Alert(clientScript, message, null, true);
        }

        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式让页面弹出(提示/警告类型)消息对话框。
        /// 该脚本将会被添加至 HTML-BODY 的结束位置。
        /// </summary>
        /// <param name="clientScript">用于注册脚本的 <see cref="System.Web.UI.ClientScriptManager"/> 对象。</param>
        /// <param name="message">表示弹出(提示/警告类型)的消息对话框中的消息文本。</param>
        /// <param name="startupScript">指示是否将脚本添加为启动脚本。如果该参数为 false，则脚本将会被添加至 HTML-BODY 的起始位置，否则脚本将会呗添加至 HTML-BODY 的结束位置。</param>
        public static void Alert(this ClientScriptManager clientScript, string message, bool startupScript)
        {
            Alert(clientScript, message, null, startupScript);
        }

        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式让页面弹出(提示/警告类型)消息对话框，并让指定的 HTML 标签控件获取焦点。
        /// 该脚本将会被添加至 HTML-BODY 的结束位置。
        /// </summary>
        /// <param name="clientScript">用于注册脚本的 <see cref="System.Web.UI.ClientScriptManager"/> 对象。</param>
        /// <param name="message">表示弹出(提示/警告类型)的消息对话框中的消息文本。</param>
        /// <param name="controlId">表示弹出(提示/警告类型)的消息对话框在点击确定后，需要自动获取焦点的 HTML 标签控件的 ID 属性值。</param>
        public static void Alert(this ClientScriptManager clientScript, string message, string controlId)
        {
            Alert(clientScript, message, controlId, true);
        }

        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式让页面弹出(提示/警告类型)消息对话框，并让指定的 HTML 标签控件获取焦点。
        /// </summary>
        /// <param name="clientScript">用于注册脚本的 <see cref="System.Web.UI.ClientScriptManager"/> 对象。</param>
        /// <param name="message">表示弹出(提示/警告类型)的消息对话框中的消息文本。</param>
        /// <param name="controlId">表示弹出(提示/警告类型)的消息对话框在点击确定后，需要自动获取焦点的 HTML 标签控件的 ID 属性值。</param>
        /// <param name="startupScript">指示是否将脚本添加为启动脚本。如果该参数为 false，则脚本将会被添加至 HTML-BODY 的起始位置，否则脚本将会呗添加至 HTML-BODY 的结束位置。</param>
        public static void Alert(this ClientScriptManager clientScript, string message, string controlId, bool startupScript)
        {
            StringBuilder script = new StringBuilder();
            script.AppendFormat("alert(\"{0}\");", message);
            if (!string.IsNullOrWhiteSpace(controlId))
            {
                script.AppendLine().AppendFormat("var control = document.all ? document.all[\"{0}\"] : document.getElementById(\"{0}\");", controlId);
                script.AppendLine().Append("if (control && control.focus) {");
                script.AppendLine().Append("    control.focus();");
                script.AppendLine().Append("}");
            }
            RegisterClientScript(clientScript, script.ToString(), startupScript);
        }



        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式让页面重定向到一个新的 URL 地址。
        /// 该脚本将会被添加至 HTML-BODY 的结束位置。
        /// </summary>
        /// <param name="clientScript">用于注册脚本的 <see cref="System.Web.UI.ClientScriptManager"/> 对象。</param>
        /// <param name="url">表示被重定向的目的页面的 URL。</param>
        public static void Redirect(this ClientScriptManager clientScript, string url)
        {
            Redirect(clientScript, url, 0);
        }

        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式让页面重定向到一个新的 URL 地址。
        /// 该脚本将会被添加至 HTML-BODY 的结束位置。
        /// </summary>
        /// <param name="clientScript">用于注册脚本的 <see cref="System.Web.UI.ClientScriptManager"/> 对象。</param>
        /// <param name="url">表示被重定向的目的页面的 URL。</param>
        /// <param name="startupScript">指示是否将脚本添加为启动脚本。如果该参数为 false，则脚本将会被添加至 HTML-BODY 的起始位置，否则脚本将会呗添加至 HTML-BODY 的结束位置。</param>
        public static void Redirect(this ClientScriptManager clientScript, string url, bool startupScript)
        {
            Redirect(clientScript, url, 0, startupScript);
        }

        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式让页面重定向到一个新的 URL 地址。
        /// 该脚本将会被添加至 HTML-BODY 的结束位置。
        /// </summary>
        /// <param name="clientScript">用于注册脚本的 <see cref="System.Web.UI.ClientScriptManager"/> 对象。</param>
        /// <param name="url">表示被重定向的目的页面的 URL。</param>
        /// <param name="parentLevel">
        /// 指示是否重定向当前页面的父级页面。
        /// 如果该参数为 0 则表示重定向当前页面，如果为 1 则表示重定向父级页面，如果为 2 则表示重定向父级页面的父级页面，以此类推。
        /// </param>
        public static void Redirect(this ClientScriptManager clientScript, string url, int parentLevel)
        {
            Redirect(clientScript, url, parentLevel, true);
        }

        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式让页面重定向到一个新的 URL 地址。
        /// 该脚本将会被添加至 HTML-BODY 的结束位置。
        /// </summary>
        /// <param name="clientScript">用于注册脚本的 <see cref="System.Web.UI.ClientScriptManager"/> 对象。</param>
        /// <param name="url">表示被重定向的目的页面的 URL。</param>
        /// <param name="isParentFrame">指示是否重定向当前页面的父级页面。</param>
        /// <param name="startupScript">指示是否将脚本添加为启动脚本。如果该参数为 false，则脚本将会被添加至 HTML-BODY 的起始位置，否则脚本将会呗添加至 HTML-BODY 的结束位置。</param>
        public static void Redirect(this ClientScriptManager clientScript, string url, bool isParentFrame, bool startupScript)
        {
            Redirect(clientScript, url, isParentFrame ? 1 : 0, startupScript);
        }

        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式让页面重定向到一个新的 URL 地址。
        /// </summary>
        /// <param name="clientScript">用于注册脚本的 <see cref="System.Web.UI.ClientScriptManager"/> 对象。</param>
        /// <param name="url">表示被重定向的目的页面的 URL。</param>
        /// <param name="parentLevel">
        /// 指示是否重定向当前页面的父级页面。
        /// 如果该参数为 0 则表示重定向当前页面，如果为 1 则表示重定向父级页面，如果为 2 则表示重定向父级页面的父级页面，以此类推。
        /// </param>
        /// <param name="startupScript">指示是否将脚本添加为启动脚本。如果该参数为 false，则脚本将会被添加至 HTML-BODY 的起始位置，否则脚本将会呗添加至 HTML-BODY 的结束位置。</param>
        public static void Redirect(this ClientScriptManager clientScript, string url, int parentLevel, bool startupScript)
        {
            Check.NotEmpty(url, "url");
            StringBuilder script = new StringBuilder("window.");
            for (int i = 0; i < parentLevel; i++)
            {
                script.Append("parent.");
            }
            script.AppendFormat("location.href=\"{0}\";", url);
            RegisterClientScript(clientScript, script.ToString(), startupScript);
        }



        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式来关闭浏览器窗口。
        /// 该脚本将会被添加至 HTML-BODY 的结束位置。
        /// </summary>
        /// <param name="clientScript">用于注册脚本的 <see cref="System.Web.UI.ClientScriptManager"/> 对象。</param>
        public static void CloseWindow(this ClientScriptManager clientScript)
        {
            CloseWindow(clientScript, false);
        }

        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式来关闭浏览器窗口。
        /// 该脚本将会被添加至 HTML-BODY 的结束位置。
        /// </summary>
        /// <param name="clientScript">用于注册脚本的 <see cref="System.Web.UI.ClientScriptManager"/> 对象。</param>
        /// <param name="withNoConfirm">该参数指示当在浏览器客户端执行关闭窗口的 Javascript 代码时，是否禁止弹出确认提示框。</param>
        public static void CloseWindow(this ClientScriptManager clientScript, bool withNoConfirm)
        {
            CloseWindow(clientScript, withNoConfirm, true);
        }

        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式来关闭浏览器窗口。
        /// </summary>
        /// <param name="clientScript">用于注册脚本的 <see cref="System.Web.UI.ClientScriptManager"/> 对象。</param>
        /// <param name="withNoConfirm">该参数指示当在浏览器客户端执行关闭窗口的 Javascript 代码时，是否禁止弹出确认提示框。</param>
        /// <param name="startupScript">指示是否将脚本添加为启动脚本。如果该参数为 false，则脚本将会被添加至 HTML-BODY 的起始位置，否则脚本将会呗添加至 HTML-BODY 的结束位置。</param>
        public static void CloseWindow(this ClientScriptManager clientScript, bool withNoConfirm, bool startupScript)
        {
            StringBuilder script = new StringBuilder();
            if (withNoConfirm)
            {
                script.Append("window.opener = null;").AppendLine();
                script.Append("window.open(\"\", \"_self\", \"\");").AppendLine();
                script.Append("window.opener = null;").AppendLine();
            }
            script.Append("window.close();");
            RegisterClientScript(clientScript, script.ToString(), startupScript);
        }



        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式使浏览器从浏览历史列表中加载指定位置的 URL 记录。
        /// 该脚本将会被添加至 HTML-BODY 的结束位置。
        /// </summary>
        /// <param name="clientScript">用于注册脚本的 <see cref="System.Web.UI.ClientScriptManager"/> 对象。</param>
        /// <param name="vLocation">要从浏览历史列表中加载的 URL 记录的索引位置。同浏览器 JavaScript 脚本中 window.history.go 方法中的参数 vLocation。</param>
        public static void HistoryGo(this ClientScriptManager clientScript, int vLocation)
        {
            HistoryGo(clientScript, vLocation, true);
        }

        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式使浏览器从浏览历史列表中加载指定位置的 URL 记录。
        /// </summary>
        /// <param name="clientScript">用于注册脚本的 <see cref="System.Web.UI.ClientScriptManager"/> 对象。</param>
        /// <param name="vLocation">要从浏览历史列表中加载的 URL 记录的索引位置。同浏览器 JavaScript 脚本中 window.history.go 方法中的参数 vLocation。</param>
        /// <param name="startupScript">指示是否将脚本添加为启动脚本。如果该参数为 false，则脚本将会被添加至 HTML-BODY 的起始位置，否则脚本将会呗添加至 HTML-BODY 的结束位置。</param>
        public static void HistoryGo(this ClientScriptManager clientScript, int vLocation, bool startupScript)
        {
            string script = string.Format("window.history.go({0});", vLocation);
            RegisterClientScript(clientScript, script, startupScript);
        }



        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式使浏览器从浏览历史列表中加载上一条 URL 记录。
        /// 该脚本将会被添加至 HTML-BODY 的结束位置。
        /// </summary>
        /// <param name="clientScript">用于注册脚本的 <see cref="System.Web.UI.ClientScriptManager"/> 对象。</param>
        public static void HistoryBack(this ClientScriptManager clientScript)
        {
            HistoryBack(clientScript, true);
        }

        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式使浏览器从浏览历史列表中加载上一条 URL 记录。
        /// </summary>
        /// <param name="clientScript">用于注册脚本的 <see cref="System.Web.UI.ClientScriptManager"/> 对象。</param>
        /// <param name="startupScript">指示是否将脚本添加为启动脚本。如果该参数为 false，则脚本将会被添加至 HTML-BODY 的起始位置，否则脚本将会呗添加至 HTML-BODY 的结束位置。</param>
        public static void HistoryBack(this ClientScriptManager clientScript, bool startupScript)
        {
            RegisterClientScript(clientScript, "window.history.back();", startupScript);
        }


        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式使浏览器从浏览历史列表中加载下一条 URL 记录。
        /// 该脚本将会被添加至 HTML-BODY 的结束位置。
        /// </summary>
        /// <param name="clientScript">用于注册脚本的 <see cref="System.Web.UI.ClientScriptManager"/> 对象。</param>
        public static void HistoryForward(this ClientScriptManager clientScript)
        {
            HistoryForward(clientScript, true);
        }

        /// <summary>
        /// 通过向 <see cref="System.Web.UI.Page"/> 对象注册脚本的方式使浏览器从浏览历史列表中加载下一条 URL 记录。
        /// </summary>
        /// <param name="clientScript">用于注册脚本的 <see cref="System.Web.UI.ClientScriptManager"/> 对象。</param>
        /// <param name="startupScript">指示是否将脚本添加为启动脚本。如果该参数为 false，则脚本将会被添加至 HTML-BODY 的起始位置，否则脚本将会呗添加至 HTML-BODY 的结束位置。</param>
        public static void HistoryForward(this ClientScriptManager clientScript, bool startupScript)
        {
            RegisterClientScript(clientScript, "window.history.forward();", startupScript);
        }






        /// <summary>
        /// 向 <see cref="System.Web.UI.Page"/> 对象注册脚本。
        /// </summary>
        /// <param name="clientScript">用于注册脚本的 <see cref="System.Web.UI.ClientScriptManager"/> 对象。</param>
        /// <param name="script">要注册的脚本文本。</param>
        public static void RegisterClientScript(this ClientScriptManager clientScript, string script)
        {
            RegisterClientScript(clientScript, script, true, true);
        }

        /// <summary>
        /// 向 <see cref="System.Web.UI.Page"/> 对象注册脚本。
        /// </summary>
        /// <param name="clientScript">用于注册脚本的 <see cref="System.Web.UI.ClientScriptManager"/> 对象。</param>
        /// <param name="script">要注册的脚本文本。</param>
        /// <param name="startupScript">指示是否将脚本添加为启动脚本。如果该参数为 false，则脚本将会被添加至 HTML-BODY 的起始位置，否则脚本将会呗添加至 HTML-BODY 的结束位置。</param>
        public static void RegisterClientScript(this ClientScriptManager clientScript, string script, bool startupScript)
        {
            RegisterClientScript(clientScript, script, true, startupScript);
        }

        /// <summary>
        /// 向 <see cref="System.Web.UI.Page"/> 对象注册脚本。
        /// </summary>
        /// <param name="clientScript">用于注册脚本的 <see cref="System.Web.UI.ClientScriptManager"/> 对象。</param>
        /// <param name="script">要注册的脚本文本。</param>
        /// <param name="addScriptTags">指示是否添加脚本标记的布尔值。</param>
        /// <param name="startupScript">指示是否将脚本添加为启动脚本。如果该参数为 false，则脚本将会被添加至 HTML-BODY 的起始位置，否则脚本将会呗添加至 HTML-BODY 的结束位置。</param>
        public static void RegisterClientScript(this ClientScriptManager clientScript, string script, bool addScriptTags, bool startupScript)
        {
            RegisterClientScript(clientScript, clientScript.GetType(), Guid.NewGuid().ToString("N"), script, true, startupScript);
        }

        /// <summary>
        /// 使用类型、键和脚本文本向 <see cref="System.Web.UI.Page"/> 对象注册脚本。
        /// </summary>
        /// <param name="clientScript">用于注册脚本的 <see cref="System.Web.UI.ClientScriptManager"/> 对象。</param>
        /// <param name="type">要注册的脚本的类型。</param>
        /// <param name="key">要注册的脚本的键。</param>
        /// <param name="script">要注册的脚本文本。</param>
        public static void RegisterClientScript(this ClientScriptManager clientScript, Type type, string key, string script)
        {
            RegisterClientScript(clientScript, type, key, script, true);
        }

        /// <summary>
        /// 使用类型、键和脚本文本向 <see cref="System.Web.UI.Page"/> 对象注册脚本。
        /// </summary>
        /// <param name="clientScript">用于注册脚本的 <see cref="System.Web.UI.ClientScriptManager"/> 对象。</param>
        /// <param name="type">要注册的脚本的类型。</param>
        /// <param name="key">要注册的脚本的键。</param>
        /// <param name="script">要注册的脚本文本。</param>
        /// <param name="startupScript">指示是否将脚本添加为启动脚本。如果该参数为 false，则脚本将会被添加至 HTML-BODY 的起始位置，否则脚本将会呗添加至 HTML-BODY 的结束位置。</param>
        public static void RegisterClientScript(this ClientScriptManager clientScript, Type type, string key, string script, bool startupScript)
        {
            RegisterClientScript(clientScript, type, key, script, true, startupScript);
        }

        /// <summary>
        /// 使用类型、键、脚本文本和指示是否添加脚本标记的布尔值向 <see cref="System.Web.UI.Page"/> 对象注册脚本。
        /// </summary>
        /// <param name="clientScript">用于注册脚本的 <see cref="System.Web.UI.ClientScriptManager"/> 对象。</param>
        /// <param name="type">要注册的脚本的类型。</param>
        /// <param name="key">要注册的脚本的键。</param>
        /// <param name="script">要注册的脚本文本。</param>
        /// <param name="addScriptTags">指示是否添加脚本标记的布尔值。</param>
        /// <param name="startupScript">指示是否将脚本添加为启动脚本。如果该参数为 false，则脚本将会被添加至 HTML-BODY 的起始位置，否则脚本将会呗添加至 HTML-BODY 的结束位置。</param>
        public static void RegisterClientScript(this ClientScriptManager clientScript, Type type, string key, string script, bool addScriptTags, bool startupScript)
        {
            Check.NotNull(clientScript, "clientScript");
            if (startupScript)
            {
                clientScript.RegisterStartupScript(type, key, script, addScriptTags);
            }
            else
            {
                clientScript.RegisterClientScriptBlock(type, key, script, addScriptTags);
            }
        }
    }
}
