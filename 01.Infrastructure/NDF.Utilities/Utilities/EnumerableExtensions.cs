using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Utilities
{
    /// <summary>
    /// 提供一组对 公开枚举数类型 <see cref="System.Collections.IEnumerable"/> 操作方法的扩展。
    /// </summary>
    public static partial class EnumerableExtensions
    {

        /// <summary>
        /// 将单个对象元素构建成一个泛型序列 IEnumerable&lt;TResult&gt; 并返回。
        /// </summary>
        /// <typeparam name="TSource">表示对象 <paramref name="_this"/> 的类型，也是返回的泛型序列中元素的类型。</typeparam>
        /// <param name="_this">待返回序列中包含的元素。</param>
        /// <returns>返回一个新创建的序列，该序列中包含一个元素即是 <paramref name="_this"/>。</returns>
        public static IEnumerable<TSource> MakeEnumerable<TSource>(this TSource _this)
        {
            yield return _this;
        }


        /// <summary>
        /// 将单个对象元素构建成一个强类型数组，数组中元素的类型即为传入元素的类型。
        /// </summary>
        /// <typeparam name="TSource">表示对象 <paramref name="_this"/> 的类型，也是返回的数组中元素的类型。</typeparam>
        /// <param name="_this">待返回数组中包含的元素。</param>
        /// <returns>返回一个新创建的强类型数组，该序列中包含一个元素即是 <paramref name="_this"/>。</returns>
        public static TSource[] MakeArray<TSource>(this TSource _this)
        {
            return MakeEnumerable(_this).ToArray();
        }




        /// <summary>
        /// 根据指定类型筛选 <see cref="System.Collections.IEnumerable"/> 的元素。
        /// </summary>
        /// <param name="_this"><see cref="System.Collections.IEnumerable"/>，其元素用于筛选。</param>
        /// <param name="c">筛选序列元素所根据的类型。</param>
        /// <returns>一个 <see cref="System.Collections.IEnumerable"/>，包含类型为 c 的输入序列中的元素。</returns>
        public static IEnumerable OfType(this IEnumerable _this, Type c)
        {
            return OfType(_this, c, true);
        }

        /// <summary>
        /// 根据指定类型筛选 <see cref="System.Collections.IEnumerable"/> 的元素。
        /// </summary>
        /// <param name="_this"><see cref="System.Collections.IEnumerable"/>，其元素用于筛选。</param>
        /// <param name="c">筛选序列元素所根据的类型。</param>
        /// <param name="inherit">值为 true 时指定返回序列中每个元素的类型可以是 <paramref name="c"/> 的子类或接口实现类，值为 false 时指示每个元素的类型必须等同于 <paramref name="c"/>；</param>
        /// <returns>一个 <see cref="System.Collections.IEnumerable"/>，包含类型为 <paramref name="c"/> 的输入序列中的元素。</returns>
        public static IEnumerable OfType(this IEnumerable _this, Type c, bool inherit)
        {
            Check.NotNull(_this);
            Check.NotNull(c);
            return ToGeneric(_this).Where(item =>
            {
                return item == null ? false : (inherit ? c.IsInstanceOfType(item) : c == item.GetType());
            });
        }

        /// <summary>
        /// 根据指定类型筛选 <see cref="System.Collections.IEnumerable"/> 的元素。
        /// </summary>
        /// <typeparam name="TResult">筛选序列元素所根据的类型。</typeparam>
        /// <param name="_this"><see cref="System.Collections.IEnumerable"/>，其元素用于筛选。</param>
        /// <param name="inherit">值为 true 时指定返回序列中每个元素的类型可以是 <typeparamref name="TResult"/> 的子类或接口实现类，值为 false 时指示每个元素的类型必须等同于 <typeparamref name="TResult"/>；该值默认为 true。</param>
        /// <returns>一个 <see cref="System.Collections.IEnumerable"/>，包含类型为 <typeparamref name="TResult"/> 的输入序列中的元素。</returns>
        public static IEnumerable<TResult> OfType<TResult>(this IEnumerable _this, bool inherit)
        {
            return inherit ? _this.OfType<TResult>() : OfType(_this, typeof(TResult), inherit).Cast<TResult>();
        }



        /// <summary>
        /// 将一个 普通枚举序列 <see cref="System.Collections.IEnumerable"/> 转换成一个泛型枚举序列 <see cref="System.Collections.Generic.IEnumerable&lt;Object&gt;"/>。
        /// </summary>
        /// <param name="_this">被转换的普通枚举序列 <see cref="System.Collections.IEnumerable"/> 对象。</param>
        /// <returns>一个 <see cref="System.Collections.Generic.IEnumerable&lt;Object&gt;"/>，包含输入序列中所有的元素。</returns>
        public static IEnumerable<object> ToGeneric(this IEnumerable _this)
        {
            if (_this == null)
                return System.Linq.Enumerable.Empty<object>();

            return _this.OfType<object>();
        }


    }
}
