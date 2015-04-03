using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Segment
{
    /// <summary>
    /// 表示中文单词的词性。
    /// <para>
    /// 注：现代汉语的词可以分为 12 类。实词：名词、动词、形容词、数词、量词和代词。虚词：副词、介词、连词、助词、叹词、拟声词。
    /// </para>
    /// </summary>
    [Flags]
    public enum PartOfSpeech
    {
        /// <summary>
        /// 未知词性。
        /// </summary>
        [Description("未知词性")]
        Unknow = 0


    }
}
