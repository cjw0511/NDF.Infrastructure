using NDF.International.Chinese.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace NDF.International.Chinese.PinYins
{
    /// <summary>
    /// 表示现代汉语中汉字的拼音。
    /// <para>通过方法 <see cref="PinYin.TryParse(string, out PinYin)"/> 或 <see cref="PinYin.Parse(string)"/> 方法可以获取该类型的实例。</para>
    /// </summary>
    public class PinYin : IEquatable<PinYin>, IEquatable<string>
    {
        private string _pinyin;
        private ushort _pinyinIndex;
        private Tone _tone;

        private string _quanPin;

        private Lazy<ReadOnlyCollection<string>> _jianPins;
        private Lazy<string> _consonant;
        private Lazy<string> _vowel;
        private Lazy<bool> _isZeroConsonant;
        private Lazy<bool> _isWholeSyllable;

        
        /// <summary>
        /// 初始化类型 <see cref="PinYin"/> 的新实例。
        /// </summary>
        /// <param name="pinyin"></param>
        /// <param name="pinyinIndex"></param>
        /// <param name="tone"></param>
        private PinYin(string pinyin, ushort pinyinIndex, Tone tone)
        {
            this._pinyin = pinyin;
            this._pinyinIndex = pinyinIndex;
            this._tone = tone;

            this.Initialize();
        }



        #region 公共属性定义 - 基础属性

        /// <summary>
        /// 获取该拼音项字符串在 <see cref="PinYin.ChinesePinYins"/> 集合中的索引号。
        /// </summary>
        internal ushort PinYinIndex
        {
            get { return this._pinyinIndex; }
        }


        /// <summary>
        /// 获取表示该拼音的全拼加声调（如果有）完整书写形式的字符串。
        /// </summary>
        public string PinYinText
        {
            get { return this._pinyin; }
        }

        /// <summary>
        /// 获取表示该拼音的全拼书写形式的字符串。
        /// </summary>
        public string QuanPin
        {
            get { return this._quanPin; }
        }

        /// <summary>
        /// 获取表示该拼音发音声调的枚举值。
        /// </summary>
        public Tone Tone
        {
            get { return this._tone; }
        }

        /// <summary>
        /// 获取一组表示该拼音的简拼数学形式的字符串。
        /// </summary>
        public ReadOnlyCollection<string> JianPins
        {
            get { return this._jianPins.Value; }
        }

        #endregion


        #region 公共属性定义 - 扩展属性

        /// <summary>
        /// 获取表示该拼音声母部分的字符串。如果该拼音是一个零声母音节，则该属性返回空字符串值（<see cref="string.Empty"/>）。
        /// </summary>
        public string Consonant
        {
            get { return this._consonant.Value; }
        }

        /// <summary>
        /// 获取表示该拼音韵母部分的字符串。
        /// <para>如果该拼音是一个零声母音节（<see cref="IsZeroConsonant"/> 属性值为 true），则该属性值与 <see cref="QuanPin"/> 属性值相同。</para>
        /// </summary>
        public string Vowel
        {
            get { return this._vowel.Value; }
        }


        /// <summary>
        /// 获取一个布尔值，表示该拼音是否为一个零声母音节。
        /// </summary>
        public bool IsZeroConsonant
        {
            get { return this._isZeroConsonant.Value; }
        }

        /// <summary>
        /// 获取一个布尔值，表示该拼音是否为一个整体认读音节。
        /// </summary>
        public bool IsWholeSyllable
        {
            get { return this._isWholeSyllable.Value; }
        }

        #endregion



        #region 公共方法定义 - 获取和判断同音字

        /// <summary>
        /// 获取该汉语拼音的所有同音字。
        /// <para>如果创建该 <see cref="PinYin"/> 对象时指定了声调，则该方法将返回同全拼和同音调的所有汉字。</para>
        /// <para>如果创建该 <see cref="PinYin"/> 对象时未指定声调，则该方法将返回同全拼情况下所有音调的汉字。</para>
        /// </summary>
        /// <returns></returns>
        public ReadOnlyCollection<char> GetChars()
        {
            return this.GetChars(this.Tone);
        }

        /// <summary>
        /// 获取与当前 <see cref="PinYin"/> 对象的 <see cref="QuanPin"/> 全拼属性相同的拼音和给定声调的所有同音字。
        /// <para>如果参数 <paramref name="tone"/> 的值为 <see cref="PinYins.Tone.Undefined"/>，则该方法将返回同全拼情况下所有音调的汉字（相当于方法 <see cref="GetCharsWithNonTone"/>）。</para>
        /// </summary>
        /// <param name="tone"></param>
        /// <returns></returns>
        public ReadOnlyCollection<char> GetChars(Tone tone)
        {
            if (tone == PinYins.Tone.Undefined)
                return this.GetCharsWithNonTone();

            string pinyin = this.QuanPin + (int)tone;
            int index;
            if (ChineseResources.TryGetPinYinIndex(pinyin, out index))
            {
                return ChineseResources.PinYinCharDictionary[index];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取与当前 <see cref="PinYin"/> 对象的 <see cref="QuanPin"/> 全拼属性相同的拼音的所有声调同音字。
        /// </summary>
        /// <returns></returns>
        public ReadOnlyCollection<char> GetCharsWithNonTone()
        {
            List<char> list = new List<char>();
            foreach (Tone tone in new Tone[] { Tone.HighLevel, Tone.Rising, Tone.FallingRising, Tone.Falling, Tone.Light })
            {
                ReadOnlyCollection<char> homophones = this.GetChars(tone);
                list.AddRange(homophones);
            }
            return new ReadOnlyCollection<char>(list.Distinct().ToList());
        }

        #endregion


        #region 内部方法定义 - 初始化相关属性的值

        private void Initialize()
        {
            this._quanPin = this.Tone == Tone.Undefined
                ? this._pinyin
                : this._pinyin.Substring(0, this._pinyin.Length - 1);

            this._jianPins = new Lazy<ReadOnlyCollection<string>>(this.GetJianPins);
            this._consonant = new Lazy<string>(this.GetConsonant);
            this._vowel = new Lazy<string>(this.GetVowel);
            this._isZeroConsonant = new Lazy<bool>(this.GetIsZeroVowel);
            this._isWholeSyllable = new Lazy<bool>(this.GetIsWholeSyllable);
        }


        private ReadOnlyCollection<string> GetJianPins()
        {
            List<string> list = new List<string>();

            string item1 = this.QuanPin.Substring(0, 1);
            list.Add(item1);

            string item2 = this.IsZeroConsonant ? this.Vowel : this.Consonant;
            if (!string.Equals(item1, item2, StringComparison.OrdinalIgnoreCase))
            {
                list.Add(item2);
            }
            return new ReadOnlyCollection<string>(list);
        }

        private string GetConsonant()
        {
            foreach (string c in Consonants)
            {
                if (this.QuanPin.StartsWith(c, StringComparison.OrdinalIgnoreCase))
                {
                    return c;
                }
            }
            return string.Empty;
        }

        private string GetVowel()
        {
            if (this.IsZeroConsonant)
                return this.QuanPin;

            return this.QuanPin.Substring(this.Consonant.Length);
        }

        private bool GetIsZeroVowel()
        {
            foreach (string item in ZeroConsonants)
            {
                if (string.Equals(item, this.QuanPin, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return string.IsNullOrEmpty(this.Consonant);
        }

        private bool GetIsWholeSyllable()
        {
            foreach (string item in WholeSyllables)
            {
                if (string.Equals(item, this.QuanPin, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion



        #region 方法重载列表

        /// <summary>
        /// 返回当前 <see cref="PinYin"/> 对象的字符串表现形式。该方法将会返回当前对象 <see cref="PinYinText"/> 属性的值。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.PinYinText;
        }


        /// <summary>
        /// 确定指定的对象是否与当前对象等效（是一个可表示汉语拼音的字符串或 <see cref="PinYin"/> 对象、具有相同的全拼字母且其中定义的声调也相同）。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(obj, null))
                return false;

            if (obj is PinYin)
                return this.Equals(obj as PinYin);
            else if (obj is string)
                return this.Equals(obj as string);
            else
                return base.Equals(obj);
        }

        /// <summary>
        /// 确定指定的汉语拼音对象 <see cref="PinYin"/> 是否与当前对象等效（具有相同的全拼字母且其中定义的声调也相同）。
        /// </summary>
        /// <param name="pinyin"></param>
        /// <returns></returns>
        public bool Equals(PinYin pinyin)
        {
            if (object.ReferenceEquals(this, pinyin))
                return true;

            if (object.ReferenceEquals(pinyin, null))
                return false;

            return this.Equals(pinyin.PinYinText);
        }

        /// <summary>
        /// 确定指定的字符串是否与当前表示汉语拼音 <see cref="PinYin"/> 的对象等效（具有相同的全拼字母且其中定义的声调也相同）。
        /// </summary>
        /// <param name="pinyin"></param>
        /// <returns></returns>
        public virtual bool Equals(string pinyin)
        {
            pinyin = pinyin != null ? pinyin.Trim() : null;
            if (string.IsNullOrEmpty(pinyin))
                return false;

            return string.Equals(this.PinYinText, pinyin, StringComparison.OrdinalIgnoreCase);
        }


        /// <summary>
        /// 返回此实例的哈希代码，重写自 <see cref="object.GetHashCode"/>。该方法将返回属性 <see cref="PinYinText"/> 的哈希代码
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.PinYinText.GetHashCode();
        }

        #endregion


        #region 运算符重载

        /// <summary>
        /// 将 <see cref="PinYin"/> 对象隐式转换成一个表示该拼音全拼加声调的字符串。该运算符重载直接返回该对象的 <see cref="PinYin.PinYinText"/> 属性。
        /// </summary>
        /// <param name="pinyin"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException">如果参数 <paramref name="pinyin"/> 为 null，则抛出该异常。</exception>
        public static implicit operator string(PinYin pinyin)
        {
            if (object.ReferenceEquals(pinyin, null))
            {
                ArgumentException ex = new ArgumentException("pinyin");
                throw new InvalidCastException(ex.Message, ex);
            }

            return pinyin.PinYinText;
        }

        /// <summary>
        /// 将单个汉字的现代汉语拼音字符串强制转换为其等效的 <see cref="PinYin"/>。
        /// <para>该运算符重载等效于调用 <see cref="Parse(string, bool)"/> 方法时参数 validateTone 传入 true。</para>
        /// </summary>
        /// <param name="pinyin"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException">如果传入的参数 <paramref name="pinyin"/> Null、空字符串或者纯空格字符串，或者其不是一个合法的现代汉语拼音文本格式字符串，则抛出该异常。</exception>
        public static explicit operator PinYin(string pinyin)
        {
            try
            {
                return Parse(pinyin);
            }
            catch (Exception ex)
            {
                throw new InvalidCastException(ex.Message, ex);
            }
        }


        /// <summary>
        /// 判断两个汉语拼音对象 <see cref="PinYin"/> 是否等效（具有相同的全拼字母且其中定义的声调也相同）。
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public static bool operator ==(PinYin item1, PinYin item2)
        {
            if (object.ReferenceEquals(item1, null))
                return object.ReferenceEquals(item2, null);

            return item1.Equals(item2);
        }

        /// <summary>
        /// 判断汉语拼音对象 <see cref="PinYin"/> 是否和另一个表示汉语拼音的字符串等效（具有相同的全拼字母且其中定义的声调也相同）。
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public static bool operator ==(PinYin item1, string item2)
        {
            if (object.ReferenceEquals(item1, null))
                return object.ReferenceEquals(item2, null);

            return item1.Equals(item2);
        }

        /// <summary>
        /// 判断表示汉语拼音的字符串是否和另一个汉语拼音对象 <see cref="PinYin"/> 等效（具有相同的全拼字母且其中定义的声调也相同）。
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public static bool operator ==(string item1, PinYin item2)
        {
            return item2 == item1;
        }


        /// <summary>
        /// 判断两个汉语拼音对象 <see cref="PinYin"/> 是否不等效（具有不同的全拼字母或其中定义的声调不同）。
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public static bool operator !=(PinYin item1, PinYin item2)
        {
            return !(item1 == item2);
        }

        /// <summary>
        /// 判断汉语拼音对象 <see cref="PinYin"/> 是否和另一个表示汉语拼音的字符串不等效（字符串不是拼音、具有不同的全拼字母或其中定义的声调不同）。
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public static bool operator !=(PinYin item1, string item2)
        {
            return !(item1 == item2);
        }

        /// <summary>
        /// 判断表示汉语拼音的字符串是否和另一个汉语拼音对象 <see cref="PinYin"/> 不等效（字符串不是拼音、具有不同的全拼字母或其中定义的声调不同）。
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public static bool operator !=(string item1, PinYin item2)
        {
            return !(item1 == item2);
        }

        #endregion



        #region 公共静态工具方法定义

        /// <summary>
        /// 识别给出的拼音是否是一个有效的拼音字符串。
        /// <para>该方法相当于调用方法 <see cref="IsValidPinYin(String, Boolean)"/> 时在第二个参数传入 true，即该方法验证拼音字符串有效性时将同时会强制验证其声调的有效性（即参数 <paramref name="pinyin"/> 包含声调部分并且声调值为 1、2、3、4、5 中一个值）。</para>
        /// </summary>
        /// <param name="pinyin"></param>
        /// <returns></returns>
        public static bool IsValidPinYin(string pinyin)
        {
            return IsValidPinYin(pinyin, true);
        }

        /// <summary>
        /// 识别给出的拼音是否是一个有效的拼音字符串。参数 <paramref name="validateTone"/> 指示在判断时是否同时强制验证拼音字符串的声调。
        /// <para>
        /// 注意：如果参数 <paramref name="pinyin"/> 字符串尾部是一个不合法的声调数值（不为 1、2、3、4、5 中任何一个值），则无论参数 <paramref name="validateTone"/> 是否为 true，该方法都返回 false。
        /// </para>
        /// </summary>
        /// <param name="pinyin">指出需要识别的字符串。</param>
        /// <param name="validateTone">如果该参数值为 true，则要求传入的参数 <paramref name="pinyin"/> 必须包含有效的声调。</param>
        /// <returns>如果指定的字符串是一个有效的拼音字符串则返回 true，否则返回 false。</returns>
        public static bool IsValidPinYin(string pinyin, bool validateTone)
        {
            int i;
            Tone tone;
            return IsValidPinYin(pinyin, validateTone, out i, out tone);
        }


        internal static bool IsValidPinYin(string pinyin, bool validateTone, out int pinyinIndex, out Tone tone)
        {
            pinyin = pinyin != null ? pinyin.Trim() : null;
            if (string.IsNullOrEmpty(pinyin))
            {
                pinyinIndex = -1;
                tone = Tone.Undefined;
                return false;
            }
            char c = pinyin[pinyin.Length - 1];
            tone = ConvertTone(c);

            if (tone == Tone.Undefined)
            {
                if (validateTone)
                {
                    pinyinIndex = -1;
                    return false;
                }
                else
                {
                    return ChineseResources.TryGetPinYinIndex(pinyin + '1', out pinyinIndex)
                        || ChineseResources.TryGetPinYinIndex(pinyin + '2', out pinyinIndex)
                        || ChineseResources.TryGetPinYinIndex(pinyin + '3', out pinyinIndex)
                        || ChineseResources.TryGetPinYinIndex(pinyin + '4', out pinyinIndex)
                        || ChineseResources.TryGetPinYinIndex(pinyin + '5', out pinyinIndex);
                }
            }
            else
            {
                return ChineseResources.TryGetPinYinIndex(pinyin, out pinyinIndex);
            }
        }

        internal static Tone ConvertTone(char tone)
        {
            switch (tone)
            {
                case '1':
                    return Tone.HighLevel;
                case '2':
                    return Tone.Rising;
                case '3':
                    return Tone.FallingRising;
                case '4':
                    return Tone.Falling;
                case '5':
                    return Tone.Light;
                default:
                    return Tone.Undefined;
            }
        }


        /// <summary>
        /// 从一个表示汉语拼音的字符串中获取其表示声调的部分。一个输出参数值指示传入的参数 <paramref name="pinyin"/> 是否是一个合法的拼音字符串。
        /// </summary>
        /// <param name="pinyin"></param>
        /// <param name="success">该输出参数值指示传入的参数 <paramref name="pinyin"/> 是否是一个合法的拼音字符串。</param>
        /// <returns></returns>
        public static Tone GetPinYinTone(string pinyin, out bool success)
        {
            int index;
            Tone tone;
            success = IsValidPinYin(pinyin, false, out index, out tone);
            return tone;
        }



        /// <summary>
        /// 将单个汉字的现代汉语拼音字符串表示形式转换为其 <see cref="PinYin"/> 等效项，并返回一个指示转换是否成功的值。
        /// <para>该方法等效于调用 <see cref="TryParse(string, bool, out PinYin)"/> 方法时参数 validateTone 传入 true。</para>
        /// </summary>
        /// <param name="pinyin"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryParse(string pinyin, out PinYin result)
        {
            return TryParse(pinyin, true, out result);
        }



        private static readonly Dictionary<string, PinYin> _pinyins = new Dictionary<string, PinYin>();

        /// <summary>
        /// 将单个汉字的现代汉语拼音字符串表示形式转换为其 <see cref="PinYin"/> 等效项，并返回一个指示转换是否成功的值。
        /// <para>参数 <paramref name="validateTone"/> 指示在解析时是否同时强制验证拼音字符串的声调。</para>
        /// <para>传入的参数 <paramref name="pinyin"/> 可选择带有或不带声调均可（如文字 "陈" 可传入拼音 "CHEN2"，也可传入 "CHEN"）。</para>
        /// <para>但如果参数 <paramref name="pinyin"/> 字符串尾部是一个不合法的声调数值（不为 1、2、3、4、5 中任何一个值），则无论参数 <paramref name="validateTone"/> 是否为 true，该方法都返回 false。</para>
        /// </summary>
        /// <param name="pinyin"></param>
        /// <param name="validateTone"></param>
        /// <param name="result">一个 out 参数；如果转换成功，则返回与参数 <paramref name="pinyin"/> 等效的 <see cref="PinYin"/> 对象，否则返回 null。</param>
        /// <returns></returns>
        public static bool TryParse(string pinyin, bool validateTone, out PinYin result)
        {
            pinyin = pinyin != null ? pinyin.Trim() : null;
            if (string.IsNullOrEmpty(pinyin))
            {
                result = null;
                return false;
            }
            pinyin = pinyin.ToUpperInvariant();
            lock (_pinyins)
            {
                if (_pinyins.TryGetValue(pinyin, out result))
                {
                    return true;
                }
                else
                {
                    int index;
                    Tone tone;
                    if (IsValidPinYin(pinyin, validateTone, out index, out tone))
                    {
                        result = new PinYin(pinyin, (ushort)index, tone);
                        _pinyins.Add(pinyin, result);
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
        /// 将单个汉字的现代汉语拼音字符串表示形式转换为其等效的 <see cref="PinYin"/>。
        /// <para>该方法等效于调用 <see cref="Parse(string, bool)"/> 方法时参数 validateTone 传入 true。</para>
        /// </summary>
        /// <param name="pinyin"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">如果传入的参数 <paramref name="pinyin"/> 为 Null、空字符串或者纯空格字符串，则抛出该异常。</exception>
        /// <exception cref="ArgumentException">如果传入的参数 <paramref name="pinyin"/> 不是一个合法的现代汉语拼音文本格式字符串，则抛出该异常。</exception>
        public static PinYin Parse(string pinyin)
        {
            return Parse(pinyin, true);
        }

        /// <summary>
        /// 将单个汉字的现代汉语拼音字符串表示形式转换为其等效的 <see cref="PinYin"/>。
        /// <para>参数 <paramref name="validateTone"/> 指示在解析时是否同时强制验证拼音字符串的声调。</para>
        /// <para>传入的参数 <paramref name="pinyin"/> 可选择带有或不带声调均可（如文字 "陈" 可传入拼音 "chen2"，也可传入 "chen"）。</para>
        /// </summary>
        /// <param name="pinyin"></param>
        /// <param name="validateTone"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">如果传入的参数 <paramref name="pinyin"/> 为 Null、空字符串或者纯空格字符串，则抛出该异常。</exception>
        /// <exception cref="ArgumentException">如果传入的参数 <paramref name="pinyin"/> 不是一个合法的现代汉语拼音文本格式字符串，则抛出该异常。</exception>
        public static PinYin Parse(string pinyin, bool validateTone)
        {
            pinyin = pinyin != null ? pinyin.Trim() : null;
            if (string.IsNullOrEmpty(pinyin))
                throw new ArgumentNullException("pinyin");

            PinYin result;
            if (!TryParse(pinyin, validateTone, out result))
            {
                throw new ArgumentException(string.Format("传入的参数 pinyin: {0} 不是一个合法的现代汉语拼音文本格式字符串。", pinyin));
            }
            return result;
        }

        #endregion


        #region 汉语拼音列表、声母、韵母和整体认读音节列表

        /// <summary>
        /// 一个包含所有可能的汉字拼音的数组，数组中的每个元素均表示一个包含声调的汉语拼音字符串（如 CHEN2、AI4 等），按照字母顺序排列。
        /// </summary>
        public static readonly ReadOnlyCollection<string> ChinesePinYins = new ReadOnlyCollection<string>(ChineseResources.ChinesePinYins.ToList());


        /// <summary>
        /// 现代汉语中文拼音中的声母表，共 23 个。
        /// </summary>
        public static readonly ReadOnlyCollection<string> Consonants = new ReadOnlyCollection<string>(ChineseResources.CompoundVowels.ToList());

        /// <summary>
        /// 现代汉语中文拼音中的单音韵母表，共 24 个（含单韵母 10 个、复韵母 10 个和鼻韵母 4 个）。
        /// </summary>
        public static readonly ReadOnlyCollection<string> SimpleVowels = new ReadOnlyCollection<string>(ChineseResources.SimpleVowels.ToList());

        /// <summary>
        /// 现代汉语中文拼音中的复音韵母表，共 15 个（含复韵母 11 个和鼻韵母 4 个）。
        /// </summary>
        public static readonly ReadOnlyCollection<string> CompoundVowels = new ReadOnlyCollection<string>(ChineseResources.CompoundVowels.ToList());

        /// <summary>
        /// 现代汉语中文拼音中的零声母音节，共 34 个（其中零声母单音节 10 个，字母 'Y' 开头零声母音节 15 个，字母 'W' 开头零声母音节 9 个）。
        /// </summary>
        public static readonly ReadOnlyCollection<string> ZeroConsonants = new ReadOnlyCollection<string>(ChineseResources.ZeroConsonants.ToList());

        /// <summary>
        /// 现代汉语中文拼音中的整体认读音节表，共 16 个。
        /// </summary>
        public static readonly ReadOnlyCollection<string> WholeSyllables = new ReadOnlyCollection<string>(ChineseResources.WholeSyllables.ToList());

        #endregion

    }
}
