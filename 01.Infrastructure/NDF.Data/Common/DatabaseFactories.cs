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
    /// 提供一组静态方法，这些方法用于创建或获取 <see cref="DatabaseFactory"/> 类型的一个或多个实例。
    /// </summary>
    public static class DatabaseFactories
    {
        internal static Type factoryType = typeof(DatabaseFactory);
        internal static Dictionary<string, DatabaseFactory> _databaseFactoryCache;



        /// <summary>
        /// 根据当前应用程序配置文件中数据库连接配置信息集合的最后一项配置获取 <see cref="DatabaseFactory"/> 对象。
        /// </summary>
        /// <returns>返回一个 <see cref="DatabaseFactory"/> 对象，该对象的 ProviderName 属性值等同于当前应用程序配置文件中数据库连接配置信息集合的最后一项配置所指示的 ProviderName 属性。</returns>
        public static DatabaseFactory GetDefaultFactory()
        {
            ConnectionStringSettings settings = GetDefaultConnectionSetting();
            if (settings == null)
            {
                throw new InvalidOperationException(Resources.NotExistsDatabaseConfig);
            }
            return GetFactory(settings.ProviderName);
        }

        /// <summary>
        /// 根据当前应用程序配置文件中数据库连接配置信息集合的最后一项配置获取 <see cref="DatabaseFactory"/> 对象。
        /// 等效于方法 <seealso cref="GetDefaultFactory"/>。
        /// </summary>
        /// <returns>返回一个 <see cref="DatabaseFactory"/> 对象，该对象的 ProviderName 属性值等同于当前应用程序配置文件中数据库连接配置信息集合的最后一项配置所指示的 ProviderName 属性。</returns>
        public static DatabaseFactory GetFactory()
        {
            return GetDefaultFactory();
        }

        /// <summary>
        /// 根据指定的数据库提供程序名称获取 <see cref="DatabaseFactory"/> 对象。
        /// </summary>
        /// <param name="providerName">指定的数据库提供程序名称。</param>
        /// <returns>返回一个 <see cref="DatabaseFactory"/> 对象，该对象的 ProviderName 属性值等同于传入的参数 <paramref name="providerName"/>。</returns>
        public static DatabaseFactory GetFactory(string providerName)
        {
            Check.NotEmpty(providerName, "providerName");
            DatabaseFactory factory = null;
            if (!DatabaseFactoryCache.TryGetValue(providerName, out factory))
            {
                Type type = factoryType.GetSubClass().FirstOrDefault(t =>
                {
                    DbProviderAttribute attribute = t.GetCustomAttributes(typeof(DbProviderAttribute), true).OfType<DbProviderAttribute>().FirstOrDefault();
                    return attribute != null && attribute.ProviderName == providerName;
                });
                if (type == null)
                {
                    if (!DbProviderFactoryExtensions.DbProviderFactoryClasses.Any(item => item.InvariantName == providerName))
                    {
                        throw new ArgumentException(string.Format(Resources.NotExistsAllowedDbProviderAttributeDatabaseFactory, providerName));
                    }
                    type = typeof(GenericDatabaseFactory);
                }
                factory = Activator.CreateInstance(type, providerName) as DatabaseFactory;
                DatabaseFactoryCache.Add(providerName, factory);
            }
            return factory;
        }




        /// <summary>
        /// 根据当前配置文件中的数据库连接配置集合中的最后一项和当前数据库提供程序名称相匹配的配置，创建一个默认的数据库访问基础组件 <see cref="Database"/> 对象。
        /// </summary>
        /// <returns>数据库访问基础组件 <see cref="Database"/> 对象。</returns>
        public static Database CreateDatabase()
        {
            return GetFactory().CreateDatabase();
        }

        /// <summary>
        /// 根据当前配置文件数据库连接配置集合中指定的配置项名称，创建一个数据库访问基础组件 <see cref="Database"/> 对象。
        /// </summary>
        /// <param name="connectionStringName">指定的数据库连接配置项名称。</param>
        /// <returns>数据库访问基础组件 <see cref="Database"/> 对象。</returns>
        public static Database CreateDatabase(string connectionStringName)
        {
            Check.NotEmpty(connectionStringName, "connectionStringName");
            ConnectionStringSettings settings = System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName];
            return GetFactory(settings.ProviderName).CreateDatabase(settings.ConnectionString);
        }





        /// <summary>
        /// 表示用来存储数据库提供程序名称(ProviderName) 及其对应的 <see cref="DatabaseFactory"/> 对象之间映射关系的缓存。
        /// </summary>
        internal static Dictionary<string, DatabaseFactory> DatabaseFactoryCache
        {
            get
            {
                if (_databaseFactoryCache == null)
                {
                    _databaseFactoryCache = new Dictionary<string, DatabaseFactory>();
                }
                return _databaseFactoryCache;
            }
        }

        /// <summary>
        /// 获取当前应用程序配置文件中数据库连接配置信息集合的最后一项配置。
        /// </summary>
        /// <returns>表示当前应用程序配置文件中数据库连接配置信息集合的最后一项配置的 <see cref="ConnectionStringSettings"/> 对象。</returns>
        internal static ConnectionStringSettings GetDefaultConnectionSetting()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings.OfType<ConnectionStringSettings>().LastOrDefault();
        }
    }
}
