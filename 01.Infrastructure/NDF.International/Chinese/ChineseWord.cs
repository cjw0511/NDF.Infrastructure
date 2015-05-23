using NDF.International.Chinese.Conversion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace NDF.International.Chinese
{
    /// <summary>
    /// 表示一个中文词语，它是由多个中文字符所组成的一个字符串。
    /// </summary>
    public class ChineseWord :
        IList<ChineseChar>, IReadOnlyList<ChineseChar>,
        IList, IEquatable<ChineseWord>, IEquatable<string>
    {
        private List<ChineseChar> _internalChars;
        private StringBuilder _str;

        private int? _strokeNumbers;
        private string _simplified;
        private string _traditional;


        #region 构造函数定义

        /// <summary>
        /// 初始化一个空内容的类型 <see cref="ChineseWord"/> 的新实例。
        /// </summary>
        private ChineseWord()
        {
            this._internalChars = new List<ChineseChar>();
            this._str = new StringBuilder();
        }


        /// <summary>
        /// 以一组中文字符初始化类型 <see cref="ChineseWord"/> 的新实例。
        /// </summary>
        /// <param name="chineseChars"></param>
        /// <exception cref="ArgumentNullException">参数 <paramref name="chineseChars"/> 为 null 或者不包含任何元素。</exception>
        /// <exception cref="ArgumentException">参数 <paramref name="chineseChars"/> 中包含 null 值元素。</exception>
        public ChineseWord(IEnumerable<ChineseChar> chineseChars)
            : this()
        {
            this.AddRange(chineseChars);
        }

        /// <summary>
        /// 以一组中文字符初始化类型 <see cref="ChineseWord"/> 的新实例。
        /// </summary>
        /// <param name="chineseChars"></param>
        /// <exception cref="ArgumentNullException">参数 <paramref name="chineseChars"/> 为 null 或者不包含任何元素。</exception>
        /// <exception cref="ArgumentException">参数 <paramref name="chineseChars"/> 中包含非中文字符。</exception>
        public ChineseWord(IEnumerable<char> chineseChars)
            : this()
        {
            this.AddRange(chineseChars);
        }

        /// <summary>
        /// 以一组中文字符初始化类型 <see cref="ChineseWord"/> 的新实例。
        /// </summary>
        /// <param name="chineseChars"></param>
        /// <exception cref="ArgumentNullException">参数 <paramref name="chineseChars"/> 为 null、空字符串或纯空格字符。</exception>
        /// <exception cref="ArgumentException">参数 <paramref name="chineseChars"/> 中包含非中文字符。</exception>
        public ChineseWord(string chineseChars)
            : this()
        {
            this.AddRange(chineseChars);
        }

        #endregion



        /// <summary>
        /// 获取当前对象的字符串表现形式。
        /// <para>该属性将返回对象中所包含的所有中文字符所组成的一个字符串。</para>
        /// </summary>
        public string Word
        {
            get { return this._str.ToString(); }
        }

        /// <summary>
        /// 获取这个中文词语中所有汉字的笔画数之和。
        /// </summary>
        public int StrokeNumbers
        {
            get
            {
                if (!this._strokeNumbers.HasValue)
                {
                    this._strokeNumbers = this._internalChars.Sum(ch => ch.StrokeNumber);
                }
                return this._strokeNumbers.Value;
            }
        }



        #region 公共属性定义 - 拼音相关属性
        #endregion


        #region 公共属性定义 - 简繁体转换相关属性

        /// <summary>
        /// 获取该中文词语的简体中文形式等效字符串。
        /// </summary>
        public string Simplified
        {
            get
            {
                if (this._simplified == null)
                {
                    this._simplified = this.GetSimplified();
                }
                return this._simplified;
            }
        }

        /// <summary>
        /// 获取该中文词语的繁体中文形式等效字符串。
        /// </summary>
        public string Traditional
        {
            get
            {
                if (this._traditional == null)
                {
                    this._traditional = this.GetTraditional();
                }
                return this._traditional;
            }
        }

        #endregion



        #region 公共方法定义 - 泛型序列类型转换

        /// <summary>
        /// 返回当前汉语词语中每个汉字字符所构成的一个序列输入。
        /// </summary>
        /// <returns></returns>
        public IEnumerable<char> AsCharEnumerable()
        {
            return this._internalChars.Select(ch => ch.ChineseCharacter);
        }

        /// <summary>
        /// 返回当前汉语词语文字集合的只读 <see cref="IList{ChineseWord}"/> 包装。
        /// </summary>
        /// <returns>作为当前 <see cref="ChineseWord"/> 周围的只读包装的 <see cref="ReadOnlyCollection{ChineseChar}"/>。</returns>
        public ReadOnlyCollection<ChineseChar> AsReadOnly()
        {
            return new ReadOnlyCollection<ChineseChar>(this);
        }

        /// <summary>
        /// 返回当前汉语词语文字集合的只读列表包装。
        /// </summary>
        /// <returns></returns>
        public ReadOnlyCollection<char> AsReadOnlyChars()
        {
            return new ReadOnlyCollection<char>(this.AsCharEnumerable().ToList());
        }

        /// <summary>
        /// 将 <see cref="ChineseWord"/> 中的所有汉字复制到一个新数组中。
        /// </summary>
        /// <returns></returns>
        public ChineseChar[] ToArray()
        {
            return this._internalChars.ToArray();
        }

        /// <summary>
        /// 将 <see cref="ChineseWord"/> 中的所有汉字复制到一个新字符数组中。
        /// </summary>
        /// <returns></returns>
        public char[] ToCharArray()
        {
            return this.Word.ToArray();
        }

        #endregion



        #region 内部方法定义 - 中文字符校验和转换

        /// <summary>
        /// 判断序列中是否存在 null 元素。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        internal static bool ExistsNullElement<T>(IEnumerable<T> items) where T : class
        {
            return items.Any(item => item == default(T));
        }

        /// <summary>
        /// 将一组字符转换成 <see cref="ChineseChar"/> 对象序列。
        /// </summary>
        /// <param name="chars"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">如果字符集合 <paramref name="chars"/> 包含非中文字符，则抛出该异常。</exception>
        internal static IEnumerable<ChineseChar> InternalConvert(IEnumerable<char> chars)
        {
            foreach (char c in chars)
            {
                ChineseChar ch;
                if (ChineseChar.TryParse(c, out ch))
                    yield return ch;
                else
                    throw new ArgumentException(string.Format("传入的字符集合中不能包含非汉字字符：{0}。", c));
            }
        }

        /// <summary>
        /// 将一个中文汉字字符转换成 <see cref="ChineseChar"/> 对象。
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">如果传入的字符不是中文汉字字符，则抛出该异常。</exception>
        internal static ChineseChar InternalConvert(char ch)
        {
            return ChineseChar.Parse(ch);
        }

        #endregion


        #region 内部方法定义 - 中文字符序列的内部操作

        /// <summary>
        /// 将 <see cref="ChineseChar"/> 元素添加或插入到当前词语中。
        /// 参数 <paramref name="index"/> 值为 -1 时表示执行添加到末尾的操作，为其他值时表示执行插入操作。
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        /// <exception cref="ArgumentNullException">如果参数 <paramref name="item"/> 为 null，则抛出该异常。</exception>
        /// <exception cref="ArgumentOutOfRangeException">如果参数 <paramref name="index"/> 小于 -1 或者大于当前对象的 <see cref="Count"/> 值，则抛出该异常。</exception>
        internal void InternalAdd(int index, ChineseChar item)
        {
            if (object.ReferenceEquals(item, null))
                throw new ArgumentNullException("ch");

            if (index < -1)
                throw new ArgumentOutOfRangeException("index");

            if (index == -1)
            {
                this._internalChars.Add(item);
                this._str.Append(item.ChineseCharacter);
            }
            else
            {
                this._internalChars.Insert(index, item);
                this._str.Insert(index, item.ChineseCharacter);
            }

            this.ResetInternalFields();
        }

        /// <summary>
        /// 将一组 <see cref="ChineseChar"/> 元素添加或插入到当前词语中。
        /// 参数 <paramref name="index"/> 值为 -1 时表示执行添加到末尾的操作，为其他值时表示执行插入操作。
        /// </summary>
        /// <param name="index"></param>
        /// <param name="items"></param>
        /// <exception cref="ArgumentNullException">如果参数 <paramref name="items"/> 为 null，则抛出该异常。</exception>
        /// <exception cref="ArgumentOutOfRangeException">如果参数 <paramref name="index"/> 小于 -1 或者大于当前对象的 <see cref="Count"/> 值，则抛出该异常。</exception>
        internal void InternalAddRange(int index, IEnumerable<ChineseChar> items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            if (index < -1)
                throw new ArgumentOutOfRangeException("index");

            char[] chars = items.Select(ch => ch.ChineseCharacter).ToArray();
            if (index == -1)
            {
                this._internalChars.AddRange(items);
                this._str.Append(chars);
            }
            else
            {
                this._internalChars.InsertRange(index, items);
                this._str.Insert(index, chars);
            }

            this.ResetInternalFields();
        }

        /// <summary>
        /// 移除当前中文词语中指定的汉字。
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        internal bool InternalRemove(ChineseChar item)
        {
            int index = this._internalChars.IndexOf(item);
            if (index >= 0)
            {
                this.InternalRemoveAt(index);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 移除当前中文词语中指定的汉字。
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        internal bool InternalRemove(char item)
        {
            int index = this._str.ToString().IndexOf(item);
            if (index >= 0)
            {
                this.InternalRemoveAt(index);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 移除与指定的谓词所定义的条件相匹配的所有中文字符。
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        internal int InternalRemoveAll(Predicate<ChineseChar> match)
        {
            int i = this._internalChars.RemoveAll(match);
            if (i > 0)
            {
                this.ResetInternalStringAndFields();
            }
            return i;
        }

        /// <summary>
        /// 从当前词语对象中移除一定范围的汉字字符。
        /// </summary>
        /// <param name="index">要移除的字符的范围索引号，从零开始计数。</param>
        /// <param name="count">要移除的元素数。</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> 小于 0 或 <paramref name="count"/> 小于 0。</exception>
        /// <exception cref="ArgumentException"><paramref name="index"/> 和 <paramref name="count"/> 不表示 <see cref="ChineseWord"/> 中字符的有效范围。</exception>
        internal void InternalRemoveRange(int index, int count)
        {
            this._internalChars.RemoveRange(index, count);
            if (count > 0)
            {
                this.ResetInternalStringAndFields();
            }
        }

        /// <summary>
        /// 移除当前中文词语中指定位置的汉字。
        /// </summary>
        /// <param name="index"></param>
        /// <exception cref="ArgumentOutOfRangeException">如果参数 <paramref name="index"/> 小于 0 或者大于当前对象的 <see cref="Count"/> 值，则抛出该异常。</exception>
        internal void InternalRemoveAt(int index)
        {
            this._internalChars.RemoveAt(index);
            this._str.Remove(index, 1);

            this.ResetInternalFields();
        }

        /// <summary>
        /// 将当前中文词语对象 <see cref="ChineseWord"/> 指定索引处位置设置为指定的中文字符元素。
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        /// <exception cref="ArgumentNullException">如果参数 <paramref name="item"/> 为 null，则抛出该异常。</exception>
        /// <exception cref="ArgumentOutOfRangeException">如果参数 <paramref name="index"/> 小于 0 或者大于当前对象的 <see cref="Count"/> 值，则抛出该异常。</exception>
        internal void InternalSet(int index, ChineseChar item)
        {
            if (object.ReferenceEquals(item, null))
                throw new ArgumentNullException("item");

            this._internalChars[index] = item;
            this._str[index] = item.ChineseCharacter;

            this.ResetInternalFields();
        }

        /// <summary>
        /// 将当前中文词语对象 <see cref="ChineseWord"/> 指定索引处位置设置为指定的中文字符元素。
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        /// <exception cref="ArgumentException">如果传入的参数 <paramref name="item"/> 不是一个中文字符，则抛出该异常。</exception>
        /// <exception cref="ArgumentOutOfRangeException">如果参数 <paramref name="index"/> 小于 0 或者大于当前对象的 <see cref="Count"/> 值，则抛出该异常。</exception>
        internal void InternalSet(int index, char item)
        {
            ChineseChar ch = InternalConvert(item);
            this.InternalSet(index, ch);
        }

        /// <summary>
        /// 将指定范围中的汉字字符的顺序反转。
        /// </summary>
        /// <param name="index">要反转的范围的从零开始的起始索引。</param>
        /// <param name="count">要反转的范围内的汉字字符数。</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> 小于 0。或 <paramref name="count"/> 小于 0。</exception>
        /// <exception cref="ArgumentException"><paramref name="index"/> 和 <paramref name="count"/> 不表示 <see cref="ChineseWord"/> 中汉字字符的有效范围。</exception>
        private void InternalReverse(int index, int count)
        {
            this._internalChars.Reverse(index, count);
            if (count > 1)
            {
                this.ResetInternalStringAndFields();
            }
        }


        internal void InternalClear()
        {
            this._internalChars.Clear();
            this._str.Clear();

            this.ResetInternalFields();
        }


        private void ResetInternalStringAndFields()
        {
            char[] chars = this._internalChars.Select(ch => ch.ChineseCharacter).ToArray();
            this._str.Clear().Append(chars);

            this.ResetInternalFields();
        }

        private void ResetInternalFields()
        {
            this._strokeNumbers = null;
            this._simplified = null;
            this._traditional = null;
        }

        #endregion


        #region 内部方法定义 - 其他操作

        private string GetSimplified()
        {
            return ChineseConverter.Convert(this.Word, ChineseConversionDirection.TraditionalToSimplified);
        }

        private string GetTraditional()
        {
            return ChineseConverter.Convert(this.Word, ChineseConversionDirection.SimplifiedToTraditional);
        }

        #endregion



        #region 接口 IList<ChineseChar>, IReadOnlyList<ChineseChar>, IList 的实现 - 属性定义

        /// <summary>
        /// 获取或设置位于指定索引处的汉字字符。
        /// </summary>
        /// <param name="index">要获得或设置的汉字从零开始的索引。</param>
        /// <returns>获取或设置位于指定索引处的汉字字符。</returns>
        /// <exception cref="ArgumentOutOfRangeException">如果参数 <paramref name="index"/> 小于 0 或者大于当前对象的 <see cref="Count"/> 值，则抛出该异常。</exception>
        /// <exception cref="ArgumentNullException">在使用 set 访问器时，如果设置的 value 值为 null，则抛出该异常。</exception>
        public ChineseChar this[int index]
        {
            get { return this._internalChars[index]; }
            set { this.InternalSet(index, value); }
        }

        /// <summary>
        /// 获取或设置位于指定索引处的汉字字符。
        /// </summary>
        /// <param name="index">要获得或设置的汉字从零开始的索引。</param>
        /// <returns>获取或设置位于指定索引处的汉字字符。</returns>
        /// <exception cref="ArgumentOutOfRangeException">如果参数 <paramref name="index"/> 小于 0 或者大于当前对象的 <see cref="Count"/> 值，则抛出该异常。</exception>
        /// <exception cref="ArgumentNullException">在使用 set 访问器时，如果设置的 value 值为 null，则抛出该异常。</exception>
        ChineseChar IList<ChineseChar>.this[int index]
        {
            get { return this[index]; }
            set { this.InternalSet(index, value); }
        }

        /// <summary>
        /// 获取或设置位于指定索引处的汉字字符。
        /// <para>在使用 set 访问器时，参数值 value 的类型必须是 <see cref="ChineseWord"/> 或 <see cref="char"/>。</para>
        /// </summary>
        /// <param name="index">要获得或设置的汉字从零开始的索引。</param>
        /// <returns>获取或设置位于指定索引处的汉字字符。</returns>
        /// <exception cref="ArgumentOutOfRangeException">如果参数 <paramref name="index"/> 小于 0 或者大于当前对象的 <see cref="Count"/> 值，则抛出该异常。</exception>
        /// <exception cref="ArgumentNullException">在使用 set 访问器时，如果设置的 value 值为 null，则抛出该异常。</exception>
        /// <exception cref="ArgumentNullException">在使用 set 访问器时，如果设置的 value 值不是一个表示中文字符的 <see cref="ChineseWord"/> 或 <see cref="char"/>，则抛出该异常。</exception>
        object IList.this[int index]
        {
            get { return this[index]; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                if (value is ChineseChar)
                    this.InternalSet(index, (ChineseChar)value);
                else if (value is char)
                    this.InternalSet(index, (char)value);
                else
                    throw new ArgumentException("传入的参数值类型不正确。", "value");
            }
        }


        /// <summary>
        /// 获取当前中文词语中包含的文字的数量。
        /// </summary>
        public int Count
        {
            get { return this._internalChars.Count; }
        }


        /// <summary>
        /// 获取一个值，该值指示 <see cref="ChineseWord"/> 是否为只读。
        /// <para>该属性始终返回 false。</para>
        /// </summary>
        bool ICollection<ChineseChar>.IsReadOnly
        {
            get { return ((ICollection<ChineseChar>)this._internalChars).IsReadOnly; }
        }

        /// <summary>
        /// 获取一个值，该值指示 <see cref="ChineseWord"/> 是否为只读。
        /// <para>该属性始终返回 false。</para>
        /// </summary>
        bool IList.IsReadOnly
        {
            get { return ((IList)this._internalChars).IsSynchronized; }
        }

        /// <summary>
        /// 获取一个值，该值指示 <see cref="ChineseWord"/> 是否具有固定大小。
        /// <para>该属性始终返回 false。</para>
        /// </summary>
        bool IList.IsFixedSize
        {
            get { return ((IList)this._internalChars).IsFixedSize; }
        }

        /// <summary>
        /// 获取一个值，该值指示是否同步对 <see cref="ICollection"/> 的访问（线程安全）。
        /// <para>该属性始终返回 false。</para>
        /// </summary>
        bool ICollection.IsSynchronized
        {
            get { return ((ICollection)this._internalChars).IsSynchronized; }
        }

        /// <summary>
        /// 获取可用于同步对 <see cref="ICollection"/> 的访问的对象。
        /// </summary>
        object ICollection.SyncRoot
        {
            get { return ((ICollection)this._internalChars).SyncRoot; }
        }

        #endregion


        #region 公共方法定义 - 集合操作方法（含接口 IList<ChineseChar>, IReadOnlyList<ChineseChar>, IList 的实现）

        /// <summary>
        /// 在当前词语 <see cref="ChineseWord"/> 中搜索指定的汉字，并返回整个 <see cref="ChineseWord"/> 中第一个匹配项的从零开始的索引。
        /// </summary>
        /// <param name="item">要在词语 <see cref="ChineseWord"/> 中定位的汉字字符，该参数不能为 null。</param>
        /// <returns>如果在整个 <see cref="ChineseWord"/> 中找到 <paramref name="item"/> 的第一个匹配的汉字字符项，则为该汉字字符项的从零开始的索引；否则为 -1。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> 为 null。</exception>
        public int IndexOf(ChineseChar item)
        {
            if (object.ReferenceEquals(item, null))
                throw new ArgumentNullException("item");

            return this._internalChars.IndexOf(item);
        }

        /// <summary>
        /// 在当前词语 <see cref="ChineseWord"/> 中搜索指定的汉字，并返回 <see cref="ChineseWord"/> 中从指定的索引处到最后一个字符的范围内第一个匹配项的从零开始的索引。
        /// </summary>
        /// <param name="item">要在词语 <see cref="ChineseWord"/> 中定位的汉字字符，该参数不能为 null。</param>
        /// <param name="index">从零开始的搜索的起始索引。0（零）为有效值。</param>
        /// <returns>如果在 <see cref="ChineseWord"/> 中从 <paramref name="index"/> 到最后一个字符范围内找到 <paramref name="item"/> 的第一个匹配的汉字字符项，则返回该汉字字符项的从零开始的索引；否则为 -1。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> 为 null。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> 不在 <see cref="ChineseWord"/> 的有效索引范围内。</exception>
        public int IndexOf(ChineseChar item, int index)
        {
            if (object.ReferenceEquals(item, null))
                throw new ArgumentNullException("item");

            return this._internalChars.IndexOf(item, index);
        }

        /// <summary>
        /// 在当前词语 <see cref="ChineseWord"/> 中搜索指定的汉字，并返回 <see cref="ChineseWord"/> 中从指定的索引开始并包含指定的字符数的范围内第一个匹配项的从零开始的索引。
        /// </summary>
        /// <param name="item">要在词语 <see cref="ChineseWord"/> 中定位的汉字字符，该参数不能为 null。</param>
        /// <param name="index">从零开始的搜索的起始索引。0（零）为有效值。</param>
        /// <param name="count">要搜索的部分中的字符数。</param>
        /// <returns>如果在 <see cref="ChineseWord"/> 中从 <paramref name="index"/> 开始并包含 <paramref name="count"/> 个字符数的范围内找到 <paramref name="item"/> 的第一个匹配项的汉字字符项，则为该项的从零开始的索引；否则为 -1。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> 为 null。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> 不在 <see cref="ChineseWord"/> 的有效索引范围内。或 <paramref name="count"/> 小于 0。或 <paramref name="index"/> 和 <paramref name="count"/> 未指定 <see cref="ChineseWord"/> 中的有效部分。</exception>
        public int IndexOf(ChineseChar item, int index, int count)
        {
            if (object.ReferenceEquals(item, null))
                throw new ArgumentNullException("item");

            return this._internalChars.IndexOf(item, index, count);
        }


        /// <summary>
        /// 在当前词语 <see cref="ChineseWord"/> 中搜索指定的汉字，并返回整个 <see cref="ChineseWord"/> 中第一个匹配项的从零开始的索引。
        /// </summary>
        /// <param name="item">要在词语 <see cref="ChineseWord"/> 中定位的汉字字符项目，该参数不能为 null。</param>
        /// <returns>如果在整个 <see cref="ChineseWord"/> 中找到 <paramref name="item"/> 的第一个匹配的汉字字符项，则为该汉字字符项的从零开始的索引；否则为 -1。</returns>
        public int IndexOf(char item)
        {
            return this.Word.IndexOf(item);
        }

        /// <summary>
        /// 在当前词语 <see cref="ChineseWord"/> 中搜索指定的汉字，并返回 <see cref="ChineseWord"/> 中从指定的索引处到最后一个字符的范围内第一个匹配项的从零开始的索引。
        /// </summary>
        /// <param name="item">要在词语 <see cref="ChineseWord"/> 中定位的汉字字符项目，该参数不能为 null。</param>
        /// <param name="index">从零开始的搜索的起始索引。0（零）为有效值。</param>
        /// <returns>如果在 <see cref="ChineseWord"/> 中从 <paramref name="index"/> 到最后一个字符范围内找到 <paramref name="item"/> 的第一个匹配的汉字字符项，则返回该汉字字符项的从零开始的索引；否则为 -1。</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> 不在 <see cref="ChineseWord"/> 的有效索引范围内。</exception>
        public int IndexOf(char item, int index)
        {
            return this.Word.IndexOf(item, index);
        }

        /// <summary>
        /// 在当前词语 <see cref="ChineseWord"/> 中搜索指定的汉字，并返回 <see cref="ChineseWord"/> 中从指定的索引开始并包含指定的字符数的范围内第一个匹配项的从零开始的索引。
        /// </summary>
        /// <param name="item">要在词语 <see cref="ChineseWord"/> 中定位的汉字字符项目，该参数不能为 null。</param>
        /// <param name="index">从零开始的搜索的起始索引。0（零）为有效值。</param>
        /// <param name="count">要搜索的部分中的字符数。</param>
        /// <returns>如果在 <see cref="ChineseWord"/> 中从 <paramref name="index"/> 开始并包含 <paramref name="count"/> 个字符数的范围内找到 <paramref name="item"/> 的第一个匹配项的汉字字符项，则为该项的从零开始的索引；否则为 -1。</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> 不在 <see cref="ChineseWord"/> 的有效索引范围内。或 <paramref name="count"/> 小于 0。或 <paramref name="index"/> 和 <paramref name="count"/> 未指定 <see cref="ChineseWord"/> 中的有效部分。</exception>
        public int IndexOf(char item, int index, int count)
        {
            return this.Word.IndexOf(item, index, count);
        }

        /// <summary>
        /// 在当前词语 <see cref="ChineseWord"/> 中搜索指定的汉字，并返回整个 <see cref="ChineseWord"/> 中第一个匹配项的从零开始的索引。
        /// <para>参数 <paramref name="value"/> 的类型必须是 <see cref="ChineseWord"/> 或 <see cref="char"/>。</para>
        /// </summary>
        /// <param name="value">要在词语 <see cref="ChineseWord"/> 中定位的汉字字符项目，该参数不能为 null。</param>
        /// <returns>如果在整个 <see cref="ChineseWord"/> 中找到 <paramref name="value"/> 的第一个匹配的汉字字符项，则为该汉字字符项的从零开始的索引；否则为 -1。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> 为 null。</exception>
        /// <exception cref="ArgumentException">参数 <paramref name="value"/> 的类型不是 <see cref="ChineseWord"/> 或 <see cref="char"/>。</exception>
        int IList.IndexOf(object value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if (value is ChineseChar)
                return this.IndexOf((ChineseChar)value);
            else if (value is char)
                return this.IndexOf((char)value);
            else
                throw new ArgumentException("传入的参数值类型不正确。", "value");
        }


        /// <summary>
        /// 搜索指定的中文字符，并返回整个 <see cref="ChineseWord"/> 中最后一个匹配项的从零开始的索引。
        /// </summary>
        /// <param name="item">要在词语 <see cref="ChineseWord"/> 中定位的汉字字符，该参数不能为 null。</param>
        /// <returns>如果在整个 <see cref="ChineseChar"/> 中找到 <paramref name="item"/> 的最后一个匹配项，则为该项的从零开始的索引；否则为 -1。</returns>
        /// <exception cref="ArgumentNullException">如果 <paramref name="item"/> 为 null。</exception>
        public int LastIndexOf(ChineseChar item)
        {
            if (object.ReferenceEquals(item, null))
                throw new ArgumentNullException("item");

            return this._internalChars.LastIndexOf(item);
        }
        
        /// <summary>
        /// 搜索指定的中文字符，并返回 <see cref="ChineseWord"/> 中从第一个元素到指定索引处的字符范围内最后一个匹配项的从零开始的索引。
        /// </summary>
        /// <param name="item">要在词语 <see cref="ChineseWord"/> 中定位的汉字字符，该参数不能为 null。</param>
        /// <param name="index">向后搜索的从零开始的起始索引。</param>
        /// <returns>如果在整个 <see cref="ChineseChar"/> 中找到 <paramref name="item"/> 的最后一个匹配项，则为该项的从零开始的索引；否则为 -1。</returns>
        /// <exception cref="ArgumentNullException">如果 <paramref name="item"/> 为 null。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> 不在 <see cref="ChineseWord"/> 的有效索引范围内。</exception>
        public int LastIndexOf(ChineseChar item, int index)
        {
            if (object.ReferenceEquals(item, null))
                throw new ArgumentNullException("item");

            return this._internalChars.LastIndexOf(item, index);
        }

        /// <summary>
        /// 搜索指定的中文字符，并返回 <see cref="ChineseWord"/> 中包含指定的字符数并在执行索引处结束的字符范围内最后一个匹配项的从零开始的索引。
        /// </summary>
        /// <param name="item">要在词语 <see cref="ChineseWord"/> 中定位的汉字字符，该参数不能为 null。</param>
        /// <param name="index">向后搜索的从零开始的起始索引。</param>
        /// <param name="count">要搜索的部分中的元素数。</param>
        /// <returns>如果在 <see cref="ChineseWord"/> 中包含 <paramref name="count"/> 个元素、在 <paramref name="index"/> 处结尾的字符范围内找到 <paramref name="item"/> 的最后一个匹配项，则返回该项的从零开始的索引；否则为 -1。</returns>
        /// <exception cref="ArgumentNullException">如果 <paramref name="item"/> 为 null。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> 不在 <see cref="ChineseWord"/> 的有效索引范围内。或 <paramref name="count"/> 小于 0。或 <paramref name="index"/> 和 <paramref name="count"/> 未指定 <see cref="ChineseWord"/> 中的有效部分。</exception>
        public int LastIndexOf(ChineseChar item, int index, int count)
        {
            if (object.ReferenceEquals(item, null))
                throw new ArgumentNullException("item");

            return this._internalChars.LastIndexOf(item, index, count);
        }


        /// <summary>
        /// 搜索指定的中文字符，并返回整个 <see cref="ChineseWord"/> 中最后一个匹配项的从零开始的索引。
        /// </summary>
        /// <param name="item">要在词语 <see cref="ChineseWord"/> 中定位的汉字字符，该参数不能为 null。</param>
        /// <returns>如果在整个 <see cref="ChineseChar"/> 中找到 <paramref name="item"/> 的最后一个匹配项，则为该项的从零开始的索引；否则为 -1。</returns>
        public int LastIndexOf(char item)
        {
            return this.Word.LastIndexOf(item);
        }

        /// <summary>
        /// 搜索指定的中文字符，并返回 <see cref="ChineseWord"/> 中从第一个元素到指定索引处的字符范围内最后一个匹配项的从零开始的索引。
        /// </summary>
        /// <param name="item">要在词语 <see cref="ChineseWord"/> 中定位的汉字字符，该参数不能为 null。</param>
        /// <param name="index">向后搜索的从零开始的起始索引。</param>
        /// <returns>如果在整个 <see cref="ChineseChar"/> 中找到 <paramref name="item"/> 的最后一个匹配项，则为该项的从零开始的索引；否则为 -1。</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> 不在 <see cref="ChineseWord"/> 的有效索引范围内。</exception>
        public int LastIndexOf(char item, int index)
        {
            return this.Word.LastIndexOf(item, index);
        }

        /// <summary>
        /// 搜索指定的中文字符，并返回 <see cref="ChineseWord"/> 中包含指定的字符数并在执行索引处结束的字符范围内最后一个匹配项的从零开始的索引。
        /// </summary>
        /// <param name="item">要在词语 <see cref="ChineseWord"/> 中定位的汉字字符，该参数不能为 null。</param>
        /// <param name="index">向后搜索的从零开始的起始索引。</param>
        /// <param name="count">要搜索的部分中的元素数。</param>
        /// <returns>如果在 <see cref="ChineseWord"/> 中包含 <paramref name="count"/> 个元素、在 <paramref name="index"/> 处结尾的字符范围内找到 <paramref name="item"/> 的最后一个匹配项，则返回该项的从零开始的索引；否则为 -1。</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> 不在 <see cref="ChineseWord"/> 的有效索引范围内。或 <paramref name="count"/> 小于 0。或 <paramref name="index"/> 和 <paramref name="count"/> 未指定 <see cref="ChineseWord"/> 中的有效部分。</exception>
        public int LastIndexOf(char item, int index, int count)
        {
            return this.Word.LastIndexOf(item, index, count);
        }


        /// <summary>
        /// 将中文字符添加到 <see cref="ChineseWord"/> 词语的结尾处。
        /// </summary>
        /// <param name="item">要添加到 <see cref="ChineseWord"/> 词语的结尾处的中文字符，不能为 null。</param>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> 为 null。</exception>
        public void Add(ChineseChar item)
        {
            this.InternalAdd(-1, item);
        }

        /// <summary>
        /// 将中文字符添加到 <see cref="ChineseWord"/> 词语的结尾处。
        /// </summary>
        /// <param name="item">要添加到 <see cref="ChineseWord"/> 词语的结尾处的中文字符。</param>
        /// <exception cref="ArgumentException">参数 <paramref name="item"/> 不是一个中文字符。</exception>
        public void Add(char item)
        {
            ChineseChar ch = InternalConvert(item);
            this.Add(ch);
        }

        /// <summary>
        /// 将中文字符添加到 <see cref="ChineseWord"/> 词语的结尾处。
        /// <para>参数 <paramref name="value"/> 的类型必须是 <see cref="ChineseWord"/> 或 <see cref="char"/>。</para>
        /// </summary>
        /// <param name="value">要添加到 <see cref="ChineseWord"/> 词语的结尾处的中文字符，不能为 null。</param>
        /// <returns>新字符的插入位置。 </returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> 为 null。</exception>
        /// <exception cref="ArgumentException">参数 <paramref name="value"/> 的类型不是 <see cref="ChineseWord"/> 或 <see cref="char"/>，或者其不是一个中文字符。</exception>
        int IList.Add(object value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if (value is ChineseChar)
                this.Add((ChineseChar)value);
            else if (value is char)
                this.Add((char)value);
            else
                throw new ArgumentException("传入的参数值类型不正确。", "value");

            return this.Count - 1;
        }


        /// <summary>
        /// 将一组中文字符添加至当前词语的末尾。
        /// </summary>
        /// <param name="chineseChars"></param>
        /// <exception cref="ArgumentNullException">参数 <paramref name="chineseChars"/> 为 null 或者不包含任何元素。</exception>
        /// <exception cref="ArgumentException">参数 <paramref name="chineseChars"/> 中包含 null 值元素。</exception>
        public void AddRange(IEnumerable<ChineseChar> chineseChars)
        {
            if (!chineseChars.Any())
                throw new ArgumentNullException("参数 chineseChars 不能为 null 且需至少包含一个元素。");

            if (ExistsNullElement(chineseChars))
                throw new ArgumentException("传入的序列中包含 null 元素项。");

            this.InternalAddRange(-1, chineseChars);
        }

        /// <summary>
        /// 将一组中文字符添加至当前词语的末尾。
        /// </summary>
        /// <param name="chineseChars"></param>
        /// <exception cref="ArgumentNullException">参数 <paramref name="chineseChars"/> 为 null 或者不包含任何元素。</exception>
        /// <exception cref="ArgumentException">参数 <paramref name="chineseChars"/> 中包含非中文字符。</exception>
        public void AddRange(IEnumerable<char> chineseChars)
        {
            if (!chineseChars.Any())
                throw new ArgumentNullException("参数 chineseChars 不能为 null 且需至少包含一个字符。");

            IEnumerable<ChineseChar> items = InternalConvert(chineseChars);
            this.InternalAddRange(-1, items);
        }

        /// <summary>
        /// 将一组中文字符添加至当前词语的末尾。
        /// </summary>
        /// <param name="chineseChars"></param>
        /// <exception cref="ArgumentNullException">参数 <paramref name="chineseChars"/> 为 null、空字符串或纯空格字符。</exception>
        /// <exception cref="ArgumentException">参数 <paramref name="chineseChars"/> 中包含非中文字符。</exception>
        public void AddRange(string chineseChars)
        {
            if (string.IsNullOrWhiteSpace(chineseChars))
                throw new ArgumentNullException("参数 chineseChars 不能为 null、空字符串或纯空格字符。");

            IEnumerable<ChineseChar> items = InternalConvert(chineseChars);
            this.InternalAddRange(-1, items);
        }


        /// <summary>
        /// 将中文字符插入到 <see cref="ChineseWord"/> 词语的指定索引处。
        /// </summary>
        /// <param name="index">从零开始的索引，应在该位置插入 <paramref name="item"/>。</param>
        /// <param name="item">要插入的中文字符，不能为 null。</param>
        /// <exception cref="ArgumentNullException">参数 <paramref name="item"/> 为 null。</exception>
        /// <exception cref="ArgumentOutOfRangeException">如果参数 <paramref name="index"/> 小于 0 或者大于等于当前对象的 <see cref="Count"/> 值，则抛出该异常。</exception>
        public void Insert(int index, ChineseChar item)
        {
            if (index <= -1)
                throw new ArgumentOutOfRangeException("index");

            this.InternalAdd(index, item);
        }

        /// <summary>
        /// 将中文字符插入到 <see cref="ChineseWord"/> 词语的指定索引处。
        /// </summary>
        /// <param name="index">从零开始的索引，应在该位置插入 <paramref name="item"/>。</param>
        /// <param name="item">要插入的中文字符，不能为 null。</param>
        /// <exception cref="ArgumentOutOfRangeException">如果参数 <paramref name="index"/> 小于 0 或者大于等于当前对象的 <see cref="Count"/> 值，则抛出该异常。</exception>
        public void Insert(int index, char item)
        {
            ChineseChar ch = InternalConvert(item);
            this.Insert(index, ch);
        }

        /// <summary>
        /// 将中文字符插入到 <see cref="ChineseWord"/> 词语的指定索引处。
        /// <para>参数 <paramref name="value"/> 的类型必须是 <see cref="ChineseWord"/> 或 <see cref="char"/>。</para>
        /// </summary>
        /// <param name="index">从零开始的索引，应在该位置插入 <paramref name="value"/>。</param>
        /// <param name="value">要插入的中文字符，不能为 null。</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> 为 null。</exception>
        /// <exception cref="ArgumentException">参数 <paramref name="value"/> 的类型不是 <see cref="ChineseWord"/> 或 <see cref="char"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException">如果参数 <paramref name="index"/> 小于 0 或者大于等于当前对象的 <see cref="Count"/> 值，则抛出该异常。</exception>
        void IList.Insert(int index, object value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if (value is ChineseChar)
                this.Insert(index, (ChineseChar)value);
            else if (value is char)
                this.Insert(index, (char)value);
            else
                throw new ArgumentException("传入的参数值类型不正确。", "value");
        }


        /// <summary>
        /// 将一组中文字符插入 <see cref="ChineseWord"/> 的指定索引处。
        /// </summary>
        /// <param name="index">应在此处插入新中文字符的从零开始的索引。</param>
        /// <param name="chineseChars">包含一组中文字符的集合。集合自身不能为 null，也不能包含为 null 的元素。</param>
        /// <exception cref="ArgumentNullException">如果参数 <paramref name="chineseChars"/> 为 null，则抛出该异常。</exception>
        /// <exception cref="ArgumentException">参数 <paramref name="chineseChars"/> 中包含 null 值元素。</exception>
        /// <exception cref="ArgumentOutOfRangeException">如果参数 <paramref name="index"/> 小于 0 或者大于等于当前对象的 <see cref="Count"/> 值，则抛出该异常。</exception>
        public void InsertRange(int index, IEnumerable<ChineseChar> chineseChars)
        {
            if (index <= -1)
                throw new ArgumentOutOfRangeException("index");

            if (ExistsNullElement(chineseChars))
                throw new ArgumentException("传入的序列中包含 null 元素项。");

            this.InternalAddRange(index, chineseChars);
        }

        /// <summary>
        /// 将一组中文字符插入 <see cref="ChineseWord"/> 的指定索引处。
        /// </summary>
        /// <param name="index">应在此处插入新中文字符的从零开始的索引。</param>
        /// <param name="chineseChars">包含一组中文字符的集合。集合自身不能为 null，也不能包含为 null 的元素。</param>
        /// <exception cref="ArgumentNullException">如果参数 <paramref name="chineseChars"/> 为 null，则抛出该异常。</exception>
        /// <exception cref="ArgumentException">参数 <paramref name="chineseChars"/> 中包含非中文字符。</exception>
        /// <exception cref="ArgumentOutOfRangeException">如果参数 <paramref name="index"/> 小于 0 或者大于等于当前对象的 <see cref="Count"/> 值，则抛出该异常。</exception>
        public void InsertRange(int index, IEnumerable<char> chineseChars)
        {
            if (!chineseChars.Any())
                throw new ArgumentNullException("参数 chineseChars 不能为 null 且需至少包含一个字符。");

            IEnumerable<ChineseChar> items = InternalConvert(chineseChars);
            this.InternalAddRange(index, items);
        }

        /// <summary>
        /// 将一组中文字符插入 <see cref="ChineseWord"/> 的指定索引处。
        /// </summary>
        /// <param name="index">应在此处插入新中文字符的从零开始的索引。</param>
        /// <param name="chineseChars">包含一组中文字符的集合。集合自身不能为 null，也不能包含为 null 的元素。</param>
        /// <exception cref="ArgumentNullException">如果参数 <paramref name="chineseChars"/> 为 null，则抛出该异常。</exception>
        /// <exception cref="ArgumentException">参数 <paramref name="chineseChars"/> 中包含非中文字符。</exception>
        /// <exception cref="ArgumentOutOfRangeException">如果参数 <paramref name="index"/> 小于 0 或者大于等于当前对象的 <see cref="Count"/> 值，则抛出该异常。</exception>
        public void InsertRange(int index, string chineseChars)
        {
            if (string.IsNullOrWhiteSpace(chineseChars))
                throw new ArgumentNullException("参数 chineseChars 不能为 null、空字符串或纯空格字符。");

            IEnumerable<ChineseChar> items = InternalConvert(chineseChars);
            this.InternalAddRange(index, items);
        }


        /// <summary>
        /// 从 <see cref="ChineseWord"/> 中移除特定汉字的第一个匹配项。
        /// </summary>
        /// <param name="item">要从 <see cref="ChineseWord"/> 中移除的汉字，不能为 null。</param>
        /// <returns>如果成功移除 <paramref name="item"/>，则返回 true，否则返回 false。</returns>
        /// <exception cref="ArgumentNullException">如果参数 <paramref name="item"/> 为 null，则抛出该异常。</exception>
        public bool Remove(ChineseChar item)
        {
            if (object.ReferenceEquals(item, null))
                throw new ArgumentNullException("item");

            return this.InternalRemove(item);
        }

        /// <summary>
        /// 从 <see cref="ChineseWord"/> 中移除特定汉字的第一个匹配项。
        /// </summary>
        /// <param name="item">要从 <see cref="ChineseWord"/> 中移除的汉字。</param>
        /// <returns>如果成功移除 <paramref name="item"/>，则返回 true，否则返回 false。</returns>
        public bool Remove(char item)
        {
            return this.InternalRemove(item);
        }

        /// <summary>
        /// 从 <see cref="ChineseWord"/> 中移除特定汉字的第一个匹配项。
        /// <para>参数 <paramref name="value"/> 的类型必须是 <see cref="ChineseWord"/> 或 <see cref="char"/>。</para>
        /// </summary>
        /// <param name="value">要从 <see cref="ChineseWord"/> 中移除的汉字，其类型必须是 <see cref="ChineseWord"/> 或 <see cref="char"/>，且不能为 null。</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> 为 null。</exception>
        /// <exception cref="ArgumentException">参数 <paramref name="value"/> 的类型不是 <see cref="ChineseWord"/> 或 <see cref="char"/>。</exception>
        void IList.Remove(object value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if (value is ChineseChar)
                this.InternalRemove((ChineseChar)value);
            else if (value is char)
                this.InternalRemove((char)value);
            else
                throw new ArgumentException("传入的参数值类型不正确。", "value");
        }


        /// <summary>
        /// 移除与指定的谓词所定义的条件相匹配的所有中文字符。
        /// </summary>
        /// <param name="match"><see cref="Predicate{ChineseChar}"/> 委托，用于定义要移除的字符应满足的条件。</param>
        /// <returns>从 <see cref="ChineseWord"/> 中移除的字符的数目。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="match"/> 为 null。</exception>
        public int RemoveAll(Predicate<ChineseChar> match)
        {
            if (match == null)
                throw new ArgumentNullException("match");

            return this.InternalRemoveAll(match);
        }

        /// <summary>
        /// 移除与指定的谓词所定义的条件相匹配的所有中文字符。
        /// </summary>
        /// <param name="match"><see cref="Predicate{T}"/> 委托，用于定义要移除的字符应满足的条件。</param>
        /// <returns>从 <see cref="ChineseWord"/> 中移除的字符的数目。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="match"/> 为 null。</exception>
        public int RemoveAll(Predicate<char> match)
        {
            if (match == null)
                throw new ArgumentNullException("match");

            Predicate<ChineseChar> predicate = ch => match(ch.ChineseCharacter);
            return this.InternalRemoveAll(predicate);
        }


        /// <summary>
        /// 移除 <see cref="ChineseWord"/> 指定索引处的汉字。
        /// </summary>
        /// <param name="index">移除的汉字的从零开始的索引。</param>
        /// <exception cref="ArgumentOutOfRangeException">如果参数 <paramref name="index"/> 小于 0 或者大于等于当前对象的 <see cref="Count"/> 值，则抛出该异常。</exception>
        public void RemoveAt(int index)
        {
            this.InternalRemoveAt(index);
        }

        /// <summary>
        /// 从当前词语对象中移除一定范围的汉字字符。
        /// </summary>
        /// <param name="index">要移除的字符的范围索引号，从零开始计数。</param>
        /// <param name="count">要移除的元素数。</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> 小于 0 或 <paramref name="count"/> 小于 0。</exception>
        /// <exception cref="ArgumentException"><paramref name="index"/> 和 <paramref name="count"/> 不表示 <see cref="ChineseWord"/> 中字符的有效范围。</exception>
        public void RemoveRange(int index, int count)
        {
            this.InternalRemoveRange(index, count);
        }


        /// <summary>
        /// 从 <see cref="ChineseWord"/> 移除所有的汉字，使得该 <see cref="ChineseWord"/> 等效于一个空字符串。
        /// </summary>
        public void Clear()
        {
            this.InternalClear();
        }


        /// <summary>
        /// 确定当前 <see cref="ChineseWord"/> 是否包含某个汉字。
        /// </summary>
        /// <param name="item">要在 <see cref="ChineseWord"/> 定位的汉字，不能为 null。</param>
        /// <returns>如果 <see cref="ChineseWord"/> 找到该 <paramref name="item"/>，则返回 true，否则返回 false。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> 为 null。</exception>
        public bool Contains(ChineseChar item)
        {
            if (object.ReferenceEquals(item, null))
                throw new ArgumentNullException("item");

            return this._internalChars.Contains(item);
        }

        /// <summary>
        /// 确定当前 <see cref="ChineseWord"/> 是否包含某个汉字。
        /// </summary>
        /// <param name="item">要在 <see cref="ChineseWord"/> 定位的汉字。</param>
        /// <returns>如果 <see cref="ChineseWord"/> 找到该 <paramref name="item"/>，则返回 true，否则返回 false。</returns>
        public bool Contains(char item)
        {
            return this.IndexOf(item) >= 0;
        }

        /// <summary>
        /// 确定当前 <see cref="ChineseWord"/> 是否包含某个汉字。
        /// </summary>
        /// <param name="value">要在 <see cref="ChineseWord"/> 定位的汉字，不能为 null。</param>
        /// <returns>如果 <see cref="ChineseWord"/> 找到该 <paramref name="value"/>，则返回 true，否则返回 false。</returns><exception cref="ArgumentNullException"><paramref name="value"/> 为 null。</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> 为 null。</exception>
        /// <exception cref="ArgumentException">参数 <paramref name="value"/> 的类型不是 <see cref="ChineseWord"/> 或 <see cref="char"/>。</exception>
        bool IList.Contains(object value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if (value is ChineseChar)
                return this.Contains((ChineseChar)value);
            else if (value is char)
                return this.Contains((char)value);
            else
                throw new ArgumentException("传入的参数值类型不正确。", "value");
        }


        /// <summary>
        /// 确定当前中文词语中是否包含与指定谓词所定义的条件相匹配的汉字。
        /// </summary>
        /// <param name="match"><see cref="Predicate{ChineseChar}"/> 委托，用于定义要搜索的汉字应满足的条件。</param>
        /// <returns>如果 <see cref="ChineseWord"/> 包含一个或多个与指定谓词所定义的条件相匹配的汉字，则为 true；否则为 false。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="match"/> 为 null。</exception>
        public bool Exists(Predicate<ChineseChar> match)
        {
            return this._internalChars.Exists(match);
        }

        /// <summary>
        /// 确定当前中文词语中是否包含与指定谓词所定义的条件相匹配的汉字。
        /// </summary>
        /// <param name="match"><see cref="Predicate{ChineseChar}"/> 委托，用于定义要搜索的汉字应满足的条件。</param>
        /// <returns>如果 <see cref="ChineseWord"/> 包含一个或多个与指定谓词所定义的条件相匹配的汉字，则为 true；否则为 false。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="match"/> 为 null。</exception>
        public bool Exists(Predicate<char> match)
        {
            if (match == null)
                throw new ArgumentNullException("match");

            return this._internalChars.Exists(ch => match(ch.ChineseCharacter));
        }


        /// <summary>
        /// 将整个 <see cref="ChineseWord"/> 词语中所有的汉字字符复制到兼容的一维数组中，从目标数组的开头开始复制。
        /// </summary>
        /// <param name="array">作为从 <see cref="ChineseWord"/> 复制的汉字的位置的一维 <see cref="Array"/>。<see cref="Array"/> 必须具有从零开始的索引。</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> 为 null。</exception>
        /// <exception cref="ArgumentException">源 <see cref="ChineseWord"/> 中的汉字数大于目标 <paramref name="array"/> 可包含的元素数。</exception>
        public void CopyTo(ChineseChar[] array)
        {
            this.CopyTo(array, 0);
        }

        /// <summary>
        /// 将整个 <see cref="ChineseWord"/> 词语中所有的汉字字符复制到兼容的一维数组中，从目标数组的指定索引位置开始复制。
        /// </summary>
        /// <param name="array">作为从 <see cref="ChineseWord"/> 复制的汉字的位置的一维 <see cref="Array"/>。<see cref="Array"/> 必须具有从零开始的索引。</param>
        /// <param name="arrayIndex"><paramref name="array"/> 中从零开始的索引，从此索引处开始进行复制。</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> 为 null。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> 小于 0。</exception>
        /// <exception cref="ArgumentException">源 <see cref="ChineseWord"/> 中的汉字数目大于从 <paramref name="arrayIndex"/> 到目标 <paramref name="array"/> 末尾之间的可用空间。</exception>
        public void CopyTo(ChineseChar[] array, int arrayIndex)
        {
            this._internalChars.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// 将当前词语中一定范围的字符复制在兼容的一位数组中，从目标数组的指定索引位置开始放置。
        /// </summary>
        /// <param name="index">当前词语中复制开始位置的从零开始的索引。</param>
        /// <param name="array">作为从当前词语对象中复制的元素的目标位置的一维 <see cref="Array"/>。 <see name="Array"/> 必须具有从零开始的索引。</param>
        /// <param name="arrayIndex"><paramref name="array"/> 中从零开始的索引，从此索引处开始进行复制。</param>
        /// <param name="count">要复制的元素数。</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> 为 null。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> 小于 0。或 <paramref name="arrayIndex"/> 小于 0。或 <paramref name="count"/> 小于 0。</exception>
        /// <exception cref="ArgumentException"><paramref name="index"/> 等于或大于当前对象的 <see cref="Count"/> 属性值，或从 <paramref name="index"/> 到当前对象末尾的元素数大于从 <paramref name="arrayIndex"/> 到目标 <paramref name="array"/> 的末尾的可用空间。</exception>
        public void CopyTo(int index, ChineseChar[] array, int arrayIndex, int count)
        {
            this._internalChars.CopyTo(index, array, arrayIndex, count);
        }


        /// <summary>
        /// 将整个 <see cref="ChineseWord"/> 词语中所有的汉字字符复制到兼容的一维数组中，从目标数组的开头开始复制。
        /// </summary>
        /// <param name="array">作为从 <see cref="ChineseWord"/> 复制的汉字的位置的一维 <see cref="Array"/>。<see cref="Array"/> 必须具有从零开始的索引。</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> 为 null。</exception>
        /// <exception cref="ArgumentException">源 <see cref="ChineseWord"/> 中的汉字数大于目标 <paramref name="array"/> 可包含的元素数。</exception>
        public void CopyTo(char[] array)
        {
            this.CopyTo(array, 0);
        }

        /// <summary>
        /// 将整个 <see cref="ChineseWord"/> 词语中所有的汉字字符复制到兼容的一维数组中，从目标数组的指定索引位置开始复制。
        /// </summary>
        /// <param name="array">作为从 <see cref="ChineseWord"/> 复制的汉字的位置的一维 <see cref="Array"/>。<see cref="Array"/> 必须具有从零开始的索引。</param>
        /// <param name="arrayIndex"><paramref name="array"/> 中从零开始的索引，从此索引处开始进行复制。</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> 为 null。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> 小于 0。</exception>
        /// <exception cref="ArgumentException">源 <see cref="ChineseWord"/> 中的汉字数目大于从 <paramref name="arrayIndex"/> 到目标 <paramref name="array"/> 末尾之间的可用空间。</exception>
        public void CopyTo(char[] array, int arrayIndex)
        {
            this._str.CopyTo(arrayIndex, array, 0, this._str.Length);
        }

        /// <summary>
        /// 将当前词语中一定范围的字符复制在兼容的一位数组中，从目标数组的指定索引位置开始放置。
        /// </summary>
        /// <param name="index">当前词语中复制开始位置的从零开始的索引。</param>
        /// <param name="array">作为从当前词语对象中复制的元素的目标位置的一维 <see cref="Array"/>。 <see name="Array"/> 必须具有从零开始的索引。</param>
        /// <param name="arrayIndex"><paramref name="array"/> 中从零开始的索引，从此索引处开始进行复制。</param>
        /// <param name="count">要复制的元素数。</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> 为 null。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> 小于 0。或 <paramref name="arrayIndex"/> 小于 0。或 <paramref name="count"/> 小于 0。</exception>
        /// <exception cref="ArgumentException"><paramref name="index"/> 等于或大于当前对象的 <see cref="Count"/> 属性值，或从 <paramref name="index"/> 到当前对象末尾的元素数大于从 <paramref name="arrayIndex"/> 到目标 <paramref name="array"/> 的末尾的可用空间。</exception>
        public void CopyTo(int index, char[] array, int arrayIndex, int count)
        {
            this._str.CopyTo(index, array, arrayIndex, count);
        }


        /// <summary>
        /// 将整个 <see cref="ChineseWord"/> 词语中所有的汉字字符复制到兼容的一维数组中，从目标数组的指定索引位置开始复制。
        /// </summary>
        /// <param name="array">作为从 <see cref="ChineseWord"/> 复制的汉字的位置的一维 <see cref="Array"/>。<see cref="Array"/> 必须具有从零开始的索引。</param>
        /// <param name="index"><paramref name="array"/> 中从零开始的索引，从此索引处开始进行复制。</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> 为 null。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> 小于 0。</exception>
        /// <exception cref="ArgumentException">
        /// 源 <see cref="ChineseWord"/> 中的汉字数目大于从 <paramref name="index"/> 到目标 <paramref name="array"/> 末尾之间的可用空间。
        /// 或者参数 <paramref name="array"/> 所表示的数组中元素的类型不是 <see cref="ChineseChar"/> 或 <see cref="char"/>。
        /// </exception>
        void ICollection.CopyTo(Array array, int index)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            Type arrayType = array.GetType();
            Type elementType = arrayType.GetElementType();
            Type chineseCharType = typeof(ChineseChar);
            Type charType = typeof(char);

            if (elementType == chineseCharType || elementType.IsSubclassOf(chineseCharType))
                this.CopyTo(array.OfType<ChineseChar>().ToArray(), index);
            else if (elementType == charType)
                this.CopyTo((char[])array, index);
            else
                throw new ArgumentException("传入的参数值类型不正确。", "array");
        }


        /// <summary>
        /// 对词语中的每个汉字执行指定操作。
        /// </summary>
        /// <param name="action">要对 <see cref="ChineseWord"/> 的每个汉字执行的 <see cref="Action{ChineseChar}"/> 委托。</param>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> 为 null。</exception>
        public void ForEach(Action<ChineseChar> action)
        {
            this._internalChars.ForEach(action);
        }

        /// <summary>
        /// 对词语中的每个汉字执行指定操作。
        /// </summary>
        /// <param name="action">要对 <see cref="ChineseWord"/> 的每个汉字执行的 <see cref="Action{T}"/> 委托。</param>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> 为 null。</exception>
        public void ForEach(Action<char> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            foreach (char c in this.AsCharEnumerable())
            {
                action(c);
            }
        }


        /// <summary>
        /// 确定是否 <see cref="ChineseWord"/> 中的每个汉字字符都与指定的谓词所定义的条件相匹配。
        /// </summary>
        /// <param name="match"><see cref="Predicate{T}"/> 委托，定义要据以检查汉字的条件。</param>
        /// <returns>如果 <see cref="ChineseWord"/> 中的每个汉字都与指定的谓词所定义的条件想匹配，则返回 true；否则返回 false。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="match"/> 为 null。</exception>
        public bool TrueForAll(Predicate<ChineseChar> match)
        {
            return this._internalChars.TrueForAll(match);
        }

        /// <summary>
        /// 确定是否 <see cref="ChineseWord"/> 中的每个汉字字符都与指定的谓词所定义的条件相匹配。
        /// </summary>
        /// <param name="match"><see cref="Predicate{T}"/> 委托，定义要据以检查汉字的条件。</param>
        /// <returns>如果 <see cref="ChineseWord"/> 中的每个汉字都与指定的谓词所定义的条件想匹配，则返回 true；否则返回 false。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="match"/> 为 null。</exception>
        public bool TrueForAll(Predicate<char> match)
        {
            if (match == null)
                throw new ArgumentNullException("match");

            return this._internalChars.TrueForAll(ch => match(ch.ChineseCharacter));
        }


        /// <summary>
        /// 创建源词语 <see cref="ChineseWord"/> 中指定汉字字符范围的浅表副本。
        /// </summary>
        /// <param name="index">范围开始处的从零开始的 <see cref="ChineseWord"/> 索引。</param>
        /// <param name="count">范围中的汉字字符数。 </param>
        /// <returns>源词语 <see cref="ChineseWord"/> 中的汉字字符范围的浅表副本。</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> 小于 0。或 <paramref name="count"/> 小于 0。</exception>
        /// <exception cref="ArgumentException"><paramref name="index"/> 和 <paramref name="count"/> 不表示 <see cref="ChineseWord"/> 中汉字字符的有效范围。</exception>
        public ChineseWord GetRange(int index, int count)
        {
            List<ChineseChar> list = this._internalChars.GetRange(index, count);
            return new ChineseWord(list);
        }


        /// <summary>
        /// 将整个 <see cref="ChineseWord"/> 中所有的汉字字符的顺序反转。
        /// </summary>
        public void Reverse()
        {
            this.Reverse(0, this.Count);
        }

        /// <summary>
        /// 将指定范围中的汉字字符的顺序反转。
        /// </summary>
        /// <param name="index">要反转的范围的从零开始的起始索引。</param>
        /// <param name="count">要反转的范围内的汉字字符数。</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> 小于 0。或 <paramref name="count"/> 小于 0。</exception>
        /// <exception cref="ArgumentException"><paramref name="index"/> 和 <paramref name="count"/> 不表示 <see cref="ChineseWord"/> 中汉字字符的有效范围。</exception>
        public void Reverse(int index, int count)
        {
            this.InternalReverse(index, count);
        }

        #endregion


        #region 接口 IEnumerable<ChineseChar>, IEnumerable 的实现

        /// <summary>
        /// 返回循环访问 <see cref="ChineseWord"/> 对象中每个 <see cref="ChineseChar"/> 字符的枚举器。
        /// </summary>
        /// <returns></returns>
        public IEnumerator<ChineseChar> GetEnumerator()
        {
            return this._internalChars.GetEnumerator();
        }

        /// <summary>
        /// 返回循环访问 <see cref="ChineseWord"/> 对象中每个 <see cref="ChineseChar"/> 字符的枚举器。
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._internalChars.GetEnumerator();
        }

        /// <summary>
        /// 返回循环访问 <see cref="ChineseWord"/> 对象中每个 <see cref="ChineseChar"/> 字符的枚举器。
        /// </summary>
        /// <returns></returns>
        IEnumerator<ChineseChar> IEnumerable<ChineseChar>.GetEnumerator()
        {
            return this._internalChars.GetEnumerator();
        }

        #endregion



        #region 方法重载列表

        /// <summary>
        /// 确定指定的对象是否与当前对象等效（表示相同的汉语单词的字符串或 <see cref="ChineseWord"/> 对象）。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(obj, null))
                return false;

            if (obj is ChineseWord)
                return this.Equals(obj as ChineseWord);
            else if (obj is string)
                return this.Equals(obj as string);
            else
                return base.Equals(obj);
        }

        /// <summary>
        /// 确定指定的 <see cref="ChineseWord"/> 对象是否与当前对象等效（表示相同的汉语单词字符串）。
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public bool Equals(ChineseWord word)
        {
            if (object.ReferenceEquals(this, word))
                return true;

            if (object.ReferenceEquals(word, null))
                return false;

            return this.Equals(word.Word);
        }

        /// <summary>
        /// 确定指定的字符串是否与当前对象等效（表示相同的汉语单词字符串）。
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(string other)
        {
            return this.Word.Equals(other);
        }


        /// <summary>
        /// 返回当前对象的字符串表现形式。
        /// <para>该方法将返回对象中所包含的所有中文字符所组成的一个字符串，其结果同属性 <see cref="Word"/>。</para>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Word;
        }

        /// <summary>
        /// 返回此实例的哈希代码，重写自 <see cref="object.GetHashCode"/>。该方法将返回属性 <see cref="Word"/> 的哈希代码
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Word.GetHashCode();
        }

        #endregion


        #region 运算符重载列表
        #endregion


    }
}
