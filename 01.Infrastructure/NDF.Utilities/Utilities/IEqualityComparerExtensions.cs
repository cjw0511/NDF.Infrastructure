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
    /// 提供一组对 对象比较器接口 <see cref="System.Collections.IEqualityComparer"/> 操作方法的扩展。
    /// </summary>
    public static class IEqualityComparerExtensions
    {
        /// <summary>
        /// 将普通对象比较器 <see cref="System.Collections.IEqualityComparer"/> 转换成支持泛型操作的 <see cref="IEqualityComparer&lt;T&gt;"/> 对象。
        /// </summary>
        /// <param name="_this">被转换的 <see cref="System.Collections.IEqualityComparer"/> 对象。</param>
        /// <returns>一个支持泛型操作的 <see cref="IEqualityComparer&lt;T&gt;"/> 对象，该对象等效于源对象 <paramref name="_this"/>。</returns>
        public static IEqualityComparer<object> ToGeneric(this IEqualityComparer _this)
        {
            return NDF.Collections.Generic.EqualityComparer<object>.Create(_this.Equals, _this.GetHashCode);
        }

        /// <summary>
        /// 将普通对象比较器 <see cref="System.Collections.IEqualityComparer"/> 转换成等价的对象比较器 <see cref="System.Collections.IEqualityComparer"/>。
        /// 参数 <paramref name="withGetHashCode"/> 指示返回的对象比较器 <see cref="System.Collections.IEqualityComparer"/> 中是否忽略方法 GetHashCode 的作用。
        /// 如果参数 <paramref name="withGetHashCode"/> 为 false，则返回的对象比较器 <see cref="System.Collections.IEqualityComparer"/> 中的 GetHashCode 方法始终返回 0，否则其依然用 <paramref name="_this"/> 的 GetHashCode 方法作为哈希算法函数。
        /// </summary>
        /// <param name="_this">被转换的 <see cref="System.Collections.IEqualityComparer"/> 对象。</param>
        /// <param name="withGetHashCode">
        /// 一个 bool 值，指示是否忽略返回的对象比较器 <see cref="System.Collections.IEqualityComparer"/> 中 GetHashCode 方法的作用。
        /// 如果该参数值 false，则返回的对象比较器 <see cref="System.Collections.IEqualityComparer"/> 中的 GetHashCode 方法始终返回 0，否则其依然用 <paramref name="_this"/> 的 GetHashCode 方法作为哈希算法函数。
        /// 该参数值默认为 false。
        /// </param>
        /// <returns>
        /// 一个对象比较器 <see cref="System.Collections.IEqualityComparer"/>。
        /// 如果参数 <paramref name="withGetHashCode"/> 为 false，则返回的对象比较器 <see cref="System.Collections.IEqualityComparer"/> 中的 GetHashCode 方法始终返回 0，否则其依然用 <paramref name="_this"/> 的 GetHashCode 方法作为哈希算法函数。
        /// </returns>
        public static IEqualityComparer AsEqualityComparer(this IEqualityComparer _this, bool withGetHashCode = false)
        {
            return withGetHashCode
                ? EqualityComparer.Create((x, y) => _this.Equals(x, y), obj => _this.GetHashCode(obj))
                : EqualityComparer.Create((x, y) => _this.Equals(x, y));
        }

        /// <summary>
        /// 将泛型对象比较器 <see cref="IEqualityComparer&lt;T&gt;"/> 转换成等价的泛型对象比较器 <see cref="IEqualityComparer&lt;T&gt;"/>。
        /// 参数 <paramref name="withGetHashCode"/> 指示返回的泛型对象比较器 <see cref="IEqualityComparer&lt;T&gt;"/> 中是否忽略方法 GetHashCode 的作用。
        /// 如果参数 <paramref name="withGetHashCode"/> 为 false，则返回的泛型对象比较器 <see cref="IEqualityComparer&lt;T&gt;"/> 中的 GetHashCode 方法始终返回 0，否则其依然用 <paramref name="_this"/> 的 GetHashCode 方法作为哈希算法函数。
        /// </summary>
        /// <typeparam name="T">返回的对象比较器可用于比较的对象类型。</typeparam>
        /// <param name="_this">被转换的 <see cref="IEqualityComparer&lt;T&gt;"/> 对象。</param>
        /// <param name="withGetHashCode">
        /// 一个 bool 值，指示是否忽略返回的泛型对象比较器 <see cref="IEqualityComparer&lt;T&gt;"/> 中 GetHashCode 方法的作用。
        /// 如果该参数值 false，则返回的泛型对象比较器 <see cref="IEqualityComparer&lt;T&gt;"/> 中的 GetHashCode 方法始终返回 0，否则其依然用 <paramref name="_this"/> 的 GetHashCode 方法作为哈希算法函数。
        /// 该参数值默认为 false。
        /// </param>
        /// <returns>
        /// 一个对象泛型比较器 <see cref="IEqualityComparer&lt;T&gt;"/>。
        /// 如果参数 <paramref name="withGetHashCode"/> 为 false，则返回的泛型对象比较器 <see cref="IEqualityComparer&lt;T&gt;"/> 中的 GetHashCode 方法始终返回 0，否则其依然用 <paramref name="_this"/> 的 GetHashCode 方法作为哈希算法函数。
        /// </returns>
        public static IEqualityComparer<T> AsEqualityComparer<T>(this IEqualityComparer<T> _this, bool withGetHashCode = false)
        {
            return withGetHashCode
                ? NDF.Collections.Generic.EqualityComparer<T>.Create((x, y) => _this.Equals(x, y), obj => _this.GetHashCode(obj))
                : NDF.Collections.Generic.EqualityComparer<T>.Create((x, y) => _this.Equals(x, y));
        }


    }
}
