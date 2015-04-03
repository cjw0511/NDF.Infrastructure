using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;

namespace NDF.Web.Utilities
{
    /// <summary>
    /// 提供一组对 WEB 应用程序缓存对象 <see cref="System.Web.Caching.Cache"/> 操作方法的扩展。
    /// </summary>
    public static class CacheExtensions
    {
        /// <summary>
        /// 获取与指定的键相关联的值。
        /// </summary>
        /// <param name="_this">实现用于 Web 应用程序的缓存 <see cref="System.Web.Caching.Cache"/> 对象。</param>
        /// <param name="key">要获取其值的键。</param>
        /// <param name="value">当此方法返回时，如果找到指定键，则返回与该键相关联的值；否则，将返回 value 参数的类型的默认值。该参数未经初始化即被传递。</param>
        /// <returns>如果实现 <see cref="System.Web.Caching.Cache"/> 对象包含具有指定键的元素，则为true；否则，为 false。</returns>
        public static bool TryGetValue(this Cache _this, string key, out object value)
        {
            value = _this.Get(key);
            return value == null;
        }
    }
}
