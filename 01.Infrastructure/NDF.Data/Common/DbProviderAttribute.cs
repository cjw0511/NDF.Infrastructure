using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Common
{
    /// <summary>
    /// 用于标记该类型所适用的数据库类型。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class DbProviderAttribute : Attribute
    {
        /// <summary>
        /// 初始化 <see cref="DbProviderAttribute"/> 对象。
        /// </summary>
        public DbProviderAttribute() { }

        /// <summary>
        /// 以 <paramref name="providerName"/> 作为数据库提供程序固定名称初始化 <see cref="DbProviderAttribute"/> 对象。
        /// </summary>
        /// <param name="providerName"></param>
        public DbProviderAttribute(string providerName) : this()
        {
            this.ProviderName = providerName;
        }

        /// <summary>
        /// 指示该程序元素所适用的数据类型的数据库提供程序固定名称。
        /// </summary>
        public string ProviderName { get; set; }
    }
}
