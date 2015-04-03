using NDF.Data.Generic;
using NDF.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.Common
{
    /// <summary>
    /// 表示一个工厂对象，提供一组方法，用于创建数据库访问基础组件 <see cref="Database"/> 对象。
    /// </summary>
    public abstract class DatabaseFactory
    {
        private string _providerName;


        /// <summary>
        /// 初始化 <see cref="DatabaseFactory"/> 对象。
        /// </summary>
        public DatabaseFactory() { }

        /// <summary>
        /// 以 <paramref name="providerName"/> 作为数据库提供程序名称初始化 <see cref="DatabaseFactory"/> 对象。
        /// </summary>
        /// <param name="providerName">数据库提供程序名称。</param>
        public DatabaseFactory(string providerName)
        {
            Check.NotEmpty(providerName, "providerName");
            DbProviderFactoryExtensions.CheckProviderNameRegisted(providerName);
            this._providerName = providerName;
        }




        /// <summary>
        /// 根据当前配置文件中的数据库连接配置集合中的最后一项和当前数据库提供程序名称相匹配的配置，创建一个默认的数据库访问基础组件 <see cref="Database"/> 对象。
        /// </summary>
        /// <returns>数据库访问基础组件 <see cref="Database"/> 对象。</returns>
        public Database CreateDefaultDatabase()
        {
            ConnectionStringSettings settings = System.Configuration.ConfigurationManager.ConnectionStrings.OfType<ConnectionStringSettings>().LastOrDefault(item => item.ProviderName == this.ProviderName);
            if (settings == null)
            {
                throw new InvalidOperationException(string.Format(Resources.NotExistsSpecifiedProviderDatabaseConfig, this.ProviderName));
            }
            return this.CreateDatabase(settings.ConnectionString);
        }

        /// <summary>
        /// 根据当前配置文件中的数据库连接配置集合中的最后一项和当前数据库提供程序名称相匹配的配置，创建一个默认的数据库访问基础组件 <see cref="Database"/> 对象。
        /// 等效于方法 <seealso cref="CreateDefaultDatabase"/> 。
        /// </summary>
        /// <returns>数据库访问基础组件 <see cref="Database"/> 对象。</returns>
        public Database CreateDatabase()
        {
            return this.CreateDefaultDatabase();
        }

        /// <summary>
        /// 根据指定的数据库连接参数以及当前工厂对象的 <seealso cref="ProviderName"/> 属性作为数据库提供程序名称，创建一个数据库访问基础组件 <see cref="Database"/> 对象。
        /// </summary>
        /// <param name="connectionString">指定的数据库连接参数。</param>
        /// <returns>数据库访问基础组件 <see cref="Database"/> 对象；</returns>
        public Database CreateDatabase(string connectionString)
        {
            return this.CreateDatabase(connectionString, this.ProviderName);
        }

        /// <summary>
        /// 根据指定的数据库连接参数和 <paramref name="providerName"/> 作为数据库提供程序名称，创建一个数据库访问基础组件 <see cref="Database"/> 对象。
        /// </summary>
        /// <param name="connectionString">数据库连接参数。</param>
        /// <param name="providerName">数据库提供程序名称。该参数值必须和工厂类型的数据库提供程序名称相对应，否则该方法将会执行出错。</param>
        /// <returns>数据库访问基础组件 <see cref="Database"/> 对象；</returns>
        public abstract Database CreateDatabase(string connectionString, string providerName);


        /// <summary>
        /// 根据当前配置文件数据库连接配置集合中指定的配置项名称，创建一个数据库访问基础组件 <see cref="Database"/> 对象。
        /// </summary>
        /// <param name="name">指定的数据库连接配置项名称。</param>
        /// <returns>数据库访问基础组件 <see cref="Database"/> 对象。</returns>
        public Database CreateDatabaseByName(string name)
        {
            ConnectionStringSettings settings = System.Configuration.ConfigurationManager.ConnectionStrings[name];
            return this.CreateDatabase(settings.ConnectionString, settings.ProviderName);
        }


        /// <summary>
        /// 获取表示与当前工厂对象的数据库提供程序名称。
        /// </summary>
        public virtual string ProviderName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this._providerName))
                {
                    DbProviderAttribute attribute = this.GetType().GetCustomAttributes(typeof(DbProviderAttribute), true).OfType<DbProviderAttribute>().FirstOrDefault();
                    if (attribute == null)
                    {
                        throw new InvalidOperationException(Resources.NotDefinedDbProviderAttribute);
                    }
                    this._providerName = attribute.ProviderName;
                }
                return this._providerName;
            }
        }





        /// <summary>
        /// 根据当前应用程序配置文件中数据库连接配置信息集合的最后一项配置获取 <see cref="DatabaseFactory"/> 对象。
        /// 等效于方法 <seealso cref="DatabaseFactories.GetDefaultFactory"/> 。
        /// </summary>
        public static DatabaseFactory Default
        {
            get { return DatabaseFactories.GetDefaultFactory(); }
        }
    }
}
