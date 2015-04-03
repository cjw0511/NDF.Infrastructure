using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Security.Cryptography
{
    /// <summary>
    /// 提供一组基于 对称算法（SymmetricAlgorithm） 进行数据加密操作的工具 API。
    /// </summary>
    public static class SymmetricAlgorithms
    {
        /// <summary>
        /// 获取或设置一个字符，用于对称加密算法中当 Key 或 IV 长度不足时的填充字符。
        /// </summary>
        public static Char FillChar = '_';



        #region 提供一组用于快速创建 对称加密算法服务 对象的静态方法

        public static SymmetricAlgorithm Create(SymmetricAlgorithmType algorithmType)
        {
            CryptoAlgorithmAttribute attr = algorithmType.GetCustomeAttributes<CryptoAlgorithmAttribute>().FirstOrDefault();
            Check.NotNull(attr);
            return Create(attr.ConfigName);
        }

        public static SymmetricAlgorithm Create(string symmetricName)
        {
            Check.NotEmpty(symmetricName);
            return CryptoConfig.CreateFromName(symmetricName) as SymmetricAlgorithm;
        }

        public static SymmetricAlgorithm Create(SymmetricAlgorithmType algorithmType, byte[] key, byte[] iv)
        {
            CryptoAlgorithmAttribute attr = algorithmType.GetCustomeAttributes<CryptoAlgorithmAttribute>().FirstOrDefault();
            Check.NotNull(attr);
            return Create(attr.ConfigName, key, iv);
        }

        public static SymmetricAlgorithm Create(string symmetricName, byte[] key, byte[] iv)
        {
            Check.NotEmpty(symmetricName);
            Check.NotEmpty(key);
            Check.NotEmpty(iv);
            SymmetricAlgorithm algorithm = CryptoConfig.CreateFromName(symmetricName) as SymmetricAlgorithm;
            algorithm.Key = key;
            algorithm.IV = iv;
            return algorithm;
        }

        public static SymmetricAlgorithm Create(SymmetricAlgorithmType algorithmType, string key, string iv)
        {
            CryptoAlgorithmAttribute attr = algorithmType.GetCustomeAttributes<CryptoAlgorithmAttribute>().FirstOrDefault();
            Check.NotNull(attr);
            return Create(attr.ConfigName, key, iv);
        }

        public static SymmetricAlgorithm Create(string symmetricName, string key, string iv)
        {
            Check.NotEmpty(symmetricName);
            Check.NotEmpty(key);
            Check.NotEmpty(iv);
            SymmetricAlgorithm algorithm = CryptoConfig.CreateFromName(symmetricName) as SymmetricAlgorithm;
            algorithm.Key = StringToBytes(key);
            algorithm.IV = StringToBytes(iv);
            return algorithm;
        }

        #endregion



        #region 提供一组用于校验 对称加密算法服务 密文 和 明文 是否匹配的静态校验方法

        public static bool CompareCrypto(string plaintext, string ciphertext, SymmetricAlgorithmType algorithmType, byte[] key, byte[] iv, char? fillChar = null)
        {
            return InternalCompareCrypto(plaintext, ciphertext, algorithmType, key, iv, fillChar);
        }

        public static bool CompareCrypto(string plaintext, string ciphertext, string symmetricName, byte[] key, byte[] iv, char? fillChar = null)
        {
            return InternalCompareCrypto(plaintext, ciphertext, symmetricName, key, iv, fillChar);
        }

        public static bool CompareCrypto(string plaintext, string ciphertext, SymmetricAlgorithmType algorithmType, string key, string iv, char? fillChar = null)
        {
            return InternalCompareCrypto(plaintext, ciphertext, algorithmType, StringToBytes(key), StringToBytes(iv), fillChar);
        }

        public static bool CompareCrypto(string plaintext, string ciphertext, string symmetricName, string key, string iv, char? fillChar = null)
        {
            return InternalCompareCrypto(plaintext, ciphertext, symmetricName, StringToBytes(key), StringToBytes(iv), fillChar);
        }


        private static bool InternalCompareCrypto(string plaintext, string ciphertext, SymmetricAlgorithmType algorithmType, byte[] key, byte[] iv, char? fillChar = null)
        {
            if (plaintext == null || ciphertext == null)
                return false;

            string algtext = InternalEncrypt(plaintext, algorithmType, key, iv, fillChar);
            return algtext.Equals(ciphertext);
        }

        private static bool InternalCompareCrypto(string plaintext, string ciphertext, string symmetricName, byte[] key, byte[] iv, char? fillChar = null)
        {
            if (plaintext == null || ciphertext == null)
                return false;

            string algtext = InternalEncrypt(plaintext, symmetricName, key, iv, fillChar);
            return algtext.Equals(ciphertext);
        }

        #endregion



        #region 基于 高级加密标准算法（AES，建议使用） 的数据加密和校验操作

        /// <summary>
        /// 基于 高级加密标准算法（AES，建议使用） 并用指定的密钥和常量进行数据加密运算并返回加密后的密文。
        /// </summary>
        /// <param name="plaintext">待加密的明文内容。</param>
        /// <param name="key">加密密钥。</param>
        /// <param name="iv">加密常量。</param>
        /// <param name="fillChar">一个用于对称加密算法中当 Key 或 IV 长度不足时的填充字符</param>
        /// <returns>返回加密后的密文。</returns>
        public static string EncryptAES(string plaintext, byte[] key, byte[] iv, char? fillChar = null)
        {
            return Encrypt(SymmetricAlgorithmType.AES, plaintext, key, iv, fillChar);
        }

        /// <summary>
        /// 基于 高级加密标准算法（AES，建议使用） 并用指定的密钥和常量进行数据加密运算并返回加密后的密文。
        /// </summary>
        /// <param name="plaintext">待加密的明文内容。</param>
        /// <param name="key">加密密钥。</param>
        /// <param name="iv">加密常量。</param>
        /// <param name="fillChar">一个用于对称加密算法中当 Key 或 IV 长度不足时的填充字符</param>
        /// <returns>返回加密后的密文。</returns>
        public static string EncryptAES(string plaintext, string key, string iv, char? fillChar = null)
        {
            return Encrypt(SymmetricAlgorithmType.AES, plaintext, key, iv, fillChar);
        }

        /// <summary>
        /// 基于 高级加密标准算法（AES，建议使用） 并用指定的密钥和常量进行数据解密运算并返回解密后的明文。
        /// </summary>
        /// <param name="cipherText">待解密的密文内容。</param>
        /// <param name="key">加密密钥。</param>
        /// <param name="iv">加密常量。</param>
        /// <param name="fillChar">一个用于对称加密算法中当 Key 或 IV 长度不足时的填充字符</param>
        /// <returns>返回解密后的明文。</returns>
        public static string DecryptAES(string cipherText, byte[] key, byte[] iv, char? fillChar = null)
        {
            return Decrypt(SymmetricAlgorithmType.AES, cipherText, key, iv, fillChar);
        }

        /// <summary>
        /// 基于 高级加密标准算法（AES，建议使用） 并用指定的密钥和常量进行数据解密运算并返回解密后的明文。
        /// </summary>
        /// <param name="cipherText">待解密的密文内容。</param>
        /// <param name="key">加密密钥。</param>
        /// <param name="iv">加密常量。</param>
        /// <param name="fillChar">一个用于对称加密算法中当 Key 或 IV 长度不足时的填充字符</param>
        /// <returns>返回解密后的明文。</returns>
        public static string DecryptAES(string cipherText, string key, string iv, char? fillChar = null)
        {
            return Decrypt(SymmetricAlgorithmType.AES, cipherText, key, iv, fillChar);
        }


        /// <summary>
        /// 基于 高级加密标准算法（AES，建议使用） 并用指定的密钥和常量校验指定的明文与密文是否匹配。
        /// </summary>
        /// <param name="plaintext">待比较的明文内容。</param>
        /// <param name="cipherText">待比较的密文内容。</param>
        /// <param name="key">加密密钥。</param>
        /// <param name="iv">加密常量。</param>
        /// <param name="fillChar">一个用于对称加密算法中当 Key 或 IV 长度不足时的填充字符</param>
        /// <returns>如果自定的明文经过加密后得到的结果与密文内容相同，则返回 true；否则返回 false。</returns>
        public static bool CompareAES(string plaintext, string cipherText, byte[] key, byte[] iv, char? fillChar = null)
        {
            return CompareCrypto(plaintext, cipherText, SymmetricAlgorithmType.AES, key, iv, fillChar);
        }

        /// <summary>
        /// 基于 高级加密标准算法（AES，建议使用） 并用指定的密钥和常量校验指定的明文与密文是否匹配。
        /// </summary>
        /// <param name="plaintext">待比较的明文内容。</param>
        /// <param name="cipherText">待比较的密文内容。</param>
        /// <param name="key">加密密钥。</param>
        /// <param name="iv">加密常量。</param>
        /// <param name="fillChar">一个用于对称加密算法中当 Key 或 IV 长度不足时的填充字符</param>
        /// <returns>如果自定的明文经过加密后得到的结果与密文内容相同，则返回 true；否则返回 false。</returns>
        public static bool CompareAES(string plaintext, string cipherText, string key, string iv, char? fillChar = null)
        {
            return CompareCrypto(plaintext, cipherText, SymmetricAlgorithmType.AES, key, iv, fillChar);
        }

        #endregion


        #region 基于 标准数据加密算法（DES） 的数据加密和校验操作

        /// <summary>
        /// 基于 标准数据加密算法（DES） 并用指定的密钥和常量进行数据加密运算并返回加密后的密文。
        /// </summary>
        /// <param name="plaintext">待加密的明文内容。</param>
        /// <param name="key">加密密钥。</param>
        /// <param name="iv">加密常量。</param>
        /// <param name="fillChar">一个用于对称加密算法中当 Key 或 IV 长度不足时的填充字符</param>
        /// <returns>返回加密后的密文。</returns>
        public static string EncryptDES(string plaintext, byte[] key, byte[] iv, char? fillChar = null)
        {
            return Encrypt(SymmetricAlgorithmType.DES, plaintext, key, iv, fillChar);
        }

        /// <summary>
        /// 基于 标准数据加密算法（DES） 并用指定的密钥和常量进行数据加密运算并返回加密后的密文。
        /// </summary>
        /// <param name="plaintext">待加密的明文内容。</param>
        /// <param name="key">加密密钥。</param>
        /// <param name="iv">加密常量。</param>
        /// <param name="fillChar">一个用于对称加密算法中当 Key 或 IV 长度不足时的填充字符</param>
        /// <returns>返回加密后的密文。</returns>
        public static string EncryptDES(string plaintext, string key, string iv, char? fillChar = null)
        {
            return Encrypt(SymmetricAlgorithmType.DES, plaintext, key, iv, fillChar);
        }

        /// <summary>
        /// 基于 标准数据加密算法（DES） 并用指定的密钥和常量进行数据解密运算并返回解密后的明文。
        /// </summary>
        /// <param name="cipherText">待解密的密文内容。</param>
        /// <param name="key">加密密钥。</param>
        /// <param name="iv">加密常量。</param>
        /// <param name="fillChar">一个用于对称加密算法中当 Key 或 IV 长度不足时的填充字符</param>
        /// <returns>返回解密后的明文。</returns>
        public static string DecryptDES(string cipherText, byte[] key, byte[] iv, char? fillChar = null)
        {
            return Decrypt(SymmetricAlgorithmType.DES, cipherText, key, iv, fillChar);
        }

        /// <summary>
        /// 基于 标准数据加密算法（DES） 并用指定的密钥和常量进行数据解密运算并返回解密后的明文。
        /// </summary>
        /// <param name="cipherText">待解密的密文内容。</param>
        /// <param name="key">加密密钥。</param>
        /// <param name="iv">加密常量。</param>
        /// <param name="fillChar">一个用于对称加密算法中当 Key 或 IV 长度不足时的填充字符</param>
        /// <returns>返回解密后的明文。</returns>
        public static string DecryptDES(string cipherText, string key, string iv, char? fillChar = null)
        {
            return Decrypt(SymmetricAlgorithmType.DES, cipherText, key, iv, fillChar);
        }


        /// <summary>
        /// 基于 标准数据加密算法（DES） 并用指定的密钥和常量校验指定的明文与密文是否匹配。
        /// </summary>
        /// <param name="plaintext">待比较的明文内容。</param>
        /// <param name="cipherText">待比较的密文内容。</param>
        /// <param name="key">加密密钥。</param>
        /// <param name="iv">加密常量。</param>
        /// <param name="fillChar">一个用于对称加密算法中当 Key 或 IV 长度不足时的填充字符</param>
        /// <returns>如果自定的明文经过加密后得到的结果与密文内容相同，则返回 true；否则返回 false。</returns>
        public static bool CompareDES(string plaintext, string cipherText, byte[] key, byte[] iv, char? fillChar = null)
        {
            return CompareCrypto(plaintext, cipherText, SymmetricAlgorithmType.DES, key, iv, fillChar);
        }

        /// <summary>
        /// 基于 标准数据加密算法（DES） 并用指定的密钥和常量校验指定的明文与密文是否匹配。
        /// </summary>
        /// <param name="plaintext">待比较的明文内容。</param>
        /// <param name="cipherText">待比较的密文内容。</param>
        /// <param name="key">加密密钥。</param>
        /// <param name="iv">加密常量。</param>
        /// <param name="fillChar">一个用于对称加密算法中当 Key 或 IV 长度不足时的填充字符</param>
        /// <returns>如果自定的明文经过加密后得到的结果与密文内容相同，则返回 true；否则返回 false。</returns>
        public static bool CompareDES(string plaintext, string cipherText, string key, string iv, char? fillChar = null)
        {
            return CompareCrypto(plaintext, cipherText, SymmetricAlgorithmType.DES, key, iv, fillChar);
        }

        #endregion


        #region 基于 分组对称加密算法（RC2） 的数据加密和校验操作

        /// <summary>
        /// 基于 分组对称加密算法（RC2） 并用指定的密钥和常量进行数据加密运算并返回加密后的密文。
        /// </summary>
        /// <param name="plaintext">待加密的明文内容。</param>
        /// <param name="key">加密密钥。</param>
        /// <param name="iv">加密常量。</param>
        /// <param name="fillChar">一个用于对称加密算法中当 Key 或 IV 长度不足时的填充字符</param>
        /// <returns>返回加密后的密文。</returns>
        public static string EncryptRC2(string plaintext, byte[] key, byte[] iv, char? fillChar = null)
        {
            return Encrypt(SymmetricAlgorithmType.RC2, plaintext, key, iv, fillChar);
        }

        /// <summary>
        /// 基于 分组对称加密算法（RC2） 并用指定的密钥和常量进行数据加密运算并返回加密后的密文。
        /// </summary>
        /// <param name="plaintext">待加密的明文内容。</param>
        /// <param name="key">加密密钥。</param>
        /// <param name="iv">加密常量。</param>
        /// <param name="fillChar">一个用于对称加密算法中当 Key 或 IV 长度不足时的填充字符</param>
        /// <returns>返回加密后的密文。</returns>
        public static string EncryptRC2(string plaintext, string key, string iv, char? fillChar = null)
        {
            return Encrypt(SymmetricAlgorithmType.RC2, plaintext, key, iv, fillChar);
        }

        /// <summary>
        /// 基于 分组对称加密算法（RC2） 并用指定的密钥和常量进行数据解密运算并返回解密后的明文。
        /// </summary>
        /// <param name="cipherText">待解密的密文内容。</param>
        /// <param name="key">加密密钥。</param>
        /// <param name="iv">加密常量。</param>
        /// <param name="fillChar">一个用于对称加密算法中当 Key 或 IV 长度不足时的填充字符</param>
        /// <returns>返回解密后的明文。</returns>
        public static string DecryptRC2(string cipherText, byte[] key, byte[] iv, char? fillChar = null)
        {
            return Decrypt(SymmetricAlgorithmType.RC2, cipherText, key, iv, fillChar);
        }

        /// <summary>
        /// 基于 分组对称加密算法（RC2） 并用指定的密钥和常量进行数据解密运算并返回解密后的明文。
        /// </summary>
        /// <param name="cipherText">待解密的密文内容。</param>
        /// <param name="key">加密密钥。</param>
        /// <param name="iv">加密常量。</param>
        /// <param name="fillChar">一个用于对称加密算法中当 Key 或 IV 长度不足时的填充字符</param>
        /// <returns>返回解密后的明文。</returns>
        public static string DecryptRC2(string cipherText, string key, string iv, char? fillChar = null)
        {
            return Decrypt(SymmetricAlgorithmType.RC2, cipherText, key, iv, fillChar);
        }


        /// <summary>
        /// 基于 分组对称加密算法（RC2） 并用指定的密钥和常量校验指定的明文与密文是否匹配。
        /// </summary>
        /// <param name="plaintext">待比较的明文内容。</param>
        /// <param name="cipherText">待比较的密文内容。</param>
        /// <param name="key">加密密钥。</param>
        /// <param name="iv">加密常量。</param>
        /// <param name="fillChar">一个用于对称加密算法中当 Key 或 IV 长度不足时的填充字符</param>
        /// <returns>如果自定的明文经过加密后得到的结果与密文内容相同，则返回 true；否则返回 false。</returns>
        public static bool CompareRC2(string plaintext, string cipherText, byte[] key, byte[] iv, char? fillChar = null)
        {
            return CompareCrypto(plaintext, cipherText, SymmetricAlgorithmType.RC2, key, iv, fillChar);
        }

        /// <summary>
        /// 基于 分组对称加密算法（RC2） 并用指定的密钥和常量校验指定的明文与密文是否匹配。
        /// </summary>
        /// <param name="plaintext">待比较的明文内容。</param>
        /// <param name="cipherText">待比较的密文内容。</param>
        /// <param name="key">加密密钥。</param>
        /// <param name="iv">加密常量。</param>
        /// <param name="fillChar">一个用于对称加密算法中当 Key 或 IV 长度不足时的填充字符</param>
        /// <returns>如果自定的明文经过加密后得到的结果与密文内容相同，则返回 true；否则返回 false。</returns>
        public static bool CompareRC2(string plaintext, string cipherText, string key, string iv, char? fillChar = null)
        {
            return CompareCrypto(plaintext, cipherText, SymmetricAlgorithmType.RC2, key, iv, fillChar);
        }

        #endregion


        #region 基于 高级加密标准基本算法（Rijndael） 的数据加密和校验操作

        /// <summary>
        /// 基于 高级加密标准基本算法（Rijndael） 并用指定的密钥和常量进行数据加密运算并返回加密后的密文。
        /// </summary>
        /// <param name="plaintext">待加密的明文内容。</param>
        /// <param name="key">加密密钥。</param>
        /// <param name="iv">加密常量。</param>
        /// <param name="fillChar">一个用于对称加密算法中当 Key 或 IV 长度不足时的填充字符</param>
        /// <returns>返回加密后的密文。</returns>
        public static string EncryptRijndael(string plaintext, byte[] key, byte[] iv, char? fillChar = null)
        {
            return Encrypt(SymmetricAlgorithmType.Rijndael, plaintext, key, iv, fillChar);
        }

        /// <summary>
        /// 基于 高级加密标准基本算法（Rijndael） 并用指定的密钥和常量进行数据加密运算并返回加密后的密文。
        /// </summary>
        /// <param name="plaintext">待加密的明文内容。</param>
        /// <param name="key">加密密钥。</param>
        /// <param name="iv">加密常量。</param>
        /// <param name="fillChar">一个用于对称加密算法中当 Key 或 IV 长度不足时的填充字符</param>
        /// <returns>返回加密后的密文。</returns>
        public static string EncryptRijndael(string plaintext, string key, string iv, char? fillChar = null)
        {
            return Encrypt(SymmetricAlgorithmType.Rijndael, plaintext, key, iv, fillChar);
        }

        /// <summary>
        /// 基于 高级加密标准基本算法（Rijndael） 并用指定的密钥和常量进行数据解密运算并返回解密后的明文。
        /// </summary>
        /// <param name="cipherText">待解密的密文内容。</param>
        /// <param name="key">加密密钥。</param>
        /// <param name="iv">加密常量。</param>
        /// <param name="fillChar">一个用于对称加密算法中当 Key 或 IV 长度不足时的填充字符</param>
        /// <returns>返回解密后的明文。</returns>
        public static string DecryptRijndael(string cipherText, byte[] key, byte[] iv, char? fillChar = null)
        {
            return Decrypt(SymmetricAlgorithmType.Rijndael, cipherText, key, iv, fillChar);
        }

        /// <summary>
        /// 基于 高级加密标准基本算法（Rijndael） 并用指定的密钥和常量进行数据解密运算并返回解密后的明文。
        /// </summary>
        /// <param name="cipherText">待解密的密文内容。</param>
        /// <param name="key">加密密钥。</param>
        /// <param name="iv">加密常量。</param>
        /// <param name="fillChar">一个用于对称加密算法中当 Key 或 IV 长度不足时的填充字符</param>
        /// <returns>返回解密后的明文。</returns>
        public static string DecryptRijndael(string cipherText, string key, string iv, char? fillChar = null)
        {
            return Decrypt(SymmetricAlgorithmType.Rijndael, cipherText, key, iv, fillChar);
        }


        /// <summary>
        /// 基于 高级加密标准基本算法（Rijndael） 并用指定的密钥和常量校验指定的明文与密文是否匹配。
        /// </summary>
        /// <param name="plaintext">待比较的明文内容。</param>
        /// <param name="cipherText">待比较的密文内容。</param>
        /// <param name="key">加密密钥。</param>
        /// <param name="iv">加密常量。</param>
        /// <param name="fillChar">一个用于对称加密算法中当 Key 或 IV 长度不足时的填充字符</param>
        /// <returns>如果自定的明文经过加密后得到的结果与密文内容相同，则返回 true；否则返回 false。</returns>
        public static bool CompareRijndael(string plaintext, string cipherText, byte[] key, byte[] iv, char? fillChar = null)
        {
            return CompareCrypto(plaintext, cipherText, SymmetricAlgorithmType.Rijndael, key, iv, fillChar);
        }

        /// <summary>
        /// 基于 高级加密标准基本算法（Rijndael） 并用指定的密钥和常量校验指定的明文与密文是否匹配。
        /// </summary>
        /// <param name="plaintext">待比较的明文内容。</param>
        /// <param name="cipherText">待比较的密文内容。</param>
        /// <param name="key">加密密钥。</param>
        /// <param name="iv">加密常量。</param>
        /// <param name="fillChar">一个用于对称加密算法中当 Key 或 IV 长度不足时的填充字符</param>
        /// <returns>如果自定的明文经过加密后得到的结果与密文内容相同，则返回 true；否则返回 false。</returns>
        public static bool CompareRijndael(string plaintext, string cipherText, string key, string iv, char? fillChar = null)
        {
            return CompareCrypto(plaintext, cipherText, SymmetricAlgorithmType.Rijndael, key, iv, fillChar);
        }

        #endregion


        #region 基于 三重数据加密标准算法（TripleDES） 的数据加密和校验操作

        /// <summary>
        /// 基于 三重数据加密标准算法（TripleDES） 并用指定的密钥和常量进行数据加密运算并返回加密后的密文。
        /// </summary>
        /// <param name="plaintext">待加密的明文内容。</param>
        /// <param name="key">加密密钥。</param>
        /// <param name="iv">加密常量。</param>
        /// <param name="fillChar">一个用于对称加密算法中当 Key 或 IV 长度不足时的填充字符</param>
        /// <returns>返回加密后的密文。</returns>
        public static string EncryptTripleDES(string plaintext, byte[] key, byte[] iv, char? fillChar = null)
        {
            return Encrypt(SymmetricAlgorithmType.TripleDES, plaintext, key, iv, fillChar);
        }

        /// <summary>
        /// 基于 三重数据加密标准算法（TripleDES） 并用指定的密钥和常量进行数据加密运算并返回加密后的密文。
        /// </summary>
        /// <param name="plaintext">待加密的明文内容。</param>
        /// <param name="key">加密密钥。</param>
        /// <param name="iv">加密常量。</param>
        /// <param name="fillChar">一个用于对称加密算法中当 Key 或 IV 长度不足时的填充字符</param>
        /// <returns>返回加密后的密文。</returns>
        public static string EncryptTripleDES(string plaintext, string key, string iv, char? fillChar = null)
        {
            return Encrypt(SymmetricAlgorithmType.TripleDES, plaintext, key, iv, fillChar);
        }

        /// <summary>
        /// 基于 三重数据加密标准算法（TripleDES） 并用指定的密钥和常量进行数据解密运算并返回解密后的明文。
        /// </summary>
        /// <param name="cipherText">待解密的密文内容。</param>
        /// <param name="key">加密密钥。</param>
        /// <param name="iv">加密常量。</param>
        /// <param name="fillChar">一个用于对称加密算法中当 Key 或 IV 长度不足时的填充字符</param>
        /// <returns>返回解密后的明文。</returns>
        public static string DecryptTripleDES(string cipherText, byte[] key, byte[] iv, char? fillChar = null)
        {
            return Decrypt(SymmetricAlgorithmType.TripleDES, cipherText, key, iv, fillChar);
        }

        /// <summary>
        /// 基于 三重数据加密标准算法（TripleDES） 并用指定的密钥和常量进行数据解密运算并返回解密后的明文。
        /// </summary>
        /// <param name="cipherText">待解密的密文内容。</param>
        /// <param name="key">加密密钥。</param>
        /// <param name="iv">加密常量。</param>
        /// <param name="fillChar">一个用于对称加密算法中当 Key 或 IV 长度不足时的填充字符</param>
        /// <returns>返回解密后的明文。</returns>
        public static string DecryptTripleDES(string cipherText, string key, string iv, char? fillChar = null)
        {
            return Decrypt(SymmetricAlgorithmType.TripleDES, cipherText, key, iv, fillChar);
        }


        /// <summary>
        /// 基于 三重数据加密标准算法（TripleDES） 并用指定的密钥和常量校验指定的明文与密文是否匹配。
        /// </summary>
        /// <param name="plaintext">待比较的明文内容。</param>
        /// <param name="cipherText">待比较的密文内容。</param>
        /// <param name="key">加密密钥。</param>
        /// <param name="iv">加密常量。</param>
        /// <param name="fillChar">一个用于对称加密算法中当 Key 或 IV 长度不足时的填充字符</param>
        /// <returns>如果自定的明文经过加密后得到的结果与密文内容相同，则返回 true；否则返回 false。</returns>
        public static bool CompareTripleDES(string plaintext, string cipherText, byte[] key, byte[] iv, char? fillChar = null)
        {
            return CompareCrypto(plaintext, cipherText, SymmetricAlgorithmType.TripleDES, key, iv, fillChar);
        }

        /// <summary>
        /// 基于 三重数据加密标准算法（TripleDES） 并用指定的密钥和常量校验指定的明文与密文是否匹配。
        /// </summary>
        /// <param name="plaintext">待比较的明文内容。</param>
        /// <param name="cipherText">待比较的密文内容。</param>
        /// <param name="key">加密密钥。</param>
        /// <param name="iv">加密常量。</param>
        /// <param name="fillChar">一个用于对称加密算法中当 Key 或 IV 长度不足时的填充字符</param>
        /// <returns>如果自定的明文经过加密后得到的结果与密文内容相同，则返回 true；否则返回 false。</returns>
        public static bool CompareTripleDES(string plaintext, string cipherText, string key, string iv, char? fillChar = null)
        {
            return CompareCrypto(plaintext, cipherText, SymmetricAlgorithmType.TripleDES, key, iv, fillChar);
        }

        #endregion



        #region 由调用方选择加密算法的数据加密操作

        public static string Encrypt(SymmetricAlgorithmType algorithmType, string plaintext, byte[] key, byte[] iv, char? fillChar = null)
        {
            return InternalEncrypt(plaintext, algorithmType, key, iv, fillChar);
        }

        public static string Encrypt(SymmetricAlgorithmType algorithmType, string plaintext, string key, string iv, char? fillChar = null)
        {
            return InternalEncrypt(plaintext, algorithmType, StringToBytes(key), StringToBytes(iv), fillChar);
        }

        public static string Encrypt(string symmetricName, string plaintext, byte[] key, byte[] iv, char? fillChar = null)
        {
            return InternalEncrypt(plaintext, symmetricName, key, iv, fillChar);
        }

        public static string Encrypt(string symmetricName, string plaintext, string key, string iv, char? fillChar = null)
        {
            return InternalEncrypt(plaintext, symmetricName, StringToBytes(key), StringToBytes(iv), fillChar);
        }


        public static string Decrypt(SymmetricAlgorithmType algorithmType, string cipherText, byte[] key, byte[] iv, char? fillChar = null)
        {
            return InternalDecrypt(cipherText, algorithmType, key, iv, fillChar);
        }

        public static string Decrypt(SymmetricAlgorithmType algorithmType, string cipherText, string key, string iv, char? fillChar = null)
        {
            return InternalDecrypt(cipherText, algorithmType, StringToBytes(key), StringToBytes(iv), fillChar);
        }

        public static string Decrypt(string symmetricName, string cipherText, byte[] key, byte[] iv, char? fillChar = null)
        {
            return InternalDecrypt(cipherText, symmetricName, key, iv, fillChar);
        }

        public static string Decrypt(string symmetricName, string cipherText, string key, string iv, char? fillChar = null)
        {
            return InternalDecrypt(cipherText, symmetricName, StringToBytes(key), StringToBytes(iv), fillChar);
        }

        #endregion




        #region Internal CodeBlock

        private static string InternalEncrypt(string plaintext, SymmetricAlgorithmType algorithmType, byte[] key, byte[] iv, char? fillChar = null)
        {
            Check.NotNull(plaintext);
            Check.NotEmpty(key);
            Check.NotEmpty(iv);
            using (SymmetricAlgorithm algorithm = Create(algorithmType))
            {
                string ret = InternalEncrypt(plaintext, algorithm, key, iv, fillChar);
                algorithm.Clear();
                return ret;
            }
        }

        private static string InternalDecrypt(string ciphertext, SymmetricAlgorithmType algorithmType, byte[] key, byte[] iv, char? fillChar = null)
        {
            Check.NotNull(ciphertext);
            Check.NotEmpty(key);
            Check.NotEmpty(iv);
            using (SymmetricAlgorithm algorithm = Create(algorithmType))
            {
                string ret = InternalDecrypt(ciphertext, algorithm, key, iv, fillChar);
                algorithm.Clear();
                return ret;
            }
        }


        private static string InternalEncrypt(string plaintext, string algorithmName, byte[] key, byte[] iv, char? fillChar = null)
        {
            Check.NotNull(plaintext);
            Check.NotEmpty(key);
            Check.NotEmpty(iv);
            using (SymmetricAlgorithm algorithm = Create(algorithmName))
            {
                string ret = InternalEncrypt(plaintext, algorithm, key, iv, fillChar);
                algorithm.Clear();
                return ret;
            }
        }

        private static string InternalDecrypt(string ciphertext, string algorithmName, byte[] key, byte[] iv, char? fillChar = null)
        {
            Check.NotNull(ciphertext);
            Check.NotEmpty(key);
            Check.NotEmpty(iv);
            using (SymmetricAlgorithm algorithm = Create(algorithmName))
            {
                string ret = InternalDecrypt(ciphertext, algorithm, key, iv, fillChar);
                algorithm.Clear();
                return ret;
            }
        }


        private static string InternalEncrypt(string plaintext, SymmetricAlgorithm algorithm, byte[] key, byte[] iv, char? fillChar = null)
        {
            Check.NotNull(algorithm);
            FillOrCutOff(ref key, algorithm.KeySize / 8, fillChar);
            FillOrCutOff(ref iv, algorithm.BlockSize / 8, fillChar);

            string ret = null;
            byte[] buffer = null;
            using (ICryptoTransform cryptor = algorithm.CreateEncryptor(key, iv))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, cryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter s = new StreamWriter(cs))
                        {
                            s.Write(plaintext);
                        }
                        buffer = ms.ToArray();
                        ret = Convert.ToBase64String(buffer);
                    }
                }
            }
            return ret;
        }

        private static string InternalDecrypt(string ciphertext, SymmetricAlgorithm algorithm, byte[] key, byte[] iv, char? fillChar = null)
        {
            Check.NotNull(algorithm);
            FillOrCutOff(ref key, algorithm.KeySize / 8, fillChar);
            FillOrCutOff(ref iv, algorithm.BlockSize / 8, fillChar);

            string ret = null;
            byte[] buffer = Convert.FromBase64String(ciphertext);
            using (ICryptoTransform cryptor = algorithm.CreateDecryptor(key, iv))
            {
                using (MemoryStream ms = new MemoryStream(buffer))
                {
                    using (CryptoStream cs = new CryptoStream(ms, cryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader s = new StreamReader(cs))
                        {
                            ret = s.ReadToEnd();
                        }
                    }
                }
            }
            return ret;
        }


        private static void FillOrCutOff(ref byte[] bytes, int length, char? fillChar = null)
        {
            if (bytes.Length > length)
                bytes = bytes.Take(length).ToArray();

            if (bytes.Length < length)
            {
                List<byte> list = new List<byte>(bytes);
                byte c = Convert.ToByte(fillChar ?? FillChar);
                while (list.Count < length)
                {
                    list.Add(c);
                }
                bytes = list.ToArray();
            }
        }


        private static byte[] StringToBytes(string text)
        {
            return System.Text.Encoding.UTF8.GetBytes(text);
        }

        #endregion

    }
}
