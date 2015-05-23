using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Security.Cryptography
{
    /// <summary>
    /// 提供一组基于 哈希算法 进行数据加密操作的工具 API。
    /// </summary>
    public static class HashAlgorithms
    {

        #region 提供一组用于快速创建哈希加密对象的静态方法

        /// <summary>
        /// 创建枚举参数 <paramref name="algorithmType"/> 所表示的哈希算法加密对象。
        /// </summary>
        /// <param name="algorithmType">用于指示哈希算法类型的 <see cref="HashAlgorithmType"/> 枚举值。</param>
        /// <returns>返回一个枚举参数 <paramref name="algorithmType"/> 所表示的 <see cref="HashAlgorithm"/> 类型实例的哈希算法加密对象。</returns>
        public static HashAlgorithm Create(HashAlgorithmType algorithmType)
        {
            CryptoAlgorithmAttribute attr = algorithmType.GetCustomeAttributes<CryptoAlgorithmAttribute>().FirstOrDefault();
            Check.NotNull(attr);
            return Create(attr.ConfigName);
        }

        /// <summary>
        /// 创建哈希名称参数 <paramref name="hashName"/> 所表示的哈希算法加密对象。
        /// </summary>
        /// <param name="hashName">用于指示哈希算法的名称。</param>
        /// <returns>
        /// 返回一个参数 <paramref name="hashName"/> 所表示的 <see cref="HashAlgorithm"/> 类型实例的哈希算法加密对象；
        /// 如果 <see cref="CryptoConfig"/> 配置类型中未定义该名称的哈希算法加密对象，则返回 Null。
        /// </returns>
        public static HashAlgorithm Create(string hashName)
        {
            Check.NotEmpty(hashName);
            return CryptoConfig.CreateFromName(hashName) as HashAlgorithm;
        }


        /// <summary>
        /// 创建枚举参数 <paramref name="algorithmType"/> 所表示的 HAMC 哈希算法加密对象。
        /// </summary>
        /// <param name="algorithmType">用于指示 HAMC 哈希算法类型的 <see cref="HashAlgorithmType"/> 枚举值。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>
        /// 返回一个枚举参数 <paramref name="algorithmType"/> 所表示的 <see cref="HashAlgorithm"/> 类型实例的 HAMC 哈希算法加密对象。
        /// 返回对象的 Key 属性值将会被设置为参数 <paramref name="key"/> 所表示的值。
        /// </returns>
        public static HashAlgorithm Create(HashAlgorithmType algorithmType, byte[] key)
        {
            CryptoAlgorithmAttribute attr = algorithmType.GetCustomeAttributes<CryptoAlgorithmAttribute>().FirstOrDefault();
            Check.NotNull(attr);
            return Create(attr.MappingName, key);
        }

        /// <summary>
        /// 创建哈希名称参数 <paramref name="hamcHashName"/> 所表示的 HAMC 哈希算法加密对象。
        /// </summary>
        /// <param name="hamcHashName">用于指示 HAMC 哈希算法的名称。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>
        /// 返回一个哈希名称参数 <paramref name="hamcHashName"/> 所表示的 <see cref="HashAlgorithm"/> 类型实例的 HAMC 哈希算法加密对象。
        /// 返回对象的 Key 属性值将会被设置为参数 <paramref name="key"/> 所表示的值。
        /// </returns>
        public static HashAlgorithm Create(string hamcHashName, byte[] key)
        {
            Check.NotEmpty(hamcHashName);
            Check.NotEmpty(key);
            HashAlgorithm hash = CryptoConfig.CreateFromName(hamcHashName) as HashAlgorithm;
            if (hash == null)
                throw new InvalidOperationException("没有找到名称为 {0} 的哈希加密算法实现。".Format(hamcHashName as object));

            if (!(hash is KeyedHashAlgorithm))
                throw new InvalidOperationException("找到的指定名称 {0} 所示的哈希算法对象 {1} 没有实现 HAMC，且不存在基于哈希密钥计算的规则。".Format(hamcHashName as object, hash.GetType()));

            ((KeyedHashAlgorithm)hash).Key = key;
            return hash;
        }


        /// <summary>
        /// 创建枚举参数 <paramref name="algorithmType"/> 所表示的 HAMC 哈希算法加密对象。
        /// </summary>
        /// <param name="algorithmType">用于指示 HAMC 哈希算法类型的 <see cref="HashAlgorithmType"/> 枚举值。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>
        /// 返回一个枚举参数 <paramref name="algorithmType"/> 所表示的 <see cref="HashAlgorithm"/> 类型实例的 HAMC 哈希算法加密对象。
        /// 返回对象的 Key 属性值将会被设置为参数 <paramref name="key"/> 所表示的值。
        /// </returns>
        public static HashAlgorithm Create(HashAlgorithmType algorithmType, string key)
        {
            Check.NotNull(key);
            return Create(algorithmType, StringToBytes(key));
        }

        /// <summary>
        /// 创建哈希名称参数 <paramref name="hamcHashName"/> 所表示的 HAMC 哈希算法加密对象。
        /// </summary>
        /// <param name="hamcHashName">用于指示 HAMC 哈希算法的名称。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>
        /// 返回一个哈希名称参数 <paramref name="hamcHashName"/> 所表示的 <see cref="HashAlgorithm"/> 类型实例的 HAMC 哈希算法加密对象。
        /// 返回对象的 Key 属性值将会被设置为参数 <paramref name="key"/> 所表示的值。
        /// </returns>
        public static HashAlgorithm Create(string hamcHashName, string key)
        {
            Check.NotNull(key);
            return Create(hamcHashName, StringToBytes(key));
        }

        #endregion



        #region 提供一组用于校验哈希算法加密结果和明文是否匹配的静态方法

        /// <summary>
        /// 以指定的 <paramref name="algorithmType"/> 枚举值所表示的哈希加密算法校验明文和密文是否匹配。
        /// </summary>
        /// <param name="plaintext">待校验的明文内容。</param>
        /// <param name="ciphertext">待校验的密文内容，执行校验时将去除其前后两端的空格。</param>
        /// <param name="algorithmType">用于指示哈希算法类型的 <paramref name="algorithmType"/> 枚举值。</param>
        /// <returns>
        /// 如果 明文字符串 <paramref name="plaintext"/> 参数和 密文字符串 <paramref name="ciphertext"/> 参数都不为 Null，且
        /// 明文字符串 <paramref name="plaintext"/> 经过 <paramref name="algorithmType"/> 枚举值所表示的哈希算法加密运算后得到的内容
        /// 与密文字符串 <paramref name="ciphertext"/> 的内容匹配（执行不区分大小写的 Equals 校验），则返回 true；否则返回 false。
        /// </returns>
        public static bool CompareHash(string plaintext, string ciphertext, HashAlgorithmType algorithmType)
        {
            return InternalCompareHash(plaintext, ciphertext, algorithmType, null);
        }

        /// <summary>
        /// 以指定名称所表示的哈希加密算法校验明文和密文是否匹配。
        /// </summary>
        /// <param name="plaintext">待校验的明文内容。</param>
        /// <param name="ciphertext">待校验的密文内容，执行校验时将去除其前后两端的空格。</param>
        /// <param name="hashName">用于指示 HAMC 哈希算法的名称。</param>
        /// <returns>
        /// 如果 明文字符串 <paramref name="plaintext"/> 参数和 密文字符串 <paramref name="ciphertext"/> 参数都不为 Null，且
        /// 明文字符串 <paramref name="plaintext"/> 经过 <paramref name="hashName"/> 名称所表示的哈希算法加密运算后得到的内容
        /// 与密文字符串 <paramref name="ciphertext"/> 的内容匹配（执行不区分大小写的 Equals 校验），则返回 true；否则返回 false。
        /// </returns>
        public static bool CompareHash(string plaintext, string ciphertext, string hashName)
        {
            return InternalCompareHash(plaintext, ciphertext, hashName, null);
        }


        /// <summary>
        /// 以指定的 <paramref name="algorithmType"/> 枚举值和密钥所表示的 HAMC 哈希加密算法校验明文和密文是否匹配。
        /// </summary>
        /// <param name="plaintext">待校验的明文内容。</param>
        /// <param name="ciphertext">待校验的密文内容，执行校验时将去除其前后两端的空格。</param>
        /// <param name="algorithmType">用于指示哈希算法类型的 <paramref name="algorithmType"/> 枚举值。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>
        /// 如果 明文字符串 <paramref name="plaintext"/> 参数和 密文字符串 <paramref name="ciphertext"/> 参数都不为 Null，且
        /// 明文字符串 <paramref name="plaintext"/> 经过 <paramref name="algorithmType"/> 枚举值密钥 <paramref name="key"/> 所表示的哈希算法加密运算后得到的内容
        /// 与密文字符串 <paramref name="ciphertext"/> 的内容匹配（执行不区分大小写的 Equals 校验），则返回 true；否则返回 false。
        /// </returns>
        public static bool CompareHash(string plaintext, string ciphertext, HashAlgorithmType algorithmType, byte[] key)
        {
            return InternalCompareHash(plaintext, ciphertext, algorithmType, key);
        }

        /// <summary>
        /// 以指定的名称和密钥所表示的 HAMC 哈希加密算法校验明文和密文是否匹配。
        /// </summary>
        /// <param name="plaintext">待校验的明文内容。</param>
        /// <param name="ciphertext">待校验的密文内容，执行校验时将去除其前后两端的空格。</param>
        /// <param name="hashName">用于指示 HAMC 哈希算法的名称。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>
        /// 如果 明文字符串 <paramref name="plaintext"/> 参数和 密文字符串 <paramref name="ciphertext"/> 参数都不为 Null，且
        /// 明文字符串 <paramref name="plaintext"/> 经过 <paramref name="hashName"/> 名称和密钥 <paramref name="key"/> 所表示的哈希算法加密运算后得到的内容
        /// 与密文字符串 <paramref name="ciphertext"/> 的内容匹配（执行不区分大小写的 Equals 校验），则返回 true；否则返回 false。
        /// </returns>
        public static bool CompareHash(string plaintext, string ciphertext, string hashName, byte[] key)
        {
            return InternalCompareHash(plaintext, ciphertext, hashName, key);
        }


        /// <summary>
        /// 以指定的 <paramref name="algorithmType"/> 枚举值和密钥所表示的 HAMC 哈希加密算法校验明文和密文是否匹配。
        /// </summary>
        /// <param name="plaintext">待校验的明文内容。</param>
        /// <param name="ciphertext">待校验的密文内容，执行校验时将去除其前后两端的空格。</param>
        /// <param name="algorithmType">用于指示哈希算法类型的 <paramref name="algorithmType"/> 枚举值。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>
        /// 如果 明文字符串 <paramref name="plaintext"/> 参数和 密文字符串 <paramref name="ciphertext"/> 参数都不为 Null，且
        /// 明文字符串 <paramref name="plaintext"/> 经过 <paramref name="algorithmType"/> 枚举值密钥 <paramref name="key"/> 所表示的哈希算法加密运算后得到的内容
        /// 与密文字符串 <paramref name="ciphertext"/> 的内容匹配（执行不区分大小写的 Equals 校验），则返回 true；否则返回 false。
        /// </returns>
        public static bool CompareHash(string plaintext, string ciphertext, HashAlgorithmType algorithmType, string key)
        {
            Check.NotNull(key);
            return InternalCompareHash(plaintext, ciphertext, algorithmType, StringToBytes(key));
        }

        /// <summary>
        /// 以指定的名称和密钥所表示的 HAMC 哈希加密算法校验明文和密文是否匹配。
        /// </summary>
        /// <param name="plaintext">待校验的明文内容。</param>
        /// <param name="ciphertext">待校验的密文内容，执行校验时将去除其前后两端的空格。</param>
        /// <param name="hashName">用于指示 HAMC 哈希算法的名称。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>
        /// 如果 明文字符串 <paramref name="plaintext"/> 参数和 密文字符串 <paramref name="ciphertext"/> 参数都不为 Null，且
        /// 明文字符串 <paramref name="plaintext"/> 经过 <paramref name="hashName"/> 名称和密钥 <paramref name="key"/> 所表示的哈希算法加密运算后得到的内容
        /// 与密文字符串 <paramref name="ciphertext"/> 的内容匹配（执行不区分大小写的 Equals 校验），则返回 true；否则返回 false。
        /// </returns>
        public static bool CompareHash(string plaintext, string ciphertext, string hashName, string key)
        {
            Check.NotNull(key);
            return InternalCompareHash(plaintext, ciphertext, hashName, StringToBytes(key));
        }


        private static bool InternalCompareHash(string plaintext, string ciphertext, HashAlgorithmType algorithmType, byte[] key)
        {
            if (plaintext == null || ciphertext == null)
                return false;

            byte[] hash = InternalComputeHash(algorithmType, StringToBytes(plaintext), key);
            string hashText = BytesToString(hash);
            return hashText.Equals(ciphertext.Replace("-", "").Trim(), StringComparison.OrdinalIgnoreCase);
        }

        private static bool InternalCompareHash(string plaintext, string ciphertext, string hashName, byte[] key)
        {
            if (plaintext == null || ciphertext == null)
                return false;

            byte[] hash = InternalComputeHash(hashName, StringToBytes(plaintext), key);
            string hashText = BytesToString(hash);
            return hashText.Equals(ciphertext.Replace("-", "").Trim(), StringComparison.OrdinalIgnoreCase);
        }

        #endregion



        #region 基于 MD5 16 位哈希算法的数据加密和校验操作

        /// <summary>
        /// 基于 MD5 16 位哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="buffer">要计算其哈希代码的输入。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] MD516(byte[] buffer)
        {
            return MD5(buffer).GetRange(4, 8).ToArray();
        }

        /// <summary>
        /// 基于 MD5 16 位哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="inputStream">要计算其哈希代码的输入。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] MD516(Stream inputStream)
        {
            return MD5(inputStream).GetRange(4, 8).ToArray();
        }

        /// <summary>
        /// 基于 MD5 16 位哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="buffer">要计算其哈希代码的输入。</param>
        /// <param name="offset">字节数组中的偏移量，从该位置开始使用数据。 </param>
        /// <param name="count">数组中用作数据的字节数。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] MD516(byte[] buffer, int offset, int count)
        {
            return MD5(buffer, offset, count).GetRange(4, 8).ToArray();
        }

        /// <summary>
        /// 基于 MD5 16 位哈希算法计算指定字符串的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="plaintext">要计算其哈希值的输入。</param>
        /// <returns>计算所得的哈希代码的字符串表现形式。</returns>
        public static string MD516(string plaintext)
        {
            Check.NotNull(plaintext);
            byte[] buffer = StringToBytes(plaintext);
            byte[] hash = MD516(buffer);
            return BytesToString(hash);
        }


        /// <summary>
        /// 使用指定的 Key 以基于 HAMC 的 MD5 16 位哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="buffer">要计算其哈希代码的输入。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] MD516(byte[] buffer, byte[] key)
        {
            return MD5(buffer, key).GetRange(4, 8).ToArray();
        }

        /// <summary>
        /// 使用指定的 Key 以基于 HAMC 的 MD5 16 位哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="inputStream">要计算其哈希代码的输入。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] MD516(Stream inputStream, byte[] key)
        {
            return MD5(inputStream, key).GetRange(4, 8).ToArray();
        }

        /// <summary>
        /// 使用指定的 Key 以基于 HAMC 的 MD5 16 位哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="buffer">要计算其哈希代码的输入。</param>
        /// <param name="offset">字节数组中的偏移量，从该位置开始使用数据。 </param>
        /// <param name="count">数组中用作数据的字节数。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] MD516(byte[] buffer, int offset, int count, byte[] key)
        {
            return MD5(buffer, offset, count, key).GetRange(4, 8).ToArray();
        }

        /// <summary>
        /// 使用指定的 Key 以基于 MD5 16 位哈希算法计算指定字符串的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="plaintext">要计算其哈希值的输入。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>计算所得的哈希代码的字符串表现形式。</returns>
        public static string MD516(string plaintext, string key)
        {
            Check.NotNull(plaintext);
            Check.NotNull(key);
            byte[] buffer = StringToBytes(plaintext);
            byte[] keybuffer = StringToBytes(key);
            byte[] hash = MD516(buffer, keybuffer);
            return BytesToString(hash);
        }


        public static bool CompareMD516(string plaintext, string ciphertext)
        {
            if (plaintext == null || ciphertext == null)
                return false;

            string hashText = MD516(plaintext);
            return hashText.Equals(ciphertext.Replace("-", "").Trim(), StringComparison.OrdinalIgnoreCase);
        }

        public static bool CompareMD516(string plaintext, string ciphertext, byte[] key)
        {
            if (plaintext == null || ciphertext == null)
                return false;

            byte[] hash = MD516(StringToBytes(plaintext), key);
            string hashText = BytesToString(hash);
            return hashText.Equals(ciphertext.Replace("-", "").Trim(), StringComparison.OrdinalIgnoreCase);
        }

        public static bool CompareMD516(string plaintext, string ciphertext, string key)
        {
            if (plaintext == null || ciphertext == null)
                return false;

            string hashText = MD516(plaintext, key);
            return hashText.Equals(ciphertext.Replace("-", "").Trim(), StringComparison.OrdinalIgnoreCase);
        }

        #endregion


        #region 基于 MD5 32 位哈希算法的数据加密和校验操作

        /// <summary>
        /// 基于 MD5 32 位哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="buffer">要计算其哈希代码的输入。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] MD5(byte[] buffer)
        {
            return InternalComputeHash(HashAlgorithmType.MD5, buffer, null);
        }

        /// <summary>
        /// 基于 MD5 32 位哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="inputStream">要计算其哈希代码的输入。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] MD5(Stream inputStream)
        {
            return InternalComputeHash(HashAlgorithmType.MD5, inputStream, null);
        }

        /// <summary>
        /// 基于 MD5 32 位哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="buffer">要计算其哈希代码的输入。</param>
        /// <param name="offset">字节数组中的偏移量，从该位置开始使用数据。 </param>
        /// <param name="count">数组中用作数据的字节数。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] MD5(byte[] buffer, int offset, int count)
        {
            return InternalComputeHash(HashAlgorithmType.MD5, buffer, offset, count, null);
        }

        /// <summary>
        /// 基于 MD5 32 位哈希算法计算指定字符串的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="plaintext">要计算其哈希值的输入。</param>
        /// <returns>计算所得的哈希代码的字符串表现形式。</returns>
        public static string MD5(string plaintext)
        {
            byte[] hash = InternalComputeHash(HashAlgorithmType.MD5, plaintext, null);
            return BytesToString(hash);
        }


        /// <summary>
        /// 使用指定的 Key 以基于 HAMC 的 MD5 32 位哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="buffer">要计算其哈希代码的输入。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] MD5(byte[] buffer, byte[] key)
        {
            return InternalComputeHash(HashAlgorithmType.MD5, buffer, key);
        }

        /// <summary>
        /// 使用指定的 Key 以基于 HAMC 的 MD5 32 位哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="inputStream">要计算其哈希代码的输入。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] MD5(Stream inputStream, byte[] key)
        {
            return InternalComputeHash(HashAlgorithmType.MD5, inputStream, key);
        }

        /// <summary>
        /// 使用指定的 Key 以基于 HAMC 的 MD5 32 位哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="buffer">要计算其哈希代码的输入。</param>
        /// <param name="offset">字节数组中的偏移量，从该位置开始使用数据。 </param>
        /// <param name="count">数组中用作数据的字节数。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] MD5(byte[] buffer, int offset, int count, byte[] key)
        {
            return InternalComputeHash(HashAlgorithmType.MD5, buffer, offset, count, key);
        }

        /// <summary>
        /// 使用指定的 Key 以基于 MD5 32 位哈希算法计算指定字符串的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="plaintext">要计算其哈希值的输入。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>计算所得的哈希代码的字符串表现形式。</returns>
        public static string MD5(string plaintext, string key)
        {
            byte[] hash = InternalComputeHash(HashAlgorithmType.MD5, plaintext, key);
            return BytesToString(hash);
        }


        public static bool CompareMD5(string plaintext, string ciphertext)
        {
            return CompareHash(plaintext, ciphertext, HashAlgorithmType.MD5);
        }

        public static bool CompareMD5(string plaintext, string ciphertext, byte[] key)
        {
            return CompareHash(plaintext, ciphertext, HashAlgorithmType.MD5, key);
        }

        public static bool CompareMD5(string plaintext, string ciphertext, string key)
        {
            return CompareHash(plaintext, ciphertext, HashAlgorithmType.MD5, key);
        }

        #endregion


        #region 基于 RIPEMD160 哈希算法的数据加密和校验操作

        /// <summary>
        /// 基于 RIPEMD160 哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="buffer">要计算其哈希代码的输入。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] RIPEMD160(byte[] buffer)
        {
            return InternalComputeHash(HashAlgorithmType.RIPEMD160, buffer, null);
        }

        /// <summary>
        /// 基于 RIPEMD160 哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="inputStream">要计算其哈希代码的输入。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] RIPEMD160(Stream inputStream)
        {
            return InternalComputeHash(HashAlgorithmType.RIPEMD160, inputStream, null);
        }

        /// <summary>
        /// 基于 RIPEMD160 哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="buffer">要计算其哈希代码的输入。</param>
        /// <param name="offset">字节数组中的偏移量，从该位置开始使用数据。 </param>
        /// <param name="count">数组中用作数据的字节数。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] RIPEMD160(byte[] buffer, int offset, int count)
        {
            return InternalComputeHash(HashAlgorithmType.RIPEMD160, buffer, offset, count, null);
        }

        /// <summary>
        /// 基于 RIPEMD160 哈希算法计算指定字符串的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="plaintext">要计算其哈希值的输入。</param>
        /// <returns>计算所得的哈希代码的字符串表现形式。</returns>
        public static string RIPEMD160(string plaintext)
        {
            byte[] hash = InternalComputeHash(HashAlgorithmType.RIPEMD160, plaintext, null);
            return BytesToString(hash);
        }


        /// <summary>
        /// 使用指定的 Key 以基于 HAMC 的 RIPEMD160 哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="buffer">要计算其哈希代码的输入。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] RIPEMD160(byte[] buffer, byte[] key)
        {
            return InternalComputeHash(HashAlgorithmType.RIPEMD160, buffer, key);
        }

        /// <summary>
        /// 使用指定的 Key 以基于 HAMC 的 RIPEMD160 哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="inputStream">要计算其哈希代码的输入。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] RIPEMD160(Stream inputStream, byte[] key)
        {
            return InternalComputeHash(HashAlgorithmType.RIPEMD160, inputStream, key);
        }

        /// <summary>
        /// 使用指定的 Key 以基于 HAMC 的 RIPEMD160 哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="buffer">要计算其哈希代码的输入。</param>
        /// <param name="offset">字节数组中的偏移量，从该位置开始使用数据。 </param>
        /// <param name="count">数组中用作数据的字节数。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] RIPEMD160(byte[] buffer, int offset, int count, byte[] key)
        {
            return InternalComputeHash(HashAlgorithmType.RIPEMD160, buffer, offset, count, key);
        }

        /// <summary>
        /// 使用指定的 Key 以基于 RIPEMD160 哈希算法计算指定字符串的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="plaintext">要计算其哈希值的输入。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>计算所得的哈希代码的字符串表现形式。</returns>
        public static string RIPEMD160(string plaintext, string key)
        {
            byte[] hash = InternalComputeHash(HashAlgorithmType.RIPEMD160, plaintext, key);
            return BytesToString(hash);
        }


        public static bool CompareRIPEMD160(string plaintext, string ciphertext)
        {
            return CompareHash(plaintext, ciphertext, HashAlgorithmType.RIPEMD160);
        }

        public static bool CompareRIPEMD160(string plaintext, string ciphertext, byte[] key)
        {
            return CompareHash(plaintext, ciphertext, HashAlgorithmType.RIPEMD160, key);
        }

        public static bool CompareRIPEMD160(string plaintext, string ciphertext, string key)
        {
            return CompareHash(plaintext, ciphertext, HashAlgorithmType.RIPEMD160, key);
        }

        #endregion


        #region 基于 SHA1 哈希算法的数据加密和校验操作

        /// <summary>
        /// 基于 SHA1 哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="buffer">要计算其哈希代码的输入。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] SHA1(byte[] buffer)
        {
            return InternalComputeHash(HashAlgorithmType.SHA1, buffer, null);
        }

        /// <summary>
        /// 基于 SHA1 哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="inputStream">要计算其哈希代码的输入。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] SHA1(Stream inputStream)
        {
            return InternalComputeHash(HashAlgorithmType.SHA1, inputStream, null);
        }

        /// <summary>
        /// 基于 SHA1 哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="buffer">要计算其哈希代码的输入。</param>
        /// <param name="offset">字节数组中的偏移量，从该位置开始使用数据。 </param>
        /// <param name="count">数组中用作数据的字节数。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] SHA1(byte[] buffer, int offset, int count)
        {
            return InternalComputeHash(HashAlgorithmType.SHA1, buffer, offset, count, null);
        }

        /// <summary>
        /// 基于 SHA1 哈希算法计算指定字符串的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="plaintext">要计算其哈希值的输入。</param>
        /// <returns>计算所得的哈希代码的字符串表现形式。</returns>
        public static string SHA1(string plaintext)
        {
            byte[] hash = InternalComputeHash(HashAlgorithmType.SHA1, plaintext, null);
            return BytesToString(hash);
        }


        /// <summary>
        /// 使用指定的 Key 以基于 HAMC 的 SHA1 哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="buffer">要计算其哈希代码的输入。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] SHA1(byte[] buffer, byte[] key)
        {
            return InternalComputeHash(HashAlgorithmType.SHA1, buffer, key);
        }

        /// <summary>
        /// 使用指定的 Key 以基于 HAMC 的 SHA1 哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="inputStream">要计算其哈希代码的输入。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] SHA1(Stream inputStream, byte[] key)
        {
            return InternalComputeHash(HashAlgorithmType.SHA1, inputStream, key);
        }

        /// <summary>
        /// 使用指定的 Key 以基于 HAMC 的 SHA1 哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="buffer">要计算其哈希代码的输入。</param>
        /// <param name="offset">字节数组中的偏移量，从该位置开始使用数据。 </param>
        /// <param name="count">数组中用作数据的字节数。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] SHA1(byte[] buffer, int offset, int count, byte[] key)
        {
            return InternalComputeHash(HashAlgorithmType.SHA1, buffer, offset, count, key);
        }

        /// <summary>
        /// 使用指定的 Key 以基于 SHA1 哈希算法计算指定字符串的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="plaintext">要计算其哈希值的输入。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>计算所得的哈希代码的字符串表现形式。</returns>
        public static string SHA1(string plaintext, string key)
        {
            byte[] hash = InternalComputeHash(HashAlgorithmType.SHA1, plaintext, key);
            return BytesToString(hash);
        }


        public static bool CompareSHA1(string plaintext, string ciphertext)
        {
            return CompareHash(plaintext, ciphertext, HashAlgorithmType.SHA1);
        }

        public static bool CompareSHA1(string plaintext, string ciphertext, byte[] key)
        {
            return CompareHash(plaintext, ciphertext, HashAlgorithmType.SHA1, key);
        }

        public static bool CompareSHA1(string plaintext, string ciphertext, string key)
        {
            return CompareHash(plaintext, ciphertext, HashAlgorithmType.SHA1, key);
        }

        #endregion


        #region 基于 SHA256 哈希算法的数据加密和校验操作

        /// <summary>
        /// 基于 SHA256 哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="buffer">要计算其哈希代码的输入。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] SHA256(byte[] buffer)
        {
            return InternalComputeHash(HashAlgorithmType.SHA256, buffer, null);
        }

        /// <summary>
        /// 基于 SHA256 哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="inputStream">要计算其哈希代码的输入。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] SHA256(Stream inputStream)
        {
            return InternalComputeHash(HashAlgorithmType.SHA256, inputStream, null);
        }

        /// <summary>
        /// 基于 SHA256 哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="buffer">要计算其哈希代码的输入。</param>
        /// <param name="offset">字节数组中的偏移量，从该位置开始使用数据。 </param>
        /// <param name="count">数组中用作数据的字节数。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] SHA256(byte[] buffer, int offset, int count)
        {
            return InternalComputeHash(HashAlgorithmType.SHA256, buffer, offset, count, null);
        }

        /// <summary>
        /// 基于 SHA256 哈希算法计算指定字符串的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="plaintext">要计算其哈希值的输入。</param>
        /// <returns>计算所得的哈希代码的字符串表现形式。</returns>
        public static string SHA256(string plaintext)
        {
            byte[] hash = InternalComputeHash(HashAlgorithmType.SHA256, plaintext, null);
            return BytesToString(hash);
        }


        /// <summary>
        /// 使用指定的 Key 以基于 HAMC 的 SHA256 哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="buffer">要计算其哈希代码的输入。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] SHA256(byte[] buffer, byte[] key)
        {
            return InternalComputeHash(HashAlgorithmType.SHA256, buffer, key);
        }

        /// <summary>
        /// 使用指定的 Key 以基于 HAMC 的 SHA256 哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="inputStream">要计算其哈希代码的输入。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] SHA256(Stream inputStream, byte[] key)
        {
            return InternalComputeHash(HashAlgorithmType.SHA256, inputStream, key);
        }

        /// <summary>
        /// 使用指定的 Key 以基于 HAMC 的 SHA256 哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="buffer">要计算其哈希代码的输入。</param>
        /// <param name="offset">字节数组中的偏移量，从该位置开始使用数据。 </param>
        /// <param name="count">数组中用作数据的字节数。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] SHA256(byte[] buffer, int offset, int count, byte[] key)
        {
            return InternalComputeHash(HashAlgorithmType.SHA256, buffer, offset, count, key);
        }

        /// <summary>
        /// 使用指定的 Key 以基于 SHA256 哈希算法计算指定字符串的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="plaintext">要计算其哈希值的输入。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>计算所得的哈希代码的字符串表现形式。</returns>
        public static string SHA256(string plaintext, string key)
        {
            byte[] hash = InternalComputeHash(HashAlgorithmType.SHA256, plaintext, key);
            return BytesToString(hash);
        }


        public static bool CompareSHA256(string plaintext, string ciphertext)
        {
            return CompareHash(plaintext, ciphertext, HashAlgorithmType.SHA256);
        }

        public static bool CompareSHA256(string plaintext, string ciphertext, byte[] key)
        {
            return CompareHash(plaintext, ciphertext, HashAlgorithmType.SHA256, key);
        }

        public static bool CompareSHA256(string plaintext, string ciphertext, string key)
        {
            return CompareHash(plaintext, ciphertext, HashAlgorithmType.SHA256, key);
        }

        #endregion


        #region 基于 SHA384 哈希算法的数据加密和校验操作

        /// <summary>
        /// 基于 SHA384 哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="buffer">要计算其哈希代码的输入。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] SHA384(byte[] buffer)
        {
            return InternalComputeHash(HashAlgorithmType.SHA384, buffer, null);
        }

        /// <summary>
        /// 基于 SHA384 哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="inputStream">要计算其哈希代码的输入。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] SHA384(Stream inputStream)
        {
            return InternalComputeHash(HashAlgorithmType.SHA384, inputStream, null);
        }

        /// <summary>
        /// 基于 SHA384 哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="buffer">要计算其哈希代码的输入。</param>
        /// <param name="offset">字节数组中的偏移量，从该位置开始使用数据。 </param>
        /// <param name="count">数组中用作数据的字节数。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] SHA384(byte[] buffer, int offset, int count)
        {
            return InternalComputeHash(HashAlgorithmType.SHA384, buffer, offset, count, null);
        }

        /// <summary>
        /// 基于 SHA384 哈希算法计算指定字符串的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="plaintext">要计算其哈希值的输入。</param>
        /// <returns>计算所得的哈希代码的字符串表现形式。</returns>
        public static string SHA384(string plaintext)
        {
            byte[] hash = InternalComputeHash(HashAlgorithmType.SHA384, plaintext, null);
            return BytesToString(hash);
        }


        /// <summary>
        /// 使用指定的 Key 以基于 HAMC 的 SHA384 哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="buffer">要计算其哈希代码的输入。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] SHA384(byte[] buffer, byte[] key)
        {
            return InternalComputeHash(HashAlgorithmType.SHA384, buffer, key);
        }

        /// <summary>
        /// 使用指定的 Key 以基于 HAMC 的 SHA384 哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="inputStream">要计算其哈希代码的输入。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] SHA384(Stream inputStream, byte[] key)
        {
            return InternalComputeHash(HashAlgorithmType.SHA384, inputStream, key);
        }

        /// <summary>
        /// 使用指定的 Key 以基于 HAMC 的 SHA384 哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="buffer">要计算其哈希代码的输入。</param>
        /// <param name="offset">字节数组中的偏移量，从该位置开始使用数据。 </param>
        /// <param name="count">数组中用作数据的字节数。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] SHA384(byte[] buffer, int offset, int count, byte[] key)
        {
            return InternalComputeHash(HashAlgorithmType.SHA384, buffer, offset, count, key);
        }

        /// <summary>
        /// 使用指定的 Key 以基于 SHA384 哈希算法计算指定字符串的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="plaintext">要计算其哈希值的输入。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>计算所得的哈希代码的字符串表现形式。</returns>
        public static string SHA384(string plaintext, string key)
        {
            byte[] hash = InternalComputeHash(HashAlgorithmType.SHA384, plaintext, key);
            return BytesToString(hash);
        }


        public static bool CompareSHA384(string plaintext, string ciphertext)
        {
            return CompareHash(plaintext, ciphertext, HashAlgorithmType.SHA384);
        }

        public static bool CompareSHA384(string plaintext, string ciphertext, byte[] key)
        {
            return CompareHash(plaintext, ciphertext, HashAlgorithmType.SHA384, key);
        }

        public static bool CompareSHA384(string plaintext, string ciphertext, string key)
        {
            return CompareHash(plaintext, ciphertext, HashAlgorithmType.SHA384, key);
        }

        #endregion


        #region 基于 SHA512 哈希算法的数据加密和校验操作

        /// <summary>
        /// 基于 SHA512 哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="buffer">要计算其哈希代码的输入。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] SHA512(byte[] buffer)
        {
            return InternalComputeHash(HashAlgorithmType.SHA512, buffer, null);
        }

        /// <summary>
        /// 基于 SHA512 哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="inputStream">要计算其哈希代码的输入。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] SHA512(Stream inputStream)
        {
            return InternalComputeHash(HashAlgorithmType.SHA512, inputStream, null);
        }

        /// <summary>
        /// 基于 SHA512 哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="buffer">要计算其哈希代码的输入。</param>
        /// <param name="offset">字节数组中的偏移量，从该位置开始使用数据。 </param>
        /// <param name="count">数组中用作数据的字节数。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] SHA512(byte[] buffer, int offset, int count)
        {
            return InternalComputeHash(HashAlgorithmType.SHA512, buffer, offset, count, null);
        }

        /// <summary>
        /// 基于 SHA512 哈希算法计算指定字符串的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="plaintext">要计算其哈希值的输入。</param>
        /// <returns>计算所得的哈希代码的字符串表现形式。</returns>
        public static string SHA512(string plaintext)
        {
            byte[] hash = InternalComputeHash(HashAlgorithmType.SHA512, plaintext, null);
            return BytesToString(hash);
        }


        /// <summary>
        /// 使用指定的 Key 以基于 HAMC 的 SHA512 哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="buffer">要计算其哈希代码的输入。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] SHA512(byte[] buffer, byte[] key)
        {
            return InternalComputeHash(HashAlgorithmType.SHA512, buffer, key);
        }

        /// <summary>
        /// 使用指定的 Key 以基于 HAMC 的 SHA512 哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="inputStream">要计算其哈希代码的输入。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] SHA512(Stream inputStream, byte[] key)
        {
            return InternalComputeHash(HashAlgorithmType.SHA512, inputStream, key);
        }

        /// <summary>
        /// 使用指定的 Key 以基于 HAMC 的 SHA512 哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="buffer">要计算其哈希代码的输入。</param>
        /// <param name="offset">字节数组中的偏移量，从该位置开始使用数据。 </param>
        /// <param name="count">数组中用作数据的字节数。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] SHA512(byte[] buffer, int offset, int count, byte[] key)
        {
            return InternalComputeHash(HashAlgorithmType.SHA512, buffer, offset, count, key);
        }

        /// <summary>
        /// 使用指定的 Key 以基于 SHA512 哈希算法计算指定字符串的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="plaintext">要计算其哈希值的输入。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>计算所得的哈希代码的字符串表现形式。</returns>
        public static string SHA512(string plaintext, string key)
        {
            byte[] hash = InternalComputeHash(HashAlgorithmType.SHA512, plaintext, key);
            return BytesToString(hash);
        }


        public static bool CompareSHA512(string plaintext, string ciphertext)
        {
            return CompareHash(plaintext, ciphertext, HashAlgorithmType.SHA512);
        }

        public static bool CompareSHA512(string plaintext, string ciphertext, byte[] key)
        {
            return CompareHash(plaintext, ciphertext, HashAlgorithmType.SHA512, key);
        }

        public static bool CompareSHA512(string plaintext, string ciphertext, string key)
        {
            return CompareHash(plaintext, ciphertext, HashAlgorithmType.SHA512, key);
        }

        #endregion



        #region 基于 MACTripleDES 哈希算法的数据加密和校验操作

        /// <summary>
        /// 使用指定的 Key 以基于 MACTripleDES 哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="buffer">要计算其哈希代码的输入。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] MACTripleDES(byte[] buffer, byte[] key)
        {
            return InternalComputeHash(HashAlgorithmType.MACTripleDES, buffer, key);
        }

        /// <summary>
        /// 使用指定的 Key 以基于 MACTripleDES 哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="inputStream">要计算其哈希代码的输入。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] MACTripleDES(Stream inputStream, byte[] key)
        {
            return InternalComputeHash(HashAlgorithmType.MACTripleDES, inputStream, key);
        }

        /// <summary>
        /// 使用指定的 Key 以基于 HAMC 的 MACTripleDES 哈希算法计算指定字节数组的指定区域的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="buffer">要计算其哈希代码的输入。</param>
        /// <param name="offset">字节数组中的偏移量，从该位置开始使用数据。 </param>
        /// <param name="count">数组中用作数据的字节数。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>计算所得的哈希代码。</returns>
        public static byte[] MACTripleDES(byte[] buffer, int offset, int count, byte[] key)
        {
            return InternalComputeHash(HashAlgorithmType.MACTripleDES, buffer, offset, count, key);
        }

        /// <summary>
        /// 使用指定的 Key 以基于 MACTripleDES 哈希算法计算指定字符串的哈希值（哈希加密计算操作）。
        /// </summary>
        /// <param name="plaintext">要计算其哈希值的输入。</param>
        /// <param name="key">用于哈希算法的密钥。</param>
        /// <returns>计算所得的哈希代码的字符串表现形式。</returns>
        public static string MACTripleDES(string plaintext, string key)
        {
            byte[] hash = InternalComputeHash(HashAlgorithmType.MACTripleDES, plaintext, key);
            return BytesToString(hash);
        }


        public static bool CompareMACTripleDES(string plaintext, string ciphertext, byte[] key)
        {
            return CompareHash(plaintext, ciphertext, HashAlgorithmType.MACTripleDES, key);
        }

        public static bool CompareMACTripleDES(string plaintext, string ciphertext, string key)
        {
            return CompareHash(plaintext, ciphertext, HashAlgorithmType.MACTripleDES, key);
        }

        #endregion




        #region 由调用方选择哈希算法的数据加密操作

        /// <summary>
        /// 以指定的哈希加密算法类型计算字节内容的哈希值。
        /// </summary>
        /// <param name="algorithmType"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] ComputeHash(HashAlgorithmType algorithmType, byte[] buffer)
        {
            return InternalComputeHash(algorithmType, buffer, null);
        }

        /// <summary>
        /// 以指定的哈希加密算法类型计算字节内容的哈希值。
        /// </summary>
        /// <param name="algorithmType"></param>
        /// <param name="inputStream"></param>
        /// <returns></returns>
        public static byte[] ComputeHash(HashAlgorithmType algorithmType, Stream inputStream)
        {
            return InternalComputeHash(algorithmType, inputStream, null);
        }

        /// <summary>
        /// 以指定的哈希加密算法类型计算字节内容的哈希值。
        /// </summary>
        /// <param name="algorithmType"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static byte[] ComputeHash(HashAlgorithmType algorithmType, byte[] buffer, int offset, int count)
        {
            return InternalComputeHash(algorithmType, buffer, offset, count, null);
        }

        /// <summary>
        /// 以指定的哈希加密算法类型计算字符串内容的哈希值。
        /// </summary>
        /// <param name="algorithmType"></param>
        /// <param name="plaintext"></param>
        /// <returns></returns>
        public static string ComputeHash(HashAlgorithmType algorithmType, string plaintext)
        {
            byte[] hash = InternalComputeHash(algorithmType, plaintext, null);
            return BytesToString(hash);
        }


        /// <summary>
        /// 以指定的哈希加密算法类型计算字节内容的哈希值。
        /// </summary>
        /// <param name="algorithmType"></param>
        /// <param name="buffer"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] ComputeHash(HashAlgorithmType algorithmType, byte[] buffer, byte[] key)
        {
            return InternalComputeHash(algorithmType, buffer, key);
        }

        /// <summary>
        /// 以指定的哈希加密算法类型计算输入流内容的哈希值。
        /// </summary>
        /// <param name="algorithmType"></param>
        /// <param name="inputStream"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] ComputeHash(HashAlgorithmType algorithmType, Stream inputStream, byte[] key)
        {
            return InternalComputeHash(algorithmType, inputStream, key);
        }

        /// <summary>
        /// 以指定的哈希加密算法类型计算字节内容的哈希值。
        /// </summary>
        /// <param name="algorithmType"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] ComputeHash(HashAlgorithmType algorithmType, byte[] buffer, int offset, int count, byte[] key)
        {
            return InternalComputeHash(algorithmType, buffer, offset, count, key);
        }

        /// <summary>
        /// 以指定的哈希加密算法类型计算字符串内容的哈希值。
        /// </summary>
        /// <param name="algorithmType"></param>
        /// <param name="plaintext"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ComputeHash(HashAlgorithmType algorithmType, string plaintext, string key)
        {
            byte[] hash = InternalComputeHash(algorithmType, plaintext, key);
            return BytesToString(hash);
        }


        /// <summary>
        /// 以指定的哈希加密算法类型名称计算字节内容的哈希值。
        /// </summary>
        /// <param name="hashName"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] ComputeHash(string hashName, byte[] buffer)
        {
            return InternalComputeHash(hashName, buffer, null);
        }

        /// <summary>
        /// 以指定的哈希加密算法类型名称计算输入流内容的哈希值。
        /// </summary>
        /// <param name="hashName"></param>
        /// <param name="inputStream"></param>
        /// <returns></returns>
        public static byte[] ComputeHash(string hashName, Stream inputStream)
        {
            return InternalComputeHash(hashName, inputStream, null);
        }

        /// <summary>
        /// 以指定的哈希加密算法类型名称计算字节内容的哈希值。
        /// </summary>
        /// <param name="hashName"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static byte[] ComputeHash(string hashName, byte[] buffer, int offset, int count)
        {
            return InternalComputeHash(hashName, buffer, offset, count, null);
        }

        /// <summary>
        /// 以指定的哈希加密算法类型名称计算字符串内容的哈希值。
        /// </summary>
        /// <param name="hashName"></param>
        /// <param name="plaintext"></param>
        /// <returns></returns>
        public static string ComputeHash(string hashName, string plaintext)
        {
            byte[] hash = InternalComputeHash(hashName, plaintext, null);
            return BytesToString(hash);
        }


        /// <summary>
        /// 以指定的哈希加密算法类型名称计算字节内容的哈希值。
        /// </summary>
        /// <param name="hashName"></param>
        /// <param name="buffer"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] ComputeHash(string hashName, byte[] buffer, byte[] key)
        {
            return InternalComputeHash(hashName, buffer, key);
        }

        /// <summary>
        /// 以指定的哈希加密算法类型名称计算输入流内容的哈希值。
        /// </summary>
        /// <param name="hashName"></param>
        /// <param name="inputStream"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] ComputeHash(string hashName, Stream inputStream, byte[] key)
        {
            return InternalComputeHash(hashName, inputStream, key);
        }

        /// <summary>
        /// 以指定的哈希加密算法类型名称计算字节内容的哈希值。
        /// </summary>
        /// <param name="hashName"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static byte[] ComputeHash(string hashName, byte[] buffer, int offset, int count, byte[] key)
        {
            return InternalComputeHash(hashName, buffer, offset, count, key);
        }

        /// <summary>
        /// 以指定的哈希加密算法类型名称计算字符串内容的哈希值。
        /// </summary>
        /// <param name="hashName"></param>
        /// <param name="plaintext"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string ComputeHash(string hashName, string plaintext, string key)
        {
            byte[] hash = InternalComputeHash(hashName, plaintext, key);
            return BytesToString(hash);
        }

        #endregion



        #region Internal CodeBlock

        private static byte[] InternalComputeHash(HashAlgorithmType algorithmType, byte[] buffer, byte[] key)
        {
            Check.NotEmpty(buffer);
            using (HashAlgorithm algorithm = (key == null) ? Create(algorithmType) : Create(algorithmType, key))
            {
                byte[] hash = algorithm.ComputeHash(buffer);
                algorithm.Clear();
                return hash;
            }
        }

        private static byte[] InternalComputeHash(HashAlgorithmType algorithmType, Stream inputStream, byte[] key)
        {
            Check.NotNull(inputStream);
            using (HashAlgorithm algorithm = (key == null) ? Create(algorithmType) : Create(algorithmType, key))
            {
                byte[] hash = algorithm.ComputeHash(inputStream);
                algorithm.Clear();
                return hash;
            }
        }

        private static byte[] InternalComputeHash(HashAlgorithmType algorithmType, byte[] buffer, int offset, int count, byte[] key)
        {
            Check.NotEmpty(buffer);
            using (HashAlgorithm algorithm = (key == null) ? Create(algorithmType) : Create(algorithmType, key))
            {
                byte[] hash = algorithm.ComputeHash(buffer, offset, count);
                algorithm.Clear();
                return hash;
            }
        }

        private static byte[] InternalComputeHash(HashAlgorithmType algorithmType, string plaintext, string key)
        {
            Check.NotEmpty(plaintext);
            return InternalComputeHash(algorithmType, plaintext == null ? null : StringToBytes(plaintext), key == null ? null : StringToBytes(key));
        }


        private static byte[] InternalComputeHash(string hashName, byte[] buffer, byte[] key)
        {
            Check.NotEmpty(buffer);
            using (HashAlgorithm algorithm = (key == null) ? Create(hashName) : Create(hashName, key))
            {
                byte[] hash = algorithm.ComputeHash(buffer);
                algorithm.Clear();
                return hash;
            }
        }

        private static byte[] InternalComputeHash(string hashName, Stream inputStream, byte[] key)
        {
            Check.NotNull(inputStream);
            using (HashAlgorithm algorithm = (key == null) ? Create(hashName) : Create(hashName, key))
            {
                byte[] hash = algorithm.ComputeHash(inputStream);
                algorithm.Clear();
                return hash;
            }
        }

        private static byte[] InternalComputeHash(string hashName, byte[] buffer, int offset, int count, byte[] key)
        {
            Check.NotEmpty(buffer);
            using (HashAlgorithm algorithm = (key == null) ? Create(hashName) : Create(hashName, key))
            {
                byte[] hash = algorithm.ComputeHash(buffer, offset, count);
                algorithm.Clear();
                return hash;
            }
        }

        private static byte[] InternalComputeHash(string hashName, string plaintext, string key)
        {
            Check.NotEmpty(hashName);
            return InternalComputeHash(hashName, plaintext == null ? null : StringToBytes(plaintext), key == null ? null : StringToBytes(key));
        }


        private static string BytesToString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", "");
        }

        private static byte[] StringToBytes(string text)
        {
            return System.Text.Encoding.UTF8.GetBytes(text);
        }

        #endregion
    }
}
