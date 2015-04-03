using NDF.Data.Common;
using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Sql
{
    /// <summary>
    /// 表示一个工厂对象，提供一组方法，用于创建 SQL Server 数据库访问基础组件 <see cref="SqlDatabase"/> 对象。
    /// </summary>
    [DbProvider("System.Data.SqlClient")]
    public class SqlDatabaseFactory : DatabaseFactory
    {
        /// <summary>
        /// 初始化 <see cref="SqlDatabaseFactory"/> 对象。
        /// </summary>
        public SqlDatabaseFactory() : base() { }

        /// <summary>
        /// 以 <paramref name="providerName"/> 作为数据库提供程序名称初始化 <see cref="SqlDatabaseFactory"/> 类型的实例。
        /// </summary>
        public SqlDatabaseFactory(string providerName) : base(providerName)
        {
            SqlDatabaseFactory.CheckProviderNameIsSqlClient(providerName);
        }



        /// <summary>
        /// 根据指定的数据库连接参数和 <paramref name="providerName"/> 作为数据库提供程序名称，创建一个数据库访问基础组件 <see cref="Database"/> 对象。
        /// 因为需要额外传入 <paramref name="providerName"/> 参数的缘故，而实际上该参数因其为固定值而变得传入无意义，所以不建议使用该方法
        /// 而是直接使用只需要 <paramref name="connectionString"/> 参数的 <seealso cref="CreateDatabase"/> 方法。
        /// </summary>
        /// <param name="connectionString">数据库连接参数。</param>
        /// <param name="providerName">数据库提供程序名称。该参数值必须和工厂类型的数据库提供程序名称相对应，否则该方法将会执行出错。</param>
        /// <returns>数据库访问基础组件 <see cref="Database"/> 对象；该对象是一个 <see cref="SqlDatabase"/> 类型实例。</returns>
        public override Database CreateDatabase(string connectionString, string providerName)
        {
            return this.CreateSqlDatabase(connectionString, providerName);
        }


        /// <summary>
        /// 根据指定的数据库连接参数，创建一个数据库访问基础组件 <see cref="SqlDatabase"/> 对象。
        /// </summary>
        /// <param name="connectionString">指定的 SQL Server 数据库连接参数。</param>
        /// <returns>SQL Server 数据库访问基础组件 <see cref="SqlDatabase"/> 对象。</returns>
        public SqlDatabase CreateSqlDatabase(string connectionString)
        {
            return this.CreateSqlDatabase(connectionString, this.ProviderName);
        }

        /// <summary>
        /// 根据指定的数据库连接参数和 <paramref name="providerName"/> 作为数据库提供程序名称，创建一个数据库访问基础组件 <see cref="SqlDatabase"/> 对象。
        /// 因为需要额外传入 <paramref name="providerName"/> 参数的缘故，而实际上该参数因其为固定值而变得传入无意义，所以不建议使用该方法
        /// 而是直接使用只需要 <paramref name="connectionString"/> 参数的 CreateSqlDatabase 方法。
        /// </summary>
        /// <param name="connectionString">指定的 SQL Server 数据库连接参数。</param>
        /// <param name="providerName">数据库提供程序名称。该值必须限定为 "System.Data.SqlClient"，否则该方法将会执行出错。</param>
        /// <returns>SQL Server 数据库访问基础组件 <see cref="SqlDatabase"/> 对象。</returns>
        public SqlDatabase CreateSqlDatabase(string connectionString, string providerName)
        {
            SqlDatabaseFactory.CheckProviderNameIsSqlClient(providerName);
            return new SqlDatabase(connectionString);
        }



        /// <summary>
        /// 检查传入的数据库提供程序名称 <paramref name="providerName"/> 是否为 "System.Data.SqlClient" 值。
        /// </summary>
        /// <param name="providerName">数据库提供程序名称。</param>
        /// <exception cref="ArgumentException">如果传入的数据库提供程序名称 <paramref name="providerName"/> 不为除 Null、空字符串且不等于 "System.Data.SqlClient"，则抛出该异常。</exception>
        internal static void CheckProviderNameIsSqlClient(string providerName)
        {
            Check.EmptyOrEquals(providerName, "System.Data.SqlClient", "providerName");
        }
    }
}
