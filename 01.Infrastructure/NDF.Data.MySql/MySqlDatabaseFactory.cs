using NDF.Data.Common;
using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.MySql
{
    /// <summary>
    /// 表示一个工厂对象，提供一组方法，用于创建 MySQL 数据库访问基础组件 <see cref="MySqlDatabase"/> 对象。
    /// </summary>
    [DbProvider("MySql.Data.MySqlClient")]
    public class MySqlDatabaseFactory : DatabaseFactory
    {
        /// <summary>
        /// 初始化 <see cref="MySqlDatabaseFactory"/> 对象。
        /// </summary>
        public MySqlDatabaseFactory() : base() { }

        /// <summary>
        /// 以 <paramref name="providerName"/> 作为数据库提供程序名称初始化 <see cref="MySqlDatabaseFactory"/> 类型的实例。
        /// </summary>
        public MySqlDatabaseFactory(string providerName) : base(providerName)
        {
            MySqlDatabaseFactory.CheckProviderNameIsMySqlClient(providerName);
        }



        /// <summary>
        /// 根据指定的数据库连接参数和 <paramref name="providerName"/> 作为数据库提供程序名称，创建一个数据库访问基础组件 <see cref="Database"/> 对象。
        /// 因为需要额外传入 <paramref name="providerName"/> 参数的缘故，而实际上该参数因其为固定值而变得传入无意义，所以不建议使用该方法
        /// 而是直接使用只需要 <paramref name="connectionString"/> 参数的 <seealso cref="CreateDatabase"/> 方法。
        /// </summary>
        /// <param name="connectionString">数据库连接参数。</param>
        /// <param name="providerName">数据库提供程序名称。该参数值必须和工厂类型的数据库提供程序名称相对应，否则该方法将会执行出错。</param>
        /// <returns>数据库访问基础组件 <see cref="Database"/> 对象；该对象是一个 <see cref="MySqlDatabase"/> 类型实例。</returns>
        public override Database CreateDatabase(string connectionString, string providerName)
        {
            return this.CreateMySqlDatabase(connectionString, providerName);
        }


        /// <summary>
        /// 根据指定的数据库连接参数，创建一个 MySQL 数据库访问基础组件 <see cref="MySqlDatabase"/> 对象。
        /// </summary>
        /// <param name="connectionString">指定的 MySQL 数据库连接参数。</param>
        /// <returns>MySQL 数据库访问基础组件 <see cref="MySqlDatabase"/> 对象。</returns>
        public MySqlDatabase CreateMySqlDatabase(string connectionString)
        {
            return this.CreateMySqlDatabase(connectionString, this.ProviderName);
        }

        /// <summary>
        /// 根据指定的数据库连接参数和 <paramref name="providerName"/> 作为数据库提供程序名称，创建一个 MySQL 数据库访问基础组件 <see cref="MySqlDatabase"/> 对象。
        /// 因为需要额外传入 <paramref name="providerName"/> 参数的缘故，而实际上该参数因其为固定值而变得传入无意义，所以不建议使用该方法
        /// 而是直接使用只需要 <paramref name="connectionString"/> 参数的 CreateMySqlDatabase 方法。
        /// </summary>
        /// <param name="connectionString">指定的 MySQL 数据库连接参数。</param>
        /// <param name="providerName">数据库提供程序名称。该值必须限定为 "MySql.Data.MySqlClient"，否则该方法将会执行出错。</param>
        /// <returns>MySQL 数据库访问基础组件 <see cref="MySqlDatabase"/> 对象。</returns>
        public MySqlDatabase CreateMySqlDatabase(string connectionString, string providerName)
        {
            MySqlDatabaseFactory.CheckProviderNameIsMySqlClient(providerName);
            return new MySqlDatabase(connectionString);
        }







        /// <summary>
        /// 检查传入的数据库提供程序名称 <paramref name="providerName"/> 是否为 "MySql.Data.MySqlClient" 值。
        /// </summary>
        /// <param name="providerName">数据库提供程序名称。</param>
        /// <exception cref="ArgumentException">如果传入的数据库提供程序名称 <paramref name="providerName"/> 不为除 Null、空字符串且不等于 "MySql.Data.MySqlClient"，则抛出该异常。</exception>
        internal static void CheckProviderNameIsMySqlClient(string providerName)
        {
            Check.EmptyOrEquals(providerName, "MySql.Data.MySqlClient", "providerName");
        }
    }
}
