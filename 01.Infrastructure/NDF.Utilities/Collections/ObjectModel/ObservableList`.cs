using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Collections.ObjectModel
{
    /// <summary>
    /// 表示一个动态的、可通过索引访问的对象的强类型列表，该列表提供用于对列表进行搜索、排序和操作的方法。
    /// <para>在添加项、移除项或刷新整个列表时，此集合将提供通知。</para>
    /// </summary>
    /// <typeparam name="T">列表中元素的类型。</typeparam>
    [Serializable]
    [DebuggerDisplay("Count = {Count}")]
    public class ObservableList<T> : IList<T>, IList, ICollection,
        IReadOnlyList<T>, IReadOnlyCollection<T>,
        IEnumerable<T>, IEnumerable,
        INotifyCollectionChanged, INotifyPropertyChanged
    {
        private const string _CountString = "Count";
        private const string _IndexerName = "Item[]";
        private const string _CapacityName = "Capacity";

        private List<T> _list;


        #region 构造函数定义

        /// <summary>
        /// 初始化 <see cref="ObservableList&lt;T&gt;"/> 类的新实例，该实例为空并且具有默认初始容量。
        /// </summary>
        public ObservableList()
        {
            this._list = new List<T>();
        }

        /// <summary>
        /// 初始化 <see cref="ObservableList&lt;T&gt;"/> 类的新实例，该实例包含从指定集合复制的元素并且具有足够的容量来容纳所复制的元素。
        /// </summary>
        /// <param name="collection">一个集合，其元素被复制到新列表中。</param>
        /// <exception cref="System.ArgumentNullException">collection 为 null。</exception>
        public ObservableList(IEnumerable<T> collection)
        {
            this._list = new List<T>(collection);
        }

        /// <summary>
        /// 初始化 <see cref="ObservableList&lt;T&gt;"/> 类的新实例，该实例为空并且具有指定的初始容量。
        /// </summary>
        /// <param name="capacity">新列表最初可以存储的元素数。</param>
        /// <exception cref="System.ArgumentOutOfRangeException">capacity 小于 0。</exception>
        public ObservableList(int capacity)
        {
            this._list = new List<T>(capacity);
        }

        #endregion



        #region 公共属性定义

        /// <summary>
        /// 获取或设置该内部数据结构在不调整大小的情况下能够容纳的元素总数。
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">Capacity 设置为小于 Count 的值。</exception>
        /// <exception cref="System.OutOfMemoryException">系统中没有足够的可用内存。</exception>
        public int Capacity
        {
            get { return this.InternalList.Capacity; }
            set
            {
                int _capacity = this.Capacity;
                this.InternalList.Capacity = value;
                if (_capacity != value)
                    this.OnPropertyChanged(_CapacityName);
            }
        }

        #endregion


        #region 公共方法定义

        /// <summary>
        /// 将指定集合的元素添加到 <see cref="ObservableList&lt;T&gt;"/> 的末尾。
        /// </summary>
        /// <param name="collection">一个集合，其元素应被添加到 <see cref="ObservableList&lt;T&gt;"/> 的末尾。集合自身不能为 null，但它可以包含为 null 的元素（如果类型 T 为引用类型）。</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="collection"/> 为 null。</exception>
        public void AddRange(IEnumerable<T> collection)
        {
            int startingIndex = this.InternalList.Count;
            int oldCapacity = this.Capacity;
            this.InternalList.AddRange(collection);
            this.OnCollectionAdd(collection.ToArray(), oldCapacity);
        }

        /// <summary>
        /// 返回当前集合的只读 <see cref="IList&lt;T&gt;"/> 包装。
        /// </summary>
        /// <returns>作为当前 <see cref="List&lt;T&gt;"/> 周围的只读包装的 <see cref="ReadOnlyCollection&lt;T&gt;"/>。</returns>
        public ReadOnlyCollection<T> AsReadOnly()
        {
            return this.InternalList.AsReadOnly();
        }


        /// <summary>
        /// 使用默认的比较器在整个已排序的 <see cref="ObservableList&lt;T&gt;"/> 中搜索元素，并返回该元素从零开始的索引。
        /// </summary>
        /// <param name="item">要定位的对象。 对于引用类型，该值可以为 null。</param>
        /// <returns>如果找到 <paramref name="item"/>，则为已排序的 <see cref="ObservableList&lt;T&gt;"/> 中 <paramref name="item"/> 的从零开始的索引；否则为一个负数，该负数是大于 <paramref name="item"/> 的第一个元素的索引的按位求补。如果没有更大的元素，则为 <see cref="Count"/> 的按位求补。</returns>
        /// <exception cref="System.InvalidOperationException">默认比较器 <see cref="Comparer&lt;T&gt;.Default"/> 找不到 T 类型的 <see cref="IComparable&lt;T&gt;"/> 泛型接口或 <see cref="IComparable"/> 接口的实现。</exception>
        public int BinarySearch(T item)
        {
            return this.InternalList.BinarySearch(item);
        }

        /// <summary>
        /// 使用指定的比较器在整个已排序的 <see cref="ObservableList&lt;T&gt;"/> 中搜索元素，并返回该元素从零开始的索引。
        /// </summary>
        /// <param name="item">要定位的对象。 对于引用类型，该值可以为 null。</param>
        /// <param name="comparer">
        /// 比较元素时要使用的 <see cref="IComparable&lt;T&gt;"/> 实现。
        /// 为 null 以使用默认比较器 <see cref="Comparer&lt;T&gt;.Default"/>。
        /// </param>
        /// <returns>如果找到 <paramref name="item"/>，则为已排序的 <see cref="ObservableList&lt;T&gt;"/> 中 <paramref name="item"/> 的从零开始的索引；否则为一个负数，该负数是大于 <paramref name="item"/> 的第一个元素的索引的按位求补。如果没有更大的元素，则为 <see cref="Count"/> 的按位求补。</returns>
        /// <exception cref="System.InvalidOperationException">comparer 为 null，且默认比较器 <see cref="Comparer&lt;T&gt;.Default"/> 找不到T 类型的 <see cref="IComparable&lt;T&gt;"/> 泛型接口或 <see cref="IComparable"/> 接口的实现。</exception>
        public int BinarySearch(T item, IComparer<T> comparer)
        {
            return this.InternalList.BinarySearch(item, comparer);
        }

        /// <summary>
        /// 使用指定的比较器在已排序 <see cref="ObservableList&lt;T&gt;"/> 的某个元素范围中搜索元素，并返回该元素从零开始的索引。
        /// </summary>
        /// <param name="index">要搜索的范围从零开始的起始索引。</param>
        /// <param name="count">要搜索的范围的长度。 </param>
        /// <param name="item">要定位的对象。 对于引用类型，该值可以为 null。</param>
        /// <param name="comparer">
        /// 比较元素时要使用的 <see cref="IComparable&lt;T&gt;"/> 实现。
        /// 或者为 null，表示使用默认比较器 <see cref="Comparer&lt;T&gt;.Default"/>。
        /// </param>
        /// <returns>如果找到 <paramref name="item"/>，则为已排序的 <see cref="ObservableList&lt;T&gt;"/> 中 <paramref name="item"/> 的从零开始的索引；否则为一个负数，该负数是大于 <paramref name="item"/> 的第一个元素的索引的按位求补。如果没有更大的元素，则为 <see cref="Count"/> 的按位求补。</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">index 小于 0。或 count 小于 0。</exception>
        /// <exception cref="System.ArgumentException"><paramref name="index"/> 和 <paramref name="count"/> 不表示 <see cref="ObservableList&lt;T&gt;"/> 中的有效范围。</exception>
        /// <exception cref="System.InvalidOperationException"><paramref name="comparer"/> 为 null，且默认比较器 <see cref="Comparer&lt;T&gt;.Default"/> 找不到T 类型的 <see cref="IComparable&lt;T&gt;"/> 泛型接口或 <see cref="IComparable"/> 接口的实现。</exception>
        public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
        {
            return this.BinarySearch(index, count, item, comparer);
        }


        /// <summary>
        /// 将当前 <see cref="ObservableList&lt;T&gt;"/> 中的元素转换为另一种类型，并返回包含转换后的元素的列表。
        /// </summary>
        /// <typeparam name="TOutput">目标数组元素的类型。</typeparam>
        /// <param name="converter">将每个元素从一种类型转换为另一种类型的 <see cref="Converter&lt;TInput, TOutput&gt;"/> 委托。</param>
        /// <returns>目标类型的 <see cref="ObservableList&lt;T&gt;"/>，其中包含当前 <see cref="ObservableList&lt;T&gt;"/> 中的转换后的元素。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="converter"/> 为 null。</exception>
        public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
        {
            return this.InternalList.ConvertAll<TOutput>(converter);
        }


        /// <summary>
        /// 将整个 <see cref="ObservableList&lt;T&gt;"/> 复制到兼容的一维数组中，从目标数组的开头开始放置。
        /// </summary>
        /// <param name="array">作为从<see cref="ObservableList&lt;T&gt;"/> 复制的元素的目标位置的一维 Array。 Array 必须具有从零开始的索引。</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="array"/> 为 null。</exception>
        /// <exception cref="System.ArgumentException">源 <see cref="ObservableList&lt;T&gt;"/> 中的元素数大于目标 <paramref name="array"/> 可包含的元素数。</exception>
        public void CopyTo(T[] array)
        {
            this.InternalList.CopyTo(array);
        }

        /// <summary>
        /// 将一定范围的元素从 <see cref="ObservableList&lt;T&gt;"/> 复制到兼容的一维数组中，从目标数组的指定索引位置开始放置。
        /// </summary>
        /// <param name="index">源 <see cref="ObservableList&lt;T&gt;"/> 中复制开始位置的从零开始的索引。</param>
        /// <param name="array">作为从 <see cref="ObservableList&lt;T&gt;"/> 复制的元素的目标位置的一维 Array。 Array 必须具有从零开始的索引。</param>
        /// <param name="arrayIndex"><paramref name="array"/> 中从零开始的索引，从此索引处开始进行复制。 </param>
        /// <param name="count">要复制的元素数。 </param>
        /// <exception cref="System.ArgumentNullException"><paramref name="array"/> 为 null。</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> 小于 0。或 <paramref name="arrayIndex"/> 小于 0。或 <paramref name="count"/> 小于 0。</exception>
        /// <exception cref="System.ArgumentException"><paramref name="index"/> 等于或大于源 <see cref="ObservableList&lt;T&gt;"/> 的 Count。或 从 <paramref name="index"/> 到源 <see cref="ObservableList&lt;T&gt;"/> 的末尾的元素数大于从 <paramref name="arrayIndex"/> 到目标 <paramref name="array"/> 的末尾的可用空间。</exception>
        public void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            this.InternalList.CopyTo(index, array, arrayIndex, count);
        }


        /// <summary>
        /// 确定 <see cref="ObservableList&lt;T&gt;"/> 是否包含与指定谓词所定义的条件相匹配的元素。
        /// </summary>
        /// <param name="match"><see cref="Predicate&lt;T&gt;"/> 委托，用于定义要搜索的元素应满足的条件。</param>
        /// <returns>如果 <see cref="ObservableList&lt;T&gt;"/> 包含一个或多个与指定谓词所定义的条件相匹配的元素，则为 true；否则为 false。 </returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="match"/> 为 null。</exception>
        public bool Exists(Predicate<T> match)
        {
            return this.InternalList.Exists(match);
        }


        /// <summary>
        /// 搜索与指定谓词所定义的条件相匹配的元素，并返回整个 <see cref="ObservableList&lt;T&gt;"/> 中的第一个匹配元素。
        /// </summary>
        /// <param name="match"><see cref="Predicate&lt;T&gt;"/> 委托，用于定义要搜索的元素的条件。</param>
        /// <returns>如果找到与指定谓词定义的条件匹配的第一个元素，则为该元素；否则为类型 <typeparamref name="T"/> 的默认值。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="match"/> 为 null。</exception>
        public T Find(Predicate<T> match)
        {
            return this.InternalList.Find(match);
        }

        /// <summary>
        /// 检索与指定谓词定义的条件匹配的所有元素。
        /// </summary>
        /// <param name="match"><see cref="Predicate&lt;T&gt;"/> 委托，用于定义要搜索的元素应满足的条件。</param>
        /// <returns>如果找到，则为一个 <see cref="ObservableList&lt;T&gt;"/>，其中包含与指定谓词所定义的条件相匹配的所有元素；否则为一个空 <see cref="ObservableList&lt;T&gt;"/>。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="match"/> 为 null。</exception>
        public List<T> FindAll(Predicate<T> match)
        {
            return this.InternalList.FindAll(match);
        }

        /// <summary>
        /// 搜索与指定谓词所定义的条件相匹配的元素，并返回整个 <see cref="ObservableList&lt;T&gt;"/> 中第一个匹配元素的从零开始的索引。
        /// </summary>
        /// <param name="match"><see cref="Predicate&lt;T&gt;"/> 委托，用于定义要搜索的元素的条件。</param>
        /// <returns>如果找到与 <paramref name="match"/> 定义的条件相匹配的第一个元素，则为该元素的从零开始的索引；否则为 -1。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="match"/> 为 null。</exception>
        public int FindIndex(Predicate<T> match)
        {
            return this.InternalList.FindIndex(match);
        }

        /// <summary>
        /// 搜索与指定谓词所定义的条件相匹配的元素，并返回 <see cref="ObservableList&lt;T&gt;"/> 中从指定索引到最后一个元素的元素范围内第一个匹配项的从零开始的索引。
        /// </summary>
        /// <param name="startIndex">从零开始的搜索的起始索引。 </param>
        /// <param name="match"><see cref="Predicate&lt;T&gt;"/> 委托，用于定义要搜索的元素的条件。</param>
        /// <returns>如果找到与 <paramref name="match"/> 定义的条件相匹配的第一个元素，则为该元素的从零开始的索引；否则为 -1。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="match"/> 为 null。</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="startIndex"/> 不在 <see cref="ObservableList&lt;T&gt;"/> 的有效索引范围内。</exception>
        public int FindIndex(int startIndex, Predicate<T> match)
        {
            return this.InternalList.FindIndex(startIndex, match);
        }

        /// <summary>
        /// 搜索与指定谓词所定义的条件相匹配的一个元素，并返回 <see cref="ObservableList&lt;T&gt;"/> 中从指定的索引开始、包含指定元素个数的元素范围内第一个匹配项的从零开始的索引。
        /// </summary>
        /// <param name="startIndex">从零开始的搜索的起始索引。</param>
        /// <param name="count">要搜索的部分中的元素数。</param>
        /// <param name="match"><see cref="Predicate&lt;T&gt;"/> 委托，用于定义要搜索的元素的条件。</param>
        /// <returns>如果找到与 <paramref name="match"/> 定义的条件相匹配的第一个元素，则为该元素的从零开始的索引；否则为 -1。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="match"/> 为 null。</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="startIndex"/> 不在 <see cref="ObservableList&lt;T&gt;"/> 的有效索引范围内。或 <paramref name="count"/> 小于 0。或 <paramref name="startIndex"/> 和 <paramref name="count"/> 未指定 <see cref="ObservableList&lt;T&gt;"/> 中的有效部分。</exception>
        public int FindIndex(int startIndex, int count, Predicate<T> match)
        {
            return this.InternalList.FindIndex(startIndex, count, match);
        }


        /// <summary>
        /// 搜索与指定谓词所定义的条件相匹配的元素，并返回整个 <see cref="ObservableList&lt;T&gt;"/> 中的最后一个匹配元素。
        /// </summary>
        /// <param name="match"><see cref="Predicate&lt;T&gt;"/> 委托，用于定义要搜索的元素的条件。</param>
        /// <returns>如果找到，则为与指定谓词所定义的条件相匹配的最后一个元素；否则为类型 <typeparamref name="T"/> 的默认值。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="match"/> 为 null。</exception>
        public T FindLast(Predicate<T> match)
        {
            return this.InternalList.FindLast(match);
        }

        /// <summary>
        /// 搜索与指定谓词所定义的条件相匹配的元素，并返回整个 <see cref="ObservableList&lt;T&gt;"/> 中最后一个匹配元素的从零开始的索引。
        /// </summary>
        /// <param name="match"><see cref="Predicate&lt;T&gt;"/> 委托，用于定义要搜索的元素的条件。</param>
        /// <returns>如果找到与 <paramref name="match"/> 定义的条件相匹配的最后一个元素，则为该元素的从零开始的索引；否则为 -1。 </returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="match"/> 为 null。</exception>
        public int FindLastIndex(Predicate<T> match)
        {
            return this.InternalList.FindLastIndex(match);
        }

        /// <summary>
        /// 搜索与由指定谓词定义的条件相匹配的元素，并返回 <see cref="ObservableList&lt;T&gt;"/> 中从第一个元素到指定索引的元素范围内最后一个匹配项的从零开始的索引。
        /// </summary>
        /// <param name="startIndex">向后搜索的从零开始的起始索引。</param>
        /// <param name="match"><see cref="Predicate&lt;T&gt;"/> 委托，用于定义要搜索的元素的条件。</param>
        /// <returns>如果找到与 <paramref name="match"/> 定义的条件相匹配的最后一个元素，则为该元素的从零开始的索引；否则为 -1。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="match"/> 为 null。</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="startIndex"/> 不在 <see cref="ObservableList&lt;T&gt;"/> 的有效索引范围内。</exception>
        public int FindLastIndex(int startIndex, Predicate<T> match)
        {
            return this.InternalList.FindLastIndex(startIndex, match);
        }

        /// <summary>
        /// 搜索与指定谓词所定义的条件相匹配的元素，并返回 <see cref="ObservableList&lt;T&gt;"/> 中包含指定元素个数、到指定索引结束的元素范围内最后一个匹配项的从零开始的索引。
        /// </summary>
        /// <param name="startIndex">向后搜索的从零开始的起始索引。</param>
        /// <param name="count">要搜索的部分中的元素数。</param>
        /// <param name="match"><see cref="Predicate&lt;T&gt;"/> 委托，用于定义要搜索的元素的条件。</param>
        /// <returns>如果找到与 <paramref name="match"/> 定义的条件相匹配的最后一个元素，则为该元素的从零开始的索引；否则为 -1。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="match"/> 为 null。</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="startIndex"/> 不在 <see cref="ObservableList&lt;T&gt;"/> 的有效索引范围内。或 <paramref name="count"/> 小于 0。或 <paramref name="startIndex"/> 和 <paramref name="count"/> 未指定 <see cref="ObservableList&lt;T&gt;"/> 中的有效部分。</exception>
        public int FindLastIndex(int startIndex, int count, Predicate<T> match)
        {
            return this.InternalList.FindLastIndex(startIndex, count, match);
        }


        /// <summary>
        /// 对 <see cref="ObservableList&lt;T&gt;"/> 的每个元素执行指定操作。
        /// </summary>
        /// <param name="action">要对 <see cref="ObservableList&lt;T&gt;"/> 的每个元素执行的 <see cref="Action&lt;T&gt;"/> 委托。</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="action"/> 为 null。</exception>
        public void ForEach(Action<T> action)
        {
            this.InternalList.ForEach(action);
        }


        /// <summary>
        /// 创建源 <see cref="ObservableList&lt;T&gt;"/> 中的元素范围的浅表副本。
        /// </summary>
        /// <param name="index">范围开始处的从零开始的 <see cref="ObservableList&lt;T&gt;"/> 索引。</param>
        /// <param name="count">范围中的元素数。</param>
        /// <returns>源 <see cref="ObservableList&lt;T&gt;"/> 中的元素范围的浅表副本。</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> 小于 0。或 <paramref name="count"/> 小于 0。</exception>
        /// <exception cref="System.ArgumentException"><paramref name="index"/> 和 <paramref name="count"/> 不表示 <see cref="ObservableList&lt;T&gt;"/> 中元素的有效范围。</exception>
        public List<T> GetRange(int index, int count)
        {
            return this.InternalList.GetRange(index, count);
        }


        /// <summary>
        /// 搜索指定的对象，并返回 <see cref="ObservableList&lt;T&gt;"/> 中从指定索引到最后一个元素的元素范围内第一个匹配项的从零开始的索引。
        /// </summary>
        /// <param name="item">要在 <see cref="ObservableList&lt;T&gt;"/> 中定位的对象。对于引用类型，该值可以为 null。</param>
        /// <param name="index">从零开始的搜索的起始索引。 空列表中 0（零）为有效值。</param>
        /// <returns>如果在 <see cref="ObservableList&lt;T&gt;"/> 中从 <paramref name="index"/> 到最后一个元素的元素范围内找到 <paramref name="item"/> 的第一个匹配项，则为该项的从零开始的索引；否则为 -1。</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> 不在 <see cref="ObservableList&lt;T&gt;"/> 的有效索引范围内。</exception>
        public int IndexOf(T item, int index)
        {
            return this.InternalList.IndexOf(item, index);
        }

        /// <summary>
        /// 搜索指定的对象，并返回 <see cref="ObservableList&lt;T&gt;"/> 中从指定的索引开始并包含指定的元素数的元素范围内第一个匹配项的从零开始的索引。
        /// </summary>
        /// <param name="item">要在 <see cref="ObservableList&lt;T&gt;"/> 中定位的对象。对于引用类型，该值可以为 null。</param>
        /// <param name="index">从零开始的搜索的起始索引。 空列表中 0（零）为有效值。</param>
        /// <param name="count">要搜索的部分中的元素数。</param>
        /// <returns>如果在 <see cref="ObservableList&lt;T&gt;"/> 中从 <paramref name="index"/> 开始并包含 <paramref name="count"/> 个元素的元素范围内找到 <paramref name="item"/> 的第一个匹配项，则为该项的从零开始的索引；否则为 -1。</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> 不在<see cref="ObservableList&lt;T&gt;"/> 的有效索引范围内。或 <paramref name="count"/> 小于 0。或 <paramref name="index"/> 和 <paramref name="count"/> 未指定 <see cref="ObservableList&lt;T&gt;"/> 中的有效部分。</exception>
        public int IndexOf(T item, int index, int count)
        {
            return this.InternalList.IndexOf(item, index, count);
        }


        /// <summary>
        /// 将集合中的某个元素插入 <see cref="ObservableList&lt;T&gt;"/> 的指定索引处。
        /// </summary>
        /// <param name="index">应在此处插入新元素的从零开始的索引。</param>
        /// <param name="collection">一个集合，应将其元素插入到 <see cref="ObservableList&lt;T&gt;"/> 中。集合自身不能为 null，但它可以包含为 null 的元素（如果类型 <typeparamref name="T"/> 为引用类型）。</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="collection"/> 为 null。</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> 小于 0。或 <paramref name="index"/> 大于 <see cref="Count"/>。</exception>
        public void InsertRange(int index, IEnumerable<T> collection)
        {
            int startingIndex = index;
            int oldCapacity = this.Capacity;
            this.InternalList.InsertRange(index, collection);
            this.OnCollectionAdd(collection.ToArray(), startingIndex, oldCapacity);
        }


        /// <summary>
        /// 搜索指定的对象，并返回整个 <see cref="ObservableList&lt;T&gt;"/> 中最后一个匹配项的从零开始的索引。
        /// </summary>
        /// <param name="item">要在 <see cref="ObservableList&lt;T&gt;"/> 中定位的对象。对于引用类型，该值可以为 null。</param>
        /// <returns>如果在整个 <see cref="ObservableList&lt;T&gt;"/> 中找到 <paramref name="item"/> 的最后一个匹配项，则为该项的从零开始的索引；否则为 -1。</returns>
        public int LastIndexOf(T item)
        {
            return this.InternalList.LastIndexOf(item);
        }

        /// <summary>
        /// 搜索指定的对象，并返回 <see cref="ObservableList&lt;T&gt;"/> 中从第一个元素到指定索引的元素范围内最后一个匹配项的从零开始的索引。
        /// </summary>
        /// <param name="item">要在 <see cref="ObservableList&lt;T&gt;"/> 中定位的对象。对于引用类型，该值可以为 null。</param>
        /// <param name="index">向后搜索的从零开始的起始索引。</param>
        /// <returns>如果在 <see cref="ObservableList&lt;T&gt;"/> 中从第一个元素到 <paramref name="index"/> 的元素范围内找到 <paramref name="item"/> 的最后一个匹配项，则为该项的从零开始的索引；否则为 -1。</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> 不在 <see cref="ObservableList&lt;T&gt;"/> 的有效索引范围内。</exception>
        public int LastIndexOf(T item, int index)
        {
            return this.InternalList.LastIndexOf(item, index);
        }

        /// <summary>
        /// 搜索指定的对象，并返回 <see cref="ObservableList&lt;T&gt;"/> 中包含指定的元素数并在指定索引处结束的元素范围内最后一个匹配项的从零开始的索引。
        /// </summary>
        /// <param name="item">要在 <see cref="ObservableList&lt;T&gt;"/> 中定位的对象。对于引用类型，该值可以为 null。</param>
        /// <param name="index">向后搜索的从零开始的起始索引。</param>
        /// <param name="count">要搜索的部分中的元素数。</param>
        /// <returns>如果在 <see cref="ObservableList&lt;T&gt;"/> 中包含 <paramref name="count"/> 个元素、在 <paramref name="index"/> 处结尾的元素范围内找到 item 的最后一个匹配项，则为该项的从零开始的索引；否则为 -1。</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> 不在 <see cref="ObservableList&lt;T&gt;"/> 的有效索引范围内。或 <paramref name="count"/> 小于 0。或 <paramref name="index"/> 和 <paramref name="count"/> 未指定 <see cref="ObservableList&lt;T&gt;"/> 中的有效部分。</exception>
        public int LastIndexOf(T item, int index, int count)
        {
            return this.InternalList.LastIndexOf(item, index, count);
        }


        /// <summary>
        /// 移除与指定的谓词所定义的条件相匹配的所有元素。
        /// </summary>
        /// <param name="match"><see cref="Predicate&lt;T&gt;"/> 委托，用于定义要移除的元素应满足的条件。</param>
        /// <returns>从 <see cref="ObservableList&lt;T&gt;"/> 中移除的元素的数目。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="match"/> 为 null。</exception>
        public int RemoveAll(Predicate<T> match)
        {
            List<T> removedItems = this.InternalList.FindAll(match);
            int ret = this.InternalList.RemoveAll(match);
            if (removedItems.Count > 0)
                this.OnCollectionRemove(removedItems);

            return ret;
        }

        /// <summary>
        /// 从 <see cref="ObservableList&lt;T&gt;"/> 中移除一定范围的元素。
        /// </summary>
        /// <param name="index">要移除的元素的范围从零开始的起始索引。</param>
        /// <param name="count">要移除的元素数。</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> 小于 0。或 <paramref name="count"/> 小于 0。</exception>
        /// <exception cref="System.ArgumentException"><paramref name="index"/> 和 <paramref name="count"/> 不表示 <see cref="ObservableList&lt;T&gt;"/> 中元素的有效范围。</exception>
        public void RemoveRange(int index, int count)
        {
            List<T> removedItems = this.InternalList.GetRange(index, count);
            this.InternalList.RemoveRange(index, count);
            this.OnCollectionRemove(removedItems, index);
        }

        /// <summary>
        /// 从 <see cref="ObservableList&lt;T&gt;"/> 中移除一组元素。
        /// 该方法返回的值不一定等于 <paramref name="range"/> 集合的元素数，因为 <paramref name="range"/> 中可能有部分元素不在 <see cref="ObservableList&lt;T&gt;"/> 中，或者 <paramref name="range"/> 中重复的项只能移除一次。
        /// </summary>
        /// <param name="range">一组要从 <see cref="ObservableList&lt;T&gt;"/> 中移除的元素，其中的项可以为 null。</param>
        /// <returns>成功移除的元素数。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="range"/> 为 null。</exception>
        public int RemoveRange(IEnumerable<T> range)
        {
            if (range == null)
                throw new ArgumentNullException("range");

            int count = 0;
            List<T> removedItems = new List<T>();

            foreach (T item in range)
            {
                if (this.InternalList.Remove(item))
                {
                    count++;
                    removedItems.Add(item);
                }
            }

            if (count > 0)
                this.OnCollectionRemove(removedItems);

            return count;
        }


        /// <summary>
        /// 将整个 <see cref="ObservableList&lt;T&gt;"/> 中元素的顺序反转。
        /// </summary>
        public void Reverse()
        {
            List<T> movedItems = new List<T>();
            if (this.InternalList.Count % 2 == 0)
            {
                movedItems.AddRange(this.InternalList);
            }
            else
            {
                int split = (this.InternalList.Count - 1) / 2;
                movedItems.AddRange(this.InternalList.Take(split));
                movedItems.AddRange(this.InternalList.Skip(split + 1));
            }
            this.InternalList.Reverse();
            if (movedItems.Count > 0)
                this.OnCollectionMove(movedItems);
        }

        /// <summary>
        /// 将指定范围中元素的顺序反转。
        /// </summary>
        /// <param name="index">要反转的范围的从零开始的起始索引。</param>
        /// <param name="count">要反转的范围内的元素数。</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> 小于 0。或 <paramref name="count"/> 小于 0。</exception>
        /// <exception cref="ArgumentException"><paramref name="index"/> 和 <paramref name="count"/> 不表示 <see cref="ObservableList&lt;T&gt;"/> 中元素的有效范围。</exception>
        public void Reverse(int index, int count)
        {
            List<T> movedItems = new List<T>();
            List<T> list = this.InternalList.GetRange(index, count);
            if (list.Count % 2 == 0)
            {
                movedItems.AddRange(list);
            }
            else
            {
                int split = (list.Count - 1) / 2;
                movedItems.AddRange(list.Take(split));
                movedItems.AddRange(list.Skip(split + 1));
            }
            this.InternalList.Reverse(index, count);
            if (movedItems.Count > 0)
                this.OnCollectionMove(movedItems);
        }


        /// <summary>
        /// 使用默认比较器对整个 <see cref="ObservableList&lt;T&gt;"/> 中的元素进行排序。
        /// </summary>
        /// <exception cref="System.InvalidOperationException">默认比较器 <see cref="Comparer&lt;T&gt;.Default"/> 找不到 <typeparamref name="T"/> 类型的 <see cref="IComparable&lt;T&gt;"/> 泛型接口或 <see cref="IComparable"/> 接口的实现。</exception>
        public void Sort()
        {
            if (this.InternalList.Count <= 1)
                return;

            T[] originalItems = this.InternalList.ToArray();
            this.InternalList.Sort();

            List<T> movedItems = new List<T>();
            for (int i = 0; i < originalItems.Length; i++)
            {
                T x = originalItems[i], y = this.InternalList[i];
                if (Comparer<T>.Default.Compare(x, y) != 0)
                    movedItems.Add(x);
            }
            if (movedItems.Count > 0)
                this.OnCollectionMove(movedItems);
        }

        /// <summary>
        /// 使用指定的 <see cref="System.Comparison&lt;T&gt;"/> 对整个 <see cref="ObservableList&lt;T&gt;"/> 中的元素进行排序。
        /// </summary>
        /// <param name="comparison">比较元素时要使用的 <see cref="System.Comparison&lt;T&gt;"/>。</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="comparison"/> 为 null。</exception>
        /// <exception cref="System.ArgumentException">在排序过程中，<paramref name="comparison"/> 的实现会导致错误。 例如，将某个项与其自身进行比较时，<paramref name="comparison"/> 可能不返回 0。</exception>
        public void Sort(Comparison<T> comparison)
        {
            if (this.InternalList.Count <= 1)
                return;

            T[] originalItems = this.InternalList.ToArray();
            this.InternalList.Sort(comparison);

            List<T> movedItems = new List<T>();
            for (int i = 0; i < originalItems.Length; i++)
            {
                T x = originalItems[i], y = this.InternalList[i];
                if (comparison(x, y) != 0)
                    movedItems.Add(x);
            }
            if (movedItems.Count > 0)
                this.OnCollectionMove(movedItems);
        }

        /// <summary>
        /// 使用指定的比较器对整个 <see cref="ObservableList&lt;T&gt;"/> 中的元素进行排序。
        /// </summary>
        /// <param name="comparer">比较元素时要使用的 <see cref="IComparer&lt;T&gt;"/> 实现，或者为null，表示使用默认比较器 <see cref="Comparer&lt;T&gt;.Default"/>。</param>
        /// <exception cref="System.InvalidOperationException"><paramref name="comparer"/> 为 null，且默认比较器 <see cref="Comparer&lt;T&gt;.Default"/> 找不到 <typeparamref name="T"/> 类型的 <see cref="IComparable&lt;T&gt;"/> 泛型接口或 <see cref="IComparable"/> 接口的实现。</exception>
        /// <exception cref="System.ArgumentException"><paramref name="comparer"/> 的实现导致排序时出现错误。 例如，将某个项与其自身进行比较时，<paramref name="comparer"/> 可能不返回 0。</exception>
        public void Sort(IComparer<T> comparer)
        {
            if (this.InternalList.Count <= 1)
                return;

            T[] originalItems = this.InternalList.ToArray();
            this.InternalList.Sort(comparer);

            List<T> movedItems = new List<T>();
            for (int i = 0; i < originalItems.Length; i++)
            {
                T x = originalItems[i], y = this.InternalList[i];
                if (comparer.Compare(x, y) != 0)
                    movedItems.Add(x);
            }
            if (movedItems.Count > 0)
                this.OnCollectionMove(movedItems);
        }

        /// <summary>
        /// 使用指定的比较器对 <see cref="ObservableList&lt;T&gt;"/> 中某个范围内的元素进行排序。
        /// </summary>
        /// <param name="index">要排序的范围的从零开始的起始索引。</param>
        /// <param name="count">要排序的范围的长度。</param>
        /// <param name="comparer">比较元素时要使用的 <see cref="IComparer&lt;T&gt;"/> 实现，或者为 null，表示使用默认比较器 <see cref="Comparer&lt;T&gt;.Default"/>。</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> 小于 0。或 <paramref name="count"/> 小于 0。</exception>
        /// <exception cref="System.ArgumentException"><paramref name="index"/> 和 <paramref name="count"/> 未指定 <see cref="ObservableList&lt;T&gt;"/> 中的有效范围。或 <paramref name="comparer"/> 的实现导致排序时出现错误。 例如，将某个项与其自身进行比较时，<paramref name="comparer"/> 可能不返回 0。</exception>
        /// <exception cref="System.InvalidOperationException"><paramref name="comparer"/> 为 null，且默认比较器 <see cref="Comparer&lt;T&gt;.Default"/> 找不到 <typeparamref name="T"/> 类型的 <see cref="IComparable&lt;T&gt;"/> 泛型接口或 <see cref="IComparable"/> 接口的实现。</exception>
        public void Sort(int index, int count, IComparer<T> comparer)
        {
            List<T> originalItems = this.InternalList.GetRange(index, count);
            this.InternalList.Sort(index, count, comparer);

            List<T> movedItems = new List<T>();
            List<T> list = this.InternalList.GetRange(index, count);
            for (int i = 0; i < originalItems.Count; i++)
            {
                T x = originalItems[i], y = list[i];
                if (comparer.Compare(x, y) != 0)
                    movedItems.Add(x);
            }
            if (movedItems.Count > 0)
                this.OnCollectionMove(movedItems);
        }


        /// <summary>
        /// 将 <see cref="ObservableList&lt;T&gt;"/> 的元素复制到新数组中。
        /// </summary>
        /// <returns>一个数组，它包含 <see cref="ObservableList&lt;T&gt;"/> 的元素的副本。</returns>
        public T[] ToArray()
        {
            return this.InternalList.ToArray();
        }


        /// <summary>
        /// 将容量设置为 <see cref="ObservableList&lt;T&gt;"/> 中的实际元素数目（如果该数目小于某个阈值）。
        /// </summary>
        public void TrimExcess()
        {
            int _capacity = this.Capacity;
            this.InternalList.TrimExcess();
            if (_capacity != this.Capacity)
                this.OnPropertyChanged(_CapacityName);
        }

        /// <summary>
        /// 确定是否 <see cref="ObservableList&lt;T&gt;"/> 中的每个元素都与指定的谓词所定义的条件相匹配。
        /// </summary>
        /// <param name="match"><see cref="Predicate&lt;T&gt;"/> 委托，定义要据以检查元素的条件。</param>
        /// <returns>如果 <see cref="ObservableList&lt;T&gt;"/> 中的每个元素都与指定的谓词所定义的条件相匹配，则为 true；否则为 false。 如果列表不包含任何元素，则返回值为 true。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="match"/> 为 null。</exception>
        public bool TrueForAll(Predicate<T> match)
        {
            return this.InternalList.TrueForAll(match);
        }


        #endregion


        #region 内部属性定义

        /// <summary>
        /// 获取此 <see cref="ObservableList&lt;Te&gt;"/> 对象内部的实际数据存储列表。
        /// </summary>
        protected List<T> InternalList
        {
            get { return this._list; }
        }

        #endregion



        #region 接口属性实现

        /// <summary>
        /// 获取或设置指定索引处的元素。
        /// </summary>
        /// <param name="index">要获得或设置的元素从零开始的索引。</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">index 小于 0。index 等于或大于 Count。</exception>
        public T this[int index]
        {
            get { return this.InternalList[index]; }
            set { this.SetItem(index, value); }
        }

        /// <summary>
        /// 获取 <see cref="ObservableList&lt;T&gt;"/> 中实际包含的元素数。
        /// </summary>
        public int Count
        {
            get { return this.InternalList.Count; }
        }

        #endregion


        #region 接口方法实现

        /// <summary>
        /// 将对象添加到 <see cref="ObservableList&lt;T&gt;"/> 的结尾处。
        /// </summary>
        /// <param name="item">要添加到 <see cref="ObservableList&lt;T&gt;"/> 的末尾处的对象。对于引用类型，该值可以为 null。</param>
        public void Add(T item)
        {
            int index = this.InternalList.Count;
            int oldCapacity = this.Capacity;
            this.InternalList.Add(item);
            this.OnCollectionAdd(item, index, oldCapacity);
        }

        /// <summary>
        /// 将元素插入 <see cref="ObservableList&lt;T&gt;"/> 的指定索引处。
        /// </summary>
        /// <param name="index">从零开始的索引，应在该位置插入 <paramref name="item"/>。</param>
        /// <param name="item">要插入的对象。 对于引用类型，该值可以为 null。</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> 小于 0。或 <paramref name="index"/> 大于 <see cref="Count"/>。</exception>
        public void Insert(int index, T item)
        {
            int oldCapacity = this.Capacity;
            this.InternalList.Insert(index, item);
            this.OnCollectionAdd(item, index, oldCapacity);
        }

        /// <summary>
        /// 从 <see cref="ObservableList&lt;T&gt;"/> 中移除特定对象的第一个匹配项。
        /// </summary>
        /// <param name="item">要从 <see cref="ObservableList&lt;T&gt;"/> 中移除的对象。对于引用类型，该值可以为 null。</param>
        /// <returns>如果成功移除 <paramref name="item"/>，则为 true；否则为 false。 如果在 <see cref="ObservableList&lt;T&gt;"/> 中没有找到 <paramref name="item"/>，该方法也会返回 false。</returns>
        public bool Remove(T item)
        {
            int index = this.IndexOf(item);
            if (index >= 0)
            {
                this.RemoveAt(index);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 移除 <see cref="ObservableList&lt;T&gt;"/> 的指定索引处的元素。
        /// </summary>
        /// <param name="index">要移除的元素的从零开始的索引。</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> 小于 0。或 <paramref name="index"/> 等于或大于 <see cref="Count"/>。</exception>
        public void RemoveAt(int index)
        {
            T removedItem = this.InternalList[index];
            this.InternalList.RemoveAt(index);
            this.OnCollectionRemove(removedItem, index);
        }

        /// <summary>
        /// 从 <see cref="ObservableList&lt;T&gt;"/> 中移除所有元素。
        /// </summary>
        public void Clear()
        {
            IList<T> items = this.InternalList.ToArray();
            this.InternalList.Clear();
            this.OnCollectionReset(items);
        }


        /// <summary>
        /// 确定某元素是否在 <see cref="ObservableList&lt;T&gt;"/> 中。
        /// </summary>
        /// <param name="item">要在 <see cref="ObservableList&lt;T&gt;"/> 中定位的对象。对于引用类型，该值可以为 null。</param>
        /// <returns>如果在 <see cref="ObservableList&lt;T&gt;"/> 中找到 <paramref name="item"/>，则为 true，否则为 false。</returns>
        public bool Contains(T item)
        {
            return this.InternalList.Contains(item);
        }

        /// <summary>
        /// 搜索指定的对象，并返回整个 <see cref="ObservableList&lt;T&gt;"/> 中第一个匹配项的从零开始的索引。
        /// </summary>
        /// <param name="item">要在 <see cref="ObservableList&lt;T&gt;"/> 中定位的对象。对于引用类型，该值可以为 null。</param>
        /// <returns>如果在整个 <see cref="ObservableList&lt;T&gt;"/> 中找到 <paramref name="item"/> 的第一个匹配项，则为该项的从零开始的索引；否则为 -1。</returns>
        public int IndexOf(T item)
        {
            return this.InternalList.IndexOf(item);
        }


        /// <summary>
        /// 将整个 <see cref="ObservableList&lt;T&gt;"/> 复制到兼容的一维数组中，从目标数组的指定索引位置开始放置。
        /// </summary>
        /// <param name="array">作为从 <see cref="ObservableList&lt;T&gt;"/> 复制的元素的目标位置的一维 Array。Array 必须具有从零开始的索引。</param>
        /// <param name="arrayIndex"><paramref name="array"/> 中从零开始的索引，从此索引处开始进行复制。</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="array"/> 为 null。</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> 小于 0。</exception>
        /// <exception cref="System.ArgumentException">源 <see cref="ObservableList&lt;T&gt;"/> 中的元素数大于从 <paramref name="arrayIndex"/> 到目标 <paramref name="array"/> 结尾处之间的可用空间。</exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.InternalList.CopyTo(array, arrayIndex);
        }


        /// <summary>
        /// 返回循环访问 <see cref="List&lt;T&gt;"/> 的枚举数。
        /// </summary>
        /// <returns>用于 <see cref="List&lt;T&gt;"/> 的 <see cref="List&lt;T&gt;.Enumerator"/>。</returns>
        public List<T>.Enumerator GetEnumerator()
        {
            return this.InternalList.GetEnumerator();
        }

        #endregion



        #region ICollection 显式接口实现

        /// <summary>
        /// 从特定的 <see cref="Array"/> 索引处开始，将 <see cref="ICollection"/> 的元素复制到一个 <see cref="Array"/> 中。
        /// </summary>
        /// <param name="array">作为从 <see cref="ICollection"/> 复制的元素的目标的一维 <see cref="Array"/>。 <see cref="Array"/> 必须具有从零开始的索引。</param>
        /// <param name="arrayIndex"><paramref name="array"/> 中从零开始的索引，从此索引处开始进行复制。</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="array"/> 为 null。</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> 小于 0。</exception>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="array"/> 是多维的。
        /// 或 <paramref name="array"/> 没有从零开始的索引。
        /// 或 源 <see cref="ICollection"/> 中的元素数目大于从 <paramref name="arrayIndex"/> 到目标 <paramref name="array"/> 末尾之间的可用空间。
        /// 或 源 <see cref="ICollection"/> 的类型无法自动转换为目标 <paramref name="array"/> 的类型。
        /// </exception>
        void ICollection.CopyTo(Array array, int arrayIndex)
        {
            ((ICollection)this.InternalList).CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// 获取一个值，该值指示 <see cref="ICollection&lt;T&gt;"/> 是否为只读。
        /// </summary>
        bool ICollection<T>.IsReadOnly
        {
            get { return ((ICollection<T>)this.InternalList).IsReadOnly; }
        }

        /// <summary>
        /// 获取一个值，该值指示是否同步对 <see cref="ICollection"/> 的访问（线程安全）。
        /// </summary>
        bool ICollection.IsSynchronized
        {
            get { return ((ICollection)this.InternalList).IsSynchronized; }
        }

        /// <summary>
        /// 获取可用于同步对 <see cref="ICollection"/> 的访问的对象。
        /// </summary>
        object ICollection.SyncRoot
        {
            get { return ((ICollection)this.InternalList).SyncRoot; }
        }

        #endregion


        #region IList 显式接口实现

        /// <summary>
        /// 获取一个值，该值指示 <see cref="IList"/> 是否具有固定大小。
        /// 如果 <see cref="IList"/> 具有固定大小，则为 true；否则为 false。 在 <see cref="ObservableList&lt;T&gt;"/> 的默认实现中，此属性始终返回 false。 
        /// </summary>
        bool IList.IsFixedSize
        {
            get { return ((IList)this.InternalList).IsFixedSize; }
        }

        /// <summary>
        /// 获取一个值，该值指示 <see cref="IList"/> 是否为只读。
        /// 在 <see cref="ObservableList&lt;T&gt;"/> 的默认实现中，此属性始终返回false。
        /// </summary>
        bool IList.IsReadOnly
        {
            get { return ((IList)this.InternalList).IsReadOnly; }
        }

        /// <summary>
        /// 获取或设置位于指定索引处的元素。
        /// </summary>
        /// <param name="index">要获得或设置的元素从零开始的索引。 </param>
        /// <returns>位于指定索引处的元素。</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> 不是 <see cref="IList"/> 中的有效索引。</exception>
        /// <exception cref="System.ArgumentException">已设置属性，且 value 属于不能对 <see cref="IList"/> 赋值的类型。</exception>
        object IList.this[int index]
        {
            get { return ((IList)this.InternalList)[index]; }
            set { ((IList)this.InternalList)[index] = value; }
        }

        /// <summary>
        /// 将某项添加到 <see cref="IList"/> 中。
        /// </summary>
        /// <param name="item">要添加到 <see cref="IList"/> 的 Object。 </param>
        /// <returns>新元素的插入位置。</returns>
        /// <exception cref="System.ArgumentException"><paramref name="item"/> 属于不能分配给 <see cref="IList"/> 的类型。</exception>
        int IList.Add(object item)
        {
            return ((IList)this.InternalList).Add(item);
        }

        /// <summary>
        /// 确定 <see cref="IList"/> 是否包含特定值。
        /// </summary>
        /// <param name="item">要在 <see cref="IList"/> 中查找的 <see cref="Object"/>。 </param>
        /// <returns>如果在 <see cref="IList"/> 中找到 <paramref name="item"/>，则为 true；否则为 false。 </returns>
        bool IList.Contains(object item)
        {
            return ((IList)this.InternalList).Contains(item);
        }

        /// <summary>
        /// 确定 <see cref="IList"/> 中特定项的索引。
        /// </summary>
        /// <param name="item">要在 <see cref="IList"/> 中查找的对象。</param>
        /// <returns>如果在列表中找到，则为 <paramref name="item"/> 的索引；否则为 -1。 </returns>
        /// <exception cref="System.ArgumentException"><paramref name="item"/> 属于不能分配给 <see cref="IList"/> 的类型。</exception>
        int IList.IndexOf(object item)
        {
            return ((IList)this.InternalList).IndexOf(item);
        }

        /// <summary>
        /// 将一个项插入指定索引处的 <see cref="IList"/>。
        /// </summary>
        /// <param name="index">从零开始的索引，应在该位置插入 <paramref name="item"/>。</param>
        /// <param name="item">要插入 <see cref="IList"/> 的对象。</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> 不是 <see cref="IList"/> 中的有效索引。</exception>
        /// <exception cref="System.ArgumentException"><paramref name="item"/> 属于不能分配给 <see cref="IList"/> 的类型。</exception>
        void IList.Insert(int index, object item)
        {
            ((IList)this.InternalList).Insert(index, item);
        }

        /// <summary>
        /// 从 <see cref="IList"/> 中移除特定对象的第一个匹配项。
        /// </summary>
        /// <param name="item">要从 IList 中移除的对象。</param>
        /// <exception cref="System.ArgumentException"><paramref name="item"/> 属于不能分配给 <see cref="IList"/> 的类型。</exception>
        void IList.Remove(object item)
        {
            ((IList)this.InternalList).Remove(item);
        }

        #endregion


        #region IEnumerable 接口显式实现

        /// <summary>
        /// 返回一个循环访问集合的枚举数。
        /// </summary>
        /// <returns>可用于循环访问集合的 <see cref="IEnumerator&lt;T&gt;"/>。</returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return ((IEnumerable<T>)this.InternalList).GetEnumerator();
        }

        /// <summary>
        /// 返回一个循环访问集合的枚举数。
        /// </summary>
        /// <returns>一个可用于循环访问集合的 <see cref="IEnumerator"/> 对象。</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.InternalList).GetEnumerator();
        }

        #endregion



        #region 内部方法定义

        private void SetItem(int index, T item)
        {
            T oldItem = this.InternalList[index];
            this.InternalList[index] = item;
            this.OnCollectionReplace(item, oldItem, index);
        }

        #endregion



        #region 公共方法定义 - 引发事件处理的元素操作

        /// <summary>
        /// 引发参数 <see cref="PropertyChangedEventArgs"/> 的 Action 属性值
        /// 为 <seealso cref="NotifyCollectionChangedAction.Add"/> 的 CollectionChanged 事件。
        /// </summary>
        /// <param name="addedItem"></param>
        /// <param name="index"></param>
        /// <param name="oldCapacity"></param>
        public void OnCollectionAdd(T addedItem, int index, int oldCapacity)
        {
            if (this.CollectionChanged != null)
            {
                NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, addedItem, index);
                this.CollectionChanged(this, e);
            }
            bool capacityChanged = oldCapacity == this.Capacity;
            this.OnPropertyChanged(capacityChanged);
        }

        /// <summary>
        /// 引发参数 <see cref="PropertyChangedEventArgs"/> 的 Action 属性值
        /// 为 <seealso cref="NotifyCollectionChangedAction.Add"/> 的 CollectionChanged 事件。
        /// </summary>
        /// <param name="addedItems"></param>
        /// <param name="oldCapacity"></param>
        public void OnCollectionAdd(IList<T> addedItems, int oldCapacity)
        {
            if (addedItems == null || addedItems.Count == 0)
                return;

            if (this.CollectionChanged != null)
            {
                NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, addedItems.ToArray());
                this.CollectionChanged(this, e);
            }
            bool capacityChanged = oldCapacity == this.Capacity;
            this.OnPropertyChanged(capacityChanged);
        }

        /// <summary>
        /// 引发参数 <see cref="PropertyChangedEventArgs"/> 的 Action 属性值
        /// 为 <seealso cref="NotifyCollectionChangedAction.Add"/> 的 CollectionChanged 事件。
        /// </summary>
        /// <param name="addedItems"></param>
        /// <param name="startingIndex"></param>
        /// <param name="oldCapacity"></param>
        public void OnCollectionAdd(IList<T> addedItems, int startingIndex, int oldCapacity)
        {
            if (addedItems == null || addedItems.Count == 0)
                return;

            if (this.CollectionChanged != null)
            {
                NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, addedItems.ToArray(), startingIndex);
                this.CollectionChanged(this, e);
            }
            bool capacityChanged = oldCapacity == this.Capacity;
            this.OnPropertyChanged(capacityChanged);
        }


        /// <summary>
        /// 引发参数 <see cref="PropertyChangedEventArgs"/> 的 Action 属性值
        /// 为 <seealso cref="NotifyCollectionChangedAction.Remove"/> 的 CollectionChanged 事件。
        /// </summary>
        /// <param name="removedItem"></param>
        /// <param name="index"></param>
        public void OnCollectionRemove(T removedItem, int index)
        {
            if (this.CollectionChanged != null)
            {
                NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItem, index);
                this.CollectionChanged(this, e);
            }
            this.OnPropertyChanged(false);
        }

        /// <summary>
        /// 引发参数 <see cref="PropertyChangedEventArgs"/> 的 Action 属性值
        /// 为 <seealso cref="NotifyCollectionChangedAction.Remove"/> 的 CollectionChanged 事件。
        /// </summary>
        /// <param name="removedItems"></param>
        public void OnCollectionRemove(IList<T> removedItems)
        {
            if (removedItems == null || removedItems.Count == 0)
                return;

            if (this.CollectionChanged != null)
            {
                NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItems.ToArray());
                this.CollectionChanged(this, e);
            }
            this.OnPropertyChanged(false);
        }

        /// <summary>
        /// 引发参数 <see cref="PropertyChangedEventArgs"/> 的 Action 属性值
        /// 为 <seealso cref="NotifyCollectionChangedAction.Remove"/> 的 CollectionChanged 事件。
        /// </summary>
        /// <param name="removedItems"></param>
        /// <param name="startingIndex"></param>
        public void OnCollectionRemove(IList<T> removedItems, int startingIndex)
        {
            if (removedItems == null || removedItems.Count == 0)
                return;

            if (this.CollectionChanged != null)
            {
                NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItems.ToArray(), startingIndex);
                this.CollectionChanged(this, e);
            }
            this.OnPropertyChanged(false);
        }


        /// <summary>
        /// 引发参数 <see cref="PropertyChangedEventArgs"/> 的 Action 属性值
        /// 为 <seealso cref="NotifyCollectionChangedAction.Move"/> 的 CollectionChanged 事件。
        /// </summary>
        /// <param name="movedItem"></param>
        /// <param name="newIndex"></param>
        /// <param name="oldIndex"></param>
        public void OnCollectionMove(T movedItem, int newIndex, int oldIndex)
        {
            if (this.CollectionChanged != null)
            {
                NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, movedItem, newIndex, oldIndex);
                this.CollectionChanged(this, e);
            }
            this.OnPropertyChanged(false);
        }

        /// <summary>
        /// 引发参数 <see cref="PropertyChangedEventArgs"/> 的 Action 属性值
        /// 为 <seealso cref="NotifyCollectionChangedAction.Move"/> 的 CollectionChanged 事件。
        /// </summary>
        /// <param name="movedItems"></param>
        public void OnCollectionMove(IList<T> movedItems)
        {
            if (movedItems == null || movedItems.Count == 0)
                return;

            if (this.CollectionChanged != null)
            {
                NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, movedItems.ToArray(), 0, 0);
                this.CollectionChanged(this, e);
            }
            this.OnPropertyChanged(false);
        }


        /// <summary>
        /// 引发参数 <see cref="PropertyChangedEventArgs"/> 的 Action 属性值
        /// 为 <seealso cref="NotifyCollectionChangedAction.Replace"/> 的 CollectionChanged 事件。
        /// </summary>
        /// <param name="newItem"></param>
        /// <param name="oldItem"></param>
        /// <param name="index"></param>
        public void OnCollectionReplace(T newItem, T oldItem, int index)
        {
            if (this.CollectionChanged != null)
            {
                NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem, index);
                this.CollectionChanged(this, e);
            }
            this.OnIndexerPropertyChanged();
        }

        /// <summary>
        /// 引发参数 <see cref="PropertyChangedEventArgs"/> 的 Action 属性值
        /// 为 <seealso cref="NotifyCollectionChangedAction.Reset"/> 的 CollectionChanged 事件。
        /// </summary>
        /// <param name="items"></param>
        public void OnCollectionReset(IList<T> items)
        {
            if (items == null || items.Count == 0)
                return;

            if (this.CollectionChanged != null)
            {
                NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset, items.ToArray());
                this.CollectionChanged(this, e);
            }
            this.OnPropertyChanged(false);
        }


        private void OnPropertyChanged(bool capacityChanged)
        {
            this.OnPropertyChanged(_CountString);
            this.OnPropertyChanged(_IndexerName);
            if (capacityChanged)
                this.OnPropertyChanged(_CapacityName);
        }

        private void OnIndexerPropertyChanged()
        {
            this.OnPropertyChanged(_IndexerName);
        }


        /// <summary>
        /// 引发带有提供参数的 PropertyChanged 事件。
        /// </summary>
        /// <param name="propertyName"></param>
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
            this.OnPropertyChanged(e);
        }

        /// <summary>
        /// 引发带有提供参数的 PropertyChanged 事件。
        /// </summary>
        /// <param name="e"></param>
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null && e != null)
            {
                this.PropertyChanged(this, e);
            }
        }

        #endregion


        #region INotifyCollectionChanged 和 INotifyPropertyChanged 接口实现

        /// <summary>
        /// 在往 <see cref="ObservableList&lt;T&gt;"/> 列表中添加、移除、更改或移动项或者在刷新整个列表时发生。
        /// </summary>
        [field: NonSerialized]
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// 在更改 <see cref="ObservableList&lt;T&gt;"/> 列表对象的属性值时发生。
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion


    }
}
