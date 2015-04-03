using NDF.Data.Common;
using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.DB2
{
    /// <summary>
    /// 表示一个工厂对象，提供一组方法，用于创建 IBM DB2 数据库访问基础组件 <see cref="DB2Database"/> 对象。
    /// </summary>
    [DbProvider("IBM.Data.DB2")]
    public class DB2DatabaseFactory : DatabaseFactory
    {
        /// <summary>
        /// 初始化 <see cref="DB2DatabaseFactory"/> 对象。
        /// </summary>
        public DB2DatabaseFactory() : base() { }

        /// <summary>
        /// 以 <paramref name="providerName"/> 作为数据库提供程序名称初始化 <see cref="DB2DatabaseFactory"/> 类型的实例。
        /// </summary>
        public DB2DatabaseFactory(string providerName)
            : base(providerName)
        {
            DB2DatabaseFactory.CheckProviderNameIsDB2Client(providerName);
        }



        /// <summary>
        /// 根据指定的数据库连接参数和 <paramref name="providerName"/> 作为数据库提供程序名称，创建一个数据库访问基础组件 <see cref="Database"/> 对象。
        /// 因为需要额外传入 <paramref name="providerName"/> 参数的缘故，而实际上该参数因其为固定值而变得传入无意义，所以不建议使用该方法
        /// 而是直接使用只需要 <paramref name="connectionString"/> 参数的 <seealso cref="CreateDatabase"/> 方法。
        /// </summary>
        /// <param name="connectionString">数据库连接参数。</param>
        /// <param name="providerName">数据库提供程序名称。该参数值必须和工厂类型的数据库提供程序名称相对应，否则该方法将会执行出错。</param>
        /// <returns>数据库访问基础组件 <see cref="Database"/> 对象；该对象是一个 <see cref="DB2Database"/> 类型实例。</returns>
        public override Database CreateDatabase(string connectionString, string providerName)
        {
            return this.CreateDB2Database(connectionString, providerName);
        }


        /// <summary>
        /// 根据指定的数据库连接参数，创建一个 IBM DB2 数据库访问基础组件 <see cref="DB2Database"/> 对象。
        /// </summary>
        /// <param name="connectionString">指定的 IBM DB2 数据库连接参数。</param>
        /// <returns>IBM DB2 数据库访问基础组件 <see cref="DB2Database"/> 对象。</returns>
        public DB2Database CreateDB2Database(string connectionString)
        {
            return this.CreateDB2Database(connectionString, this.ProviderName);
        }

        /// <summary>
        /// 根据指定的数据库连接参数和 <paramref name="providerName"/> 作为数据库提供程序名称，创建一个 IBM DB2 数据库访问基础组件 <see cref="DB2Database"/> 对象。
        /// 因为需要额外传入 <paramref name="providerName"/> 参数的缘故，而实际上该参数因其为固定值而变得传入无意义，所以不建议使用该方法
        /// 而是直接使用只需要 <paramref name="connectionString"/> 参数的 <see cref="CreateDB2Database(string)"/> 方法。
        /// </summary>
        /// <param name="connectionString">指定的 IBM DB2 数据库连接参数。</param>
        /// <param name="providerName">数据库提供程序名称。该值必须限定为 "IBM.Data.DB2"，否则该方法将会执行出错。</param>
        /// <returns>IBM DB2 数据库访问基础组件 <see cref="DB2Database"/> 对象。</returns>
        public DB2Database CreateDB2Database(string connectionString, string providerName)
        {
            DB2DatabaseFactory.CheckProviderNameIsDB2Client(providerName);
            return new DB2Database(connectionString);
        }




        /// <summary>
        /// 检查传入的数据库提供程序名称 <paramref name="providerName"/> 是否为 "IBM.Data.DB2" 值。
        /// </summary>
        /// <param name="providerName">数据库提供程序名称。</param>
        /// <exception cref="ArgumentException">如果传入的数据库提供程序名称 <paramref name="providerName"/> 不为除 Null、空字符串且不等于 "IBM.Data.DB2"，则抛出该异常。</exception>
        internal static void CheckProviderNameIsDB2Client(string providerName)
        {
            Check.EmptyOrEquals(providerName, "IBM.Data.DB2", "providerName");
        }
    }
}
