using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Collections.Concurrent
{
    /// <summary>
    /// 表示可由多个线程同时访问键/值对线程安全的动态键和值数据集合字典。在添加项、移除项或刷新整个列表时，此集合将提供通知。
    /// </summary>
    [Serializable]
    [ComVisible(false)]
    [HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
    [DebuggerDisplay("Count = {Count}")]
    public class ObservableConcurrentDictionary<TKey, TValue> : IDictionary<TKey, TValue>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private const string _CountString = "Count";
        private const string _IndexerName = "Item[]";
        private const string _KeysName = "Keys";
        private const string _ValuesName = "Values";

        private IEqualityComparer<TKey> _comparer;
        private ConcurrentDictionary<TKey, TValue> _dictionary;

        private object _locker = new object();


        #region 构造函数定义

        /// <summary>
        /// 初始化 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 类的新实例，该实例为空，具有默认的并发级别和默认的初始容量，并为键类型使用默认比较器。
        /// </summary>
        public ObservableConcurrentDictionary()
        {
            this._dictionary = new ConcurrentDictionary<TKey, TValue>();
        }


        /// <summary>
        /// 初始化 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 类的新实例，该实例包含从指定的 <see cref="IEnumerable&lt;T&gt;"/> 中复制的元素，具有默认的并发级别和默认的初始容量，并为键类型使用默认比较器。
        /// </summary>
        /// <param name="collection"><see cref="IEnumerable&lt;T&gt;"/>，其元素被复制到新的 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 中。</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="collection"/> 集合为 null 或集合中任何一个元素为 null。</exception>
        /// <exception cref="System.ArgumentException"><paramref name="collection"/> 包含一个或多个重复键。</exception>
        public ObservableConcurrentDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection)
        {
            this._dictionary = new ConcurrentDictionary<TKey, TValue>(collection);
        }

        /// <summary>
        /// 初始化 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 类的新实例，该实例为空，具有默认的并发级别和容量，并使用指定的 <see cref="IEqualityComparer&lt;T&gt;"/>。
        /// </summary>
        /// <param name="comparer">比较键时要使用的 <see cref="IEqualityComparer&lt;T&gt;"/> 实现，或者为 null，以便为键类型使用默认的 <see cref="IEqualityComparer&lt;T&gt;"/>。</param>
        public ObservableConcurrentDictionary(IEqualityComparer<TKey> comparer)
        {
            comparer = comparer ?? EqualityComparer<TKey>.Default;
            this._comparer = comparer;
            this._dictionary = new ConcurrentDictionary<TKey, TValue>(comparer);
        }

        /// <summary>
        /// 初始化 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 类的新实例，该实例包含从指定的 <see cref="IEnumerable&lt;T&gt;"/> 中复制的元素，具有默认的并发级别和默认的初始容量，并使用指定的 <see cref="IEqualityComparer&lt;T&gt;"/>。
        /// </summary>
        /// <param name="collection"><see cref="IEqualityComparer&lt;T&gt;"/>，其元素被复制到新的 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 中。</param>
        /// <param name="comparer">比较键时要使用的 <see cref="IEqualityComparer&lt;T&gt;"/> 实现，或者为 null，以便为键类型使用默认的 <see cref="IEqualityComparer&lt;T&gt;"/>。</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="collection"/> 为 null。</exception>
        public ObservableConcurrentDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer)
        {
            comparer = comparer ?? EqualityComparer<TKey>.Default;
            this._comparer = comparer;
            this._dictionary = new ConcurrentDictionary<TKey, TValue>(collection, comparer);
        }


        /// <summary>
        /// 初始化 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 类的新实例，该实例为空，具有指定的并发级别和容量，并为键类型使用默认比较器。
        /// </summary>
        /// <param name="concurrencyLevel">将同时更新 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 的线程的估计数量。</param>
        /// <param name="capacity"><see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 可包含的初始元素数。</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="concurrencyLevel"/> 小于 1。或 <paramref name="capacity"/> 小于 0。</exception>
        public ObservableConcurrentDictionary(int concurrencyLevel, int capacity)
        {
            this._dictionary = new ConcurrentDictionary<TKey, TValue>(concurrencyLevel, capacity);
        }


        /// <summary>
        /// 初始化 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 类的新实例，该实例包含从指定的 <see cref="IEnumerable&lt;T&gt;"/> 中复制的元素并使用指定的 <see cref="IEqualityComparer&lt;T&gt;"/>。
        /// </summary>
        /// <param name="concurrencyLevel">将同时更新 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 的线程的估计数量。</param>
        /// <param name="collection"><see cref="IEnumerable&lt;T&gt;"/>，其元素被复制到新的 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 中。</param>
        /// <param name="comparer">比较键时要使用的 <see cref="IEqualityComparer&lt;T&gt;"/> 实现，或者为 null，以便为键类型使用默认的 <see cref="IEqualityComparer&lt;T&gt;"/>。</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="collection"/> 为 null。</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="concurrencyLevel"/> 小于 1。</exception>
        /// <exception cref="System.ArgumentException"><paramref name="collection"/> 包含一个或多个重复键。</exception>
        public ObservableConcurrentDictionary(int concurrencyLevel, IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer)
        {
            comparer = comparer ?? EqualityComparer<TKey>.Default;
            this._comparer = comparer;
            this._dictionary = new ConcurrentDictionary<TKey, TValue>(concurrencyLevel, collection, comparer);
        }

        /// <summary>
        /// 初始化 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 类的新实例，该实例为空，具有指定的并发级别和指定的初始容量，并使用指定的 <see cref="IEqualityComparer&lt;T&gt;"/>。
        /// </summary>
        /// <param name="concurrencyLevel">将同时更新 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 的线程的估计数量。</param>
        /// <param name="capacity"><see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 可包含的初始元素数。</param>
        /// <param name="comparer">比较键时要使用的 <see cref="IEqualityComparer&lt;T&gt;"/> 实现，或者为 null，以便为键类型使用默认的 <see cref="IEqualityComparer&lt;T&gt;"/>。</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="concurrencyLevel"/> 或 <paramref name="capacity"/> 小于 1。</exception>
        public ObservableConcurrentDictionary(int concurrencyLevel, int capacity, IEqualityComparer<TKey> comparer)
        {
            comparer = comparer ?? EqualityComparer<TKey>.Default;
            this._comparer = comparer;
            this._dictionary = new ConcurrentDictionary<TKey, TValue>(concurrencyLevel, capacity, comparer);
        }


        #endregion



        #region 公共属性定义

        /// <summary>
        /// 获取用于确定字典中的键是否相等的 <see cref="IEqualityComparer&lt;TKey&gt;"/>。
        /// </summary>
        public IEqualityComparer<TKey> Comparer
        {
            get { return _comparer ?? EqualityComparer<TKey>.Default; }
        }

        /// <summary>
        /// 获取一个指示 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 是否为空的值。
        /// </summary>
        public bool IsEmpty
        {
            get { return this.Dictionary.IsEmpty; }
        }

        #endregion


        #region 公共方法定义

        /// <summary>
        /// 确定 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 是否包含特定值。
        /// </summary>
        /// <param name="value">要在 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 中定位的值。 对于引用类型，该值可以为 null。</param>
        /// <returns>如果 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 包含具有指定值的元素，则为 true；否则为 false。</returns>
        public bool ContainsValue(TValue value)
        {
            if (this.Count == 0)
                return false;

            return this.Values.Contains(value);
        }


        /// <summary>
        /// 如果指定的键尚不存在，则将键/值对添加到 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 中；如果指定的键已存在，则更新 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 中的键/值对。
        /// <para>该操作将会引发 CollectionChanged 事件 和 PropertyChanged 事件。</para>
        /// <para>如果在不同的线程同时调用 AddOrUpdate，addValueFactory 可能多次调用，但是，其键/值对可能不添加到每个字典调用。</para>
        /// </summary>
        /// <param name="key">要添加的键或应更新其值的键 </param>
        /// <param name="addValueFactory">用于为空缺键生成值的函数</param>
        /// <param name="updateValueFactory">用于根据现有键的现有值为键生成新值的函数</param>
        /// <returns>键的新值。 这将是 <paramref name="addValueFactory"/> 的结果（如果缺少键）或 <paramref name="updateValueFactory"/> 的结果（如果存在键）。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="key"/>、<paramref name="addValueFactory"/> 或 <paramref name="updateValueFactory"/> 为 null</exception>
        /// <exception cref="System.OverflowException">字典已包含元素 ( MaxValue) 的最大数目。</exception>
        public TValue AddOrUpdate(TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory)
        {
            TValue oldValue;
            TValue value;
            if (this.TryGetValue(key, out oldValue))
            {
                lock (_locker)
                {
                    value = this.Dictionary.AddOrUpdate(key, addValueFactory, updateValueFactory);
                    var newItem = new KeyValuePair<TKey, TValue>(key, value);
                    var oldItem = new KeyValuePair<TKey, TValue>(key, oldValue);
                    this.OnCollectionReplace(newItem, oldItem);
                }
            }
            else
            {
                lock (_locker)
                {
                    value = this.Dictionary.AddOrUpdate(key, addValueFactory, updateValueFactory);
                    var item = new KeyValuePair<TKey, TValue>(key, value);
                    this.OnCollectionAdd(item);
                }
            }
            return value;
        }

        /// <summary>
        /// 如果指定的键尚不存在，则将键/值对添加到 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 中；如果指定的键已存在，则更新 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 中的键/值对。
        /// <para>该操作将会引发 CollectionChanged 事件 和 PropertyChanged 事件。</para>
        /// </summary>
        /// <param name="key">要添加的键或应更新其值的键</param>
        /// <param name="addValue">要为空缺键添加的值</param>
        /// <param name="updateValueFactory">用于根据现有键的现有值为键生成新值的函数</param>
        /// <returns>键的新值。 这将是 <paramref name="addValue"/> 的结果（如果缺少键）或 <paramref name="updateValueFactory"/> 的结果（如果存在键）。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="key"/> 是 null 引用（在 Visual Basic 中为 Nothing）。 - 或 - <paramref name="updateValueFactory"/> 是 null 引用（在 Visual Basic 中为 Nothing）。</exception>
        /// <exception cref="System.OverflowException">字典已包含最大数目的元素，System.Int32.MaxValue。</exception>
        public TValue AddOrUpdate(TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory)
        {
            TValue oldValue;
            TValue value;
            if (this.TryGetValue(key, out oldValue))
            {
                lock (_locker)
                {
                    value = this.Dictionary.AddOrUpdate(key, addValue, updateValueFactory);
                    var newItem = new KeyValuePair<TKey, TValue>(key, value);
                    var oldItem = new KeyValuePair<TKey, TValue>(key, oldValue);
                    this.OnCollectionReplace(newItem, oldItem);
                }
            }
            else
            {
                lock (_locker)
                {
                    value = this.Dictionary.AddOrUpdate(key, addValue, updateValueFactory);
                    var item = new KeyValuePair<TKey, TValue>(key, value);
                    this.OnCollectionAdd(item);
                }
            }
            return value;
        }

        /// <summary>
        /// 如果指定的键尚不存在，则将键/值对添加到 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 中；如果指定的键已存在，则更新 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 中的键/值对。
        /// <para>该操作将会引发 CollectionChanged 事件 和 PropertyChanged 事件。</para>
        /// </summary>
        /// <param name="key">要添加的键或应更新其值的键</param>
        /// <param name="addValue">要为空缺键添加的值</param>
        /// <param name="updateValue">要为已存在键更新的新值。</param>
        /// <returns>键的新值。 这将是 <paramref name="addValue"/> （如果缺少键）或 <paramref name="updateValue"/> （如果存在键）。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="key"/> 是 null 引用（在 Visual Basic 中为 Nothing）。</exception>
        /// <exception cref="System.OverflowException">字典已包含最大数目的元素，System.Int32.MaxValue。</exception>
        public TValue AddOrUpdate(TKey key, TValue addValue, TValue updateValue)
        {
            Func<TKey, TValue, TValue> updateValueFactory = (k, v) => updateValue;
            return this.AddOrUpdate(key, addValue, updateValueFactory);
        }

        /// <summary>
        /// 如果指定的键尚不存在，则将键/值对添加到 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 中；如果指定的键已存在，则更新 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 中的键/值对。
        /// <para>该操作将会引发 CollectionChanged 事件 和 PropertyChanged 事件。</para>
        /// </summary>
        /// <param name="key">要添加的键或应更新其值的键</param>
        /// <param name="value">要为空缺键添加的值；如果字典中已存在 <paramref name="key"/> 键，则用该参数更新 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 中 <paramref name="key"/> 键对应的值。</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="key"/> 为 null。</exception>
        public void AddOrUpdate(TKey key, TValue value)
        {
            this.AddOrUpdate(key, value, value);
        }


        /// <summary>
        /// 如果指定的键尚不存在，则将键/值对添加到 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/>。
        /// <para>如果该方法往字典中执行了添加数据操作，则将会引发 CollectionChanged 事件 和 PropertyChanged 事件。</para>
        /// <para>如果您在不同的线程同时调用 GetOrAdd，<paramref name="valueFactory"/> 可能多次调用，但是，其键/值对可能不添加到每个字典调用。</para>
        /// </summary>
        /// <param name="key">要添加的元素的键。 </param>
        /// <param name="valueFactory">用于为键生成值的函数</param>
        /// <returns>键的值。 如果字典中已存在指定的键，则为该键的现有值；如果字典中不存在指定的键，则为 <paramref name="valueFactory"/> 返回的键的新值。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="key"/> 或 <paramref name="valueFactory"/> 为 null。</exception>
        /// <exception cref="System.OverflowException">字典已包含元素 ( MaxValue) 的最大数目。</exception>
        public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
        {
            TValue value;
            if (this.Dictionary.TryGetValue(key, out value))
            {
                return value;
            }
            else
            {
                lock (_locker)
                {
                    value = this.Dictionary.GetOrAdd(key, valueFactory);
                    var item = new KeyValuePair<TKey, TValue>(key, value);
                    this.OnCollectionAdd(item);
                }
            }
            return value;
        }

        /// <summary>
        /// 如果指定的键尚不存在，则将键/值对添加到 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 中。
        /// <para>如果该方法往字典中执行了添加数据操作，则将会引发 CollectionChanged 事件 和 PropertyChanged 事件。</para>
        /// </summary>
        /// <param name="key">要添加的元素的键。 </param>
        /// <param name="value">指定的键不存在时要添加的值 </param>
        /// <returns>键的值。 如果字典中已存在指定的键，则为该键的现有值；如果字典中不存在指定的键，则为新值。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="key"/> 为 null。</exception>
        /// <exception cref="System.OverflowException">字典已包含元素 ( MaxValue) 的最大数目。</exception>
        public TValue GetOrAdd(TKey key, TValue value)
        {
            TValue local;
            if (this.Dictionary.TryGetValue(key, out local))
            {
                return local;
            }
            else
            {
                lock (_locker)
                {
                    local = this.Dictionary.GetOrAdd(key, value);
                    var item = new KeyValuePair<TKey, TValue>(key, local);
                    this.OnCollectionAdd(item);
                }
            }
            return local;
        }


        /// <summary>
        /// 将 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 中存储的键和值对复制到新数组中。
        /// </summary>
        /// <returns>一个新数组，其中包含从 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 复制的键和值对的快照。</returns>
        public KeyValuePair<TKey, TValue>[] ToArray()
        {
            return this.Dictionary.ToArray();
        }


        /// <summary>
        /// 尝试将指定的键和值添加到 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 中。
        /// <para>如果该方法往字典中执行了添加数据操作，则将会引发 CollectionChanged 事件 和 PropertyChanged 事件。</para>
        /// </summary>
        /// <param name="key">要添加的元素的键。</param>
        /// <param name="value">要添加的元素的值。 对于引用类型，该值可以为 null。</param>
        /// <returns>如果该键/值对已成功添加到 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 则返回 true；如果此键已存在则返回 false。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="key"/> 是 null 引用（在 Visual Basic 中为 Nothing）。</exception>
        /// <exception cref="System.OverflowException">字典已包含最大数目的元素，System.Int32.MaxValue。</exception>
        public bool TryAdd(TKey key, TValue value)
        {
            lock (_locker)
            {
                bool added = this.Dictionary.TryAdd(key, value);
                if (added)
                {
                    var item = new KeyValuePair<TKey, TValue>(key, value);
                    this.OnCollectionAdd(item);
                }
                return added;
            }
        }

        /// <summary>
        /// 尝试从 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 中移除并返回具有指定键的值。
        /// <para>如果该方法往字典中执行了移除数据操作，则将会引发 CollectionChanged 事件 和 PropertyChanged 事件。</para>
        /// </summary>
        /// <param name="key">要移除并返回的元素的键。</param>
        /// <param name="value">此方法返回时，<paramref name="value"/> 包含从 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 中移除的对象；如果操作失败，则包含默认值。</param>
        /// <returns>如果成功移除了对象，则为 true；否则为 false。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="key"/> 是 null 引用（在 Visual Basic 中为 Nothing）。</exception>
        public bool TryRemove(TKey key, out TValue value)
        {
            lock (_locker)
            {
                bool removed = this.Dictionary.TryRemove(key, out value);
                if (removed)
                {
                    var item = new KeyValuePair<TKey, TValue>(key, value);
                    this.OnCollectionRemove(item);
                }
                return removed;
            }
        }

        /// <summary>
        /// 将指定键的现有值与指定值进行比较，如果相等，则用第三个值更新该键。
        /// <para>如果该方法往字典中执行了更新数据操作，则将会引发 CollectionChanged 事件 和 PropertyChanged 事件。</para>
        /// </summary>
        /// <param name="key">其值将与 <paramref name="comparisonValue"/> 进行比较并且可能被替换的键。</param>
        /// <param name="newValue">一个值，当比较结果相等时，将用该值替换具有 key 的元素的值。</param>
        /// <param name="comparisonValue">与具有 <paramref name="key"/> 的元素的值进行比较的值。</param>
        /// <returns>如果具有 <paramref name="key"/> 的值与 <paramref name="comparisonValue"/> 相等并替换为 <paramref name="newValue"/>，则为 true；否则为 false。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="key"/> 为 null 引用。</exception>
        public bool TryUpdate(TKey key, TValue newValue, TValue comparisonValue)
        {
            lock (_locker)
            {
                bool updated = this.Dictionary.TryUpdate(key, newValue, comparisonValue);
                if (updated)
                {
                    var newItem = new KeyValuePair<TKey, TValue>(key, newValue);
                    var oldItem = new KeyValuePair<TKey, TValue>(key, comparisonValue);
                    this.OnCollectionReplace(newItem, oldItem);
                }
                return updated;
            }
        }


        #endregion


        #region 内部属性定义

        /// <summary>
        /// 获取此 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 对象内部的实际数据存储字典。
        /// </summary>
        protected ConcurrentDictionary<TKey, TValue> Dictionary
        {
            get { return this._dictionary; }
        }

        #endregion



        #region IDictionary 接口属性实现

        /// <summary>
        /// 获取或设置与指定的键相关联的值。
        /// </summary>
        /// <param name="key">要获取或设置的值的键。</param>
        /// <returns>与指定的键相关联的值。 如果找不到指定的键，get 操作便会引发 <see cref="KeyNotFoundException"/>，而 set 操作会创建一个具有指定键的新元素。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="key"/> 为 null。</exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">已检索该属性，并且集合中不存在 <paramref name="key"/>。</exception>
        public TValue this[TKey key]
        {
            get { return this.Dictionary[key]; }
            set { this.AddOrUpdate(key, value); }
        }

        /// <summary>
        /// 获取包含在 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 中的键/值对的数目。
        /// </summary>
        public int Count
        {
            get { return this.Dictionary.Count; }
        }

        /// <summary>
        /// 获取包含 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 中的键的集合。
        /// </summary>
        public ICollection<TKey> Keys
        {
            get { return this.Dictionary.Keys; }
        }

        /// <summary>
        /// 获取包含 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 中的值的集合。
        /// </summary>
        public ICollection<TValue> Values
        {
            get { return this.Dictionary.Values; }
        }

        #endregion


        #region IDictionary 接口方法实现

        /// <summary>
        /// 在 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 中添加一个带有所提供的键和值的元素。
        /// <para>该操作将会引发 Action 为 <seealso cref="NotifyCollectionChangedAction.Add"/> 的 CollectionChanged 事件 和 PropertyChanged 事件。</para>
        /// <para>
        /// 如果当前 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 字典对象同时被不同的线程进行更新集合操作，则即使在
        /// 通过 TryGetValue 方法判断字典中不存在 <paramref name="key"/> 键项后执行 Add 操作，该方法仍可能出现重复键异常；
        /// 在这种情况下，建议使用 TryAdd 进行添加项操作。</para>
        /// </summary>
        /// <param name="key">被添加的元素的键。</param>
        /// <param name="value">被添加的元素的值。</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="key"/> 为 null</exception>
        /// <exception cref="System.ArgumentException"><see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 中已存在具有相同键的元素。</exception>
        /// <exception cref="System.OverflowException">字典已包含最大数目的元素，System.Int32.MaxValue。</exception>
        public void Add(TKey key, TValue value)
        {
            if (!this.TryAdd(key, value))
            {
                throw new ArgumentException("当前字典中已存在具有相同键的元素。");
            }
        }

        /// <summary>
        /// 从 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 中移除带有指定键的元素。
        /// <para>该操作成功时将会引发 Action 为 <seealso cref="NotifyCollectionChangedAction.Remove"/> 的 CollectionChanged 事件 和 PropertyChanged 事件。</para>
        /// </summary>
        /// <param name="key">要移除的元素的键。</param>
        /// <returns>如果该元素已成功移除，则为 true；否则为 false。 如果在原始 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 中没有找到 <paramref name="key"/>，此方法也会返回 false。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="key"/> 为 null</exception>
        public bool Remove(TKey key)
        {
            TValue value;
            return this.TryRemove(key, out value);
        }

        /// <summary>
        /// 获取与指定的键相关联的值。
        /// </summary>
        /// <param name="key">要获取的值的键。</param>
        /// <param name="value">当此方法返回值时，如果找到该键，便会返回与指定的键相关联的值；否则，则会返回 value 参数的类型默认值。 该参数未经初始化即被传递。</param>
        /// <returns>如果 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 包含具有指定键的元素，则为 true；否则为 false。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="key"/> 为 null。</exception>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.Dictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// 从 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 中移除所有的键和值。
        /// <para>该操作将会引发 Action 为 <seealso cref="NotifyCollectionChangedAction.Reset"/> 的 CollectionChanged 事件 和 PropertyChanged 事件。</para>
        /// </summary>
        public void Clear()
        {
            if (this.Dictionary.Count > 0)
            {
                lock (_locker)
                {
                    List<KeyValuePair<TKey, TValue>> items = this.Dictionary.ToList();
                    this.Dictionary.Clear();
                    this.OnCollectionReset(items);
                }
            }
        }

        /// <summary>
        /// 确定 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 是否包含指定的键。
        /// </summary>
        /// <param name="key">要在 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 中定位的键。</param>
        /// <returns>如果 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 包含具有指定键的元素，则为 true；否则为 false。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="key"/> 为 null。</exception>
        public bool ContainsKey(TKey key)
        {
            return this.Dictionary.ContainsKey(key);
        }

        #endregion



        #region ICollection 接口属性实现

        /// <summary>
        /// 获取一个值，该值指示 <see cref="ICollection&lt;T&gt;"/> 是否为只读。
        /// </summary>
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return ((ICollection<KeyValuePair<TKey, TValue>>)this.Dictionary).IsReadOnly; }
        }

        #endregion


        #region ICollection 接口方法实现

        /// <summary>
        /// 在 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 中添加一个带有所提供的键和值的元素。
        /// <para>该操作将会引发 Action 为 <seealso cref="NotifyCollectionChangedAction.Add"/> 的 CollectionChanged 事件 和 PropertyChanged 事件。</para>
        /// </summary>
        /// <param name="item">需要添加的键值对元素。</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="item"/> 的 Key 属性值为 null</exception>
        /// <exception cref="System.ArgumentException"><see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 中已存在具有相同键的元素。</exception>
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            this.Add(item.Key, item.Value);
        }

        /// <summary>
        /// 从 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 中移除带有指定键的元素。
        /// <para>该操作成功时将会引发 Action 为 <seealso cref="NotifyCollectionChangedAction.Add"/> 的 CollectionChanged 事件 和 PropertyChanged 事件。</para>
        /// </summary>
        /// <param name="item">要移除的元素。</param>
        /// <returns>如果该元素已成功移除，则为 true；否则为 false。 如果在原始 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 中没有找到 <paramref name="item"/> 的 Key 属性所示的键值，此方法也会返回 false。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="item"/> 或 <paramref name="item"/> 的 Key 属性值为 null</exception>
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            return this.Remove(item.Key);
        }

        /// <summary>
        /// 确定 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 是否包含特定值。
        /// </summary>
        /// <param name="item">要在 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 中定位的对象。</param>
        /// <returns>如果在 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 中找到 <paramref name="item"/>，则为 true；否则为 false。</returns>
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)this.Dictionary).Contains(item);
        }

        /// <summary>
        /// 从特定的 <see cref="System.Array"/> 索引开始，将 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 的元素复制到一个 <see cref="System.Array"/> 中。
        /// </summary>
        /// <param name="array">作为从 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 复制的元素的目标的一维 <see cref="System.Array"/>。 <see cref="System.Array"/> 必须具有从零开始的索引。</param>
        /// <param name="arrayIndex"><paramref name="array"/> 中从零开始的索引，从此索引处开始进行复制。</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="array"/> 为 null。</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> 小于 0。</exception>
        /// <exception cref="System.ArgumentException">源 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 中的元素数目大于从 <paramref name="arrayIndex"/> 到目标 <paramref name="array"/> 末尾之间的可用空间。</exception>
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)this.Dictionary).CopyTo(array, arrayIndex);
        }

        #endregion



        #region 批量处理元素的方法定义

        /// <summary>
        /// 将将一组键值对元素添加到 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 字典中。
        /// <para>如果被添加的元素数量大于 0 ，该操作将会引发 Action 为 <seealso cref="NotifyCollectionChangedAction.Add"/> 的 CollectionChanged 事件 和 PropertyChanged 事件。</para>
        /// <para>如果当前 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 字典对象同时被不同的线程进行更新集合操作，则该方法的返回值集合的内容不一定完全等于传入的集合参数。</para>
        /// </summary>
        /// <param name="items">将被添加至 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 字典中的元素集合。</param>
        /// <returns>返回 <paramref name="items"/> 集合中被成功添加至 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 字典的部分。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="items"/> 为 null。</exception>
        public IEnumerable<KeyValuePair<TKey, TValue>> TryAddRange(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            var array = items.ToArray();
            if (array.Length > 0)
            {
                if (array.Length != array.Select(item => item.Key).Distinct(this.Comparer).Count())
                    throw new ArgumentException("传入的键值对集合中存在键值重复的项。");

                List<KeyValuePair<TKey, TValue>> list = new List<KeyValuePair<TKey, TValue>>();
                lock (_locker)
                {
                    foreach (var item in array)
                    {
                        if (this.Dictionary.TryAdd(item.Key, item.Value))
                        {
                            var pair = new KeyValuePair<TKey, TValue>(item.Key, item.Value);
                            list.Add(pair);
                        }
                    }
                    this.OnCollectionAdd(list);
                }
                return new ReadOnlyCollection<KeyValuePair<TKey, TValue>>(list);
            }

            var empty = Enumerable.Empty<KeyValuePair<TKey, TValue>>().ToList();
            return new ReadOnlyCollection<KeyValuePair<TKey, TValue>>(empty);
        }

        /// <summary>
        /// 从 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 字典中移除一组键值所指定的元素。
        /// <para>如果被移除的元素数量大于 0，该操作将会引发 Action 为 <seealso cref="NotifyCollectionChangedAction.Remove"/> 的 CollectionChanged 事件 和 PropertyChanged 事件。</para>
        /// <para>如果当前 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 字典对象同时被不同的线程进行更新集合操作，则该方法的返回值集合的内容不一定完全等于传入的键集合参数。</para>
        /// </summary>
        /// <param name="keys">一组键值。</param>
        /// <returns>返回 <paramref name="keys"/> 键集合中被成功从 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 字典中移除的部分。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="keys"/> 为 null。</exception>
        public IEnumerable<KeyValuePair<TKey, TValue>> TryRemoveRange(IEnumerable<TKey> keys)
        {
            if (keys == null)
                throw new ArgumentNullException("keys");

            List<KeyValuePair<TKey, TValue>> items = new List<KeyValuePair<TKey, TValue>>();
            lock (_locker)
            {
                foreach (var key in keys)
                {
                    TValue value;
                    if (this.Dictionary.TryRemove(key, out value))
                    {
                        var item = new KeyValuePair<TKey, TValue>(key, value);
                        items.Add(item);
                    }
                }
                this.OnCollectionRemove(items);
            }

            return new ReadOnlyCollection<KeyValuePair<TKey, TValue>>(items);
        }

        #endregion


        #region IEnumerable 接口实现

        /// <summary>
        /// 返回一个循环访问集合的枚举器。
        /// </summary>
        /// <returns>可用于循环访问集合的 <see cref="IEnumerator&lt;T&gt;"/>。</returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this.Dictionary.GetEnumerator();
        }

        /// <summary>
        /// 返回一个循环访问集合的枚举数。
        /// </summary>
        /// <returns>一个可用于循环访问集合的 <see cref="IEnumerator"/> 对象。</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.Dictionary).GetEnumerator();
        }

        #endregion



        #region 公共方法定义 - 引发事件处理的元素操作

        /// <summary>
        /// 引发参数 <see cref="PropertyChangedEventArgs"/> 的 Action 属性值
        /// 为 <seealso cref="NotifyCollectionChangedAction.Add"/> 的 CollectionChanged 事件。
        /// </summary>
        /// <param name="addedItem"></param>
        public void OnCollectionAdd(KeyValuePair<TKey, TValue> addedItem)
        {
            if (this.CollectionChanged != null)
            {
                NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, addedItem);
                this.CollectionChanged(this, e);
            }
            this.OnPropertyChanged();
        }

        /// <summary>
        /// 引发参数 <see cref="PropertyChangedEventArgs"/> 的 Action 属性值
        /// 为 <seealso cref="NotifyCollectionChangedAction.Add"/> 的 CollectionChanged 事件。
        /// </summary>
        /// <param name="addedItems"></param>
        public void OnCollectionAdd(IList<KeyValuePair<TKey, TValue>> addedItems)
        {
            if (addedItems == null || addedItems.Count == 0)
                return;

            if (this.CollectionChanged != null)
            {
                NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, addedItems.ToArray());
                this.CollectionChanged(this, e);
            }
            this.OnPropertyChanged();
        }


        /// <summary>
        /// 引发参数 <see cref="PropertyChangedEventArgs"/> 的 Action 属性值
        /// 为 <seealso cref="NotifyCollectionChangedAction.Remove"/> 的 CollectionChanged 事件。
        /// </summary>
        /// <param name="removedItem"></param>
        public void OnCollectionRemove(KeyValuePair<TKey, TValue> removedItem)
        {
            if (this.CollectionChanged != null)
            {
                NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItem);
                this.CollectionChanged(this, e);
            }
            this.OnPropertyChanged();
        }

        /// <summary>
        /// 引发参数 <see cref="PropertyChangedEventArgs"/> 的 Action 属性值
        /// 为 <seealso cref="NotifyCollectionChangedAction.Remove"/> 的 CollectionChanged 事件。
        /// </summary>
        /// <param name="removedItems"></param>
        public void OnCollectionRemove(IList<KeyValuePair<TKey, TValue>> removedItems)
        {
            if (removedItems == null || removedItems.Count == 0)
                return;

            if (this.CollectionChanged != null)
            {
                NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItems.ToArray());
                this.CollectionChanged(this, e);
            }
            this.OnPropertyChanged();
        }


        /// <summary>
        /// 引发参数 <see cref="PropertyChangedEventArgs"/> 的 Action 属性值
        /// 为 <seealso cref="NotifyCollectionChangedAction.Replace"/> 的 CollectionChanged 事件。
        /// </summary>
        /// <param name="newItem"></param>
        /// <param name="oldItem"></param>
        public void OnCollectionReplace(KeyValuePair<TKey, TValue> newItem, KeyValuePair<TKey, TValue> oldItem)
        {
            if (this.CollectionChanged != null)
            {
                NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem);
                this.CollectionChanged(this, e);
            }
            this.OnPropertyChangedReplace();
        }

        /// <summary>
        /// 引发参数 <see cref="PropertyChangedEventArgs"/> 的 Action 属性值
        /// 为 <seealso cref="NotifyCollectionChangedAction.Reset"/> 的 CollectionChanged 事件。
        /// </summary>
        /// <param name="items"></param>
        public virtual void OnCollectionReset(IList<KeyValuePair<TKey, TValue>> items)
        {
            if (items == null || items.Count == 0)
                return;

            if (this.CollectionChanged != null)
            {
                NotifyCollectionChangedEventArgs e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset, items.ToArray());
                this.CollectionChanged(this, e);
            }
            this.OnPropertyChanged();
        }


        private void OnPropertyChanged()
        {
            this.OnPropertyChanged(_CountString);
            this.OnPropertyChanged(_IndexerName);
            this.OnPropertyChanged(_KeysName);
            this.OnPropertyChanged(_ValuesName);
        }

        private void OnPropertyChangedReplace()
        {
            this.OnPropertyChanged(_IndexerName);
            this.OnPropertyChanged(_KeysName);
            this.OnPropertyChanged(_ValuesName);
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
        /// 在往 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 字典中添加、移除、更改、移动项或者在刷新整个列表时发生。
        /// </summary>
        [field: NonSerialized]
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// 在更改 <see cref="ObservableConcurrentDictionary&lt;TKey, TValue&gt;"/> 字典对象的属性值时发生。
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion


    }
}
