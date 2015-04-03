using NDF.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Utilities
{
    /// <summary>
    /// 提供一组对 对象比较器接口 <see cref="System.Collections.IComparer"/> 操作方法的扩展。
    /// </summary>
    public static class IComparerExtensions
    {
        /// <summary>
        /// 将普通对象比较器 <see cref="System.Collections.IComparer"/> 转换成支持泛型操作的 <see cref="IComparer&lt;T&gt;"/> 对象。
        /// </summary>
        /// <param name="_this">被转换的 <see cref="System.Collections.IComparer"/> 对象。</param>
        /// <returns>一个支持泛型操作的 <see cref="IComparer&lt;T&gt;"/> 对象，该对象等效于源对象 <paramref name="_this"/>。</returns>
        public static IComparer<object> ToGeneric(this IComparer _this)
        {
            return Comparer<object>.Create(_this.Compare);
        }

        /// <summary>
        /// 将普通对象比较器 <see cref="System.Collections.IComparer"/> 转换成相等对象比较器 <see cref="IEqualityComparer"/> 。
        /// </summary>
        /// <param name="_this">被转换的 <see cref="System.Collections.IComparer"/> 对象。</param>
        /// <returns>一个 <see cref="IEqualityComparer"/> 对象，其 Equals 方法等效于源对象 <paramref name="_this"/> 的 Compare 方法，其 GetHashCode 方法始终返回 0。</returns>
        public static IEqualityComparer ToEqualityComparer(this IComparer _this)
        {
            return EqualityComparer.Create(_this);
        }

        /// <summary>
        /// 将普通对象比较器 <see cref="System.Collections.IComparer"/> 转换成支持泛型操作的 <see cref="IEqualityComparer&lt;T&gt;"/> 对象。
        /// </summary>
        /// <typeparam name="T"><paramref name="_this"/> 支持比较的对象类型。</typeparam>
        /// <param name="_this">被转换的 <see cref="IComparer&lt;T&gt;"/> 对象。</param>
        /// <returns>一个支持泛型操作的 <see cref="IEqualityComparer&lt;T&gt;"/> 对象，其 Equals 方法等效于源对象 <paramref name="_this"/> 的 Compare 方法，其 GetHashCode 方法始终返回 0。</returns>
        public static IEqualityComparer<T> ToEqualityComparer<T>(this IComparer<T> _this)
        {
            return NDF.Collections.Generic.EqualityComparer<T>.Create(_this);
        }

    }
}
