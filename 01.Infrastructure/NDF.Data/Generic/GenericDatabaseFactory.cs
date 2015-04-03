using NDF.Data.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Generic
{
    /// <summary>
    /// 表示一个工厂对象，提供一组方法，用于创建普通 SQL数据库(适用于 Access 等 OleDb 接口数据库) 访问基础组件 <see cref="GenericDatabase"/> 对象。
    /// </summary>
    [DbProvider("System.Data.OleDb")]
    public class GenericDatabaseFactory : DatabaseFactory
    {
        /// <summary>
        /// 以 <paramref name="providerName"/> 作为数据库提供程序名称初始化 <see cref="GenericDatabaseFactory"/> 类型的实例。
        /// </summary>
        public GenericDatabaseFactory(string providerName) : base(providerName) { }



        /// <summary>
        /// 根据指定的数据库连接字符串和数据库提供程序名称，创建一个数据库访问基础组件 <see cref="Database"/> 对象。
        /// </summary>
        /// <param name="connectionString">数据库连接字符串。</param>
        /// <param name="providerName">数据库提供程序名称。</param>
        /// <returns>数据库访问基础组件 <see cref="Database"/> 对象。该对象是一个 <see cref="GenericDatabase"/> 类型实例。</returns>
        public override Database CreateDatabase(string connectionString, string providerName)
        {
            return this.CreateGenericDatabase(connectionString, providerName);
        }



        /// <summary>
        /// 根据指定的数据库连接字符串，创建一个数据库访问基础组件 <see cref="GenericDatabase"/> 对象。
        /// </summary>
        /// <param name="connectionString">数据库连接字符串。</param>
        /// <returns>数据库访问基础组件 <see cref="GenericDatabase"/> 对象。</returns>
        public GenericDatabase CreateGenericDatabase(string connectionString)
        {
            return this.CreateGenericDatabase(connectionString, this.ProviderName);
        }

        /// <summary>
        /// 根据指定的数据库连接字符串和数据库提供程序名称，创建一个数据库访问基础组件 <see cref="GenericDatabase"/> 对象。
        /// </summary>
        /// <param name="connectionString">数据库连接字符串。</param>
        /// <param name="providerName">数据库提供程序名称。</param>
        /// <returns>数据库访问基础组件 <see cref="GenericDatabase"/> 对象。</returns>
        public GenericDatabase CreateGenericDatabase(string connectionString, string providerName)
        {
            return new GenericDatabase(connectionString, providerName);
        }
    }
}
