using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDF.Data.EntityFramework
{
    /// <summary>
    /// 为 <see cref="System.Data.Entity.Database"/> 实例提供一组工具方法扩展。
    /// </summary>
    public static partial class DatabaseExtensions
    {

        /// <summary>
        /// 基于当前 <paramref name="database"/> 获取与其相同数据库环境的 <see cref="NDF.Data.Common.Database"/> 实例对象。
        /// </summary>
        /// <param name="database">表示一个 <see cref="System.Data.Entity.Database"/> 对象。</param>
        /// <returns>与 <paramref name="database"/> 相同数据库环境的 <see cref="NDF.Data.Common.Database"/> 实例对象。</returns>
        public static NDF.Data.Common.Database GetGeneralDatabase(this Database database)
        {
            NDF.Data.Common.Database db = null;
            DbConnection connection = database.Connection;
            string connectionString = connection.ConnectionString;
            if (!DatabaseCache.TryGetValue(connectionString, out db))
            {
                DbProviderFactory factory = DbProviderFactories.GetFactory(connection);
                string providerName = NDF.Data.Common.DbProviderFactoryExtensions.GetProviderName(factory);
                db = NDF.Data.Common.DatabaseFactories.GetFactory(providerName).CreateDatabase(connection.ConnectionString);
                DatabaseCache.TryAdd(connectionString, db);
            }
            return db;
        }


        /// <summary>
        /// 基于当前 <paramref name="database"/> 获取其所使用的实体数据库上下文 <see cref="System.Data.Entity.DbContext"/> 实例对象。
        /// 返回的对象是一个 <see cref="NDF.Data.EntityFramework.DbContext"/> 实例。
        /// </summary>
        /// <param name="database">表示一个 <see cref="System.Data.Entity.Database"/> 对象。</param>
        /// <returns>
        /// <paramref name="database"/> 所使用的实体数据库上下文 <see cref="System.Data.Entity.DbContext"/> 实例对象。
        /// 返回的对象是一个 <see cref="NDF.Data.EntityFramework.DbContext"/> 实例。
        /// </returns>
        public static System.Data.Entity.DbContext CreateDbContext(this Database database)
        {
            return new DbContext(database.Connection, true);
        }



        /// <summary>
        /// 创建并返回与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbConnection"/> 对象，并在返回前立即将数据库链接对象打开(执行 Open 方法)。
        /// </summary>
        /// <param name="database">表示一个 <see cref="System.Data.Entity.Database"/> 对象。</param>
        /// <returns>与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbConnection"/> 对象，该对象处于打开状态。</returns>
        public static DbConnection GetNewOpenConnection(this Database database)
        {
            return GetGeneralDatabase(database).GetNewOpenConnection();
        }

        /// <summary>
        /// 获取 <paramref name="database"/> 中已经打开的 <see cref="System.Data.Common.DbConnection"/> 对象。
        /// </summary>
        /// <param name="database">表示一个 <see cref="System.Data.Entity.Database"/> 对象。</param>
        /// <returns><paramref name="database"/> 中已经打开的 <see cref="System.Data.Common.DbConnection"/> 包装器。</returns>
        public static DbConnection GetOpenConnection(this Database database)
        {
            return GetGeneralDatabase(database).GetOpenConnection();
        }




        /// <summary>
        /// 解析 SQL 脚本中的参数名称列表并返回。
        /// </summary>
        /// <param name="database">表示一个 <see cref="System.Data.Entity.Database"/> 对象。</param>
        /// <param name="sqlScript">待解析的 SQL 脚本。</param>
        /// <returns>返回 <paramref name="sqlScript"/> 中定义的所有脚本参数名称所构成的一个数组。</returns>
        public static string[] GetParameterNames(this Database database, string sqlScript)
        {
            return GetGeneralDatabase(database).GetParameterNames(sqlScript);
        }


        /// <summary>
        /// 获取表示与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbCommand"/> 查询参数名称前缀字符。
        /// </summary>
        /// <param name="database">表示一个 <see cref="System.Data.Entity.Database"/> 对象。</param>
        /// <returns>表示与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="DbCommand"/> 查询参数名称前缀字符。</returns>
        public static string GetParameterToken(this Database database)
        {
            return GetGeneralDatabase(database).ParameterToken;
        }

        /// <summary>
        /// 获取该数据库访问对象的数据库连接字符串值。
        /// </summary>
        /// <param name="database">表示一个 <see cref="System.Data.Entity.Database"/> 对象。</param>
        /// <returns>该数据库访问对象的数据库连接字符串值。</returns>
        public static string GetConnectionString(this Database database)
        {
            return GetGeneralDatabase(database).ConnectionString;
        }

        /// <summary>
        /// 获取该数据库访问对象的不带安全凭据信息的数据库连接字符串值。
        /// </summary>
        /// <param name="database">表示一个 <see cref="System.Data.Entity.Database"/> 对象。</param>
        /// <returns>该数据库访问对象的不带安全凭据信息的数据库连接字符串值。</returns>
        public static string ConnectionStringWithoutCredentials(this Database database)
        {
            return GetGeneralDatabase(database).ConnectionStringWithoutCredentials;
        }

        /// <summary>
        /// 返回当前 <paramref name="database"/> 所使用的 <see cref="DbProviderFactory"/> 工厂对象。
        /// </summary>
        /// <param name="database">表示一个 <see cref="System.Data.Entity.Database"/> 对象。</param>
        /// <returns>当前 <paramref name="database"/> 所使用的 <see cref="DbProviderFactory"/> 工厂对象。</returns>
        public static DbProviderFactory GetDbProviderFactory(this Database database)
        {
            return GetGeneralDatabase(database).DbProviderFactory;
        }

        /// <summary>
        /// 获取表示与当前 <see cref="DbProviderFactory"/> 关联的数据库提供程序名称。
        /// </summary>
        /// <param name="database">表示一个 <see cref="System.Data.Entity.Database"/> 对象。</param>
        /// <returns>表示与当前 <see cref="DbProviderFactory"/> 关联的数据库提供程序名称。</returns>
        public static string GetProviderName(this Database database)
        {
            return GetGeneralDatabase(database).ProviderName;
        }

        /// <summary>
        /// 获取表示与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="IScriptParameterParser"/> 对象。
        /// </summary>
        /// <param name="database">表示一个 <see cref="System.Data.Entity.Database"/> 对象。</param>
        /// <returns>表示与当前 <see cref="DbProviderFactory"/> 关联的 <see cref="IScriptParameterParser"/> 对象。</returns>
        public static NDF.Data.Common.DbScriptParameterParser GetDbScriptParameterParser(this Database database)
        {
            return GetGeneralDatabase(database).DbScriptParameterParser;
        }




        private static ConcurrentDictionary<string, NDF.Data.Common.Database> _databaseCache;

        /// <summary>
        /// 提供基于数据库连接字符串所谓主键的 <see cref="NDF.Data.Common.Database"/> 对象集合缓存。
        /// </summary>
        internal static ConcurrentDictionary<string, NDF.Data.Common.Database> DatabaseCache
        {
            get
            {
                if (_databaseCache == null)
                {
                    _databaseCache = new ConcurrentDictionary<string, NDF.Data.Common.Database>();
                }
                return _databaseCache;
            }
        }



        /// <summary>
        /// 检查数据库是否与当前 Code First 模型兼容。
        /// </summary>
        /// <param name="_this">表示 <see cref="System.Data.Entity.DbContext"/> 实体上下文对象中引用的 数据库实例。</param>
        /// <returns>如果该数据库实例与当前 Code First 模型兼容，则返回 true，否则返回 false。</returns>
        /// <remarks>
        /// 该方法内部调用 <see cref="System.Data.Entity.Database"/> 对象的 CompatibleWithModel(false) 进行判断。
        /// </remarks>
        public static bool CompatibleWithModel(this Database _this)
        {
            return _this.CompatibleWithModel(false);
        }
    }
}
