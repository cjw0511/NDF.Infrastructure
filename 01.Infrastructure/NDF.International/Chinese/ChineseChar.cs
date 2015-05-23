using NDF.International.Chinese.Internal;
using NDF.International.Chinese.PinYins;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NDF.International.Chinese
{
    /// <summary>
    /// 表示一个中文字符。
    /// <para>该类型提供对中文字符的简繁体转换支持，并可以获取中文字符的拼音、同音字、笔画数等信息。</para>
    /// <para>通过方法 <see cref="ChineseChar.TryParse(char, out ChineseChar)"/> 或 <see cref="ChineseChar.Parse(char)"/> 方法可以获取该类型的实例。</para>
    /// </summary>
    public class ChineseChar : IEquatable<ChineseChar>, IEquatable<char>, IEquatable<string>
    {
        private char _ch;
        private ChineseCharUnit _charUnit;

        private Lazy<ReadOnlyCollection<PinYin>> _pinyins;
        private Lazy<ReadOnlyCollection<string>> _jianPins;


        /// <summary>
        /// 使用指定的中文汉字字符初始化类型 <see cref="ChineseChar"/> 的新实例。
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="charUnit"></param>
        private ChineseChar(char ch, ChineseCharUnit charUnit)
        {
            this._ch = ch;
            this._charUnit = charUnit;
            this.Initialize();
        }



        /// <summary>
        /// 获取这个汉字字符。
        /// </summary>
        public char ChineseCharacter
        {
            get { return this._ch; }
        }

        /// <summary>
        /// 获取这个字符的笔画数。
        /// </summary>
        public byte StrokeNumber
        {
            get { return this._charUnit.StrokeNumber; }
        }



        #region 公共属性定义 - 拼音相关属性

        /// <summary>
        /// 获取这个字符是否是多音字。
        /// </summary>
        public bool IsPolyphone
        {
            get { return this._charUnit.PinYinIndexes.Length > 1; }
        }

        /// <summary>
        /// 获取这个字符的拼音个数。
        /// </summary>
        public byte PinYinCount
        {
            get { return (byte)this._charUnit.PinYinIndexes.Length; }
        }

        /// <summary>
        /// 获取这个字符的拼音。
        /// <para>该属性相当于属性 <see cref="ChineseChar.PinYins"/> 中每个 <see cref="PinYin"/> 对象的 <see cref="PinYin.PinYinText"/> 属性所构成的一个集合。</para>
        /// <para>该属性返回一个只读集合，集合中的每个元素都是该汉字的拼音之一；如果该汉字是多音字，则集合中将包含多个元素，否则集合中将仅包含一个元素。</para>
        /// </summary>
        public ReadOnlyCollection<string> PinYinTexts
        {
            get { return this._charUnit.PinYins; }
        }

        /// <summary>
        /// 获取包含一组表示该汉字字符拼音 <see cref="PinYin"/> 对象的只读一个。
        /// <para>如果该汉字是一个多音字，则该只读集合将包含表示每个拼音读音的 <see cref="PinYin"/> 对象，否则该集合仅包含一个元素。</para>
        /// </summary>
        public ReadOnlyCollection<PinYin> PinYins
        {
            get { return this._pinyins.Value; }
        }

        /// <summary>
        /// 获取一组表示该汉字字符拼音的简拼形式（仅拼音首字母部分、仅声母或零声母字节情况下的仅韵母部分）的字符串。
        /// <para>如果该汉字是一个多音字，则该只读集合将包含每个读音的简拼形式字符串。</para>
        /// <para>该属性相当于属性 <see cref="ChineseChar.PinYins"/> 中每个 <see cref="PinYin"/> 对象的 <see cref="PinYin.JianPins"/> 属性所构成的一个集合。</para>
        /// </summary>
        public ReadOnlyCollection<string> JianPins
        {
            get { return this._jianPins.Value; }
        }

        #endregion


        #region 公共属性定义 - 简繁体转换相关属性

        /// <summary>
        /// 获取该中文字符的简体中文形式等效字符。
        /// <para>如果该中文字符本身就是简体中文字符，即 <see cref="IsSimplified"/> 属性值为 true，则该属性的值与 <see cref="ChineseCharacter"/> 相同。</para>
        /// </summary>
        public char Simplified
        {
            get { return this._charUnit.Simplified; }
        }

        /// <summary>
        /// 获取一个布尔值，表示该中文字符是否为简体中文。
        /// </summary>
        public bool IsSimplified
        {
            get { return this.ChineseCharacter == this.Simplified; }
        }


        /// <summary>
        /// 获取该中文字符的繁体中文形式等效字符。
        /// <para>如果该中文字符本身就是繁体中文字符，即 <see cref="IsTraditional"/> 属性值为 true，则该属性的值与 <see cref="ChineseCharacter"/> 相同。</para>
        /// </summary>
        public char Traditional
        {
            get { return this._charUnit.Traditional; }
        }

        /// <summary>
        /// 获取一个布尔值，表示该中文字符是否为繁体中文。
        /// </summary>
        public bool IsTraditional
        {
            get { return this.ChineseCharacter == this.Traditional; }
        }

        #endregion



        #region 公共方法定义 - 获取和判断同音字

        /// <summary>
        /// 识别字符是否有指定的读音。该方法在验证时将同时考虑被验证的拼音字符串的声调部分，判断声调是否也和当前中文字符读音的声调相同。
        /// <para>如果仅需要判断当前字符是否具有指定的读音而不考虑声调，请使用方法 <see cref="HasSound(String, Boolean)"/> 请将第二个参数设置为 false。</para>
        /// </summary>
        /// <param name="pinyin">指定的需要被识别的拼音。</param>
        /// <returns>如果指定的拼音字符串在实例字符的拼音集合中则返回 ture，否则返回 false。</returns>
        /// <exception cref="ArgumentNullException">如果参数 <paramref name="pinyin"/> 为 null 或空字符串，则抛出该异常。</exception>
        public bool HasSound(string pinyin)
        {
            //....
            return this.HasSound(pinyin, true);
        }

        /// <summary>
        /// 识别字符是否有指定的读音。一个布尔参数指示在验证时是否同时当前汉字和传入的拼音参数的声调也是否相同。
        /// <para>如果参数 <paramref name="pinyin"/> 字符串尾部是一个不合法的声调数值（不为 1、2、3、4、5 中任何一个值），则无论参数 <paramref name="validateTone"/> 是否为 true，该方法都返回 false。</para>
        /// </summary>
        /// <param name="pinyin"></param>
        /// <param name="validateTone"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">如果参数 <paramref name="pinyin"/> 为 null 或空字符串，则抛出该异常。</exception>
        public bool HasSound(string pinyin, bool validateTone)
        {
            if (string.IsNullOrEmpty(pinyin))
                throw new ArgumentNullException(pinyin);

            PinYin p;
            if (!PinYin.TryParse(pinyin, validateTone, out p))
                return false;

            return validateTone
                //? this.PinYins.Any(item => string.Equals(item.QuanPin, p.QuanPin, StringComparison.Ordinal) && item.Tone == p.Tone)
                ? this._charUnit.PinYinIndexes.Contains(p.PinYinIndex)
                : this.PinYins.Any(item => string.Equals(item.QuanPin, p.QuanPin, StringComparison.Ordinal));
        }


        /// <summary>
        /// 识别给出的字符是否是实例字符的同音字。该方法在验证时会同时验证传入字符 <paramref name="ch"/> 的拼音和声调是否都和当前汉字相同。
        /// </summary>
        /// <param name="ch">指出需要识别的字符。</param>
        /// <returns>如果给出的实例字符是同音字则返回 ture，否则返回 false。</returns>
        /// <exception cref="ArgumentException">传入的字符 <paramref name="ch"/> 不是一个汉字字符时，抛出该异常。</exception>
        public bool IsHomophone(char ch)
        {
            return this.IsHomophone(ch, true);
        }

        /// <summary>
        /// 识别给出的字符是否是实例字符的同音字。参数 <paramref name="validateTone"/> 指示在验证时是否同时验证传入字符 <paramref name="ch"/> 的声调也和当前汉字相同。
        /// </summary>
        /// <param name="ch">指出需要识别的字符。</param>
        /// <param name="validateTone">指示在验证时是否同时验证传入字符 <paramref name="ch"/> 的声调也和当前汉字相同。</param>
        /// <returns>如果给出的实例字符是同音字则返回 ture，否则返回 false。</returns>
        /// <exception cref="ArgumentException">传入的字符 <paramref name="ch"/> 不是一个汉字字符时，抛出该异常。</exception>
        public bool IsHomophone(char ch, bool validateTone)
        {
            ChineseChar c = Parse(ch);
            return validateTone
                ? this.PinYins.Any(
                    pinyin1 => c.PinYins.Any(
                        pinyin2 => string.Equals(pinyin1.PinYinText, pinyin2.PinYinText, StringComparison.Ordinal)))
                : this.PinYins.Any(
                    pinyin1 => c.PinYins.Any(
                        pinyin2 => string.Equals(pinyin1.QuanPin, pinyin2.QuanPin, StringComparison.Ordinal)));
        }

        #endregion


        #region 公共方法定义 - 获取和判断字符的笔画数

        /// <summary>
        /// 将给出的字符和实例字符的笔画数进行比较。
        /// </summary>
        /// <param name="ch">显示给出的字符。</param>
        /// <returns>说明比较操作的结果。如果给出字符和实例字符的笔画数相同，返回值为 0。如果实例字符比给出字符的笔画多，则返回值大于 0。 如果实例字符比给出字符的笔画少，则返回值小于 0。</returns>
        /// <exception cref="ArgumentException">传入的字符 <paramref name="ch"/> 不是一个汉字字符时，抛出该异常。</exception>
        public int CompareStrokeNumber(char ch)
        {
            ChineseCharUnit charUnit;
            if (!ChineseResources.TryGetCharUnit(ch, out charUnit))
            {
                throw new ArgumentException(string.Format("传入的字符 {0} 不是一个汉字字符。", ch));
            }
            return this.StrokeNumber - charUnit.StrokeNumber;
        }

        #endregion


        #region 内部方法定义 - 初始化相关属性的值

        private void Initialize()
        {
            this._pinyins = new Lazy<ReadOnlyCollection<PinYin>>(this.GetPinYins);
            this._jianPins = new Lazy<ReadOnlyCollection<string>>(this.GetJianPins);
        }


        private ReadOnlyCollection<PinYin> GetPinYins()
        {
            List<PinYin> list = new List<PinYin>(this.PinYinTexts.Count);
            foreach (string text in this.PinYinTexts)
            {
                PinYin pinyin = PinYin.Parse(text);
                list.Add(pinyin);
            }
            return new ReadOnlyCollection<PinYin>(list);
        }

        private ReadOnlyCollection<string> GetJianPins()
        {
            List<string> list = new List<string>();
            foreach (PinYin pinyin in this.PinYins)
            {
                list.AddRange(pinyin.JianPins);
            }
            return new ReadOnlyCollection<string>(list.Distinct().ToList());
        }

        #endregion



        #region 方法重载列表

        /// <summary>
        /// 确定指定的对象是否与当前对象等效（是一个可表示汉语拼音的字符、字符串或 <see cref="ChineseChar"/> 对象）。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(obj, null))
                return false;

            if (obj is ChineseChar)
                return this.Equals(obj as ChineseChar);
            else if (obj is string)
                return this.Equals(obj as string);
            else if (obj is char)
                return this.Equals((char)obj);
            else
                return base.Equals(obj);
        }

        /// <summary>
        /// 确定指定的汉字字符 <see cref="ChineseChar"/> 是否与当前对象等效（表示相同的汉字）。
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public bool Equals(ChineseChar ch)
        {
            if (object.ReferenceEquals(this, ch))
                return true;

            if (object.ReferenceEquals(ch, null))
                return false;

            return this.Equals(ch.ChineseCharacter);
        }

        /// <summary>
        /// 确定指定的字符串是否与当前汉字字符 <see cref="ChineseChar"/> 对象等效（表示相同的汉字）。
        /// <para>如果参数 <paramref name="ch"/> 不为 null 或空字符串、且长度为 1、且内容等于当前对象的 <see cref="ChineseCharacter"/> 属性，则返回 true。</para>
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public bool Equals(string ch)
        {
            if (string.IsNullOrEmpty(ch) || ch.Length != 1)
                return false;

            return this.Equals(ch[0]);
        }

        /// <summary>
        /// 确定指定的字符是否与当前汉字字符 <see cref="ChineseChar"/> 对象等效（表示相同的汉字）。
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public bool Equals(char ch)
        {
            return this.ChineseCharacter == ch;
        }


        /// <summary>
        /// 返回当前 <see cref="ChineseChar"/> 对象的字符串表现形式。该方法将会返回当前对象 <see cref="ChineseCharacter"/> 属性值的字符串格式。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.ChineseCharacter.ToString();
        }

        /// <summary>
        /// 返回此实例的哈希代码，重写自 <see cref="object.GetHashCode"/>。该方法将返回属性 <see cref="ChineseCharacter"/> 的哈希代码
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.ChineseCharacter.GetHashCode();
        }

        #endregion


        #region 运算符重载列表

        /// <summary>
        /// 将 <see cref="ChineseChar"/> 对象隐式转换成 <see cref="char"/> 字符。该运算符重载直接返回该对象的 <see cref="ChineseChar.ChineseCharacter"/> 属性。
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException">如果参数 <paramref name="ch"/> 为 null，则抛出该异常。</exception>
        public static implicit operator char(ChineseChar ch)
        {
            if (object.ReferenceEquals(ch, null))
            {
                ArgumentException ex = new ArgumentException("ch");
                throw new InvalidCastException(ex.Message, ex);
            }

            return ch.ChineseCharacter;
        }

        /// <summary>
        /// 将 <see cref="char"/> 显示强制转换成 <see cref="ChineseChar"/> 对象。如果 <paramref name="ch"/> 不是一个中文字符，该类型转换操作将会抛出 <see cref="ArgumentException"/> 异常。
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException">如果 <paramref name="ch"/> 不是一个中文字符，将会抛出该异常。</exception>
        public static explicit operator ChineseChar(char ch)
        {
            try
            {
                return Parse(ch);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(ex.Message, ex);
            }
        }


        /// <summary>
        /// 判断两个汉字字符对象 <see cref="ChineseChar"/> 是否等效（表示相同的汉字）。
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public static bool operator ==(ChineseChar item1, ChineseChar item2)
        {
            if (object.ReferenceEquals(item1, null))
                return object.ReferenceEquals(item2, null);

            return item1.Equals(item2);
        }

        /// <summary>
        /// 判断一个汉字字符对象 <see cref="ChineseChar"/> 和另一个字符串是否等效（表示相同的汉字）。
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public static bool operator ==(ChineseChar item1, string item2)
        {
            if (object.ReferenceEquals(item1, null))
                return object.ReferenceEquals(item2, null);

            return item1.Equals(item2);
        }

        /// <summary>
        /// 判断一个字符串和另一个汉字字符对象 <see cref="ChineseChar"/> 是否等效（表示相同的汉字）。
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public static bool operator ==(string item1, ChineseChar item2)
        {
            return item2 == item1;
        }

        /// <summary>
        /// 判断一个汉字字符对象 <see cref="ChineseChar"/> 和另一个 <see cref="char"/> 字符是否等效（表示相同的汉字）。
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public static bool operator ==(ChineseChar item1, char item2)
        {
            if (object.ReferenceEquals(item1, null))
                return false;

            return item1.Equals(item2);
        }

        /// <summary>
        /// 判断一个 <see cref="char"/> 字符和另一个汉字字符对象 <see cref="ChineseChar"/> 是否等效（表示相同的汉字）。
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public static bool operator ==(char item1, ChineseChar item2)
        {
            return item2 == item1;
        }


        /// <summary>
        /// 判断两个汉字字符对象 <see cref="ChineseChar"/> 是否不等效（表示不同的汉字）。
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public static bool operator !=(ChineseChar item1, ChineseChar item2)
        {
            return !(item1 == item2);
        }

        /// <summary>
        /// 判断一个汉字字符对象 <see cref="ChineseChar"/> 和另一个字符串是否不等效（字符串不是汉字、或表示不同的汉字）。
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public static bool operator !=(ChineseChar item1, string item2)
        {
            return !(item1 == item2);
        }

        /// <summary>
        /// 判断一个字符串和另一个汉字字符对象 <see cref="ChineseChar"/> 是否不等效（字符串不是汉字、或表示不同的汉字）。
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public static bool operator !=(string item1, ChineseChar item2)
        {
            return !(item1 == item2);
        }

        /// <summary>
        /// 判断一个汉字字符对象 <see cref="ChineseChar"/> 和另一个 <see cref="char"/> 字符是否不等效（字符不是汉字、或表示不同的汉字）。
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public static bool operator !=(ChineseChar item1, char item2)
        {
            return !(item1 == item2);
        }

        /// <summary>
        /// 判断一个 <see cref="char"/> 字符和另一个汉字字符对象 <see cref="ChineseChar"/> 是否不等效（字符不是汉字、或表示不同的汉字）。
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public static bool operator !=(char item1, ChineseChar item2)
        {
            return !(item1 == item2);
        }

        #endregion



        #region 公共静态工具方法定义

        /// <summary>
        /// 检索具有指定笔画数的所有字符串。
        /// </summary>
        /// <param name="strokeNumber">指出需要被识别的笔画数。</param>
        /// <returns>返回具有指定笔画数的字符列表。如果笔画数是无效值返回空。</returns>
        public static ReadOnlyCollection<char> GetChars(byte strokeNumber)
        {
            int index = Array.BinarySearch(ChineseResources.ChineseStrokeNumbers, strokeNumber);
            if (index >= 0)
            {
                ReadOnlyCollection<char> chars = ChineseResources.StrokeNumberCharDictionary[index];
                return chars;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 获取给定拼音的所有同音字。
        /// <para>如果创建该 <paramref name="pinyin"/> 对象时指定了声调，则该方法将返回同全拼和同音调的所有汉字。</para>
        /// <para>如果创建该 <paramref name="pinyin"/> 对象时未指定声调，则该方法将返回同全拼情况下所有音调的汉字。</para>
        /// <para>如果传入的字符串不是一个有效的拼音值，则该方法将返回 null。</para>
        /// </summary>
        /// <param name="pinyin">指出读音。</param>
        /// <returns>返回具有相同的指定拼音的字符串列表。如果拼音不是有效值则返回空。</returns>
        public static ReadOnlyCollection<char> GetChars(string pinyin)
        {
            PinYin p = null;
            if (!PinYin.TryParse(pinyin, false, out p))
            {
                return null;
            }
            return p.GetChars();
        }

        /// <summary>
        /// 获取给定拼音和给定声调的所有同音字。
        /// <para>该操作不考虑拼音字符串参数 <paramref name="pinyin"/> 中的声调部分，而是以使用参数 <paramref name="tone"/> 与拼音字符串中的全拼部分共同组成一个新的拼音来获取其所有同音字。</para>
        /// <para>如果参数 <paramref name="tone"/> 的值为 <see cref="Tone.Undefined"/>，则该方法将返回同全拼情况下所有音调的汉字（相当于方法 <see cref="GetCharsWithNonTone"/>）。</para>
        /// <para>如果传入的字符串不是一个有效的拼音值，则该方法将返回 null。</para>
        /// </summary>
        /// <param name="pinyin"></param>
        /// <param name="tone"></param>
        /// <returns></returns>
        public static ReadOnlyCollection<char> GetChars(string pinyin, Tone tone)
        {
            PinYin p = null;
            if (!PinYin.TryParse(pinyin, false, out p))
            {
                return null;
            }
            return p.GetChars(tone);
        }

        /// <summary>
        /// 获取给定拼音的所有同音字。该操作不考虑拼音字符串中所包含的声调部分，意即返回该拼音全拼部分所有声调（1声 - 5声）的所有同音字。
        /// <para>如果传入的字符串不是一个有效的拼音值，则该方法将返回 null。</para>
        /// </summary>
        /// <param name="pinyin"></param>
        /// <returns></returns>
        public static ReadOnlyCollection<char> GetCharsWithNonTone(string pinyin)
        {
            PinYin p = null;
            if (!PinYin.TryParse(pinyin, false, out p))
            {
                return null;
            }
            return p.GetCharsWithNonTone();
        }


        /// <summary>
        /// 检索具有指定笔画数的字符个数。
        /// </summary>
        /// <param name="strokeNumber">显示需要被识别的笔画数。</param>
        /// <returns>返回具有指定笔画数的字符数。 如果笔画数是无效值返回-1。</returns>
        public static short GetCharCount(byte strokeNumber)
        {
            ReadOnlyCollection<char> chars = GetChars(strokeNumber);
            return chars != null
                ? (short)chars.Count
                : (short)-1;
        }


        /// <summary>
        /// 检索具有指定拼音的字符数。
        /// <para>如果创建该 <paramref name="pinyin"/> 对象时指定了声调，则该方法将返回同全拼和同音调的所有汉字数量。</para>
        /// <para>如果创建该 <paramref name="pinyin"/> 对象时未指定声调，则该方法将返回同全拼情况下所有音调的汉字数量。</para>
        /// <para>如果传入的字符串不是一个有效的拼音值，则该方法将返回 -1。</para>
        /// </summary>
        /// <param name="pinyin">显示需要被识别的拼音字符串。</param>
        /// <returns>返回具有指定拼音的字符数。 如果拼音不是有效值则返回-1。</returns>
        public static short GetHomophoneCount(string pinyin)
        {
            ReadOnlyCollection<char> chars = GetChars(pinyin);
            return chars != null ? (short)chars.Count : (short)-1;
        }

        /// <summary>
        /// 检索具有指定拼音且指定声调的字符数。
        /// <para>该操作不考虑拼音字符串参数 <paramref name="pinyin"/> 中的声调部分，而是以使用参数 <paramref name="tone"/> 与拼音字符串中的全拼部分共同组成一个新的拼音来获取其所有同音字数量。</para>
        /// <para>如果参数 <paramref name="tone"/> 的值为 <see cref="Tone.Undefined"/>，则该方法将返回同全拼情况下所有音调的汉字（相当于方法 <see cref="GetHomophoneCountWithNonTone"/>）。</para>
        /// <para>如果传入的字符串不是一个有效的拼音值，则该方法将返回 -1。</para>
        /// </summary>
        /// <param name="pinyin"></param>
        /// <param name="tone"></param>
        /// <returns></returns>
        public static short GetHomophoneCount(string pinyin, Tone tone)
        {
            ReadOnlyCollection<char> chars = GetChars(pinyin, tone);
            return chars != null ? (short)chars.Count : (short)-1;
        }

        /// <summary>
        /// 检索具有指定拼音的字符数。该操作不考虑拼音字符串中所包含的声调部分，意即返回该拼音全拼部分所有声调（1声 - 5声）的所有同音字数量。
        /// <para>如果传入的字符串不是一个有效的拼音值，则该方法将返回 -1。</para>
        /// </summary>
        /// <param name="pinyin"></param>
        /// <returns></returns>
        public static short GetHomophoneCountWithNonTone(string pinyin)
        {
            ReadOnlyCollection<char> chars = GetCharsWithNonTone(pinyin);
            return chars != null ? (short)chars.Count : (short)-1;
        }


        /// <summary>
        /// 检索指定字符的笔画数。
        /// </summary>
        /// <param name="ch">指出需要识别的字符。</param>
        /// <returns>返回指定字符的笔画数。如果字符不是有效值则返回 -1。</returns>
        public static short GetStrokeNumber(char ch)
        {
            ChineseCharUnit charUnit;
            return ChineseResources.TryGetCharUnit(ch, out charUnit)
                ? charUnit.StrokeNumber
                : (short)-1;
        }


        /// <summary>
        /// 识别给出的两个字符是否是同音字。
        /// <para>该方法同时验证这两个字符的声调也是否相同，等效于方法 <see cref="IsHomophone(Char, Char, Boolean)"/> 调用时第三个参数传入 true 值。</para>
        /// <para>如果这两个字符中存在其中一个或者两个都是多音字，则判断其中任何拼音同音即返回 true。</para>
        /// </summary>
        /// <param name="ch1">指出第一个字符</param>
        /// <param name="ch2">指出第二个字符</param>
        /// <returns>如果给出的字符是同音字且声调相同则返回 true，否则返回 false。</returns>
        /// <exception cref="ArgumentException">如果这两个字符中任意一个不为中文字符，则抛出该异常。</exception>
        public static bool IsHomophone(char ch1, char ch2)
        {
            return IsHomophone(ch1, ch2, true);
        }

        /// <summary>
        /// 识别给出的两个字符是否是同音字。参数 <paramref name="validateTone"/> 指示是否同时验证这两个字符的声调是否相同。
        /// <para>如果这两个字符中存在其中一个或者两个都是多音字，则判断其中任何拼音同音即返回 true。</para>
        /// </summary>
        /// <param name="ch1">指出第一个字符。</param>
        /// <param name="ch2">指出第二个字符。</param>
        /// <param name="validateTone">指示是否同时验证这两个字符的声调是否相同。</param>
        /// <returns>如果给出的字符是同音字返回 true，否则返回 false。</returns>
        /// <exception cref="ArgumentException">如果这两个字符中任意一个不为中文字符，则抛出该异常。</exception>
        public static bool IsHomophone(char ch1, char ch2, bool validateTone)
        {
            ChineseChar c1 = Parse(ch1);
            return c1.IsHomophone(ch2, validateTone);
        }


        /// <summary>
        /// 识别给出的字符串是否是一个有效的汉字字符。
        /// </summary>
        /// <param name="ch">指出需要识别的字符。</param>
        /// <returns>如果指定的字符是一个有效的汉字字符则返回 true，否则返回 false。</returns>
        public static bool IsValidChar(char ch)
        {
            return ch >= ChineseResources._BitMap_BeginMark
                && ch <= ChineseResources._BitMap_EndMark
                && ChineseResources.ChineseCharsBitMap[ch - ChineseResources._BitMap_BeginMark];
        }


        /// <summary>
        /// 识别给出的拼音是否是一个有效的拼音字符串。
        /// <para>该方法相当于调用方法 <see cref="IsValidPinYin(String, Boolean)"/> 时在第二个参数传入 true，即该方法验证拼音字符串有效性时将会强制验证其声调的有效性（即参数 <paramref name="pinyin"/> 包含声调部分并且声调值为 1、2、3、4、5 中一个值）。</para>
        /// <para>同方法 <see cref="PinYin.IsValidPinYin(String)"/>。</para>
        /// </summary>
        /// <param name="pinyin"></param>
        /// <returns></returns>
        public static bool IsValidPinYin(string pinyin)
        {
            return PinYin.IsValidPinYin(pinyin, true);
        }

        /// <summary>
        /// 识别给出的拼音是否是一个有效的拼音字符串。参数 <paramref name="validateTone"/> 指示在判断时是否同时强制验证拼音字符串的声调。
        /// <para>
        /// 注意：如果参数 <paramref name="pinyin"/> 字符串尾部是一个不合法的声调数值（不为 1、2、3、4、5 中任何一个值），则无论参数 <paramref name="validateTone"/> 是否为 true，该方法都返回 false。
        /// </para>
        /// <para>同方法 <see cref="PinYin.IsValidPinYin(String, Boolean)"/>。</para>
        /// </summary>
        /// <param name="pinyin">指出需要识别的字符串。</param>
        /// <param name="validateTone">如果该参数值为 true，则要求传入的参数 <paramref name="pinyin"/> 必须包含有效的声调。</param>
        /// <returns>如果指定的字符串是一个有效的拼音字符串则返回 true，否则返回 false。</returns>
        public static bool IsValidPinYin(string pinyin, bool validateTone)
        {
            return PinYin.IsValidPinYin(pinyin, validateTone);
        }


        /// <summary>
        /// 识别给出的笔画数是否是一个有效的笔画数。
        /// </summary>
        /// <param name="strokeNumber">指出需要识别的笔画数。</param>
        /// <returns>如果指定的笔画数是一个有效的笔画数则返回 true，否则返回 false。</returns>
        public static bool IsValidStrokeNumber(byte strokeNumber)
        {
            return strokeNumber >= ChineseResources._StrokeNumber_BeginMark
                && strokeNumber <= ChineseResources._StrokeNumber_EndMark
                && Array.BinarySearch(ChineseResources.ChineseStrokeNumbers, strokeNumber) >= 0;
        }



        private static readonly Dictionary<char, ChineseChar> _chars = new Dictionary<char, ChineseChar>();

        /// <summary>
        /// 尝试将指定的中文汉字字符转换为 <see cref="ChineseChar"/> 对象，并返回一个指示是否转换成功的值。
        /// <para>如果传入的字符 <paramref name="ch"/> 不在简体中文扩展字符集中时，则将转换失败。</para>
        /// <para>如果该方法返回一个表示转换失败的布尔值 false，则输出参数 <paramref name="result"/> 的值将会被设置为 null。</para>
        /// </summary>
        /// <param name="ch"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParse(char ch, out ChineseChar result)
        {
            lock (_chars)
            {
                if (_chars.TryGetValue(ch, out result))
                {
                    return true;
                }
                else
                {
                    ChineseCharUnit charUnit;
                    if (ChineseResources.TryGetCharUnit(ch, out charUnit))
                    {
                        result = new ChineseChar(ch, charUnit);
                        _chars.Add(ch, result);
                        return true;
                    }
                    else
                    {
                        result = null;
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 将指定的中文汉字字符转换解析为 <see cref="ChineseChar"/> 对象。
        /// <para>如果传入的字符 <paramref name="ch"/> 不在简体中文扩展字符集中时，该方法将抛出 <see cref="ArgumentException"/> 异常。</para>
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">字符不在简体中文扩展字符集中时，将抛出该异常。</exception>
        public static ChineseChar Parse(char ch)
        {
            ChineseChar result;
            if (TryParse(ch, out result))
                return result;
            else
                throw new ArgumentException(string.Format("传入的字符 {0} 不是一个汉字字符。", ch));
        }

        #endregion


        #region 汉语拼音和笔画方案中的公共资源定义

        /// <summary>
        /// 一个包含所有中文字符的数组，共 20591 个元素，字符范围从 \u3007 - \ue863（即 12295 - 59491，存在断续）
        /// </summary>
        public static readonly string ChineseChars = new string(ChineseResources.ChineseChars);

        /// <summary>
        /// 一个包含所有可能的汉字笔画数的数组，数组中的每个元素均表示一个汉字的笔画数。
        /// </summary>
        public static readonly ReadOnlyCollection<byte> ChineseStrokeNumbers = new ReadOnlyCollection<byte>(ChineseResources.ChineseStrokeNumbers.ToList());

        #endregion

    }
}
