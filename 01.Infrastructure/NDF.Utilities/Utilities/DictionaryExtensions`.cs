using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Utilities
{
    /// <summary>
    /// 提供一组对字典对象 <see cref="System.Collections.Generic.Dictionary&lt;TKey, TValue&gt;"/> 操作方法的扩展。
    /// </summary>
    public static class DictionaryExtensions
    {

        /// <summary>
        /// 根据键的值获取与该键关联的值。
        /// <para>该扩展方法与 <see cref="IDictionary&lt;TKey, TValue&gt;"/> 字典对象的默认索引器 get 方法差别在于：</para>
        /// <para>    默认索引器 get 方法在找不到指定的键时，会引发 <see cref="KeyNotFoundException"/> 异常；</para>
        /// <para>    而该方法在找不到指定的键时候，返回值将为 <typeparamref name="TValue"/> 类型的默认值，但同时 out 参数 <paramref name="success"/> 的输出值为 false 表示未成功获取键值数据。</para>
        /// </summary>
        /// <typeparam name="TKey">字典中的键的类型。</typeparam>
        /// <typeparam name="TValue">字典中的值的类型。</typeparam>
        /// <param name="_this">表示当前 <see cref="IDictionary&lt;TKey, TValue&gt;"/> 字典对象。</param>
        /// <param name="key">字典中的键。</param>
        /// <param name="success">一个 bool 值，标识是否成功获取 <paramref name="_this"/> 字典对象中 <paramref name="key"/> 键的值；如果字典中不存在该键，则返回 false，否则返回 true。</param>
        /// <returns>返回一个 <typeparamref name="TValue"/> 类型值。</returns>
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> _this, TKey key, out bool success)
        {
            Check.NotNull(_this);
            TValue ret;
            success = _this.TryGetValue(key, out ret);
            return ret;
        }

        /// <summary>
        /// 根据键的值获取与该键关联的值。
        /// <para>该扩展方法与 <see cref="IDictionary&lt;TKey, TValue&gt;"/> 字典对象的默认索引器 get 方法差别在于：</para>
        /// <para>    默认索引器 get 方法在找不到指定的键时，会引发 <see cref="KeyNotFoundException"/> 异常；</para>
        /// <para>    而该方法在找不到指定的键时候，返回值将为 <typeparamref name="TValue"/> 类型的默认值。</para>
        /// <para>如果需要在调用该方法后确认是否成功获取到 <paramref name="key"/> 键对应的值，请使用带有 out bool success 参数的 GetValue 方法。</para>
        /// </summary>
        /// <typeparam name="TKey">字典中的键的类型。</typeparam>
        /// <typeparam name="TValue">字典中的值的类型。</typeparam>
        /// <param name="_this">表示当前 <see cref="IDictionary&lt;TKey, TValue&gt;"/> 字典对象。</param>
        /// <param name="key">字典中的键。</param>
        /// <returns>返回一个 <typeparamref name="TValue"/> 类型值。</returns>
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> _this, TKey key)
        {
            Check.NotNull(_this);
            TValue ret;
            _this.TryGetValue(key, out ret);
            return ret;
        }



        /// <summary>
        /// 从 <paramref name="_this"/> 参数所示的键值对集合中移除所有指定的键的值。
        /// </summary>
        /// <typeparam name="TKey">表示 <paramref name="_this"/> 键值对集合中键的类型。</typeparam>
        /// <typeparam name="TValue">表示 <paramref name="_this"/> 键值对集合中值的类型。</typeparam>
        /// <param name="_this">表示一个被操作的 <see cref="IDictionary&lt;TKey, TValue&gt;"/> 键值对集合。</param>
        /// <param name="keys">表示一组要移除的元素的键。</param>
        public static void RemoveRange<TKey, TValue>(this IDictionary<TKey, TValue> _this, IEnumerable<TKey> keys)
        {
            Check.NotNull(_this);
            if (keys == null)
                return;

            foreach (TKey key in keys)
            {
                _this.Remove(key);
            }
        }


    }
}
