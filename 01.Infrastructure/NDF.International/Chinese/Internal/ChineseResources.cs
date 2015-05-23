using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace NDF.International.Chinese.Internal
{
    /// <summary>
    /// 定义汉语拼音、笔画和简繁体转换方案相关资源和操作的 API 定义。
    /// </summary>
    internal class ChineseResources
    {

        #region 汉语拼音、笔画和简繁体转换方案相关资源属性定义

        /// <summary>
        /// 一个包含所有中文字符的数组，共 20591 个元素，字符范围从 \u3007 - \ue863（即 12295 - 59491，存在断续）
        /// </summary>
        public static readonly char[] ChineseChars;

        /// <summary>
        /// 一个包含所有可能的汉字拼音的数组，数组中的每个元素均表示一个包含声调的汉语拼音字符串（如 CHEN2、AI4 等），按照字母顺序排列。
        /// </summary>
        public static readonly string[] ChinesePinYins;


        /// <summary>
        /// 表示汉字的最小笔画数，该值为常量 1（即）0x1。
        /// </summary>
        public static readonly byte _StrokeNumber_BeginMark = 1;

        /// <summary>
        /// 表示汉字的最大笔画数，该值为常量 48（即）0x30。
        /// </summary>
        public static readonly byte _StrokeNumber_EndMark = 48;

        /// <summary>
        /// 一个包含所有可能的汉字笔画数的数组，数组中的每个元素均表示一个汉字的笔画数。
        /// </summary>
        public static readonly byte[] ChineseStrokeNumbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 35, 36, 39, 48 };


        /// <summary>
        /// 一个包含所有汉字与拼音和笔画数映射的数组，数组中的每个元素均是一个 <see cref="ChineseCharUnit"/> 结构。
        /// <para>该数组的长度等同于 <see cref="ChineseChars"/> 属性数组的长度，两个属性数组间相同索引号位置表示相同的汉字字符。</para>
        /// </summary>
        public static readonly ChineseCharUnit[] ChineseCharUnits;

        /// <summary>
        /// 一个包含所有拼音与汉字映射的数组，数组中的每个元素同时又是一个 <see cref="char"/> 只读数组。子数组中的每个值表示对应于 <see cref="ChineseChars"/> 中对应的中文字符。
        /// <para>该数组的长度等同于 <see cref="ChinesePinYins"/> 属性数组的长度，两个属性数组间相同索引号位置表示相同的拼音字符串。</para>
        /// </summary>
        public static readonly ReadOnlyCollection<char>[] PinYinCharDictionary;

        /// <summary>
        /// 一个包含所有笔画数与汉字映射的数组，数组中的每个元素同时又是一个 <see cref="char"/> 只读数组。子数组中的每个值表示对应于 <see cref="ChineseChars"/> 中对应的中文字符。
        /// <para>该数组的长度等同于 <see cref="ChineseStrokeNumbers"/> 属性数组的长度，两个属性数组间相同索引号位置表示相同的汉字笔画数。</para>
        /// </summary>
        public static readonly ReadOnlyCollection<char>[] StrokeNumberCharDictionary;


        /// <summary>
        /// 表示在 Unicode 编码中汉子字符的起始编码 \u3007（即 12295）。
        /// </summary>
        public static readonly ushort _BitMap_BeginMark = 0x3007; //12295

        /// <summary>
        /// 表示在 Unicode 编码中汉子字符的起始编码 \ue863（即 59491）。
        /// </summary>
        public static readonly ushort _BitMap_EndMark = 0xe863;   //59491

        /// <summary>
        /// 一个长度为 47197 的数组，该数组中索引号位置从零开始的每个 <see cref="bool"/> 值依次表示字符范围从 \u3007 - \ue863（即 12295 - 59491）中的每个字符是否为一个汉字。
        /// </summary>
        public static readonly bool[] ChineseCharsBitMap;

        #endregion


        #region 汉语拼音方案中声母、韵母和整体认读音节列表

        /// <summary>
        /// 现代汉语中文拼音中的声母表，共 23 个。
        /// </summary>
        public static readonly string[] Consonants = { "B", "P", "M", "F", "D", "T", "N", "L", "G", "K", "H", "J", "Q", "X", "ZH", "CH", "SH", "R", "Z", "C", "S", "Y", "W" };

        /// <summary>
        /// 现代汉语中文拼音中的单音韵母表，共 24 个（含单韵母 10 个、复韵母 10 个和鼻韵母 4 个）。
        /// </summary>
        public static readonly string[] SimpleVowels = { "A", "O", "E", "I", "U", "V", "AI", "EI", "UI", "AO", "OU", "IU", "IE", "VE", "ER", "AN", "EN", "IN", "UN", "VN", "ANG", "ENG", "ING", "ONG" };

        /// <summary>
        /// 现代汉语中文拼音中的复音韵母表，共 15 个（含复韵母 11 个和鼻韵母 4 个）。
        /// </summary>
        public static readonly string[] CompoundVowels = { "IA", "UA", "UO", "IAO", "IOU", "UAI", "UEI", "IAN", "UAN", "VAN", "UEN", "IANG", "UANG", "UENG", "IONG" };

        /// <summary>
        /// 现代汉语中文拼音中的零声母音节，共 34 个（其中零声母单音节 10 个，字母 'Y' 开头零声母音节 15 个，字母 'W' 开头零声母音节 9 个）。
        /// </summary>
        public static readonly string[] ZeroConsonants = { "A", "AI", "AO", "AN", "ANG", "E", "ER", "EN", "O", "OU", "YA", "YAO", "YAN", "YANG", "YE", "YI", "YIN", "YING", "YO", "YOU", "YONG", "YU", "YUE", "YUN", "YUAN", "WA", "WAI", "WAN", "WANG", "WO", "WEI", "WEN", "WENG", "WU" };

        /// <summary>
        /// 现代汉语中文拼音中的整体认读音节表，共 16 个。
        /// </summary>
        public static readonly string[] WholeSyllables = { "ZHI", "CHI", "SHI", "RI", "ZI", "CI", "SI", "YI", "WU", "YU", "YE", "YUE", "YUAN", "YIN", "YUN", "YING" };

        #endregion



        #region 汉语拼音、笔画和简繁体转换方案相关资源读取操作

        /// <summary>
        /// 尝试获取表示中文汉字信息的 <see cref="ChineseCharUnit"/> 结构对象。如果传入的字符不是一个中文字符，则会获取失败。
        /// 如果获取失败，则输出参数 <paramref name="result"/> 将会被设置为 <see cref="ChineseCharUnit.Empty"/>。
        /// </summary>
        /// <param name="c"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryGetCharUnit(char c, out ChineseCharUnit result)
        {
            int i = Array.BinarySearch(ChineseChars, c);
            if (i >= 0)
            {
                result = ChineseCharUnits[i];
                return true;
            }
            else
            {
                result = ChineseCharUnit.Empty;
                return false;
            }
        }

        /// <summary>
        /// 尝试获取指定拼音字符串的内部索引号。传入的拼音必须是汉字的全拼加声调格式（如字符 "陈" 的拼音为 "CHEN2"）；如果拼音参数无效（不是全拼或者声调部分无效），则会获取失败。
        /// 如果获取失败，则输出参数 <paramref name="result"/> 将会被设置为 -1。
        /// </summary>
        /// <param name="pinyin"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryGetPinYinIndex(string pinyin, out int result)
        {
            int i = Array.BinarySearch(ChinesePinYins, pinyin, StringComparer.OrdinalIgnoreCase);
            if (i >= 0)
            {
                result = i;
                return true;
            }
            else
            {
                result = -1;
                return false;
            }
        }

        #endregion


        #region 初始化相关资源属性的值

        static ChineseResources()
        {
            ChineseChars = GetChineseChars();
            ChinesePinYins = GetChinesePinYins();
            ChineseCharUnits = GetChineseCharUnits();
            PinYinCharDictionary = GetPinYinCharDictionary();
            StrokeNumberCharDictionary = GetStrokeNumberCharDictionary();
            ChineseCharsBitMap = GetChineseCharsBitMap();
        }

        private static char[] GetChineseChars()
        {
            char[] array;
            using (MemoryStream stream = new MemoryStream(Resources.ChineseChars))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    array = new char[reader.ReadInt32()];
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = reader.ReadChar();
                    }
                    reader.Close();
                }
            }
            return array;
        }

        private static string[] GetChinesePinYins()
        {
            List<string> list = new List<string>();
            using (MemoryStream stream = new MemoryStream(Resources.ChinesePinYins))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    int length = reader.ReadInt32();
                    for (int i = 0; i < length; i++)
                    {
                        var str = reader.ReadString();
                        list.Add(str);
                    }
                    reader.Close();
                }
            }
            return list.ToArray();
        }

        private static ChineseCharUnit[] GetChineseCharUnits()
        {
            List<ChineseCharUnit> list = new List<ChineseCharUnit>();
            using (MemoryStream stream = new MemoryStream(Resources.ChineseCharUnits))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    int length = reader.ReadInt32();
                    for (int i = 0; i < length; i++)
                    {
                        int pinyinsCount = reader.ReadByte();
                        ushort[] pinyins = new ushort[pinyinsCount];
                        for (int j = 0; j < pinyinsCount; j++)
                        {
                            pinyins[j] = reader.ReadUInt16();
                        }
                        byte strokeNumber = reader.ReadByte();
                        char simplified = reader.ReadChar();
                        char traditional = reader.ReadChar();
                        ChineseCharUnit ch = new ChineseCharUnit(pinyins, strokeNumber, simplified, traditional);
                        list.Add(ch);
                    }
                    reader.Close();
                }
            }
            return list.ToArray();
        }

        private static ReadOnlyCollection<char>[] GetPinYinCharDictionary()
        {
            char[][] array;
            using (MemoryStream stream = new MemoryStream(Resources.PinYinCharDictionary))
            {
                using (var reader = new BinaryReader(stream))
                {
                    array = new char[reader.ReadUInt16()][];
                    for (int i = 0; i < array.Length; i++)
                    {
                        char[] chars = new char[reader.ReadUInt16()];
                        for (int j = 0; j < chars.Length; j++)
                        {
                            chars[j] = reader.ReadChar();
                        }
                        array[i] = chars;
                    }
                    reader.Close();
                }
            }
            return array.Select(items => new ReadOnlyCollection<char>(items.ToList())).ToArray();
        }

        private static ReadOnlyCollection<char>[] GetStrokeNumberCharDictionary()
        {
            char[][] array;
            using (MemoryStream stream = new MemoryStream(Resources.StrokeNumberCharDictionary))
            {
                using (var reader = new BinaryReader(stream))
                {
                    array = new char[reader.ReadByte()][];
                    for (int i = 0; i < array.Length; i++)
                    {
                        char[] chars = new char[reader.ReadUInt16()];
                        for (int j = 0; j < chars.Length; j++)
                        {
                            chars[j] = reader.ReadChar();
                        }
                        array[i] = chars;
                    }
                    reader.Close();
                }
            }
            return array.Select(items => new ReadOnlyCollection<char>(items.ToList())).ToArray();
        }

        private static bool[] GetChineseCharsBitMap()
        {
            bool[] array;
            using (MemoryStream stream = new MemoryStream(Resources.ChineseCharsBitMap))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    array = new bool[reader.ReadInt32()];
                    for (int i = 0; i < array.Length; i++)
                    {
                        array[i] = reader.ReadBoolean();
                    }
                }
            }
            return array;
        }

        #endregion

    }
}
