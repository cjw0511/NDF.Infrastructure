using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Optimization;

namespace NDF.Web.Optimization
{
    /// <summary>
    /// 提供一组对 WEB 资源引用绑定对象集合 <see cref="System.Web.Optimization.BundleCollection"/> 操作方法的扩展。
    /// </summary>
    public static class BundleCollectionExtensions
    {
        /// <summary>
        /// 将一组表示 WEB 资源文件引用的虚拟路径添加至 <see cref="System.Web.Optimization.BundleCollection"/> 集合映射中。
        /// </summary>
        /// <param name="bundles">表示 <see cref="System.Web.Optimization.BundleCollection"/> 映射集合。</param>
        /// <param name="key">表示被添加入 <see cref="System.Web.Optimization.BundleCollection"/> 映射集合的虚拟路径访问索引键值。</param>
        /// <param name="bundlePaths">
        /// 一个数组，数组中的每个元素都是一个表示要引入的资源文件虚拟路径(以字符 "~/" 开头)的对象，只能
        /// 为 <see cref="System.String"/> 或 <see cref="NDF.Web.Optimization.BundlePath"/> 类型值。
        /// 注意：该参数不接收带通配符的虚拟路径。
        /// </param>
        public static void Add(this BundleCollection bundles, object key, params object[] bundlePaths)
        {
            Check.NotNull(bundles);
            Check.NotNull(key);
            Check.ArrayIsRangeTypes(bundlePaths, typeof(string), typeof(BundlePath));
            foreach (object path in bundlePaths)
            {
                BundleWrapper wrapper = BundleWrapperFactory.GetBundleWrapper(key, path);
                if (!bundles.Contains(wrapper.Bundle))
                {
                    bundles.Add(wrapper.Bundle);
                }
            }
        }
    }
}
