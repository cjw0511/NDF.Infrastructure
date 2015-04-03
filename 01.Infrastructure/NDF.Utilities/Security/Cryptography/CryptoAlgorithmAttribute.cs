using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Security.Cryptography
{
    /// <summary>
    /// 用于指示加密算法类型枚举中每个枚举字段所表示的名称或关系。
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    internal class CryptoAlgorithmAttribute : Attribute
    {

        /// <summary>
        /// 初始化 <see cref="CryptoAlgorithmAttribute"/> 特性类的一个新实例。
        /// </summary>
        internal CryptoAlgorithmAttribute() { }

        /// <summary>
        /// 以参数 <paramref name="configName"/> 作为 HashName 属性值
        /// 初始化 <see cref="CryptoAlgorithmAttribute"/> 特性类的一个新实例。
        /// </summary>
        /// <param name="configName"></param>
        internal CryptoAlgorithmAttribute(string configName)
        {
            this.ConfigName = configName;
        }

        /// <summary>
        /// 以参数 <paramref name="configName"/> 作为 HashName 属性值、参数 <paramref name="mappingName"/> 作为 HAMC 属性值
        /// 初始化 <see cref="CryptoAlgorithmAttribute"/> 特性类的一个新实例。
        /// </summary>
        /// <param name="configName"></param>
        /// <param name="mappingName"></param>
        internal CryptoAlgorithmAttribute(string configName, string mappingName) : this(configName)
        {
            this.MappingName = mappingName;
        }



        /// <summary>
        /// 获取或设置该特性所表示的算法的配置名称。
        /// </summary>
        public string ConfigName { get; set; }

        /// <summary>
        /// 获取或设置该特性所表示的算法映射的名称。
        /// </summary>
        public string MappingName { get; set; }
    }
}
