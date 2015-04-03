using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Web.Optimization
{
    /// <summary>
    /// 表示 System.Web.Optimization.dll 组件绑定的 WEB 资源的类型。
    /// </summary>
    public enum BundleType
    {
        /// <summary>
        /// 表示 JavaScript 脚本文件；
        /// </summary>
        JavaScript = 0,

        /// <summary>
        /// 表示 CSS Style 样式文件；
        /// </summary>
        Style = 1,

        /// <summary>
        /// 表示该文件没有扩展名
        /// </summary>
        NoType = 2,

        /// <summary>
        /// 表示不可识别的 WEB 资源文件类型
        /// </summary>
        Unknown = 3
    }
}
