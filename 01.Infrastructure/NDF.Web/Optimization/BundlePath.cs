using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Web.Optimization
{
    /// <summary>
    /// 用于包装 WEB 资源的访问路径。
    /// </summary>
    public class BundlePath
    {
        private string _virtualPath;
        private string _extension;


        /// <summary>
        /// 初始化 <see cref="BundlePath"/> 类型的实例。
        /// </summary>
        public BundlePath()
        {
            this.Type = BundleType.JavaScript;
            this.KeepPath = false;
        }

        /// <summary>
        /// 以指定的参数初始化 <see cref="BundlePath"/> 类型的实例。
        /// </summary>
        /// <param name="virtualPath">一个 <see cref="System.String"/> 字符串值，将作为对象的 <seealso cref="VirtualPath"/> 属性。</param>
        public BundlePath(string virtualPath)
        {
            Check.NotNull(virtualPath);
            this.VirtualPath = virtualPath;
            //this.Type = BundleUtility.GetBundleType(virtualPath);
            this.KeepPath = false;
        }

        /// <summary>
        /// 以指定的参数初始化 <see cref="BundlePath"/> 类型的实例。
        /// </summary>
        /// <param name="virtualPath">一个字符串值，将作为对象的 <seealso cref="VirtualPath"/> 属性。</param>
        /// <param name="keepPath">一个布尔值，将作为对象的 <seealso cref="KeepPath"/> 属性。</param>
        public BundlePath(string virtualPath, bool keepPath)
        {
            Check.NotNull(virtualPath);
            this.VirtualPath = virtualPath;
            //this.Type = BundleUtility.GetBundleType(virtualPath);
            this.KeepPath = keepPath;
        }

        /// <summary>
        /// 以指定的参数初始化 <see cref="BundlePath"/> 类型的实例。
        /// </summary>
        /// <param name="virtualPath">一个字符串值，将作为对象的 <seealso cref="VirtualPath"/> 属性。</param>
        /// <param name="bundleType">一个 <see cref="BundleType"/> 枚举值，将作为对象的 <seealso cref="Type"/> 属性。</param>
        public BundlePath(string virtualPath, BundleType bundleType)
        {
            Check.NotNull(virtualPath);
            this.VirtualPath = virtualPath;
            this.Type = bundleType;
            this.KeepPath = false;
        }

        /// <summary>
        /// 以指定的参数初始化 <see cref="BundlePath"/> 类型的实例。
        /// </summary>
        /// <param name="virtualPath">一个字符串值，将作为对象的 <seealso cref="VirtualPath"/> 属性。</param>
        /// <param name="keepPath">一个布尔值，将作为对象的 <seealso cref="KeepPath"/> 属性。</param>
        /// <param name="bundleType">一个 <see cref="BundleType"/> 枚举值，将作为对象的 <seealso cref="Type"/> 属性。</param>
        public BundlePath(string virtualPath, bool keepPath, BundleType bundleType)
        {
            Check.NotNull(virtualPath);
            this.VirtualPath = virtualPath;
            this.Type = bundleType;
            this.KeepPath = keepPath;
        }



        /// <summary>
        /// 获取或设置 WEB 资源的类型。
        /// </summary>
        public BundleType Type { get; set; }

        /// <summary>
        /// 获取或设置 WEB 资源的虚拟路径。
        /// 如果在设置该属性时，设置的值所表示的虚拟路径具备文件扩展名，则将同步更新当前对象的 <seealso cref="Type"/> 属性值。
        /// </summary>
        public string VirtualPath
        {
            get { return this._virtualPath; }
            set
            {
                string extension = null;
                BundleType type = BundleUtility.GetBundleType(value, out extension);
                if (extension != null)
                {
                    this._extension = extension;
                    this.Type = type;
                }
                this._virtualPath = value;
            }
        }

        /// <summary>
        /// 获取或设置一个 bool 值，该值表示将 WEB 资源文件压缩后输出值客户端时，在输出路径中是否保持原虚拟路径的目录结构。
        /// </summary>
        public bool KeepPath { get; set; }

        /// <summary>
        /// 获取当前对象的 <seealso cref="VirtualPath"/> 属性所示路径的文件扩展名。
        /// 如果 <seealso cref="VirtualPath"/> 属性为 Null 或空字符串，则该属性将返回 Null。
        /// </summary>
        public string Extension { get { return this._extension; } }




        /// <summary>
        /// 创建一个 <see cref="BundlePath"/> 类型的新实例。
        /// </summary>
        /// <returns>返回 <see cref="BundlePath"/> 类型的新实例。</returns>
        public static BundlePath CreateBundlePath()
        {
            return new BundlePath();
        }

        /// <summary>
        /// 以指定的参数创建一个 <see cref="BundlePath"/> 类型的新实例。
        /// </summary>
        /// <param name="virtualPath">一个字符串值，将作为对象的 <seealso cref="VirtualPath"/> 属性。</param>
        /// <returns>返回 <see cref="BundlePath"/> 类型的新实例，其属性将是传入的相应参数的值。</returns>
        public static BundlePath CreateBundlePath(string virtualPath)
        {
            return new BundlePath(virtualPath);
        }

        /// <summary>
        /// 以指定的参数创建一个 <see cref="BundlePath"/> 类型的新实例。
        /// </summary>
        /// <param name="virtualPath">一个字符串值，将作为对象的 <seealso cref="VirtualPath"/> 属性。</param>
        /// <param name="keepPath">一个布尔值，将作为对象的 <seealso cref="KeepPath"/> 属性。</param>
        /// <returns>返回 <see cref="BundlePath"/> 类型的新实例，其属性将是传入的相应参数的值。</returns>
        public static BundlePath CreateBundlePath(string virtualPath, bool keepPath)
        {
            return new BundlePath(virtualPath, keepPath);
        }

        /// <summary>
        /// 以指定的参数创建一个 <see cref="BundlePath"/> 类型的新实例。
        /// </summary>
        /// <param name="virtualPath">一个字符串值，将作为对象的 <seealso cref="VirtualPath"/> 属性。</param>
        /// <param name="bundleType">一个 <see cref="BundleType"/> 枚举值，将作为对象的 <seealso cref="Type"/> 属性。</param>
        /// <returns>返回 <see cref="BundlePath"/> 类型的新实例，其属性将是传入的相应参数的值。</returns>
        public static BundlePath CreateBundlePath(string virtualPath, BundleType bundleType)
        {
            return new BundlePath(virtualPath, bundleType);
        }

        /// <summary>
        /// 以指定的参数创建一个 <see cref="BundlePath"/> 类型的新实例。
        /// </summary>
        /// <param name="virtualPath">一个字符串值，将作为对象的 <seealso cref="VirtualPath"/> 属性。</param>
        /// <param name="keepPath">一个布尔值，将作为对象的 <seealso cref="KeepPath"/> 属性。</param>
        /// <param name="bundleType">一个 <see cref="BundleType"/> 枚举值，将作为对象的 <seealso cref="Type"/> 属性。</param>
        /// <returns>返回 <see cref="BundlePath"/> 类型的新实例，其属性将是传入的相应参数的值。</returns>
        public static BundlePath CreateBundlePath(string virtualPath, bool keepPath, BundleType bundleType)
        {
            return new BundlePath(virtualPath, keepPath, bundleType);
        }
    }
}
