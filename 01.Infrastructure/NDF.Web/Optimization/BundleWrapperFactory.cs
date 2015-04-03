using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Optimization;

namespace NDF.Web.Optimization
{
    /// <summary>
    /// 提供一组用于创建或获取 <see cref="BundleWrapper"/> 包装器对象的方法。
    /// </summary>
    internal class BundleWrapperFactory
    {
        private static Dictionary<object, List<BundleWrapper>> _cache;

        /// <summary>
        /// 获取用于存储 键与 <see cref="BundleWrapper"/> 集合映射的缓存对象。
        /// </summary>
        public static Dictionary<object, List<BundleWrapper>> BundleCache
        {
            get
            {
                if (BundleWrapperFactory._cache == null)
                {
                    BundleWrapperFactory._cache = new Dictionary<object, List<BundleWrapper>>();
                }
                return BundleWrapperFactory._cache;
            }
        }


        /// <summary>
        /// 判断 BundleChache 缓存对象中是否存在指定的索引键值所表示的 <see cref="BundleWrapper"/> 集合列表。
        /// </summary>
        /// <param name="key">被判断的索引键值。</param>
        /// <returns>
        /// 如果 BundleChache 缓存对象中存在 <paramref name="key"/> 索引键值所表示的 <see cref="BundleWrapper"/> 集合列表，则返回 true，否则返回 false。
        /// </returns>
        public static bool HasKey(object key)
        {
            return BundleCache.ContainsKey(key);
        }


        /// <summary>
        /// 根据 <see cref="BundleWrapper"/> 集合的键值获取缓存中的 <see cref="BundleWrapper"/> 集合列表。
        /// </summary>
        /// <param name="key">用于搜索 <see cref="BundleWrapper"/> 集合的索引键值。</param>
        /// <returns>
        /// 返回一个 <see cref="List&lt;T&gt;"/>，集合中的每个元素都是一个 <see cref="BundleWrapper"/> 包装器元素。
        /// </returns>
        internal static List<BundleWrapper> GetBundleWrapperList(object key)
        {
            Check.NotNull(key);
            List<BundleWrapper> list = null;
            if (!BundleCache.TryGetValue(key, out list))
            {
                list = new List<BundleWrapper>();
                BundleCache.Add(key, list);
            }
            return list;
        }

        /// <summary>
        /// 根据 <see cref="BundleWrapper"/> 所在集合对应的键值获取缓存中该键值对应的 <see cref="BundleWrapper"/> 集合中的 <see cref="BundleWrapper"/> 包装器对象。
        /// 该方法会首先查询 <seealso cref="BundleCache"/> 缓存对象中指定 <paramref name="key"/> 键值所对应的 <see cref="List&lt;T&gt;"/> 列表，然后在该列表中查找 <paramref name="bundlePath"/> 参数所对应的 <see cref="BundleWrapper"/> 对象。
        /// </summary>
        /// <param name="key">用于搜索 <see cref="BundleWrapper"/> 集合的索引键值。</param>
        /// <param name="bundlePath">用于搜索 <see cref="BundleWrapper"/> 集合的绑定对象路径。</param>
        /// <returns>返回一个 <see cref="BundleWrapper"/>。</returns>
        public static BundleWrapper GetBundleWrapper(object key, object bundlePath)
        {
            Check.NotNull(key);
            Check.IsRangeTypes(bundlePath, typeof(string), typeof(BundlePath));

            BundlePath path = bundlePath is BundlePath ? bundlePath as BundlePath : BundlePath.CreateBundlePath(bundlePath as string);
            string virtualPath = path.VirtualPath;
            string directory = VirtualPathUtility.GetDirectory(virtualPath);
            string relative = VirtualPathUtility.ToAppRelative(directory);
            string extension = path.Extension;
            BundleType type = path.Type;

            List<BundleWrapper> list = GetBundleWrapperList(key);
            BundleWrapper wrapper = list.LastOrDefault();

            if (wrapper == null || wrapper.Type != type ||
                !wrapper.Relative.Equals(relative, StringComparison.InvariantCultureIgnoreCase))
            {
                wrapper = new BundleWrapper();
                wrapper.Key = key;
                wrapper.OriginalPath = virtualPath;
                wrapper.Extension = extension;
                wrapper.Directory = directory;
                wrapper.Relative = relative;
                wrapper.Type = type;
                wrapper.VirtualPath = path.KeepPath
                    ? virtualPath
                    : (wrapper.Directory + (wrapper.Directory.EndsWith("/") ? "" : "/") + Guid.NewGuid().ToString("N"));
                switch (type)
                {
                    case BundleType.JavaScript:
                        wrapper.Bundle = new ScriptBundle(wrapper.VirtualPath);
                        break;
                    case BundleType.Style:
                        wrapper.Bundle = new StyleBundle(wrapper.VirtualPath);
                        break;
                    case BundleType.NoType:
                    case BundleType.Unknown:
                    default:
                        wrapper.Bundle = new Bundle(wrapper.VirtualPath);
                        break;
                }
                list.Add(wrapper);
            }
            wrapper.Bundle.Include(virtualPath);
            return wrapper;
        }
    }
}
