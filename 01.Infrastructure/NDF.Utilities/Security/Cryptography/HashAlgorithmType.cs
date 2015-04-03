using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Security.Cryptography
{
    /// <summary>
    /// 表示 哈希加密算法（HashAlgorithm） 的名称或类型。
    /// </summary>
    public enum HashAlgorithmType
    {

        /// <summary>
        /// 表示基于 MD5 的哈希加密算法
        /// </summary>
        [CryptoAlgorithm("System.Security.Cryptography.MD5", "System.Security.Cryptography.HMACMD5")]
        MD5 = 0,

        /// <summary>
        /// 表示基于 RIPEMD160 的哈希加密算法
        /// </summary>
        [CryptoAlgorithm("System.Security.Cryptography.RIPEMD160Managed", "System.Security.Cryptography.HMACRIPEMD160")]
        RIPEMD160 = 1,

        /// <summary>
        /// 表示基于 SHA1 的哈希加密算法
        /// </summary>
        [CryptoAlgorithm("System.Security.Cryptography.SHA1", "System.Security.Cryptography.HMACSHA1")]
        SHA1 = 2,

        /// <summary>
        /// 表示基于 SHA256 的哈希加密算法
        /// </summary>
        [CryptoAlgorithm("System.Security.Cryptography.SHA256", "System.Security.Cryptography.HMACSHA256")]
        SHA256 = 3,

        /// <summary>
        /// 表示基于 SHA384 的哈希加密算法
        /// </summary>
        [CryptoAlgorithm("System.Security.Cryptography.SHA384", "System.Security.Cryptography.HMACSHA384")]
        SHA384 = 4,

        /// <summary>
        /// 表示基于 SHA512 的哈希加密算法
        /// </summary>
        [CryptoAlgorithm("System.Security.Cryptography.SHA512", "System.Security.Cryptography.HMACSHA512")]
        SHA512 = 5,

        /// <summary>
        /// 表示基于 MACTripleDES 的哈希加密算法。
        /// 注意，该枚举所表示的 Hash 加密算法不能直接用于通过 HashAlgorithms 类型调用来执行 Hash 加密操作，否则会抛出异常。
        /// </summary>
        [CryptoAlgorithm("", "System.Security.Cryptography.MACTripleDES")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        MACTripleDES = 6,

    }
}
