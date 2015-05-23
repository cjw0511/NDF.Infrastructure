using NDF.ComponentModel.Paging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Utilities
{
    /// <summary>
    /// 提供对数据类型已知的特定数据源的查询进行计算的扩展功能。
    /// </summary>
    /// <remarks>
    /// 该静态类型扩展 IQueryable&lt;out TSource&gt; 接口的查询计算方法。
    /// </remarks>
    public static partial class Queryable
    {

        /// <summary>
        /// 判断指定的查询是否为 Null 或者查询结果中不包含任何元素。
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="_this"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<TSource>(this IQueryable<TSource> _this)
        {
            return _this == null || _this.Count() == 0;
        }

        /// <summary>
        /// 基于指定的过滤表达式判断指定的查询是否为 Null 或者查询结果中不包含符合过滤表达式条件判断的任何元素。
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="_this"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<TSource>(this IQueryable<TSource> _this, Expression<Func<TSource, bool>> predicate)
        {
            return _this == null || _this.Count(predicate) == 0;
        }


        /// <summary>
        /// 返回源序列 <paramref name="_this"/> 中的一个子集。
        /// <para>必须在调用该方法前调用 "OrderBy" 方法，否则将出现 <see cref="System.NotSupportedException"/> 异常。</para>
        /// </summary>
        /// <typeparam name="TSource"><paramref name="_this"/> 中的元素的类型。</typeparam>
        /// <param name="_this">一个 <see cref="IQueryable&lt;TSource&gt;"/>，用于从中返回元素。</param>
        /// <param name="startIndex">返回剩余元素前要跳过的元素数量，也可理解为范围开始处的从零开始的 <see cref="IQueryable&lt;TSource&gt;"/> 索引。</param>
        /// <returns>一个 <see cref="IQueryable&lt;TSource&gt;"/>，它表示源序列 <paramref name="_this"/> 中的一个子集。</returns>
        public static IQueryable<TSource> GetRange<TSource>(this IQueryable<TSource> _this, int startIndex)
        {
            //Check.NotNull(_this);
            //string commandText = _this.ToString();
            //bool isOrdered = commandText.IndexOf(" ORDER BY ", StringComparison.OrdinalIgnoreCase) > -1;
            //if (!isOrdered)
            //    _this = _this.OrderBy(item => 0);

            return _this.Skip(startIndex);
        }

        /// <summary>
        /// 返回源序列 <paramref name="_this"/> 中的一个子集。
        /// <para>必须在调用该方法前调用 "OrderBy" 方法，否则将出现 <see cref="System.NotSupportedException"/> 异常。</para>
        /// </summary>
        /// <typeparam name="TSource"><paramref name="_this"/> 中的元素的类型。</typeparam>
        /// <param name="_this">一个 <see cref="IQueryable&lt;TSource&gt;"/>，用于从中返回元素。</param>
        /// <param name="startIndex">返回剩余元素前要跳过的元素数量，也可理解为范围开始处的从零开始的 <see cref="IQueryable&lt;TSource&gt;"/> 索引。</param>
        /// <param name="count">范围中的元素数，同时也表示要返回的元素数量。</param>
        /// <returns>一个 <see cref="IQueryable&lt;TSource&gt;"/>，它表示源序列 <paramref name="_this"/> 中的一个子集。</returns>
        public static IQueryable<TSource> GetRange<TSource>(this IQueryable<TSource> _this, int startIndex, int count)
        {
            //Check.NotNull(_this);
            //string commandText = _this.ToString();
            //bool isOrdered = commandText.IndexOf(" ORDER BY ", StringComparison.OrdinalIgnoreCase) > -1;
            //if (!isOrdered)
            //    _this = _this.OrderBy(item => 0);

            return _this.Skip(startIndex).Take(count);
        }



        /// <summary>
        /// 根据键对序列的元素排序，并指定是按升序还是降序排列。
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <typeparam name="TKey">由 keySelector 表示的函数返回的键类型。</typeparam>
        /// <param name="source">一个要排序的值序列。</param>
        /// <param name="keySelector">用于从元素中提取键的函数。</param>
        /// <param name="sortDirection">用于指定数据排序方式，升序还是降序。</param>
        /// <returns>一个 System.Linq.IOrderedQueryable(TSource)，根据键对其元素排序。</returns>
        /// <exception cref="ArgumentNullException">source 或 keySelector 为 null。</exception>
        public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, ListSortDirection sortDirection)
        {
            return sortDirection == ListSortDirection.Ascending ? System.Linq.Queryable.OrderBy(source, keySelector) : source.OrderByDescending(keySelector);
        }
        
        /// <summary>
        /// 使用指定的比较器和排序规则对序列的元素排序。
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <typeparam name="TKey">由 keySelector 表示的函数返回的键类型。</typeparam>
        /// <param name="source">一个要排序的值序列。</param>
        /// <param name="keySelector">用于从元素中提取键的函数。</param>
        /// <param name="comparer">一个用于比较键的 System.Collections.Generic.IComparer(TSource)。</param>
        /// <param name="sortDirection">用于指定数据排序方式，升序还是降序。</param>
        /// <returns>一个 System.Linq.IOrderedQueryable(TSource)，根据键对其元素排序。</returns>
        /// <exception cref="System.ArgumentNullException">source 或 keySelector 或 comparer 为 null。</exception>
        public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer, ListSortDirection sortDirection)
        {
            return sortDirection == ListSortDirection.Ascending ? System.Linq.Queryable.OrderBy(source, keySelector, comparer) : source.OrderByDescending(keySelector, comparer);
        }





        /// <summary>
        /// 以 <seealso cref="DataPager.DefaultPageSize"/> 值指示的默认页面尺寸大小获取泛型序列中的第一页(pageIndex 为 0)分页数据。
        /// <para>必须在调用该方法前调用 "OrderBy" 方法，否则将出现 <see cref="System.NotSupportedException"/> 异常。</para>
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <param name="source">一个 IQueryable&lt;TSource&gt;，用于从中返回分页元素。</param>
        /// <returns>
        /// 返回一个 <see cref="IQueryable&lt;TSource&gt;"/>，返回的序列为源序列 source 的子集，其表示 source 第 1 页(pageIndex 为 0)的
        /// 数据，返回序列中元素数量为 <seealso cref="DataPager.DefaultPageSize"/>(如果是返回的是最后一页数据，其数量可能不足 <seealso cref="DataPager.DefaultPageSize"/>)。
        /// </returns>
        public static IQueryable<TSource> SplitPage<TSource>(this IQueryable<TSource> source)
        {
            return SplitPage(source, 0, DataPager.DefaultPageSize);
        }

        /// <summary>
        /// 以指定的页面索引号、<seealso cref="DataPager.DefaultPageSize"/> 值指示的默认页面尺寸大小获取泛型序列中的分页数据。
        /// <para>必须在调用该方法前调用 "OrderBy" 方法，否则将出现 <see cref="System.NotSupportedException"/> 异常。</para>
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <param name="source">一个 IQueryable&lt;TSource&gt;，用于从中返回分页元素。</param>
        /// <param name="pageIndex">返回数据页所在源序列中的页面索引号，从 0 开始计数。</param>
        /// <returns>
        /// 返回一个 <see cref="IQueryable&lt;TSource&gt;"/>，返回的序列为源序列 source 的子集，其表示 source 第 pageIndex(从 0 开始计数) 页的
        /// 数据，返回序列中元素数量为 <seealso cref="DataPager.DefaultPageSize"/>(如果是返回的是最后一页数据，其数量可能不足 <seealso cref="DataPager.DefaultPageSize"/>)。
        /// </returns>
        public static IQueryable<TSource> SplitPage<TSource>(this IQueryable<TSource> source, int pageIndex)
        {
            return SplitPage(source, pageIndex, DataPager.DefaultPageSize);
        }

        /// <summary>
        /// 以指定的页面索引号、页面尺寸大小获取泛型序列中的分页数据。
        /// <para>必须在调用该方法前调用 "OrderBy" 方法，否则将出现 <see cref="System.NotSupportedException"/> 异常。</para>
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <param name="source">一个 IQueryable&lt;TSource&gt;，用于从中返回分页元素。</param>
        /// <param name="pageIndex">返回数据页所在源序列中的页面索引号，从 0 开始计数。</param>
        /// <param name="pageSize">返回数据页的页面尺寸，即一页有多少行数据。</param>
        /// <returns>
        /// 返回一个 <see cref="IQueryable&lt;TSource&gt;"/>，返回的序列为源序列 source 的子集，其表示 source 第 pageIndex(从 0 开始计数) 页的
        /// 数据，返回序列中元素数量为 pageSize(如果是返回的是最后一页数据，其数量可能不足 pageSize)。
        /// </returns>
        public static IQueryable<TSource> SplitPage<TSource>(this IQueryable<TSource> source, int pageIndex, int pageSize)
        {
            return source.GetRange(pageIndex * pageSize, pageSize);
        }


        public static IQueryable<TSource> SplitPage<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            return SplitPage(source, 0, keySelector, sortDirection);
        }

        public static IQueryable<TSource> SplitPage<TSource, TKey>(this IQueryable<TSource> source, int pageIndex, Expression<Func<TSource, TKey>> keySelector, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            return SplitPage(source, pageIndex, DataPager.DefaultPageSize, keySelector, sortDirection);
        }

        public static IQueryable<TSource> SplitPage<TSource, TKey>(this IQueryable<TSource> source, int pageIndex, int pageSize, Expression<Func<TSource, TKey>> keySelector, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            return OrderBy(source, keySelector, sortDirection).GetRange(pageIndex * pageSize, pageSize);
        }


        public static IQueryable<TSource> SplitPage<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            return SplitPage(source, 0, keySelector, comparer, sortDirection);
        }

        public static IQueryable<TSource> SplitPage<TSource, TKey>(this IQueryable<TSource> source, int pageIndex, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            return SplitPage(source, pageIndex, DataPager.DefaultPageSize, keySelector, comparer, sortDirection);
        }

        public static IQueryable<TSource> SplitPage<TSource, TKey>(this IQueryable<TSource> source, int pageIndex, int pageSize, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            return OrderBy(source, keySelector, comparer, sortDirection).GetRange(pageIndex * pageSize, pageSize);
        }




        /// <summary>
        /// 以 <seealso cref="DataPager.DefaultPageSize"/> 值指示的默认页面尺寸大小进行分页查询，并获取查询后的结果。
        /// 以指定的页面索引号、页面尺寸大小获取泛型序列中的分页数据。
        /// <para>必须在调用该方法前调用 "OrderBy" 方法，否则将出现 <see cref="System.NotSupportedException"/> 异常。</para>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static PagingData<TSource> ToPagingData<TSource>(this IQueryable<TSource> source)
        {
            return ToPagingData(source, 0);
        }

        /// <summary>
        /// 以指定的页面索引号、<seealso cref="DataPager.DefaultPageSize"/> 值指示的默认页面尺寸大小进行分页查询，并获取查询后的结果。
        /// <para>必须在调用该方法前调用 "OrderBy" 方法，否则将出现 <see cref="System.NotSupportedException"/> 异常。</para>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public static PagingData<TSource> ToPagingData<TSource>(this IQueryable<TSource> source, int pageIndex)
        {
            return ToPagingData(source, pageIndex, DataPager.DefaultPageSize);
        }

        /// <summary>
        /// 以指定的页面索引号、和页面尺寸 值进行分页查询，并获取查询后的结果。
        /// <para>必须在调用该方法前调用 "OrderBy" 方法，否则将出现 <see cref="System.NotSupportedException"/> 异常。</para>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static PagingData<TSource> ToPagingData<TSource>(this IQueryable<TSource> source, int pageIndex, int pageSize)
        {
            return new PagingData<TSource>(pageIndex, pageSize, source.Count(), SplitPage(source, pageIndex, pageSize).ToArray());
        }


        public static PagingData<TSource> ToPagingData<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            return ToPagingData(source, 0, keySelector, sortDirection);
        }

        public static PagingData<TSource> ToPagingData<TSource, TKey>(this IQueryable<TSource> source, int pageIndex, Expression<Func<TSource, TKey>> keySelector, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            return ToPagingData(source, pageIndex, DataPager.DefaultPageSize, keySelector, sortDirection);
        }

        public static PagingData<TSource> ToPagingData<TSource, TKey>(this IQueryable<TSource> source, int pageIndex, int pageSize, Expression<Func<TSource, TKey>> keySelector, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            return new PagingData<TSource>(pageIndex, pageSize, source.Count(), SplitPage(source, pageIndex, pageSize, keySelector, sortDirection).ToArray());
        }


        public static PagingData<TSource> ToPagingData<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            return ToPagingData(source, 0, keySelector, comparer, sortDirection);
        }

        public static PagingData<TSource> ToPagingData<TSource, TKey>(this IQueryable<TSource> source, int pageIndex, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            return ToPagingData(source, pageIndex, DataPager.DefaultPageSize, keySelector, comparer, sortDirection);
        }

        public static PagingData<TSource> ToPagingData<TSource, TKey>(this IQueryable<TSource> source, int pageIndex, int pageSize, Expression<Func<TSource, TKey>> keySelector, IComparer<TKey> comparer, ListSortDirection sortDirection = ListSortDirection.Ascending)
        {
            return new PagingData<TSource>(pageIndex, pageSize, source.Count(), SplitPage(source, pageIndex, pageSize, keySelector, comparer, sortDirection).ToArray());
        }
    }
}
