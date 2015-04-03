using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Optimization;

namespace NDF.Web.Optimization
{
    /// <summary>
    /// 封装 <see cref="System.Web.Optimization.Bundle"/> 对象的结构。
    /// </summary>
    internal class BundleWrapper
    {
        /// <summary>
        /// 表示该包装器其内部所引用的 <see cref="System.Web.Optimization.Bundle"/> 对象。
        /// </summary>
        public Bundle Bundle { get; set; }

        /// <summary>
        /// 表示该 <see cref="BundleWrapper"/> 对象的键值，该键值被当作对象的索引键，用于在缓存集合中查找对象。
        /// </summary>
        public object Key { get; set; }

        /// <summary>
        /// 表示该包装器引用的 <see cref="System.Web.Optimization.Bundle"/> 对象所示的 WEB 资源文件扩展名。
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// 表示该包装器引用的 <see cref="System.Web.Optimization.Bundle"/> 对象所示的 WEB 资源文件所在的目录。
        /// </summary>
        public string Directory { get; set; }

        /// <summary>
        /// 表示该包装器引用的 <see cref="System.Web.Optimization.Bundle"/> 对象所示的 WEB 资源文件的绝对路径。
        /// </summary>
        public string Relative { get; set; }

        /// <summary>
        /// 表示该包装器引用的 <see cref="System.Web.Optimization.Bundle"/> 对象所示的 WEB 资源文件的相对路径。
        /// </summary>
        public string VirtualPath { get; set; }

        /// <summary>
        /// 表示该包装器引用的 <see cref="System.Web.Optimization.Bundle"/> 对象所示的 WEB 资源文件的类型。
        /// 参考 <see cref="BundleType"/> 。
        /// </summary>
        public BundleType Type { get; set; }

        /// <summary>
        /// 表示该包装器引用的 <see cref="System.Web.Optimization.Bundle"/> 对象所示的 WEB 资源文件的原始输入路径。
        /// </summary>
        public string OriginalPath { get; set; }
    }
}
