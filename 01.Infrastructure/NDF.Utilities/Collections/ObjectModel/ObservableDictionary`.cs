using System;
using System.Collections;
using System.Collections.Generic;
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
    /// 表示一个动态键和值数据集合字典。在添加项、移除项或刷新整个列表时，此集合将提供通知。
    /// </summary>
    /// <typeparam name="TKey">字典中的键的类型。</typeparam>
    /// <typeparam name="TValue">字典中的值的类型。</typeparam>
    [Serializable]
    [ComVisible(false)]
    [DebuggerDisplay("Count = {Count}")]
    public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private const string _CountString = "Count";
        private const string _IndexerName = "Item[]";
        private const string _KeysName = "Keys";
        private const string _ValuesName = "Values";

        private Dictionary<TKey, TValue> _dictionary;


        #region 构造函数定义

        /// <summary>
        /// 初始化 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 类的新实例，该实例为空且具有默认的初始容量，并使用键类型的默认相等比较器。
        /// </summary>
        public ObservableDictionary()
        {
            this._dictionary = new Dictionary<TKey, TValue>();
        }


        /// <summary>
        /// 初始化 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 类的新实例，该实例包含从指定的 <see cref="IDictionary&lt;TKey, TValue&gt;"/> 中复制的元素并为键类型使用默认的相等比较器。
        /// </summary>
        /// <param name="dictionary"><see cref="IDictionary&lt;TKey, TValue&gt;"/>，它的元素被复制到新的 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 中。</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="dictionary"/> 为 null。</exception>
        /// <exception cref="System.ArgumentException"><paramref name="dictionary"/> 包含一个或多个重复键。</exception>
        public ObservableDictionary(IDictionary<TKey, TValue> dictionary)
        {
            this._dictionary = new Dictionary<TKey, TValue>(dictionary);
        }

        /// <summary>
        /// 初始化 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 类的新实例，该实例为空且具有默认的初始容量，并使用指定的 <see cref="IEqualityComparer&lt;T&gt;"/>。
        /// </summary>
        /// <param name="comparer">比较键时要使用的 <see cref="IEqualityComparer&lt;T&gt;"/> 实现，或者为 null，以便为键类型使用默认的 <see cref="IEqualityComparer&lt;T&gt;"/>。</param>
        public ObservableDictionary(IEqualityComparer<TKey> comparer)
        {
            this._dictionary = new Dictionary<TKey, TValue>(comparer);
        }


        /// <summary>
        /// 初始化 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 类的新实例，该实例为空且具有指定的初始容量，并为键类型使用默认的相等比较器。
        /// </summary>
        /// <param name="capacity"><see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 可包含的初始元素数。</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="capacity"/> 小于 0。</exception>
        public ObservableDictionary(int capacity)
        {
            this._dictionary = new Dictionary<TKey, TValue>(capacity);
        }


        /// <summary>
        /// 初始化 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 类的新实例，该实例包含从指定的 <see cref="IDictionary&lt;TKey, TValue&gt;"/> 中复制的元素并使用指定的 <see cref="IEqualityComparer&lt;T&gt;"/>。
        /// </summary>
        /// <param name="dictionary"><see cref="IDictionary&lt;TKey, TValue&gt;"/>，它的元素被复制到新的 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 中。</param>
        /// <param name="comparer">比较键时要使用的 <see cref="IEqualityComparer&lt;T&gt;"/> 实现，或者为 null，以便为键类型使用默认的 <see cref="EqualityComparer&lt;T&gt;"/>。</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="dictionary"/> 为 null。</exception>
        /// <exception cref="System.ArgumentException"><paramref name="dictionary"/> 包含一个或多个重复键。</exception>
        public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            this._dictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
        }

        /// <summary>
        /// 初始化 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 类的新实例，该实例为空且具有指定的初始容量，并使用指定的 <see cref="IEqualityComparer&lt;T&gt;"/>。
        /// </summary>
        /// <param name="capacity"><see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 可包含的初始元素数。</param>
        /// <param name="comparer">比较键时要使用的 <see cref="IEqualityComparer&lt;T&gt;"/> 实现，或者为 null，以便为键类型使用默认的 <see cref="EqualityComparer&lt;T&gt;"/>。</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="capacity"/> 小于 0。</exception>
        public ObservableDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            this._dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
        }


        #endregion



        #region 公共属性定义

        /// <summary>
        /// 获取用于确定字典中的键是否相等的 <see cref="IEqualityComparer&lt;TKey&gt;"/>。
        /// </summary>
        public IEqualityComparer<TKey> Comparer
        {
            get { return this.Dictionary.Comparer; }
        }

        #endregion


        #region 公共方法定义

        /// <summary>
        /// 确定 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 是否包含特定值。
        /// </summary>
        /// <param name="value">要在 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 中定位的值。 对于引用类型，该值可以为 null。</param>
        /// <returns>如果 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 包含具有指定值的元素，则为 true；否则为 false。</returns>
        public bool ContainsValue(TValue value)
        {
            return this.Dictionary.ContainsValue(value);
        }

        #endregion


        #region 内部属性定义

        /// <summary>
        /// 获取此 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 对象内部的实际数据存储字典。
        /// </summary>
        protected Dictionary<TKey, TValue> Dictionary
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
            set { this.AddOrUpdate(key, value, false); }
        }

        /// <summary>
        /// 获取包含在 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 中的键/值对的数目。
        /// </summary>
        public int Count
        {
            get { return this.Dictionary.Count; }
        }

        /// <summary>
        /// 获取包含 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 中的键的集合。
        /// </summary>
        public ICollection<TKey> Keys
        {
            get { return this.Dictionary.Keys; }
        }

        /// <summary>
        /// 获取包含 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 中的值的集合。
        /// </summary>
        public ICollection<TValue> Values
        {
            get { return this.Dictionary.Values; }
        }

        #endregion


        #region IDictionary 接口方法实现

        /// <summary>
        /// 在 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 中添加一个带有所提供的键和值的元素。
        /// <para>该操作将会引发 Action 为 <seealso cref="NotifyCollectionChangedAction.Add"/> 的 CollectionChanged 事件 和 PropertyChanged 事件。</para>
        /// </summary>
        /// <param name="key">被添加的元素的键。</param>
        /// <param name="value">被添加的元素的值。</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="key"/> 为 null</exception>
        /// <exception cref="System.ArgumentException"><see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 中已存在具有相同键的元素。</exception>
        public void Add(TKey key, TValue value)
        {
            this.AddOrUpdate(key, value, true);
        }

        /// <summary>
        /// 从 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 中移除带有指定键的元素。
        /// <para>该操作成功时将会引发 Action 为 <seealso cref="NotifyCollectionChangedAction.Remove"/> 的 CollectionChanged 事件 和 PropertyChanged 事件。</para>
        /// </summary>
        /// <param name="key">要移除的元素的键。</param>
        /// <returns>如果该元素已成功移除，则为 true；否则为 false。 如果在原始 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 中没有找到 <paramref name="key"/>，此方法也会返回 false。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="key"/> 为 null</exception>
        public bool Remove(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            TValue value;
            this.Dictionary.TryGetValue(key, out value);
            bool removed = this.Dictionary.Remove(key);
            if (removed)
            {
                var item = new KeyValuePair<TKey, TValue>(key, value);
                this.OnCollectionRemove(item);
            }

            return removed;
        }

        /// <summary>
        /// 获取与指定的键相关联的值。
        /// </summary>
        /// <param name="key">要获取的值的键。</param>
        /// <param name="value">当此方法返回值时，如果找到该键，便会返回与指定的键相关联的值；否则，则会返回 value 参数的类型默认值。 该参数未经初始化即被传递。</param>
        /// <returns>如果 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 包含具有指定键的元素，则为 true；否则为 false。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="key"/> 为 null。</exception>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.Dictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// 从 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 中移除所有的键和值。
        /// <para>该操作将会引发 Action 为 <seealso cref="NotifyCollectionChangedAction.Reset"/> 的 CollectionChanged 事件 和 PropertyChanged 事件。</para>
        /// </summary>
        public void Clear()
        {
            if (this.Dictionary.Count > 0)
            {
                List<KeyValuePair<TKey, TValue>> items = this.Dictionary.ToList();
                this.Dictionary.Clear();
                this.OnCollectionReset(items);
            }
        }

        /// <summary>
        /// 确定 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 是否包含指定的键。
        /// </summary>
        /// <param name="key">要在 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 中定位的键。</param>
        /// <returns>如果 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 包含具有指定键的元素，则为 true；否则为 false。</returns>
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
        /// 在 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 中添加一个带有所提供的键和值的元素。
        /// <para>该操作将会引发 Action 为 <seealso cref="NotifyCollectionChangedAction.Add"/> 的 CollectionChanged 事件 和 PropertyChanged 事件。</para>
        /// </summary>
        /// <param name="item">需要添加的键值对元素。</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="item"/> 的 Key 属性值为 null</exception>
        /// <exception cref="System.ArgumentException"><see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 中已存在具有相同键的元素。</exception>
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            this.Add(item.Key, item.Value);
        }

        /// <summary>
        /// 从 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 中移除带有指定键的元素。
        /// <para>该操作成功时将会引发 Action 为 <seealso cref="NotifyCollectionChangedAction.Add"/> 的 CollectionChanged 事件 和 PropertyChanged 事件。</para>
        /// </summary>
        /// <param name="item">要移除的元素。</param>
        /// <returns>如果该元素已成功移除，则为 true；否则为 false。 如果在原始 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 中没有找到 <paramref name="item"/> 的 Key 属性所示的键值，此方法也会返回 false。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="item"/> 或 <paramref name="item"/> 的 Key 属性值为 null</exception>
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            return this.Remove(item.Key);
        }

        /// <summary>
        /// 确定 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 是否包含特定值。
        /// </summary>
        /// <param name="item">要在 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 中定位的对象。</param>
        /// <returns>如果在 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 中找到 <paramref name="item"/>，则为 true；否则为 false。</returns>
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)this.Dictionary).Contains(item);
        }

        /// <summary>
        /// 从特定的 <see cref="System.Array"/> 索引开始，将 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 的元素复制到一个 <see cref="System.Array"/> 中。
        /// </summary>
        /// <param name="array">作为从 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 复制的元素的目标的一维 <see cref="System.Array"/>。 <see cref="System.Array"/> 必须具有从零开始的索引。</param>
        /// <param name="arrayIndex"><paramref name="array"/> 中从零开始的索引，从此索引处开始进行复制。</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="array"/> 为 null。</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> 小于 0。</exception>
        /// <exception cref="System.ArgumentException">源 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 中的元素数目大于从 <paramref name="arrayIndex"/> 到目标 <paramref name="array"/> 末尾之间的可用空间。</exception>
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)this.Dictionary).CopyTo(array, arrayIndex);
        }

        #endregion



        #region 批量处理元素的方法定义

        /// <summary>
        /// 将将一组键值对元素添加到 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 字典中。
        /// <para>如果被添加的元素数量大于 0 ，该操作将会引发 Action 为 <seealso cref="NotifyCollectionChangedAction.Add"/> 的 CollectionChanged 事件 和 PropertyChanged 事件。</para>
        /// </summary>
        /// <param name="items">将被添加至 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 字典中的元素集合。</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="items"/> 为 null。</exception>
        /// <exception cref="System.ArgumentException"><see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 字典中已经存在一个或多个和被添加列表 <paramref name="items"/> 中元素相同的键；或者 <paramref name="items"/> 存在键值重复的项。</exception>
        public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            var array = items.ToArray();
            if (array.Length > 0)
            {
                if (array.Length != array.Select(item => item.Key).Distinct(this.Comparer).Count())
                    throw new ArgumentException("传入的键值对集合中存在键值重复的项。");

                if (array.Any(item => this.Dictionary.ContainsKey(item.Key)))
                    throw new ArgumentException("当前字典中已经存在一个或多个和被添加列表中元素相同的键。");

                foreach (var item in array)
                {
                    this.Dictionary.Add(item.Key, item.Value);
                }
                this.OnCollectionAdd(array);
            }
        }

        /// <summary>
        /// 从 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 字典中移除一组键值所指定的元素。
        /// <para>如果被移除的元素数量大于 0，该操作将会引发 Action 为 <seealso cref="NotifyCollectionChangedAction.Remove"/> 的 CollectionChanged 事件 和 PropertyChanged 事件。</para>
        /// </summary>
        /// <param name="keys">一组键值。</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="keys"/> 为 null。</exception>
        public void RemoveRange(IEnumerable<TKey> keys)
        {
            if (keys == null)
                throw new ArgumentNullException("keys");

            List<KeyValuePair<TKey, TValue>> items = new List<KeyValuePair<TKey, TValue>>();
            foreach (var key in keys)
            {
                TValue value;
                if (this.Dictionary.TryGetValue(key, out value) && this.Dictionary.Remove(key))
                {
                    var item = new KeyValuePair<TKey, TValue>(key, value);
                    items.Add(item);
                }
            }
            this.OnCollectionRemove(items);
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


        #region 内部方法定义

        private void AddOrUpdate(TKey key, TValue value, bool add)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            TValue item;
            if (this.Dictionary.TryGetValue(key, out item))
            {
                if (add)
                    throw new ArgumentException("当前字典中已存在具有相同键的元素。");

                if (object.Equals(item, value))
                    return;

                this.Dictionary[key] = value;

                var newItem = new KeyValuePair<TKey, TValue>(key, value);
                var oldItem = new KeyValuePair<TKey, TValue>(key, item);
                this.OnCollectionReplace(newItem, oldItem);
            }
            else
            {
                this.Dictionary[key] = value;
                var entry = new KeyValuePair<TKey, TValue>(key, value);
                this.OnCollectionAdd(entry);
            }
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
        public void OnCollectionReset(IList<KeyValuePair<TKey, TValue>> items)
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
        /// 在往 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 字典中添加、移除、更改或移动项或者在刷新整个列表时发生。
        /// </summary>
        [field: NonSerialized]
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// 在更改 <see cref="ObservableDictionary&lt;TKey, TValue&gt;"/> 字典对象的属性值时发生。
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion


    }
}
