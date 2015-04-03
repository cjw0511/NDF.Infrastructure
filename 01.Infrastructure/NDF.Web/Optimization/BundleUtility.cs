using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NDF.Web.Optimization
{
    /// <summary>
    /// 定义一组用于快速操作或解析 System.Web.Optimization.Bundle 对象的工具方法。
    /// </summary>
    public static class BundleUtility
    {
        /// <summary>
        /// 获取虚拟路径所表示的 WEB 资源文件类型；
        /// </summary>
        /// <param name="virtualPath">表示指向 WEB 资源文件的虚拟路径。</param>
        /// <returns>返回虚拟路径所表示的 WEB 资源文件类型；</returns>
        public static BundleType GetBundleType(string virtualPath)
        {
            string ext = null;
            return GetBundleType(virtualPath, out ext);
        }

        /// <summary>
        /// 获取虚拟路径所表示的 WEB 资源文件类型，并返回虚拟路径所表示的 WEB 资源文件的文件扩展名；
        /// </summary>
        /// <param name="virtualPath">表示指向 WEB 资源文件的虚拟路径。</param>
        /// <param name="extension">返回 virtualPath 表示的 WEB 资源文件的文件扩展名。</param>
        /// <returns>返回虚拟路径所表示的 WEB 资源文件类型；</returns>
        public static BundleType GetBundleType(string virtualPath, out string extension)
        {
            BundleType ret = BundleType.Unknown;
            extension = VirtualPathUtility.GetExtension(virtualPath);
            if (string.IsNullOrWhiteSpace(extension))
            {
                ret = BundleType.NoType;
            }
            else if (extension.Equals(".js", StringComparison.InvariantCultureIgnoreCase))
            {
                ret = BundleType.JavaScript;
            }
            else if (extension.Equals(".css", StringComparison.InvariantCultureIgnoreCase))
            {
                ret = BundleType.Style;
            }
            return ret;
        }
    }
}
