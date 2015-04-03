using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Security.Cryptography
{
    /// <summary>
    /// 表示 对称加密算法（SymmetricAlgorithm） 的名称或类型。
    /// </summary>
    public enum SymmetricAlgorithmType
    {
        /// <summary>
        /// 表示 高级加密标准算法（AES，建议使用）
        /// </summary>
        [CryptoAlgorithm("System.Security.Cryptography.AesCryptoServiceProvider")]
        AES = 0,

        /// <summary>
        /// 表示 标准数据加密算法（DES）
        /// </summary>
        [CryptoAlgorithm("System.Security.Cryptography.DES")]
        DES = 1,

        /// <summary>
        /// 表示 分组对称加密算法（RC2）
        /// </summary>
        [CryptoAlgorithm("System.Security.Cryptography.RC2")]
        RC2 = 2,

        /// <summary>
        /// 表示 高级加密标准基本算法（Rijndael）
        /// </summary>
        [CryptoAlgorithm("System.Security.Cryptography.Rijndael")]
        Rijndael = 3,

        /// <summary>
        /// 表示 三重数据加密标准算法（TripleDES）
        /// </summary>
        [CryptoAlgorithm("System.Security.Cryptography.TripleDES")]
        TripleDES = 4,


    }
}
