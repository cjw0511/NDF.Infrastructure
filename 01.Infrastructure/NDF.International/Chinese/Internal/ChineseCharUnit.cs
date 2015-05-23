using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace NDF.International.Chinese.Internal
{
    /// <summary>
    /// 表示中文字符所包含的汉语拼音和笔画数。
    /// </summary>
    internal struct ChineseCharUnit
    {
        private ushort[] _pinyinIndexes;
        private byte _strokeNumberIndex;
        private char _simplified;
        private char _traditional;

        private ReadOnlyCollection<string> _pinyins;
        private byte _strokeNumber;

        private static readonly ChineseCharUnit _empty = new ChineseCharUnit()
        {
            _pinyinIndexes = null,
            _strokeNumberIndex = byte.MaxValue,
            _simplified = char.MinValue,
            _traditional = char.MinValue
        };


        /// <summary>
        /// 用指定的 <see cref="PinYinIndexes"/> 属性值、 <see cref="StrokeNumberIndex"/> 属性值、<see cref="Simplified"/> 属性值和 <see cref="Traditional"/> 属性值创建一个 <see cref="ChineseCharUnit"/> 的新实例。
        /// </summary>
        /// <param name="pinyinIndexes"></param>
        /// <param name="strokeNumberIndex"></param>
        /// <param name="simplified"></param>
        /// <param name="traditional"></param>
        public ChineseCharUnit(ushort[] pinyinIndexes, byte strokeNumberIndex, char simplified, char traditional)
        {
            this._pinyinIndexes = pinyinIndexes;
            this._strokeNumberIndex = strokeNumberIndex;
            this._simplified = simplified;
            this._traditional = traditional;

            this._pinyins = GetPinYins(pinyinIndexes);
            this._strokeNumber = GetStrokeNumber(strokeNumberIndex);
        }


        /// <summary>
        /// 表示某个汉字所拥有的一组拼音。
        /// 该属性是一个 <see cref="ushort"/> 数组，数组中的每一项都表示 <see cref="ChineseResources.ChinesePinYins"/> 属性数组中某个拼音字符串的索引号。
        /// </summary>
        public ushort[] PinYinIndexes
        {
            get { return this._pinyinIndexes; }
            set { this._pinyinIndexes = value; }
        }

        /// <summary>
        /// 表示某个汉字的笔画数索引号（并非直接笔画数值）。
        /// 该属性是一个 <see cref="byte"/> 值，表示 <see cref="ChineseResources.ChineseStrokeNumbers"/> 属性数组中某个笔画数值的索引号。
        /// </summary>
        public byte StrokeNumberIndex
        {
            get { return this._strokeNumberIndex; }
            set { this._strokeNumberIndex = value; }
        }

        /// <summary>
        /// 表示某个汉字所对应的简体中文字符。
        /// </summary>
        public char Simplified
        {
            get { return this._simplified; }
        }

        /// <summary>
        /// 表示某个汉字所对应的繁体中文字符。
        /// </summary>
        public char Traditional
        {
            get { return this._traditional; }
        }



        /// <summary>
        /// 表示某个汉字所拥有的一组拼音。
        /// </summary>
        public ReadOnlyCollection<string> PinYins
        {
            get { return this._pinyins; }
        }

        /// <summary>
        /// 表示某个汉字的笔画数。
        /// </summary>
        public byte StrokeNumber
        {
            get { return this._strokeNumber; }
        }


        private static ReadOnlyCollection<string> GetPinYins(ushort[] pinyinIndexes)
        {
            string[] array = new string[pinyinIndexes.Length];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = ChineseResources.ChinesePinYins[pinyinIndexes[i]];
            }
            return new ReadOnlyCollection<string>(array.ToList());
        }

        private static byte GetStrokeNumber(byte strokeNumberIndex)
        {
            return ChineseResources.ChineseStrokeNumbers[strokeNumberIndex];
        }



        /// <summary>
        /// 返回一个表示无效或空的 <see cref="ChineseCharUnit"/> 的结构实例。
        /// <para>该属性返回的实例中，所有的内部属性值都是无效的。</para>
        /// </summary>
        internal static ChineseCharUnit Empty
        {
            get { return _empty; }
        }

    }
}
