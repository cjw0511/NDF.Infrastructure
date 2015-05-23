using NDF.ComponentModel.Paging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Utilities
{
    /// <summary>
    /// 提供一组对 公开泛型枚举数类型 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/> 操作方法的扩展。
    /// </summary>
    public static partial class EnumerableExtensions
    {

        /// <summary>
        /// 判断指定的序列对象 <paramref name="_this"/> 是否为 Null 或不包含任何元素。
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="_this">被判断的序列 <see cref="IEnumerable"/> 对象。</param>
        /// <returns>如果序列对象 <paramref name="_this"/> 为 Null 或者不包含任何元素，则返回 true；否则返回 false。</returns>
        public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> _this)
        {
            return _this == null || _this.Count() == 0;
        }

        /// <summary>
        /// 基于指定的谓词判断指定的序列对象 <paramref name="_this"/> 是否为 Null 或不包含符合谓词条件判断的任何元素。
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="_this">被判断的序列 <see cref="IEnumerable"/> 对象。</param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> _this, Func<TSource, bool> predicate)
        {
            return _this == null || _this.Count(predicate) == 0;
        }

        /// <summary>
        /// 基于指定的谓词判断指定的序列对象 <paramref name="_this"/> 是否为 Null 或不包含符合谓词条件判断的任何元素。
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="_this">被判断的序列 <see cref="IEnumerable"/> 对象。</param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> _this, Func<TSource, int, bool> predicate)
        {
            return _this == null || _this.Count(predicate) == 0;
        }



        /// <summary>
        /// 基于谓词确定序列中的所有元素是否满足条件。将在谓词函数的逻辑中使用每个元素的索引。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="_this"/> 中的元素的类型。</typeparam>
        /// <param name="_this">一个 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/>，其元素将应用谓词。</param>
        /// <param name="predicate">用于测试每个源元素是否满足条件的函数；该函数的第二个参数表示源元素的索引。</param>
        /// <returns>如果源序列中的每个元素都通过指定谓词中的测试，或者序列为空，则为 true；否则为 false。</returns>
        public static bool All<TSource>(this IEnumerable<TSource> _this, Func<TSource, int, bool> predicate)
        {
            Check.NotNull(_this);
            TSource[] array = _this.ToArray();
            for (var i = 0; i < array.Length; i++)
            {
                if (!predicate.Invoke(array[i], i))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 基于谓词确定序列中的任何元素是否都满足条件。 将在谓词函数的逻辑中使用每个元素的索引。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="_this"/> 中的元素的类型。</typeparam>
        /// <param name="_this">一个 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/>，其元素将应用谓词。</param>
        /// <param name="predicate">用于测试每个源元素是否满足条件的函数；该函数的第二个参数表示源元素的索引。</param>
        /// <returns>如果源序列中的任意一元素通过指定谓词中的测试，则为 true；否则为 false。</returns>
        public static bool Any<TSource>(this IEnumerable<TSource> _this, Func<TSource, int, bool> predicate)
        {
            Check.NotNull(_this);
            TSource[] array = _this.ToArray();
            for (var i = 0; i < array.Length; i++)
            {
                if (predicate.Invoke(array[i], i))
                {
                    return true;
                }
            }
            return false;
        }



        /// <summary>
        /// 将元素合并入泛型序列 <paramref name="_this"/> 中。如果源序列中包含该元素，则不进行合并操作。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="_this"/> 中的元素的类型。</typeparam>
        /// <param name="_this">一个 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/> 泛型序列。</param>
        /// <param name="item">要被合并入 <paramref name="_this"/> 序列的元素。</param>
        /// <returns>
        /// 一个新创建的 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/> 泛型序列，其包含源序列 <paramref name="_this"/> 的所有项。
        /// 如果 <paramref name="item"/> 元素不包含在 <paramref name="_this"/> 中，则返回的序列中也会包含 <paramref name="item"/> 元素。
        /// </returns>
        public static IEnumerable<TSource> Attach<TSource>(this IEnumerable<TSource> _this, TSource item)
        {
            return _this.Contains(item) ? _this.AsEnumerable() : Append(_this, item);
        }

        /// <summary>
        /// 将元素合并入泛型序列 <paramref name="_this"/> 中。如果源序列中包含该元素，则不进行合并操作。
        /// 以指定的对象相等比较器 <see cref="IEqualityComparer&lt;TSource&gt;"/> 来判断 <paramref name="item"/> 是否包含在 <paramref name="_this"/> 中。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="_this"/> 中的元素的类型。</typeparam>
        /// <param name="_this">一个 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/> 泛型序列。</param>
        /// <param name="item">要被合并入 <paramref name="_this"/> 序列的元素。</param>
        /// <param name="comparer">用于判断 <paramref name="item"/> 是否包含在 <paramref name="_this"/> 中的对象相等比较器 <see cref="IEqualityComparer&lt;TSource&gt;"/>。</param>
        /// <returns>
        /// 一个新创建的 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/> 泛型序列，其包含源序列 <paramref name="_this"/> 的所有项。
        /// 如果 <paramref name="item"/> 元素不包含在 <paramref name="_this"/> 中，则返回的序列中也会包含 <paramref name="item"/> 元素。
        /// </returns>
        public static IEnumerable<TSource> Attach<TSource>(this IEnumerable<TSource> _this, TSource item, IEqualityComparer<TSource> comparer)
        {
            return _this.Contains(item, comparer) ? _this.AsEnumerable() : Append(_this, item);
        }

        /// <summary>
        /// 将元素合并入泛型序列 <paramref name="_this"/> 中。如果源序列中包含该元素，则不进行合并操作。
        /// 以指定的对象相等比较器 Func&lt;TSource, TSource, bool&gt; 来判断 <paramref name="item"/> 是否包含在 <paramref name="_this"/> 中。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="_this"/> 中的元素的类型。</typeparam>
        /// <param name="_this">一个 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/> 泛型序列。</param>
        /// <param name="item">要被合并入 <paramref name="_this"/> 序列的元素。</param>
        /// <param name="comparer">用于判断 <paramref name="item"/> 是否包含在 <paramref name="_this"/> 中的对象相等比较器 Func&lt;TSource, TSource, bool&gt;。</param>
        /// <returns>
        /// 一个新创建的 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/> 泛型序列，其包含源序列 <paramref name="_this"/> 的所有项。
        /// 如果 <paramref name="item"/> 元素不包含在 <paramref name="_this"/> 中，则返回的序列中也会包含 <paramref name="item"/> 元素。
        /// </returns>
        public static IEnumerable<TSource> Attach<TSource>(this IEnumerable<TSource> _this, TSource item, Func<TSource, TSource, bool> comparer)
        {
            return _this.Contains(item, NDF.Collections.Generic.EqualityComparer<TSource>.Create(comparer)) ? _this.AsEnumerable() : Append(_this, item);
        }

        /// <summary>
        /// 将元素合并入泛型序列 <paramref name="_this"/> 中。如果源序列中包含该元素，则不进行合并操作。
        /// 以指定的对象相等比较器 <see cref="IComparer&lt;TSource&gt;"/> 来判断 <paramref name="item"/> 是否包含在 <paramref name="_this"/> 中。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="_this"/> 中的元素的类型。</typeparam>
        /// <param name="_this">一个 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/> 泛型序列。</param>
        /// <param name="item">要被合并入 <paramref name="_this"/> 序列的元素。</param>
        /// <param name="comparer">用于判断 <paramref name="item"/> 是否包含在 <paramref name="_this"/> 中的对象相等比较器 <see cref="IComparer&lt;TSource&gt;"/>。</param>
        /// <returns>
        /// 一个新创建的 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/> 泛型序列，其包含源序列 <paramref name="_this"/> 的所有项。
        /// 如果 <paramref name="item"/> 元素不包含在 <paramref name="_this"/> 中，则返回的序列中也会包含 <paramref name="item"/> 元素。
        /// </returns>
        public static IEnumerable<TSource> Attach<TSource>(this IEnumerable<TSource> _this, TSource item, IComparer<TSource> comparer)
        {
            return _this.Contains(item, comparer.ToEqualityComparer()) ? _this.AsEnumerable() : Append(_this, item);
        }



        /// <summary>
        /// 将 <see cref="IEnumerable&lt;TSource&gt;"/> 的元素强制转换为指定的类型。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="_this"/> 中的元素的类型。</typeparam>
        /// <typeparam name="TResult"><paramref name="_this"/> 中的元素要强制转换成的类型。</typeparam>
        /// <param name="_this">一个要转换类型的值序列。</param>
        /// <param name="converter">应用于每个元素的转换函数。</param>
        /// <returns>一个 IEnumerable&lt;TResult&gt;，其元素为对 <paramref name="_this"/> 的每个元素调用转换函数的结果。</returns>
        public static IEnumerable<TResult> Cast<TSource, TResult>(this IEnumerable<TSource> _this, Func<TSource, TResult> converter)
        {
            Check.NotNull(_this);
            return _this.Select(item => converter(item));
        }

        /// <summary>
        /// 将 <see cref="IEnumerable&lt;TSource&gt;"/> 的元素强制转换为指定的类型。将在操作函数的逻辑中使用每个元素的索引。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="_this"/> 中的元素的类型。</typeparam>
        /// <typeparam name="TResult"><paramref name="_this"/> 中的元素要强制转换成的类型。</typeparam>
        /// <param name="_this">一个要转换类型的值序列。</param>
        /// <param name="converter">一个应用于每个源元素的转换函数；函数的第二个参数表示源元素的索引。</param>
        /// <returns>一个 IEnumerable&lt;TResult&gt;，其元素为对 <paramref name="_this"/> 的每个元素调用转换函数的结果。</returns>
        public static IEnumerable<TResult> Cast<TSource, TResult>(this IEnumerable<TSource> _this, Func<TSource, int, TResult> converter)
        {
            Check.NotNull(_this);
            return _this.Select((i, item) => converter(i, item));
        }



        /// <summary>
        /// 通过使用指定的 Func&lt;TSource, TSource, bool&gt; 确定序列是否包含指定的元素。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="_this"/> 中的元素的类型。</typeparam>
        /// <param name="_this">要在其中定位某个值的序列。</param>
        /// <param name="value">要在序列中定位的值。</param>
        /// <param name="comparer">一个对值进行比较的相等比较器。</param>
        /// <returns>如果源序列包含具有指定值的元素，则为 true；否则为 false。</returns>
        public static bool Contains<TSource>(this IEnumerable<TSource> _this, TSource value, Func<TSource, TSource, bool> comparer)
        {
            Check.NotNull(_this);
            return _this.Contains(value, NDF.Collections.Generic.EqualityComparer<TSource>.Create(comparer));
        }

        ///// <summary>
        ///// 通过使用指定的 <see cref="IComparer&lt;TSource&gt;"/> 确定序列是否包含指定的元素。
        ///// </summary>
        ///// <typeparam name="TSource"><paramref name="_this"/> 中的元素的类型。</typeparam>
        ///// <param name="_this">要在其中定位某个值的序列。</param>
        ///// <param name="value">要在序列中定位的值。</param>
        ///// <param name="comparer">一个对值进行比较的相等比较器。</param>
        ///// <returns>如果源序列包含具有指定值的元素，则为 true；否则为 false。</returns>
        //public static bool Contains<TSource>(this IEnumerable<TSource> _this, TSource value, IComparer<TSource> comparer)
        //{
        //    Check.NotNull(_this);
        //    return _this.Contains(value, comparer.ToEqualityComparer());
        //}



        /// <summary>
        /// 返回一个数字，表示在指定的序列中满足条件的元素数量。将在操作函数的逻辑中使用每个元素的索引。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="_this"/> 中的元素的类型。</typeparam>
        /// <param name="_this">包含要测试和计数的元素的序列。</param>
        /// <param name="predicate">用于测试每个元素是否满足条件的函数；函数的第二个参数表示源元素的索引。</param>
        /// <returns>一个数字，表示序列中满足谓词函数条件的元素数量。</returns>
        public static int Count<TSource>(this IEnumerable<TSource> _this, Func<TSource, int, bool> predicate)
        {
            Check.NotNull(_this);
            int counter = 0;
            TSource[] array = _this.ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                TSource element = array[i];
                if (predicate(element, i))
                {
                    checked { counter++; }
                }
            }
            return counter;
        }



        /// <summary>
        /// 将当前序列与另一序列执行笛卡尔积运算。
        /// 源序列 <paramref name="_this"/> 中的每个元素都会与目标序列 <paramref name="values"/> 中的每个元素执行一次委托方法 <paramref name="operate"/> 指定的操作。
        /// </summary>
        /// <typeparam name="TSource">源序列 <paramref name="_this"/> 中的元素类型。</typeparam>
        /// <typeparam name="TValue">目标序列 <paramref name="values"/> 中的元素类型。</typeparam>
        /// <param name="_this">表示执行笛卡尔积操作的源序列。</param>
        /// <param name="values">表示执行笛卡尔积操作的目标序列。</param>
        /// <param name="operate">表示源序列 <paramref name="_this"/> 中的每个元素与目标序列 <paramref name="values"/> 中的每个元素所进行的笛卡尔积操作的方法体。</param>
        public static void Descartes<TSource, TValue>(this IEnumerable<TSource> _this, IEnumerable<TValue> values, Action<TSource, TValue> operate)
        {
            Descartes(_this, values, (item, i, value, j) => operate(item, value));
        }

        /// <summary>
        /// 将当前序列与另一序列执行笛卡尔积运算。
        /// 源序列 <paramref name="_this"/> 中的每个元素都会与目标序列 <paramref name="values"/> 中的每个元素执行一次委托方法 <paramref name="operate"/> 指定的操作。
        /// 该委托方法中的 int 参数指示执行该方法时源序列 <paramref name="_this"/> 中元素的索引号（从 0 开始计数）。
        /// </summary>
        /// <typeparam name="TSource">源序列 <paramref name="_this"/> 中的元素类型。</typeparam>
        /// <typeparam name="TValue">目标序列 <paramref name="values"/> 中的元素类型。</typeparam>
        /// <param name="_this">表示执行笛卡尔积操作的源序列。</param>
        /// <param name="values">表示执行笛卡尔积操作的目标序列。</param>
        /// <param name="operate">
        /// 表示源序列 <paramref name="_this"/> 中的每个元素与目标序列 <paramref name="values"/> 中的每个元素所进行的笛卡尔积操作的方法体。
        /// 该委托方法中的 int 参数指示执行该方法时源序列 <paramref name="_this"/> 中元素的索引号（从 0 开始计数）。
        /// </param>
        public static void Descartes<TSource, TValue>(this IEnumerable<TSource> _this, IEnumerable<TValue> values, Action<TSource, int, TValue> operate)
        {
            Descartes(_this, values, (item, i, value, j) => operate(item, i, value));
        }

        /// <summary>
        /// 将当前序列与另一序列执行笛卡尔积运算。
        /// 源序列 <paramref name="_this"/> 中的每个元素都会与目标序列 <paramref name="values"/> 中的每个元素执行一次委托方法 <paramref name="operate"/> 指定的操作。
        /// 该委托方法中的 int 参数指示执行该方法时目标序列 <paramref name="values"/> 中元素的索引号（从 0 开始计数）。
        /// </summary>
        /// <typeparam name="TSource">源序列 <paramref name="_this"/> 中的元素类型。</typeparam>
        /// <typeparam name="TValue">目标序列 <paramref name="values"/> 中的元素类型。</typeparam>
        /// <param name="_this">表示执行笛卡尔积操作的源序列。</param>
        /// <param name="values">表示执行笛卡尔积操作的目标序列。</param>
        /// <param name="operate">
        /// 表示源序列 <paramref name="_this"/> 中的每个元素与目标序列 <paramref name="values"/> 中的每个元素所进行的笛卡尔积操作的方法体。
        /// 该委托方法中的 int 参数指示执行该方法时目标序列 <paramref name="values"/> 中元素的索引号（从 0 开始计数）。
        /// </param>
        public static void Descartes<TSource, TValue>(this IEnumerable<TSource> _this, IEnumerable<TValue> values, Action<TSource, TValue, int> operate)
        {
            Descartes(_this, values, (item, i, value, j) => operate(item, value, j));
        }

        /// <summary>
        /// 将当前序列与另一序列执行笛卡尔积运算。
        /// 源序列 <paramref name="_this"/> 中的每个元素都会与目标序列 <paramref name="values"/> 中的每个元素执行一次委托方法 <paramref name="operate"/> 指定的操作。
        /// 该委托方法中的：
        ///     第 1 个 int 参数指示执行该方法时源序列 <paramref name="_this"/> 中元素的索引号（从 0 开始计数）。
        ///     第 2 个 int 参数指示执行该方法时目标序列 <paramref name="values"/> 中元素的索引号（从 0 开始计数）。
        /// </summary>
        /// <typeparam name="TSource">源序列 <paramref name="_this"/> 中的元素类型。</typeparam>
        /// <typeparam name="TValue">目标序列 <paramref name="values"/> 中的元素类型。</typeparam>
        /// <param name="_this">表示执行笛卡尔积操作的源序列。</param>
        /// <param name="values">表示执行笛卡尔积操作的目标序列。</param>
        /// <param name="operate">
        /// 表示源序列 <paramref name="_this"/> 中的每个元素与目标序列 <paramref name="values"/> 中的每个元素所进行的笛卡尔积操作的方法体。
        /// 该委托方法中的：
        ///     第 1 个 int 参数指示执行该方法时源序列 <paramref name="_this"/> 中元素的索引号（从 0 开始计数）。
        ///     第 2 个 int 参数指示执行该方法时目标序列 <paramref name="values"/> 中元素的索引号（从 0 开始计数）。
        /// </param>
        public static void Descartes<TSource, TValue>(this IEnumerable<TSource> _this, IEnumerable<TValue> values, Action<TSource, int, TValue, int> operate)
        {
            Descartes(_this, values, (item, i, value, j) =>
                {
                    operate(item, i, value, j);
                    return item;
                });
        }


        /// <summary>
        /// 将当前序列与另一序列执行笛卡尔积运算并返回运算结果。
        /// 源序列 <paramref name="_this"/> 中的每个元素都会与目标序列 <paramref name="values"/> 中的每个元素执行一次委托方法 <paramref name="operate"/> 指定的操作，该操作将返回一个结果。
        /// </summary>
        /// <typeparam name="TSource">源序列 <paramref name="_this"/> 中的元素类型。</typeparam>
        /// <typeparam name="TValue">值序列 <paramref name="values"/> 中的元素类型。</typeparam>
        /// <typeparam name="TResult">目标序列 <paramref name="values"/> 中的元素类型。</typeparam>
        /// <param name="_this">表示执行笛卡尔积操作的源序列。</param>
        /// <param name="values">表示执行笛卡尔积操作的目标序列。</param>
        /// <param name="operate">表示源序列 <paramref name="_this"/> 中的每个元素与目标序列 <paramref name="values"/> 中的每个元素所进行的笛卡尔积操作的方法体。</param>
        /// <returns>
        /// 返回源序列 <paramref name="_this"/> 中的每个元素与目标序列 <paramref name="values"/> 中的每个元素所进行的笛卡尔积操作的结果所组成的一个新的序列。
        /// </returns>
        public static IEnumerable<TResult> Descartes<TSource, TValue, TResult>(this IEnumerable<TSource> _this, IEnumerable<TValue> values, Func<TSource, TValue, TResult> operate)
        {
            return Descartes(_this, values, (item, i, value, j) => operate(item, value));
        }

        /// <summary>
        /// 将当前序列与另一序列执行笛卡尔积运算并返回运算结果。
        /// 源序列 <paramref name="_this"/> 中的每个元素都会与目标序列 <paramref name="values"/> 中的每个元素执行一次委托方法 <paramref name="operate"/> 指定的操作，该操作将返回一个结果。
        /// 该委托方法中的 int 参数指示执行该方法时源序列 <paramref name="_this"/> 中元素的索引号（从 0 开始计数）。
        /// </summary>
        /// <typeparam name="TSource">源序列 <paramref name="_this"/> 中的元素类型。</typeparam>
        /// <typeparam name="TValue">值序列 <paramref name="values"/> 中的元素类型。</typeparam>
        /// <typeparam name="TResult">目标序列 <paramref name="values"/> 中的元素类型。</typeparam>
        /// <param name="_this">表示执行笛卡尔积操作的源序列。</param>
        /// <param name="values">表示执行笛卡尔积操作的目标序列。</param>
        /// <param name="operate">
        /// 表示源序列 <paramref name="_this"/> 中的每个元素与目标序列 <paramref name="values"/> 中的每个元素所进行的笛卡尔积操作的方法体。
        /// 该委托方法中的 int 参数指示执行该方法时源序列 <paramref name="_this"/> 中元素的索引号（从 0 开始计数）。
        /// </param>
        /// <returns>
        /// 返回源序列 <paramref name="_this"/> 中的每个元素与目标序列 <paramref name="values"/> 中的每个元素所进行的笛卡尔积操作的结果所组成的一个新的序列。
        /// </returns>
        public static IEnumerable<TResult> Descartes<TSource, TValue, TResult>(this IEnumerable<TSource> _this, IEnumerable<TValue> values, Func<TSource, int, TValue, TResult> operate)
        {
            return Descartes(_this, values, (item, i, value, j) => operate(item, i, value));
        }

        /// <summary>
        /// 将当前序列与另一序列执行笛卡尔积运算并返回运算结果。
        /// 源序列 <paramref name="_this"/> 中的每个元素都会与目标序列 <paramref name="values"/> 中的每个元素执行一次委托方法 <paramref name="operate"/> 指定的操作，该操作将返回一个结果。
        /// 该委托方法中的 int 参数指示执行该方法时目标序列 <paramref name="values"/> 中元素的索引号（从 0 开始计数）。
        /// </summary>
        /// <typeparam name="TSource">源序列 <paramref name="_this"/> 中的元素类型。</typeparam>
        /// <typeparam name="TValue">值序列 <paramref name="values"/> 中的元素类型。</typeparam>
        /// <typeparam name="TResult">目标序列 <paramref name="values"/> 中的元素类型。</typeparam>
        /// <param name="_this">表示执行笛卡尔积操作的源序列。</param>
        /// <param name="values">表示执行笛卡尔积操作的目标序列。</param>
        /// <param name="operate">
        /// 表示源序列 <paramref name="_this"/> 中的每个元素与目标序列 <paramref name="values"/> 中的每个元素所进行的笛卡尔积操作的方法体。
        /// 该委托方法中的 int 参数指示执行该方法时目标序列 <paramref name="values"/> 中元素的索引号（从 0 开始计数）。
        /// </param>
        /// <returns>
        /// 返回源序列 <paramref name="_this"/> 中的每个元素与目标序列 <paramref name="values"/> 中的每个元素所进行的笛卡尔积操作的结果所组成的一个新的序列。
        /// </returns>
        public static IEnumerable<TResult> Descartes<TSource, TValue, TResult>(this IEnumerable<TSource> _this, IEnumerable<TValue> values, Func<TSource, TValue, int, TResult> operate)
        {
            return Descartes(_this, values, (item, i, value, j) => operate(item, value, j));
        }

        /// <summary>
        /// 将当前序列与另一序列执行笛卡尔积运算并返回运算结果。
        /// 源序列 <paramref name="_this"/> 中的每个元素都会与目标序列 <paramref name="values"/> 中的每个元素执行一次委托方法 <paramref name="operate"/> 指定的操作，该操作将返回一个结果。
        /// 该委托方法中的：
        ///     第 1 个 int 参数指示执行该方法时源序列 <paramref name="_this"/> 中元素的索引号（从 0 开始计数）。
        ///     第 2 个 int 参数指示执行该方法时目标序列 <paramref name="values"/> 中元素的索引号（从 0 开始计数）。
        /// </summary>
        /// <typeparam name="TSource">源序列 <paramref name="_this"/> 中的元素类型。</typeparam>
        /// <typeparam name="TValue">值序列 <paramref name="values"/> 中的元素类型。</typeparam>
        /// <typeparam name="TResult">目标序列 <paramref name="values"/> 中的元素类型。</typeparam>
        /// <param name="_this">表示执行笛卡尔积操作的源序列。</param>
        /// <param name="values">表示执行笛卡尔积操作的目标序列。</param>
        /// <param name="operate">
        /// 表示源序列 <paramref name="_this"/> 中的每个元素与目标序列 <paramref name="values"/> 中的每个元素所进行的笛卡尔积操作的方法体。
        /// 该委托方法中的：
        ///     第 1 个 int 参数指示执行该方法时源序列 <paramref name="_this"/> 中元素的索引号（从 0 开始计数）。
        ///     第 2 个 int 参数指示执行该方法时目标序列 <paramref name="values"/> 中元素的索引号（从 0 开始计数）。
        /// </param>
        /// <returns>
        /// 返回源序列 <paramref name="_this"/> 中的每个元素与目标序列 <paramref name="values"/> 中的每个元素所进行的笛卡尔积操作的结果所组成的一个新的序列。
        /// </returns>
        public static IEnumerable<TResult> Descartes<TSource, TValue, TResult>(this IEnumerable<TSource> _this, IEnumerable<TValue> values, Func<TSource, int, TValue, int, TResult> operate)
        {
            Check.NotNull(_this);
            Check.NotNull(values);
            Check.NotNull(operate);
            TSource[] array1 = _this.ToArray();
            TValue[] array2 = values.ToArray();
            for (int i = 0; i < array1.Length; i++)
            {
                TSource item = array1[i];
                for (int j = 0; j < array2.Length; j++)
                {
                    TValue value = array2[j];
                    yield return operate(item, i, value, j);
                }
            }
        }



        /// <summary>
        /// 根据指定的键选择器函数对序列中的元素进行去除重复项操作。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="_this"/> 中的元素的类型。</typeparam>
        /// <typeparam name="TKey"><paramref name="keySelector"/> 返回的键的类型。</typeparam>
        /// <param name="_this">要从中移除重复元素的序列。</param>
        /// <param name="keySelector">用于提取每个元素的键的函数。</param>
        /// <returns>返回一个 IEnumerable&lt;TSource&gt; 序列，该序列中的每个元素通过键函数 <paramref name="keySelector"/> 得到的值都不会重复。</returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> _this, Func<TSource, TKey> keySelector)
        {
            return DistinctBy(_this, keySelector, null);
        }

        /// <summary>
        /// 根据指定的键选择器函数对序列中的元素进行去除重复项操作，并使用指定的比较器对键进行比较。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="_this"/> 中的元素的类型。</typeparam>
        /// <typeparam name="TKey"><paramref name="keySelector"/> 返回的键的类型。</typeparam>
        /// <param name="_this">要从中移除重复元素的序列。</param>
        /// <param name="keySelector">用于提取每个元素的键的函数。</param>
        /// <param name="comparer">一个用于对键进行比较的 IEqualityComparer&lt;TKey&gt;。</param>
        /// <returns>返回一个 IEnumerable&lt;TSource&gt; 序列，该序列中的每个元素通过键函数 <paramref name="keySelector"/> 得到的值都不会重复。</returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> _this, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            Check.NotNull(_this);
            Check.NotNull(keySelector);

            if (comparer == null)
                comparer = EqualityComparer<TKey>.Default;

            return CreateDistinctByIterator(_this, keySelector, comparer);
        }

        private static IEnumerable<TSource> CreateDistinctByIterator<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>(comparer);
            foreach (var item in source)
            {
                TKey key = keySelector(item);
                if (!seenKeys.Contains(key))
                {
                    seenKeys.Add(key);
                    yield return item;
                }
            }
        }



        /// <summary>
        /// 将一个二维序列中每个序列的每个元素提取出来，重新构建成一个新的序列并返回。
        /// 该方法等效于 System.Collections.Generic.Enumerable.SelectMany 方法。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="_this"/> 中的元素的类型。</typeparam>
        /// <param name="_this">一个要投影的值序列。</param>
        /// <returns>一个 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/>，其元素是 <paramref name="_this"/> 中的每个序列元素。</returns>
        public static IEnumerable<TSource> Flatten<TSource>(this IEnumerable<IEnumerable<TSource>> _this)
        {
            return _this.SelectMany(e => e);
        }



        /// <summary>
        /// 对泛型序列 <paramref name="_this"/> 中的每个元素执行指定操作。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="_this"/> 中的元素的类型。</typeparam>
        /// <param name="_this">一个要泛型序列，包含要执行操作的元素。</param>
        /// <param name="action">要对 <paramref name="_this"/> 的每个元素执行的 <see cref="Action&lt;TSource&gt;"/>。</param>
        /// <returns>一个新的泛型序列 <see cref="IEnumerable&lt;TSource&gt;"/>，其按原结构和顺序包含 <paramref name="_this"/> 中的每个元素。</returns>
        public static IEnumerable<TSource> ForEach<TSource>(this IEnumerable<TSource> _this, Action<TSource> action)
        {
            Check.NotNull(_this);
            foreach (TSource element in _this)
            {
                action(element);
            }
            return _this;
        }

        /// <summary>
        /// 对泛型序列 <paramref name="_this"/> 中的每个元素执行指定操作。将在操作函数的逻辑中使用每个元素的索引。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="_this"/> 中的元素的类型。</typeparam>
        /// <param name="_this">一个要泛型序列，包含要执行操作的元素。</param>
        /// <param name="action">要对 <paramref name="_this"/> 的每个元素执行的 Action&lt;TSource, int&gt;。该函数的第二个参数表示源元素的索引。</param>
        /// <returns>一个新的泛型序列 <see cref="IEnumerable&lt;TSource&gt;"/>，其按原结构和顺序包含 <paramref name="_this"/> 中的每个元素。</returns>
        public static IEnumerable<TSource> ForEach<TSource>(this IEnumerable<TSource> _this, Action<TSource, int> action)
        {
            Check.NotNull(_this);
            TSource[] array = _this.ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                TSource element = array[i];
                action(element, i);
            }
            return _this;
        }

        /// <summary>
        /// 对泛型序列 <paramref name="_this"/> 中的每个元素执行指定操作。该方法返回调用 <paramref name="func"/> 函数时返回 true 值的元素。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="_this"/> 中的元素的类型。</typeparam>
        /// <param name="_this">一个要泛型序列，包含要执行操作的元素。</param>
        /// <param name="func">要对 <paramref name="_this"/> 的每个元素执行的 Func&lt;TSource, bool&gt;。该函数返回一个 bool 值表示操作是否成功。</param>
        /// <param name="breakOnFuncReturnFalse">一个 bool 值，该值指示当 <paramref name="func"/> 被循环调用过程中返回 false 时，是否立即中断循环并返回结果而放弃未被循环调用操作的元素。</param>
        /// <returns>一个新的泛型序列 <see cref="IEnumerable&lt;TSource&gt;"/>，其包含 <paramref name="_this"/> 中每个元素调用 <paramref name="func"/> 时返回 true 的元素列表。</returns>
        public static IEnumerable<TSource> ForEach<TSource>(this IEnumerable<TSource> _this, Func<TSource, bool> func, bool breakOnFuncReturnFalse = false)
        {
            Check.NotNull(_this);
            foreach (TSource element in _this)
            {
                if (!func(element) && breakOnFuncReturnFalse)
                {
                    break;
                }
            }
            return _this;
        }

        /// <summary>
        /// 对泛型序列 <paramref name="_this"/> 中的每个元素执行指定操作。该方法返回调用 <paramref name="func"/> 函数时返回 true 值的元素。将在操作函数的逻辑中使用每个元素的索引。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="_this"/> 中的元素的类型。</typeparam>
        /// <param name="_this">一个要泛型序列，包含要执行操作的元素。</param>
        /// <param name="func">要对 <paramref name="_this"/> 的每个元素执行的 Func&lt;TSource, int, bool&gt;。该函数的第二个参数表示源元素的索引。该函数返回一个 bool 值表示操作是否成功。</param>
        /// <param name="breakOnFuncReturnFalse">一个 bool 值，该值指示当 <paramref name="func"/> 被循环调用过程中返回 false 时，是否立即中断循环并返回结果而放弃未被循环调用操作的元素。</param>
        /// <returns>一个新的泛型序列 <see cref="IEnumerable&lt;TSource&gt;"/>，其包含 <paramref name="_this"/> 中每个元素调用 <paramref name="func"/> 时返回 true 的元素列表。</returns>
        public static IEnumerable<TSource> ForEach<TSource>(this IEnumerable<TSource> _this, Func<TSource, int, bool> func, bool breakOnFuncReturnFalse = false)
        {
            Check.NotNull(_this);
            TSource[] array = _this.ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                TSource element = array[i];
                if (!func(element, i) && breakOnFuncReturnFalse)
                {
                    break;
                }
            }
            return _this;
        }




        /// <summary>
        /// 返回源序列 <paramref name="_this"/> 中的一个子集。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="_this"/> 中的元素的类型。</typeparam>
        /// <param name="_this">一个 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/>，用于从中返回元素。</param>
        /// <param name="startIndex">返回剩余元素前要跳过的元素数量，也可理解为范围开始处的从零开始的 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/> 索引。</param>
        /// <returns>一个 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/>，它表示源序列 <paramref name="_this"/> 中的一个子集。</returns>
        public static IEnumerable<TSource> GetRange<TSource>(this IEnumerable<TSource> _this, int startIndex)
        {
            return _this.Skip(startIndex);
        }

        /// <summary>
        /// 返回源序列 <paramref name="_this"/> 中的一个子集。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="_this"/> 中的元素的类型。</typeparam>
        /// <param name="_this">一个 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/>，用于从中返回元素。</param>
        /// <param name="startIndex">返回剩余元素前要跳过的元素数量，也可理解为范围开始处的从零开始的 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/> 索引。</param>
        /// <param name="count">范围中的元素数，同时也表示要返回的元素数量。</param>
        /// <returns>一个 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/>，它表示源序列 <paramref name="_this"/> 中的一个子集。</returns>
        public static IEnumerable<TSource> GetRange<TSource>(this IEnumerable<TSource> _this, int startIndex, int count)
        {
            return _this.Skip(startIndex).Take(count);
        }




        /// <summary>
        /// 将 <see cref="IEnumerable&lt;TSource&gt;"/> 的元素转换为指定的类型。序列中不能成功转换的项将不会包含在返回结果中。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="_this"/> 中的元素的类型。</typeparam>
        /// <typeparam name="TResult"><paramref name="_this"/> 中的元素要转换成的类型。</typeparam>
        /// <param name="_this">一个要转换类型的值序列。</param>
        /// <returns>
        /// 一个 IEnumerable&lt;TResult&gt;，其元素为对 <paramref name="_this"/> 的每个元素调用转换函数的结果。
        /// 序列中不能成功转换（转换过程中抛出异常或者转换结果为 Null）的项将不会包含在返回结果中。
        /// </returns>
        public static IEnumerable<TResult> Map<TSource, TResult>(this IEnumerable<TSource> _this) where TResult : class
        {
            Check.NotNull(_this);
            return _this.Select(item => item is TResult ? (TResult)(object)item : null).Where(item => item != null);
        }

        /// <summary>
        /// 将 <see cref="IEnumerable&lt;TSource&gt;"/> 的元素转换为指定的类型。序列中不能成功转换的项将不会包含在返回结果中。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="_this"/> 中的元素的类型。</typeparam>
        /// <typeparam name="TResult"><paramref name="_this"/> 中的元素要转换成的类型。</typeparam>
        /// <param name="_this">一个要转换类型的值序列。</param>
        /// <param name="converter">应用于每个元素的转换函数。</param>
        /// <returns>
        /// 一个 IEnumerable&lt;TResult&gt;，其元素为对 <paramref name="_this"/> 的每个元素调用转换函数的结果。
        /// 序列中不能成功转换（转换过程中抛出异常或者转换结果为 Null）的项将不会包含在返回结果中。
        /// </returns>
        public static IEnumerable<TResult> Map<TSource, TResult>(this IEnumerable<TSource> _this, Func<TSource, TResult> converter) where TResult : class
        {
            Check.NotNull(_this);
            return _this.Select(item => Trying.Try(() => converter(item), (exception) => null));
        }

        /// <summary>
        /// 将 <see cref="IEnumerable&lt;TSource&gt;"/> 的元素转换为指定的类型。序列中不能成功转换的项将不会包含在返回结果中。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="_this"/> 中的元素的类型。</typeparam>
        /// <typeparam name="TResult"><paramref name="_this"/> 中的元素要转换成的类型。</typeparam>
        /// <param name="_this">一个要转换类型的值序列。</param>
        /// <param name="converter">应用于每个元素的转换函数。</param>
        /// <returns>
        /// 一个 IEnumerable&lt;TResult&gt;，其元素为对 <paramref name="_this"/> 的每个元素调用转换函数的结果。
        /// 序列中不能成功转换（转换过程中抛出异常或者转换结果为 Null）的项将不会包含在返回结果中。
        /// </returns>
        public static IEnumerable<TResult> Map<TSource, TResult>(this IEnumerable<TSource> _this, Func<TSource, TResult?> converter) where TResult : struct
        {
            Check.NotNull(_this);
            return _this.Select(item => Trying.Try(() => converter(item), (exception) => new Nullable<TResult>())).Where(item => item.HasValue).Select(item => item.Value);
        }

        /// <summary>
        /// 将 <see cref="IEnumerable&lt;TSource&gt;"/> 的元素转换为指定的类型。序列中不能成功转换的项将不会包含在返回结果中。将在操作函数的逻辑中使用每个元素的索引。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="_this"/> 中的元素的类型。</typeparam>
        /// <typeparam name="TResult"><paramref name="_this"/> 中的元素要转换成的类型。</typeparam>
        /// <param name="_this">一个要转换类型的值序列。</param>
        /// <param name="converter">应用于每个元素的转换函数。该函数的第二个参数表示源元素的索引。</param>
        /// <returns>
        /// 一个 IEnumerable&lt;TResult&gt;，其元素为对 <paramref name="_this"/> 的每个元素调用转换函数的结果。
        /// 序列中不能成功转换（转换过程中抛出异常或者转换结果为 Null）的项将不会包含在返回结果中。
        /// </returns>
        public static IEnumerable<TResult> Map<TSource, TResult>(this IEnumerable<TSource> _this, Func<TSource, int, TResult> converter) where TResult : class
        {
            Check.NotNull(_this);
            return _this.Select((i, item) => Trying.Try(() => converter(i, item), (exception) => null));
        }

        /// <summary>
        /// 将 <see cref="IEnumerable&lt;TSource&gt;"/> 的元素转换为指定的类型。序列中不能成功转换的项将不会包含在返回结果中。将在操作函数的逻辑中使用每个元素的索引。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="_this"/> 中的元素的类型。</typeparam>
        /// <typeparam name="TResult"><paramref name="_this"/> 中的元素要转换成的类型。</typeparam>
        /// <param name="_this">一个要转换类型的值序列。</param>
        /// <param name="converter">应用于每个元素的转换函数。该函数的第二个参数表示源元素的索引。</param>
        /// <returns>
        /// 一个 IEnumerable&lt;TResult&gt;，其元素为对 <paramref name="_this"/> 的每个元素调用转换函数的结果。
        /// 序列中不能成功转换（转换过程中抛出异常或者转换结果为 Null）的项将不会包含在返回结果中。
        /// </returns>
        public static IEnumerable<TResult> Map<TSource, TResult>(this IEnumerable<TSource> _this, Func<TSource, int, TResult?> converter) where TResult : struct
        {
            Check.NotNull(_this);
            return _this.Select((i, item) => Trying.Try(() => converter(i, item), (exception) => new Nullable<TResult>())).Where(item => item.HasValue).Select(item => item.Value);
        }




        /// <summary>
        /// 根据键和指定的排序规则对序列的元素排序。
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <typeparam name="TKey">keySelector 返回的键的类型。</typeparam>
        /// <param name="source">一个要排序的值序列。</param>
        /// <param name="keySelector">用于从元素中提取键的函数。</param>
        /// <param name="sortDirection">用于指定数据排序方式，升序还是降序。</param>
        /// <returns>一个 System.Linq.IOrderedEnumerable(TElement)，其元素按键排序。</returns>
        /// <exception cref="System.ArgumentNullException">source 或 keySelector 为 null。</exception>
        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, ListSortDirection sortDirection)
        {
            return sortDirection == ListSortDirection.Ascending ? source.OrderBy(keySelector) : source.OrderByDescending(keySelector);
        }

        /// <summary>
        /// 使用指定的比较器和排序规则对序列的元素排序。
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <typeparam name="TKey">keySelector 返回的键的类型。</typeparam>
        /// <param name="source">一个要排序的值序列。</param>
        /// <param name="keySelector">用于从元素中提取键的函数。</param>
        /// <param name="comparer">一个用于比较键的 System.Collections.Generic.IComparer&lt;TSource&gt;。</param>
        /// <param name="sortDirection">用于指定数据排序方式，升序还是降序。</param>
        /// <returns>一个 System.Linq.IOrderedEnumerable(TElement)，其元素按键排序。</returns>
        /// <exception cref="System.ArgumentNullException">source 或 keySelector 为 null。</exception>
        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer, ListSortDirection sortDirection)
        {
            return sortDirection == ListSortDirection.Ascending ? source.OrderBy(keySelector, comparer) : source.OrderByDescending(keySelector, comparer);
        }






        /// <summary>
        /// 使用默认比较器对整个 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/> 中的元素进行排序。
        /// 并返回该序列排序操作后的一个副本。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="_this"/> 中的元素的类型。</typeparam>
        /// <param name="_this">一个要进行排序操作的值序列。</param>
        /// <returns>一个 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/> ，它是源序列 <paramref name="_this"/> 排序操作后的一个副本。</returns>
        public static IEnumerable<TSource> Sort<TSource>(this IEnumerable<TSource> _this)
        {
            List<TSource> list = _this.ToList();
            list.Sort();
            return list;
        }

        /// <summary>
        /// 使用指定的 <see cref="System.Comparison&lt;TSource&gt;"/> 对整个 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/> 中的元素进行排序。
        /// 并返回该序列排序操作后的一个副本。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="_this"/> 中的元素的类型。</typeparam>
        /// <param name="_this">一个要进行排序操作的值序列。</param>
        /// <param name="comparison">比较元素时要使用的 <see cref="System.Comparison&lt;TSource&gt;"/>。</param>
        /// <returns>一个 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/> ，它是源序列 <paramref name="_this"/> 排序操作后的一个副本。</returns>
        public static IEnumerable<TSource> Sort<TSource>(this IEnumerable<TSource> _this, Comparison<TSource> comparison)
        {
            List<TSource> list = _this.ToList();
            list.Sort(comparison);
            return list;
        }

        /// <summary>
        /// 使用指定的 <see cref="System.Collections.Generic.IComparer&lt;TSource&gt;"/> 对整个 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/> 中的元素进行排序。
        /// 并返回该序列排序操作后的一个副本。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="_this"/> 中的元素的类型。</typeparam>
        /// <param name="_this">一个要进行排序操作的值序列。</param>
        /// <param name="comparer">
        /// 比较元素时要使用的 <see cref="System.Collections.Generic.IComparer&lt;TSource&gt;"/> 实现，或者
        /// 为 null，表示使用默认比较器 <seealso cref="System.Collections.Generic.Comparer&lt;TSource&gt;.Default"/>。
        /// </param>
        /// <returns>一个 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/> ，它是源序列 <paramref name="_this"/> 排序操作后的一个副本。</returns>
        public static IEnumerable<TSource> Sort<TSource>(this IEnumerable<TSource> _this, IComparer<TSource> comparer)
        {
            List<TSource> list = _this.ToList();
            list.Sort(comparer);
            return list;
        }

        /// <summary>
        /// 将一组序列中的所有元素进行随机排序，并返回该序列随机排序操作后的一个副本。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="_this"/> 中的元素的类型。</typeparam>
        /// <param name="_this">一个要进行随机排序操作的值序列。</param>
        /// <returns>一个 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/> ，它是源序列 <paramref name="_this"/> 进行随机排序操作后的一个副本。</returns>
        public static IEnumerable<TSource> SortRandom<TSource>(this IEnumerable<TSource> _this)
        {
            Check.NotNull(_this);
            List<TSource>
                temps = _this.ToList(),
                list = new List<TSource>();

            Random r = new Random(Guid.NewGuid().GetHashCode());
            while (temps.Count > 0)
            {
                var i = r.Next(temps.Count);
                var item = temps[i];
                temps.RemoveAt(i);
                list.Add(item);
            }
            return list;
        }



        /// <summary>
        /// 以 <seealso cref="DataPager.DefaultPageSize"/> 值指示的默认页面尺寸大小获取泛型序列中的第一页(pageIndex 为 0)分页数据。
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <param name="source">一个 System.Collections.Generic.IEnumerable&lt;TSource&gt;，用于从中返回分页元素。</param>
        /// <returns>
        /// 返回一个 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/>，返回的序列为源序列 source 的子集，其表示 source 第 1 页(pageIndex 为 0)的
        /// 数据，返回序列中元素数量为 <seealso cref="DataPager.DefaultPageSize"/>(如果是返回的是最后一页数据，其数量可能不足 <seealso cref="DataPager.DefaultPageSize"/>)。
        /// </returns>
        public static IEnumerable<TSource> SplitPage<TSource>(this IEnumerable<TSource> source)
        {
            return SplitPage(source, 0, DataPager.DefaultPageSize);
        }

        /// <summary>
        /// 以指定的页面索引号、<seealso cref="DataPager.DefaultPageSize"/> 值指示的默认页面尺寸大小获取泛型序列中的分页数据。
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <param name="source">一个 System.Collections.Generic.IEnumerable&lt;TSource&gt;，用于从中返回分页元素。</param>
        /// <param name="pageIndex">返回数据页所在源序列中的页面索引号，从 0 开始计数。</param>
        /// <returns>
        /// 返回一个 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/>，返回的序列为源序列 source 的子集，其表示 source 第 pageIndex(从 0 开始计数) 页的
        /// 数据，返回序列中元素数量为 <seealso cref="DataPager.DefaultPageSize"/>(如果是返回的是最后一页数据，其数量可能不足 <seealso cref="DataPager.DefaultPageSize"/>)。
        /// </returns>
        public static IEnumerable<TSource> SplitPage<TSource>(this IEnumerable<TSource> source, int pageIndex)
        {
            return SplitPage(source, pageIndex);
        }

        /// <summary>
        /// 以指定的页面索引号、页面尺寸大小获取泛型序列中的分页数据。
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <param name="source">一个 System.Collections.Generic.IEnumerable&lt;TSource&gt;，用于从中返回分页元素。</param>
        /// <param name="pageIndex">返回数据页所在源序列中的页面索引号，从 0 开始计数。</param>
        /// <param name="pageSize">返回数据页的页面尺寸，即一页有多少行数据。</param>
        /// <returns>
        /// 返回一个 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/>，返回的序列为源序列 source 的子集，其表示 source 第 pageIndex(从 0 开始计数) 页的
        /// 数据，返回序列中元素数量为 pageSize(如果是返回的是最后一页数据，其数量可能不足 pageSize)。
        /// </returns>
        public static IEnumerable<TSource> SplitPage<TSource>(this IEnumerable<TSource> source, int pageIndex, int pageSize)
        {
            return source.GetRange(pageIndex * pageSize, pageSize);
        }


        /// <summary>
        /// 以 <seealso cref="DataPager.DefaultPageSize"/> 值指示的默认页面尺寸大小和数据排序方式获取泛型序列中的第一页(pageIndex 为 0)分页数据。
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <param name="source">一个 System.Collections.Generic.IEnumerable&lt;TSource&gt;，用于从中返回分页元素。</param>
        /// <param name="sorter">用于排序比较元素时要使用的 <see cref="System.Comparison&lt;TSource&gt;"/>。</param>
        /// <returns>
        /// 返回一个 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/>，返回的序列为源序列 source 的子集，其表示 source 第 1 页(pageIndex 为 0)的
        /// 数据，返回序列中元素数量为 <seealso cref="DataPager.DefaultPageSize"/>(如果是返回的是最后一页数据，其数量可能不足 <seealso cref="DataPager.DefaultPageSize"/>)。
        /// </returns>
        public static IEnumerable<TSource> SplitPage<TSource>(this IEnumerable<TSource> source, Comparison<TSource> sorter)
        {
            return SplitPage(source, 0, sorter);
        }

        /// <summary>
        /// 以指定的页面索引号、<seealso cref="DataPager.DefaultPageSize"/> 值指示的默认页面尺寸大小和数据排序方式获取泛型序列中的分页数据。
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <param name="source">一个 System.Collections.Generic.IEnumerable&lt;TSource&gt;，用于从中返回分页元素。</param>
        /// <param name="pageIndex">返回数据页所在源序列中的页面索引号，从 0 开始计数。</param>
        /// <param name="sorter">用于排序比较元素时要使用的 <see cref="System.Comparison&lt;TSource&gt;"/>。</param>
        /// <returns>
        /// 返回一个 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/>，返回的序列为源序列 source 的子集，其表示 source 第 pageIndex(从 0 开始计数) 页的
        /// 数据，返回序列中元素数量为 <seealso cref="DataPager.DefaultPageSize"/>(如果是返回的是最后一页数据，其数量可能不足 <seealso cref="DataPager.DefaultPageSize"/>)。
        /// </returns>
        public static IEnumerable<TSource> SplitPage<TSource>(this IEnumerable<TSource> source, int pageIndex, Comparison<TSource> sorter)
        {
            return SplitPage(source, pageIndex, DataPager.DefaultPageSize, sorter);
        }

        /// <summary>
        /// 以指定的页面索引号、页面尺寸大小和数据排序方式获取泛型序列中的分页数据。
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <param name="source">一个 System.Collections.Generic.IEnumerable&lt;TSource&gt;，用于从中返回分页元素。</param>
        /// <param name="pageIndex">返回数据页所在源序列中的页面索引号，从 0 开始计数。</param>
        /// <param name="pageSize">返回数据页的页面尺寸，即一页有多少行数据。</param>
        /// <param name="sorter">用于排序比较元素时要使用的 <see cref="System.Comparison&lt;TSource&gt;"/>。</param>
        /// <returns>
        /// 返回一个 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/>，返回的序列为源序列 source 的子集，其表示 source 第 pageIndex(从 0 开始计数) 页的
        /// 数据，返回序列中元素数量为 pageSize(如果是返回的是最后一页数据，其数量可能不足 pageSize)。
        /// </returns>
        public static IEnumerable<TSource> SplitPage<TSource>(this IEnumerable<TSource> source, int pageIndex, int pageSize, Comparison<TSource> sorter)
        {
            return Sort(source, sorter).GetRange(pageIndex * pageSize, pageSize);
        }


        /// <summary>
        /// 以 <seealso cref="DataPager.DefaultPageSize"/> 值指示的默认页面尺寸大小和数据排序方式获取泛型序列中的第一页(pageIndex 为 0)分页数据。
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <param name="source">一个 System.Collections.Generic.IEnumerable&lt;TSource&gt;，用于从中返回分页元素。</param>
        /// <param name="sorter">用于排序比较元素时要使用的 <see cref="System.Collections.Generic.IComparer&lt;TSource&gt;"/>。</param>
        /// <returns>
        /// 返回一个 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/>，返回的序列为源序列 source 的子集，其表示 source 第 1 页(pageIndex 为 0)的
        /// 数据，返回序列中元素数量为 <seealso cref="DataPager.DefaultPageSize"/>(如果是返回的是最后一页数据，其数量可能不足 <seealso cref="DataPager.DefaultPageSize"/>)。
        /// </returns>
        public static IEnumerable<TSource> SplitPage<TSource>(this IEnumerable<TSource> source, IComparer<TSource> sorter)
        {
            return SplitPage(source, 0, sorter);
        }

        /// <summary>
        /// 以指定的页面索引号、<seealso cref="DataPager.DefaultPageSize"/> 值指示的默认页面尺寸大小和数据排序方式获取泛型序列中的分页数据。
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <param name="source">一个 System.Collections.Generic.IEnumerable&lt;TSource&gt;，用于从中返回分页元素。</param>
        /// <param name="pageIndex">返回数据页所在源序列中的页面索引号，从 0 开始计数。</param>
        /// <param name="sorter">用于排序比较元素时要使用的 <see cref="System.Collections.Generic.IComparer&lt;TSource&gt;"/>。</param>
        /// <returns>
        /// 返回一个 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/>，返回的序列为源序列 source 的子集，其表示 source 第 pageIndex(从 0 开始计数) 页的
        /// 数据，返回序列中元素数量为 <seealso cref="DataPager.DefaultPageSize"/>(如果是返回的是最后一页数据，其数量可能不足 <seealso cref="DataPager.DefaultPageSize"/>)。
        /// </returns>
        public static IEnumerable<TSource> SplitPage<TSource>(this IEnumerable<TSource> source, int pageIndex, IComparer<TSource> sorter)
        {
            return SplitPage(source, pageIndex, DataPager.DefaultPageSize, sorter);
        }

        /// <summary>
        /// 以指定的页面索引号、页面尺寸大小和数据排序方式获取泛型序列中的分页数据。
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <param name="source">一个 System.Collections.Generic.IEnumerable&lt;TSource&gt;，用于从中返回分页元素。</param>
        /// <param name="pageIndex">返回数据页所在源序列中的页面索引号，从 0 开始计数。</param>
        /// <param name="pageSize">返回数据页的页面尺寸，即一页有多少行数据。</param>
        /// <param name="sorter">用于排序比较元素时要使用的 <see cref="System.Collections.Generic.IComparer&lt;TSource&gt;"/>。</param>
        /// <returns>
        /// 返回一个 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/>，返回的序列为源序列 source 的子集，其表示 source 第 pageIndex(从 0 开始计数) 页的
        /// 数据，返回序列中元素数量为 pageSize(如果是返回的是最后一页数据，其数量可能不足 pageSize)。
        /// </returns>
        public static IEnumerable<TSource> SplitPage<TSource>(this IEnumerable<TSource> source, int pageIndex, int pageSize, IComparer<TSource> sorter)
        {
            return Sort(source, sorter).GetRange(pageIndex * pageSize, pageSize);
        }



        public static IEnumerable<TSource> SplitPage<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            return SplitPage(source, 0, keySelector, sortDirection);
        }

        public static IEnumerable<TSource> SplitPage<TSource, TKey>(this IEnumerable<TSource> source, int pageIndex, Func<TSource, TKey> keySelector, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            return SplitPage(source, pageIndex, DataPager.DefaultPageSize, keySelector, sortDirection);
        }

        public static IEnumerable<TSource> SplitPage<TSource, TKey>(this IEnumerable<TSource> source, int pageIndex, int pageSize, Func<TSource, TKey> keySelector, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            return OrderBy(source, keySelector, sortDirection).GetRange(pageIndex * pageSize, pageSize);
        }


        public static IEnumerable<TSource> SplitPage<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            return SplitPage(source, 0, keySelector, comparer, sortDirection);
        }

        public static IEnumerable<TSource> SplitPage<TSource, TKey>(this IEnumerable<TSource> source, int pageIndex, Func<TSource, TKey> keySelector, IComparer<TKey> comparer, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            return SplitPage(source, pageIndex, DataPager.DefaultPageSize, keySelector, comparer, sortDirection);
        }

        public static IEnumerable<TSource> SplitPage<TSource, TKey>(this IEnumerable<TSource> source, int pageIndex, int pageSize, Func<TSource, TKey> keySelector, IComparer<TKey> comparer, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            return OrderBy(source, keySelector, comparer, sortDirection).GetRange(pageIndex * pageSize, pageSize);
        }




        /// <summary>
        /// 以 <seealso cref="DataPager.DefaultPageSize"/> 值指示的默认页面尺寸大小对序列进行分页处理并返回分页后第一页(pageIndex 为 0)数据所构成的分页结果。
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。>source 中的元素的类型。</typeparam>
        /// <param name="source">一个 System.Collections.Generic.IEnumerable&lt;TSource&gt;，用于进行分页处理。</param>
        /// <returns>
        /// 返回一个 <see cref="NDF.ComponentModel.Paging.PagingData&lt;TSource&gt;"/>：
        /// 其 Data 属性表示的页面数据内容为源序列 source 的子集，其 PageSize 属性为 <seealso cref="DataPager.DefaultPageSize"/> 属性值，
        /// 其 PageIndex 属性为 0(表示第一页)，其 RowCount 属性为源序列 source 的总元素数量。
        /// </returns>
        public static PagingData<TSource> ToPagingData<TSource>(this IEnumerable<TSource> source)
        {
            return ToPagingData(source, 0);
        }

        /// <summary>
        /// 以指定的页面索引号、<seealso cref="DataPager.DefaultPageSize"/> 值指示的默认页面尺寸大小对序列进行分页处理并返回分页后的结果。
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <param name="source">一个 System.Collections.Generic.IEnumerable&lt;TSource&gt;，用于进行分页处理。</param>
        /// <param name="pageIndex">返回数据页所在源序列中的页面索引号，从 0 开始计数。</param>
        /// <returns>
        /// 返回一个 <see cref="NDF.ComponentModel.Paging.PagingData&lt;TSource&gt;"/>：
        /// 其 Data 属性表示的页面数据内容为源序列 source 的子集，其 PageSize 属性为 <seealso cref="DataPager.DefaultPageSize"/> 属性值，
        /// 其 PageIndex 属性为传入的参数 pageIndex，其 RowCount 属性为源序列 source 的总元素数量。
        /// </returns>
        public static PagingData<TSource> ToPagingData<TSource>(this IEnumerable<TSource> source, int pageIndex)
        {
            return ToPagingData(source, pageIndex, DataPager.DefaultPageSize);
        }

        /// <summary>
        /// 以指定的页面索引号、页面尺寸大小对序列进行分页处理并返回分页后的结果。
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <param name="source">一个 System.Collections.Generic.IEnumerable&lt;TSource&gt;，用于进行分页处理。</param>
        /// <param name="pageIndex">返回数据页所在源序列中的页面索引号，从 0 开始计数。</param>
        /// <param name="pageSize">返回数据页的页面尺寸，即一页有多少行数据。</param>
        /// <returns>
        /// 返回一个 <see cref="NDF.ComponentModel.Paging.PagingData&lt;TSource&gt;"/>：
        /// 其 Data 属性表示的页面数据内容为源序列 source 的子集，其 PageSize 属性为传入的参数 pageSize，
        /// 其 PageIndex 属性为传入的参数 pageIndex，其 RowCount 属性为源序列 source 的总元素数量。
        /// </returns>
        public static PagingData<TSource> ToPagingData<TSource>(this IEnumerable<TSource> source, int pageIndex, int pageSize)
        {
            return new PagingData<TSource>(pageIndex, pageSize, source.Count(), SplitPage(source, pageIndex, pageSize));
        }


        /// <summary>
        /// 以 <seealso cref="DataPager.DefaultPageSize"/> 值指示的默认页面尺寸大小和数据排序方式对序列进行分页处理并返回分页后第一页(pageIndex 为 0)数据所构成的分页结果。
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <param name="source">一个 System.Collections.Generic.IEnumerable&lt;TSource&gt;，用于进行分页处理。</param>
        /// <param name="sorter">用于排序比较元素时要使用的 <see cref="System.Comparison&lt;TSource&gt;"/>。</param>
        /// <returns>
        /// 返回一个 <see cref="NDF.ComponentModel.Paging.PagingData&lt;TSource&gt;"/>：
        /// 其 Data 属性表示的页面数据内容为源序列 source 的子集，其 PageSize 属性为 <seealso cref="DataPager.DefaultPageSize"/> 属性值，
        /// 其 PageIndex 属性为 0(表示第一页)，其 RowCount 属性为源序列 source 的总元素数量。
        /// </returns>
        public static PagingData<TSource> ToPagingData<TSource>(this IEnumerable<TSource> source, Comparison<TSource> sorter)
        {
            return ToPagingData(source, 0, sorter);
        }

        /// <summary>
        /// 以指定的页面索引号、<seealso cref="DataPager.DefaultPageSize"/> 值指示的默认页面尺寸大小和数据排序方式对序列进行分页处理并返回分页后的结果。
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <param name="source">一个 System.Collections.Generic.IEnumerable&lt;TSource&gt;，用于进行分页处理。</param>
        /// <param name="pageIndex">返回数据页所在源序列中的页面索引号，从 0 开始计数。</param>
        /// <param name="sorter">用于排序比较元素时要使用的 <see cref="System.Comparison&lt;TSource&gt;"/>。</param>
        /// <returns>
        /// 返回一个 <see cref="NDF.ComponentModel.Paging.PagingData&lt;TSource&gt;"/>：
        /// 其 Data 属性表示的页面数据内容为源序列 source 的子集，其 PageSize 属性为 <seealso cref="DataPager.DefaultPageSize"/> 属性值，
        /// 其 PageIndex 属性为传入的参数 pageIndex，其 RowCount 属性为源序列 source 的总元素数量。
        /// </returns>
        public static PagingData<TSource> ToPagingData<TSource>(this IEnumerable<TSource> source, int pageIndex, Comparison<TSource> sorter)
        {
            return ToPagingData(source, pageIndex, DataPager.DefaultPageSize, sorter);
        }

        /// <summary>
        /// 以指定的页面索引号、页面尺寸大小和数据排序方式对序列进行分页处理并返回分页后的结果。
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <param name="source">一个 System.Collections.Generic.IEnumerable&lt;TSource&gt;，用于进行分页处理。</param>
        /// <param name="pageIndex">返回数据页所在源序列中的页面索引号，从 0 开始计数。</param>
        /// <param name="pageSize">返回数据页的页面尺寸，即一页有多少行数据。</param>
        /// <param name="sorter">用于排序比较元素时要使用的 <see cref="System.Comparison&lt;TSource&gt;"/>。</param>
        /// <returns>
        /// 返回一个 <see cref="NDF.ComponentModel.Paging.PagingData&lt;TSource&gt;"/>：
        /// 其 Data 属性表示的页面数据内容为源序列 source 的子集，其 PageSize 属性为传入的参数 pageSize，
        /// 其 PageIndex 属性为传入的参数 pageIndex，其 RowCount 属性为源序列 source 的总元素数量。
        /// </returns>
        public static PagingData<TSource> ToPagingData<TSource>(this IEnumerable<TSource> source, int pageIndex, int pageSize, Comparison<TSource> sorter)
        {
            return new PagingData<TSource>(pageIndex, pageSize, source.Count(), SplitPage(source, pageIndex, pageSize, sorter));
        }


        /// <summary>
        /// 以 <seealso cref="DataPager.DefaultPageSize"/> 值指示的默认页面尺寸大小和数据排序方式对序列进行分页处理并返回分页后第一页(pageIndex 为 0)数据所构成的分页结果。
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <param name="source">一个 System.Collections.Generic.IEnumerable&lt;TSource&gt;，用于进行分页处理。</param>
        /// <param name="sorter">用于排序比较元素时要使用的 <see cref="System.Collections.Generic.IComparer&lt;TSource&gt;"/>。</param>
        /// <returns>
        /// 返回一个 <see cref="NDF.ComponentModel.Paging.PagingData&lt;TSource&gt;"/>：
        /// 其 Data 属性表示的页面数据内容为源序列 source 的子集，其 PageSize 属性为 <seealso cref="DataPager.DefaultPageSize"/> 属性值，
        /// 其 PageIndex 属性为 0(表示第一页)，其 RowCount 属性为源序列 source 的总元素数量。
        /// </returns>
        public static PagingData<TSource> ToPagingData<TSource>(this IEnumerable<TSource> source, IComparer<TSource> sorter)
        {
            return ToPagingData(source, 0, sorter);
        }

        /// <summary>
        /// 以指定的页面索引号、<seealso cref="DataPager.DefaultPageSize"/> 值指示的默认页面尺寸大小和数据排序方式对序列进行分页处理并返回分页后的结果。
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <param name="source">一个 System.Collections.Generic.IEnumerable&lt;TSource&gt;，用于进行分页处理。</param>
        /// <param name="pageIndex">返回数据页所在源序列中的页面索引号，从 0 开始计数。</param>
        /// <param name="sorter">用于排序比较元素时要使用的 <see cref="System.Collections.Generic.IComparer&lt;TSource&gt;"/>。</param>
        /// <returns>
        /// 返回一个 <see cref="NDF.ComponentModel.Paging.PagingData&lt;TSource&gt;"/>：
        /// 其 Data 属性表示的页面数据内容为源序列 source 的子集，其 PageSize 属性为 <seealso cref="DataPager.DefaultPageSize"/> 属性值，
        /// 其 PageIndex 属性为传入的参数 pageIndex，其 RowCount 属性为源序列 source 的总元素数量。
        /// </returns>
        public static PagingData<TSource> ToPagingData<TSource>(this IEnumerable<TSource> source, int pageIndex, IComparer<TSource> sorter)
        {
            return ToPagingData(source, pageIndex, DataPager.DefaultPageSize, sorter);
        }

        /// <summary>
        /// 以指定的页面索引号、页面尺寸大小和数据排序方式对序列进行分页处理并返回分页后的结果。
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <param name="source">一个 System.Collections.Generic.IEnumerable&lt;TSource&gt;，用于进行分页处理。</param>
        /// <param name="pageIndex">返回数据页所在源序列中的页面索引号，从 0 开始计数。</param>
        /// <param name="pageSize">返回数据页的页面尺寸，即一页有多少行数据。</param>
        /// <param name="sorter">用于排序比较元素时要使用的 <see cref="System.Collections.Generic.IComparer&lt;TSource&gt;"/>。</param>
        /// <returns>
        /// 返回一个 <see cref="NDF.ComponentModel.Paging.PagingData&lt;TSource&gt;"/>：
        /// 其 Data 属性表示的页面数据内容为源序列 source 的子集，其 PageSize 属性为传入的参数 pageSize，
        /// 其 PageIndex 属性为传入的参数 pageIndex，其 RowCount 属性为源序列 source 的总元素数量。
        /// </returns>
        public static PagingData<TSource> ToPagingData<TSource>(this IEnumerable<TSource> source, int pageIndex, int pageSize, IComparer<TSource> sorter)
        {
            return new PagingData<TSource>(pageIndex, pageSize, source.Count(), SplitPage(source, pageIndex, pageSize, sorter));
        }



        public static PagingData<TSource> ToPagingData<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            return ToPagingData(source, 0, keySelector, sortDirection);
        }

        public static PagingData<TSource> ToPagingData<TSource, TKey>(this IEnumerable<TSource> source, int pageIndex, Func<TSource, TKey> keySelector, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            return ToPagingData(source, pageIndex, DataPager.DefaultPageSize, keySelector, sortDirection);
        }

        public static PagingData<TSource> ToPagingData<TSource, TKey>(this IEnumerable<TSource> source, int pageIndex, int pageSize, Func<TSource, TKey> keySelector, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            return new PagingData<TSource>(pageIndex, pageSize, source.Count(), SplitPage(source, pageIndex, pageSize, keySelector, sortDirection));
        }


        public static PagingData<TSource> ToPagingData<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            return ToPagingData(source, 0, keySelector, comparer, sortDirection);
        }

        public static PagingData<TSource> ToPagingData<TSource, TKey>(this IEnumerable<TSource> source, int pageIndex, Func<TSource, TKey> keySelector, IComparer<TKey> comparer, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            return ToPagingData(source, pageIndex, DataPager.DefaultPageSize, keySelector, comparer, sortDirection);
        }

        public static PagingData<TSource> ToPagingData<TSource, TKey>(this IEnumerable<TSource> source, int pageIndex, int pageSize, Func<TSource, TKey> keySelector, IComparer<TKey> comparer, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            return new PagingData<TSource>(pageIndex, pageSize, source.Count(), SplitPage(source, pageIndex, pageSize, keySelector, comparer, sortDirection));
        }



        /// <summary>
        /// 将一个泛型序列转换成一个 <see cref="DataTable"/>，表格中的每个 DataRow 表示泛型序列中的一个元素。
        /// </summary>
        /// <typeparam name="TSource">泛型序列中元素的类型。</typeparam>
        /// <param name="_this">一个要进行转换操作的泛型序列。</param>
        /// <returns>返回一个 <see cref="DataTable"/>，表格中的每个 DataRow 表示泛型序列中的一个元素</returns>
        public static DataTable ToDataTable<TSource>(this IEnumerable<TSource> _this)
        {
            Type type = typeof(TSource);
            DataTable table = new DataTable(type.Name);

            IEnumerable<PropertyInfo> properties = type.GetProperties().Where(p => p.GetMethod != null);

            foreach (var p in properties)
            {
                DataColumn column = new DataColumn(p.Name);
                column.DataType = p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)
                    ? Nullable.GetUnderlyingType(p.PropertyType)
                    : p.PropertyType;
                table.Columns.Add(column);
            }

            foreach (TSource item in _this)
            {
                DataRow row = table.NewRow();
                foreach (PropertyInfo p in properties)
                {
                    object value = p.GetValue(item);
                    row[p.Name] = value == null ? DBNull.Value : value;
                }
                table.Rows.Add(row);
            }
            return table;
        }




        /// <summary>
        /// 将一个元素添加至泛型序列 IEnumerable&lt;TSource&gt; 的起始位置。并返回该序列添加元素后的一个副本。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="_this"/> 中的元素的类型。</typeparam>
        /// <param name="_this">一个要进行合并操作的值序列。</param>
        /// <param name="item">将被添加值 <paramref name="_this"/> 序列起始位置的元素。</param>
        /// <returns>一个 IEnumerable&lt;TSource&gt; ，它是源序列 <paramref name="_this"/> 排序操作后的一个副本，同时包含新添加的元素 <paramref name="item"/>。</returns>
        public static IEnumerable<TSource> Prepend<TSource>(this IEnumerable<TSource> _this, TSource item)
        {
            List<TSource> list = _this.ToList();
            list.Insert(0, item);
            return list;
        }

        /// <summary>
        /// 将一个元素添加至泛型序列 IEnumerable&lt;TSource&gt; 的结束位置。并返回该序列添加元素后的一个副本。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="_this"/> 中的元素的类型。</typeparam>
        /// <param name="_this">一个要进行合并操作的值序列。</param>
        /// <param name="item">将被添加值 <paramref name="_this"/> 序列结束位置的元素。</param>
        /// <returns>一个 IEnumerable&lt;TSource&gt; ，它是源序列 <paramref name="_this"/> 排序操作后的一个副本，同时包含新添加的元素 <paramref name="item"/>。</returns>
        public static IEnumerable<TSource> Append<TSource>(this IEnumerable<TSource> _this, TSource item)
        {
            List<TSource> list = _this.ToList();
            list.Add(item);
            return list;
        }



        /// <summary>
        /// 通过使用指定的 Func&lt;TSource, TSource, bool&gt; 作为对象相等比较器生成两个序列的并集。
        /// </summary>
        /// <typeparam name="TSource">输入序列中的元素的类型。</typeparam>
        /// <param name="first">一个 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/>，它的非重复元素构成联合的第一个集。</param>
        /// <param name="second">一个 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/>，它的非重复元素构成联合的第二个集。</param>
        /// <param name="comparer">用于对值进行比较的 Func&lt;TSource, TSource, bool&gt;。</param>
        /// <returns>一个 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/>，包含两个输入序列中的非重复元素。</returns>
        public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, Func<TSource, TSource, bool> comparer)
        {
            Check.NotNull(first);
            return first.Union(second, NDF.Collections.Generic.EqualityComparer<TSource>.Create(comparer));
        }

        /// <summary>
        /// 通过使用指定的 <see cref="IComparer&lt;TSource&gt;"/> 作为对象相等比较器生成两个序列的并集。
        /// </summary>
        /// <typeparam name="TSource">输入序列中的元素的类型。</typeparam>
        /// <param name="first">一个 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/>，它的非重复元素构成联合的第一个集。</param>
        /// <param name="second">一个 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/>，它的非重复元素构成联合的第二个集。</param>
        /// <param name="comparer">用于对值进行比较的 <see cref="IComparer&lt;TSource&gt;"/>。</param>
        /// <returns>一个 <see cref="System.Collections.Generic.IEnumerable&lt;TSource&gt;"/>，包含两个输入序列中的非重复元素。</returns>
        public static IEnumerable<TSource> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IComparer<TSource> comparer)
        {
            Check.NotNull(first);
            return first.Union(second, comparer.ToEqualityComparer());
        }

    }
}
